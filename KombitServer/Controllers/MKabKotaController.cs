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
  [Route ("api/kabkota")]
  public class MKabKotaController : Controller
  {
    private readonly KombitDBContext _context;
    public MKabKotaController (KombitDBContext context)
    {
      _context = context;
    }

    [HttpGet]
    public IEnumerable<MKabKota> Get ()
    {
      var kabKota = _context.MKabKota.ToList ();
      return kabKota;
    }

    [HttpGet ("provinsi/{id}")]
    public IActionResult Get (int? id)
    {
      if (id == null)
      {
        return BadRequest (new Exception ("Invalid Provinsi"));
      }
      var kabKota = _context.MKabKota.Include (x => x.Provinsi).FirstOrDefault (x => x.ProvinsiId == id);
      return Ok (kabKota);
    }

  }

}