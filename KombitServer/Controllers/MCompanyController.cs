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
  [Route ("api/company")]
  public class MCompanyController : Controller
  {
    private readonly KombitDBContext _context;
    public MCompanyController (KombitDBContext context)
    {
      _context = context;
    }

    [HttpGet]
    public IEnumerable<MCompany> Get ()
    {
      var company = _context.MCompany.Include (x => x.Holding).ToList ();
      return company;
    }

    [HttpGet ("{id}")]
    public IActionResult Get (int? id)
    {
      if (id == null)
      {
        return BadRequest ();
      }
      var company = _context.MCompany.Include (x => x.Holding).FirstOrDefault (x => x.Id == id);
      if (company == null)
      {
        return NotFound ();
      }
      return Ok (company);
    }

  }

}