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

    [HttpPost ("product_interval")]
    public async Task<IActionResult> ChangeProductInterval ([FromBody] SysParamRequest paramRequest)
    {
      if (paramRequest.ParamCode == null && !paramRequest.ParamCode.Equals("DEFAULT_PRODUCT_INTERVAL") && paramRequest.ParamValue == null)
      {
        return BadRequest ();
      }

      var config = await _context.SysParam.FirstOrDefaultAsync(x => x.ParamCode.Equals(paramRequest.ParamCode));
      if (config == null) {
        return BadRequest ();
      }
      config.ParamValue = paramRequest.ParamValue;
      _context.Update(config);
      _context.Commit();

      // var products = _context.Product.ToList();
      // foreach (var item in products)
      // {
      //   item.UpdateIntervalInSecond = paramRequest.ParamValue;
      //   _context.Update(item);
      // }
      // _context.Commit();

      return Ok();
    }


  }
}