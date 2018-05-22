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
    public IEnumerable<ProductResponse> Get ()
    {
      var product = _context.Product
        .Include (x => x.Holding)
        .Include (x => x.Company)
        .Include (x => x.User)
        .Include (x => x.FotoUpload)
        .Include (x => x.Interaction)
        .ToArray ();
      return ProductResponse.FromArray (product);
    }

    [HttpGet ("{id}")]
    public IActionResult Get (int? id)
    {
      if (id == null)
      {
        return BadRequest ();
      }
      var product = _context.Product
        .Include (x => x.Holding)
        .Include (x => x.Company)
        .Include (x => x.User)
        .Include (x => x.FotoUpload)
        .Include (x => x.Interaction)
        .Include (x => x.Category)
        .FirstOrDefault (x => x.Id == id);
      if (product == null)
      {
        return NotFound ();
      }
      return Ok (ProductDetailResponse.FromData (product));
    }

    [HttpPost]
    public IActionResult NewProduct ([FromBody] ProductRequest productRequest)
    {
      if (!ModelState.IsValid) { return BadRequest (ModelState); }
      var newProduct = Product.ProductMapping (productRequest);
      _context.Product.Add (newProduct);
      _context.Commit ();

      var product = _context.Product.FirstOrDefaultAsync (x => x.ProductName == productRequest.ProductName && x.Description == productRequest.Description && x.UserId == productRequest.UserId);
      var newFoto = FotoUpload.FotoUploadMapping (productRequest, product.Id);
      _context.FotoUpload.Add (newFoto);
      _context.Commit ();
      return Ok ();
    }

  }

}