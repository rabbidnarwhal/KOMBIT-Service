﻿using KombitServer.Models;
using KombitServer.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KombitServer.Controllers
{
    [Route("api/product")]
    public class ProductController : Controller
    {
        private readonly KombitDBContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;


        public ProductController(KombitDBContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>Get All product</summary>
        [HttpGet]
        public IEnumerable<ProductResponse> GetAll()
        {
            return GetAllWithLikedIndicator(null);
        }

        /// <summary>Get All product with liked indicator by user</summary>
        [HttpGet("like/user/{id}")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IEnumerable<ProductResponse> GetAllWithLikedIndicator(int? id)
        {
            var product = _context.Product
                .Include(x => x.Holding)
                .Include(x => x.Company)
                .Include(x => x.User)
                .Include(x => x.FotoUpload)
                .Include(x => x.Interaction)
                .Include(x => x.Category)
                .ToList();
            return ProductMapping.ListResponseMapping(product, id).OrderByDescending(x => x.Id);
        }

        /// <summary>Get all prodcut created by user</summary>
        [HttpGet("user/{id}")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IEnumerable<ProductResponse> GetAllByUser(int? id)
        {
            if (id == null) return GetAll();
            var product = _context.Product
                .Include(x => x.Holding)
                .Include(x => x.Company)
                .Include(x => x.User)
                .Include(x => x.FotoUpload)
                .Include(x => x.Interaction)
                .Include(x => x.Category)
                .Where(x => x.UserId == id)
                .ToList();
            return ProductMapping.ListResponseMapping(product, id).OrderByDescending(x => x.Id);
        }

        /// <summary>Get product with liked indicator by user</summary>
        [HttpGet("{id}/user/{userId}")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult GetDetail(int id, int userId)
        {
            var product = _context.Product
                .Include(x => x.Holding)
                .Include(x => x.Company)
                .Include(x => x.User)
                .Include(x => x.FotoUpload)
                .Include(x => x.Category)
                .Include(x => x.Interaction)
                .Include(x => x.AttachmentFile)
                .FirstOrDefault(x => x.Id == id);
            var interaction = _context.Interaction.Where(x => x.ProductId == id).Include(x => x.CommentUser).ToList();
            if (product == null) return NotFound(new Exception("Product not found"));
            return Ok(new ProductDetailResponse(product, interaction, userId));
        }

        /// <summary>Get editable product request</summary>
        [HttpGet("{id}/edit")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult GetProductEdit(int id)
        {
            var product = _context.Product
                .Include(x => x.Holding)
                .Include(x => x.Company)
                .Include(x => x.User)
                .Include(x => x.FotoUpload)
                .Include(x => x.Category)
                .Include(x => x.Interaction)
                .Include(x => x.AttachmentFile)
                .FirstOrDefault(x => x.Id == id);
            if (product == null) return NotFound(new Exception("Product not found"));
            return Ok(new ProductRequest(product));
        }

        /// <summary>Add new product</summary>
        [HttpPost]
        public IActionResult NewProduct([FromBody] ProductRequest productRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _context.Product.Add(new Product(productRequest));
            _context.Commit();

            if (!productRequest.Foto.Any()) return Ok(new {msg = "Post Published"});
            var productId = _context.Product.LastOrDefault(x =>
                x.ProductName == productRequest.ProductName && x.CategoryId == productRequest.CategoryId &&
                x.UserId == productRequest.UserId).Id;
            foreach (var foto in productRequest.Foto)
            {
                _context.FotoUpload.Add(new FotoUpload(foto, productId, "foto"));
                _context.Commit();
            }

            var body = NotificationEmptyRequest.Init();
            var jsonBody = JsonConvert.SerializeObject(body);
            Utility.sendPushNotification(jsonBody);
            return Ok(new {msg = "Post Published"});
        }

        /// <summary>Update product</summary>
        [HttpPost("{id}")]
        public IActionResult UpdateProduct([FromBody] ProductRequest productRequest, int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var product = _context.Product.FirstOrDefault(x => x.Id == id);
            product = ProductMapping.UpdateProductMapping(productRequest, product);
            _context.Product.Update(product);

            
            if (!productRequest.Foto.Any()) return Ok(new {msg = "Post Published"});
            var removedFotoPath = new List<string>();
            foreach (var foto in productRequest.Foto)
            {
                if (foto.Id == 0)
                {
                    _context.FotoUpload.Add(new FotoUpload(foto, id, "foto"));
                }
                else
                {
                    var existingFoto = _context.FotoUpload.FirstOrDefault(x => x.Id == foto.Id);
                    removedFotoPath.Add(existingFoto.FotoPath);
                    if (foto.FotoPath == null)
                    {
                        _context.FotoUpload.Remove(existingFoto);                       
                    }
                    else
                    {
                        existingFoto = FotoUploadMapping.UpdateFotoUploadMapping(foto, existingFoto);
                        _context.FotoUpload.Update(existingFoto);
                    }
                }
            }

            foreach (var deletedFoto in removedFotoPath)
            {
                var webRootPath = _hostingEnvironment.WebRootPath;

                var request = HttpContext.Request;
                var uriBuilder = new UriBuilder
                {
                    Host = request.Host.Host,
                    Scheme = request.Scheme
                };
                if (request.Host.Port.HasValue)
                    uriBuilder.Port = request.Host.Port.Value;

                var urlPath = uriBuilder.ToString();

                var path = deletedFoto.Replace(urlPath, webRootPath + @"\").Replace("/", @"\");

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }

            _context.Commit();

            return Ok(new {msg = "Post Edited"});
        }

        /// <summary>Delete product</summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if (id == null) return BadRequest();
            var product = await _context.Product.FirstOrDefaultAsync(x => x.Id == id);
            var foto = await _context.FotoUpload.FirstOrDefaultAsync(x => x.ProductId == id);
            var interaction = _context.Interaction.Where(x => x.ProductId == id);
            _context.Remove(foto);
            _context.Remove(product);
            _context.Remove(interaction);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}