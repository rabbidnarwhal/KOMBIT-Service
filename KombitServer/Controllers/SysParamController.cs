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
  [Route ("api/config")]
  public class SysParamController : Controller
  {
    private readonly KombitDBContext _context;
    public SysParamController (KombitDBContext context)
    {
      _context = context;
    }

    [HttpGet]
    public IEnumerable<SysParam> GetAll ()
    {
      var config = _context.SysParam.ToList ();
      return config;
    }

    [HttpGet ("{code}")]
    public IActionResult GetParamsByCode (string code)
    {
      if (code == null)
      {
        return Ok (GetAll ());
      }
      var config = _context.SysParam
        .FirstOrDefault (x => x.ParamCode == code);
      return Ok (config);
    }
  }
}