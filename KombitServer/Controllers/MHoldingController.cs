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
  [Route ("api/holding")]
  public class MHoldingController : Controller
  {
    private readonly KombitDBContext _context;
    public MHoldingController (KombitDBContext context)
    {
      _context = context;
    }

    [HttpGet]
    public IEnumerable<MHolding> Get ()
    {
      var holding = _context.MHolding.ToList ();
      return holding;
    }

    [HttpGet ("{id}")]
    public IActionResult Get (int? id)
    {
      if (id == null)
      {
        return BadRequest (new Exception ("Invalid Holding"));
      }
      var holding = _context.MHolding.FirstOrDefault (x => x.Id == id);
      if (holding == null)
      {
        return NotFound (new Exception ("Holding not found"));
      }
      return Ok (holding);
    }

    [HttpGet ("{id}/company")]
    public async Task<IActionResult> GetCompany (int? id)
    {
      if (id == null)
      {
        return BadRequest (new Exception ("Invalid Holding"));
      }
      var company = await _context.MCompany.Where (x => x.HoldingId == id).ToListAsync ();
      if (company == null)
      {
        return NotFound (new Exception ("Holding not found"));
      }
      return Ok (company);
    }

  }

}