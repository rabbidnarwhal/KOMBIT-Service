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
  [Route ("api/provinsi")]
  public class MProvinsiController : Controller
  {
    private readonly KombitDBContext _context;
    public MProvinsiController (KombitDBContext context)
    {
      _context = context;
    }

    [HttpGet]
    public IEnumerable<MProvinsi> Get ()
    {
      var provinsi = _context.MProvinsi.ToList ();
      return provinsi;
    }

    [HttpGet ("{id}")]
    public IActionResult Get (int? id)
    {
      if (id == null)
      {
        return BadRequest (new Exception ("Invalid Provinsi"));
      }
      var provinsi = _context.MProvinsi.FirstOrDefault (x => x.Id == id);

      return Ok (provinsi);
    }

  }

}