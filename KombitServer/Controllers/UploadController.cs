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
    private readonly KombitDBContext _context;
    public UploadController (IHostingEnvironment env, KombitDBContext context)
    {
      _env = env;
      _context = context;

    }

    [HttpPost ("product/{type}")]
    [RequestSizeLimit (268435456)]
    public async Task<IActionResult> UploadProductFile (string type)
    {
      try
      {
        if (!type.Equals ("foto") && !type.Equals ("video"))
          return BadRequest (new Exception ("Invalid Request " + type));
        var file = Request.Form.Files[0];
        string path = Path.Combine ("uploads", "product_" + type + "s");
        string physicalPath = Path.Combine ("wwwroot", path);
        if (!Directory.Exists (physicalPath))
          Directory.CreateDirectory (physicalPath);
        if (file.Length > 0)
        {
          string fileName = type.Equals ("foto") ? Guid.NewGuid ().ToString ("N") + ".jpg" : Guid.NewGuid ().ToString ("N") + ".mp4";
          string fullPath = Path.Combine (physicalPath, fileName);
          using (var stream = new FileStream (fullPath, FileMode.Create))
          {
            await file.CopyToAsync (stream);
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

          return Ok (new { name = fileName, path = urlPath });
        }
        else
          return BadRequest (new Exception ("Invalid File"));
      }
      catch (System.Exception ex)
      {
        return StatusCode (StatusCodes.Status500InternalServerError, ex);
      }
    }

    [HttpPost ("/company/{id}")]
    [RequestSizeLimit (268435456)]
    public async Task<IActionResult> UploadCompanyImage (int? id)
    {
      if (id == null)
      {
        return BadRequest (new Exception ("Invalid Company"));
      }
      try
      {
        var updatedCompany = _context.MCompany
          .FirstOrDefault (x => x.Id == id);
        if (updatedCompany == null)
        {
          return NotFound (new Exception ("Company not found"));
        }
        var file = Request.Form.Files[0];
        string path = Path.Combine ("uploads", "comp_imgs");
        string physicalPath = Path.Combine ("wwwroot", path);
        if (!Directory.Exists (physicalPath))
          Directory.CreateDirectory (physicalPath);
        if (file.Length > 0)
        {
          var extArray = file.FileName.Split ('.');
          string ext = extArray[extArray.Length - 1];
          string fileName = Guid.NewGuid ().ToString ("N") + "." + ext;
          string fullPath = Path.Combine (physicalPath, fileName);
          using (var stream = new FileStream (fullPath, FileMode.Create))
          {
            await file.CopyToAsync (stream);
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
          updatedCompany.Image = urlPath;
          _context.Update (updatedCompany);
          _context.Commit ();
          return Ok (new { name = fileName, path = urlPath });
        }
        else
          return BadRequest (new Exception ("Invalid File"));
      }
      catch (System.Exception ex)
      {
        return StatusCode (StatusCodes.Status500InternalServerError, ex);
      }
    }

    [HttpPost ("user/{id}")]
    [RequestSizeLimit (268435456)]
    public async Task<IActionResult> UploadUserImage (int? id)
    {
      try
      {
        var updatedUser = _context.MUser
          .FirstOrDefault (x => x.Id == id);
        if (updatedUser == null)
        {
          return NotFound (new Exception ("User not found"));
        }
        var file = Request.Form.Files[0];
        string path = Path.Combine ("uploads", "usr_imgs");
        string physicalPath = Path.Combine ("wwwroot", path);
        if (!Directory.Exists (physicalPath))
          Directory.CreateDirectory (physicalPath);
        if (file.Length > 0)
        {
          // Int32 unixTimestamp = (Int32) (DateTime.UtcNow.Subtract (new DateTime (1970, 1, 1))).TotalSeconds;
          // string fileName = string.Concat (unixTimestamp.ToString (), "_", ContentDispositionHeaderValue.Parse (file.ContentDisposition).FileName.Trim ('"'));
          var extArray = file.FileName.Split ('.');
          string ext = extArray[extArray.Length - 1];
          string fileName = Guid.NewGuid ().ToString ("N") + "." + ext;
          string fullPath = Path.Combine (physicalPath, fileName);
          using (var stream = new FileStream (fullPath, FileMode.Create))
          {
            await file.CopyToAsync (stream);
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
          updatedUser.Image = urlPath;
          _context.Update (updatedUser);
          _context.Commit ();
          return Ok (new { name = fileName, path = urlPath });
        }
        else
          return BadRequest (new Exception ("Invalid File"));
      }
      catch (System.Exception ex)
      {
        return StatusCode (StatusCodes.Status500InternalServerError, ex);
      }
    }
  }
}