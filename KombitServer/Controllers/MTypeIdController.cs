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
  [Route ("api/idtype")]
  public class MTypeIdController : Controller
  {
    private readonly KombitDBContext _context;
    public MTypeIdController (KombitDBContext context)
    {
      _context = context;
    }

    [HttpGet]
    public IEnumerable<MTypeId> Get ()
    {
      var idType = _context.MTypeId.ToList ();
      return idType;
    }

    [HttpGet ("{id}")]
    public IActionResult Get (int? id)
    {
      if (id == null)
      {
        return BadRequest ();
      }
      var idType = _context.MTypeId.FirstOrDefault (x => x.Id == id);
      if (idType == null)
      {
        return NotFound ();
      }
      return Ok (idType);
    }

  }

}