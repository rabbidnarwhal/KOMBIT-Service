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
  [Route ("api/category")]
  public class MCategoryController : Controller
  {
    private readonly KombitDBContext _context;
    public MCategoryController (KombitDBContext context)
    {
      _context = context;
    }

    [HttpGet]
    public IEnumerable<MCategory> Get ()
    {
      var category = _context.MCategory.ToList ();
      return category;
    }

    [HttpGet ("{id}")]
    public IActionResult Get (int? id)
    {
      if (id == null)
      {
        return BadRequest ();
      }
      var category = _context.MCategory.FirstOrDefault (x => x.Id == id);
      if (category == null)
      {
        return NotFound ();
      }
      return Ok (category);
    }

  }

}