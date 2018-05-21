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
        .FirstOrDefault (x => x.Id == id);
      if (product == null)
      {
        return NotFound ();
      }
      return Ok (ProductDetailResponse.FromData (product));
    }

  }

}