using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KombitServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KombitServer.Controllers
{
  [Route ("api/product")]
  public class ProductController : Controller
  {
    private readonly KombitDBContext _context;
    public ProductController (KombitDBContext context)
    {
      _context = context;
    }

    // GET All
    [HttpGet]
    // Get All with like by user
    [HttpGet ("like/user/{id}")]
    [ResponseCache (Location = ResponseCacheLocation.None, NoStore = true)]
    public IEnumerable<ProductResponse> GetAll (int? id)
    {
      var product = _context.Product
        .Include (x => x.Holding)
        .Include (x => x.Company)
        .Include (x => x.User)
        .Include (x => x.FotoUpload)
        .Include (x => x.Interaction)
        .Include (x => x.Category)
        .ToList ();
      return ProductResponse.FromArray (product, id);
    }

    // Get All by user
    [HttpGet ("user/{id}")]
    [ResponseCache (Location = ResponseCacheLocation.None, NoStore = true)]
    public IEnumerable<ProductResponse> GetAllByUser (int? id)
    {
      if (id == null)
      {
        return GetAll (id);
      }
      var product = _context.Product
        .Include (x => x.Holding)
        .Include (x => x.Company)
        .Include (x => x.User)
        .Include (x => x.FotoUpload)
        .Include (x => x.Interaction)
        .Include (x => x.Category)
        .Where (x => x.UserId == id)
        .ToList ();
      return ProductResponse.FromArray (product, id);
    }

    [HttpGet ("{id}")]
    [ResponseCache (Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult GetDetail (int id)
    {
      var product = _context.Product
        .Include (x => x.Holding)
        .Include (x => x.Company)
        .Include (x => x.User)
        .Include (x => x.FotoUpload)
        .Include (x => x.Category)
        .Include (x => x.Interaction)
        .FirstOrDefault (x => x.Id == id);
      var comment = _context.Interaction.Where (x => x.ProductId == id).Include (x => x.CommentUser).ToList ();
      if (product == null)
      {
        return NotFound (new Exception ("Product not found"));
      }
      return Ok (ProductDetailResponse.FromData (product, comment));
    }

    [HttpGet ("{id}/edit")]
    [ResponseCache (Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult GetProductEdit (int id)
    {
      var product = _context.Product
        .Include (x => x.Holding)
        .Include (x => x.Company)
        .Include (x => x.User)
        .Include (x => x.FotoUpload)
        .Include (x => x.Category)
        .Include (x => x.Interaction)
        .FirstOrDefault (x => x.Id == id);
      if (product == null)
      {
        return NotFound (new Exception ("Product not found"));
      }
      return Ok (ProductRequest.RequestMapping (product));
    }

    [HttpPost]
    public IActionResult NewProduct ([FromBody] ProductRequest productRequest)
    {

      if (!ModelState.IsValid) { return BadRequest (ModelState); }
      var newProduct = Product.NewProductMapping (productRequest);
      _context.Product.Add (newProduct);
      _context.Commit ();

      if (productRequest.Foto.Count () < 1) return Ok (new { msg = "Post Published" });
      var productId = _context.Product.LastOrDefault (x => x.ProductName == productRequest.ProductName && x.CategoryId == productRequest.CategoryId && x.UserId == productRequest.UserId).Id;
      foreach (var foto in productRequest.Foto)
      {
        var newFoto = FotoUpload.NewFotoUploadMapping (foto, productId);
        _context.FotoUpload.Add (newFoto);
        _context.Commit ();
      }
      return Ok (new { msg = "Post Published" });
    }

    [HttpPost ("{id}")]
    public IActionResult UpdateProduct ([FromBody] ProductRequest productRequest, int id)
    {
      if (!ModelState.IsValid) { return BadRequest (ModelState); }
      var updatedProduct = _context.Product.FirstOrDefault (x => x.Id == id);
      updatedProduct = Product.UpdateProductMapping (updatedProduct, productRequest);
      _context.Product.Update (updatedProduct);
      _context.Commit ();

      if (productRequest.Foto.Count () < 1) return Ok (new { msg = "Post Published" });
      foreach (var foto in productRequest.Foto)
      {
        if (foto.Id == 0)
        {
          var newFoto = FotoUpload.NewFotoUploadMapping (foto, id);
          _context.FotoUpload.Add (newFoto);
        }
        else
        {
          var updateFoto = _context.FotoUpload.FirstOrDefault (x => x.Id == foto.Id);
          if (foto.FotoPath == null)
          {
            _context.FotoUpload.Remove (updateFoto);
          }
          else
          {
            updateFoto = FotoUpload.UpdateFotoUploadMapping (foto, updateFoto);
            _context.FotoUpload.Update (updateFoto);
          }
        }
        _context.Commit ();
      }
      return Ok (new { msg = "Post Edited" });
    }

    [HttpDelete ("{id}")]
    public async Task<IActionResult> DeleteProduct (int? id)
    {
      if (id == null)
        return BadRequest ();
      var product = await _context.Product.FirstOrDefaultAsync (x => x.Id == id);
      var foto = await _context.FotoUpload.FirstOrDefaultAsync (x => x.ProductId == id);
      var interaction = _context.Interaction.Where (x => x.ProductId == id);
      _context.Remove (foto);
      _context.Remove (product);
      _context.Remove (interaction);
      await _context.SaveChangesAsync ();
      return Ok ();
    }

  }

}