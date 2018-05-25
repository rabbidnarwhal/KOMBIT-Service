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

    [HttpGet]
    [HttpGet ("interaction/user/{id}")]
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

    [HttpGet ("user/{id}")]
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
    public IActionResult GetDetail (int? id)
    {
      if (id == null)
      {
        return BadRequest (new Exception ("Invalid Product"));
      }
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
      // return Ok (product);
    }

    [HttpPost]
    public IActionResult NewProduct ([FromBody] ProductRequest productRequest)
    {
      if (!ModelState.IsValid) { return BadRequest (ModelState); }
      var newProduct = Product.ProductMapping (productRequest);
      _context.Product.Add (newProduct);
      _context.Commit ();

      if (productRequest.FotoName == null && productRequest.FotoPath == null) return Ok (new { msg = "Product without foto saved" });

      var productId = _context.Product.LastOrDefault (x => x.ProductName == productRequest.ProductName && x.CategoryId == productRequest.CategoryId && x.UserId == productRequest.UserId).Id;
      var newFoto = FotoUpload.FotoUploadMapping (productRequest, productId);
      _context.FotoUpload.Add (newFoto);
      _context.Commit ();
      return Ok (new { msg = "Product saved" });
    }

  }

}