using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using KombitServer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KombitServer.Controllers
{
  [Route ("api/[controller]")]
  public class UploadController : Controller
  {
    private readonly IHostingEnvironment _env;
    public UploadController (IHostingEnvironment env)
    {
      _env = env;
    }

    [HttpPost ("foto")]
    public IActionResult UploadFoto2 ()
    {
      try
      {
        var file = Request.Form.Files[0];
        string path = Path.Combine ("foto", "uploads");
        string physicalPath = Path.Combine ("www", "foto", "uploads");
        if (!Directory.Exists (physicalPath))
          Directory.CreateDirectory (physicalPath);
        if (file.Length > 0)
        {
          Int32 unixTimestamp = (Int32) (DateTime.UtcNow.Subtract (new DateTime (1970, 1, 1))).TotalSeconds;
          string fileName = string.Concat (unixTimestamp.ToString (), "_", ContentDispositionHeaderValue.Parse (file.ContentDisposition).FileName.Trim ('"'));
          string fullPath = Path.Combine (physicalPath, fileName);
          using (var stream = new FileStream (fullPath, FileMode.Create))
          {
            file.CopyTo (stream);
          }
          var request = HttpContext.Request;
          var uriBuilder = new UriBuilder
          {
            Host = request.Host.Host,
            Scheme = request.Scheme,
            Path = Path.Combine (path, fileName),
          };
          if (request.Host.Port.HasValue)
            uriBuilder.Port = request.Host.Port.Value;
          var urlPath = uriBuilder.ToString ();
          return Ok (new { path = urlPath });
        }
        else
        {
          return BadRequest ();
        }
      }
      catch (System.Exception ex)
      {
        return StatusCode (StatusCodes.Status500InternalServerError, ex);
      }
    }
  }
}