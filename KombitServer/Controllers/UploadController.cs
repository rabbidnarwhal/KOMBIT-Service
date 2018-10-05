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

namespace KombitServer.Controllers {
  [Route ("api/[controller]")]
  public class UploadController : Controller {
    private readonly IHostingEnvironment _env;
    private readonly KombitDBContext _context;
    public UploadController (IHostingEnvironment env, KombitDBContext context) {
      _env = env;
      _context = context;
    }
    /// <summary>Upload File</summary>
    [HttpPost ("product/{type}/{useCase}")]
    [RequestSizeLimit (268435456)]
    public async Task<IActionResult> UploadProductFileWithUseCase (string type, string useCase) {
      return await UploadProductFile (type, useCase);
    }

    /// <summary>Upload File</summary>
    [HttpPost ("product/{type}")]
    [RequestSizeLimit (268435456)]
    public async Task<IActionResult> UploadProductFile (string type, string useCase) {
      try {
        if (!type.Equals ("foto") && !type.Equals ("video") && !type.Equals ("kit"))
          return BadRequest (new Exception ("Invalid Request " + type));
        var file = Request.Form.Files[0];
        string path = Path.Combine ("upload", "product_" + type + "s" + (useCase.Equals ("") ? "" : "_" + useCase));
        string physicalPath = Path.Combine ("assets", path);
        if (!Directory.Exists (physicalPath))
          Directory.CreateDirectory (physicalPath);
        if (file.Length > 0) {
          var extArray = file.FileName.Split ('.');
          string ext = extArray[extArray.Length - 1];
          string fileName = Guid.NewGuid ().ToString ("N") + "." + ext;
          string fullPath = Path.Combine (physicalPath, fileName);
          using (var stream = new FileStream (fullPath, FileMode.Create)) {
            await file.CopyToAsync (stream);
          }
          var request = HttpContext.Request;
          var uriBuilder = new UriBuilder {
            Host = request.Host.Host,
            Scheme = request.Scheme,
            Path = Path.Combine (path, fileName),
          };
          if (request.Host.Port.HasValue)
            uriBuilder.Port = request.Host.Port.Value;
          var urlPath = uriBuilder.ToString ();

          return Ok (new { name = fileName, path = urlPath });
        } else
          return BadRequest (new Exception ("Invalid File"));
      } catch (System.Exception ex) {
        return StatusCode (StatusCodes.Status500InternalServerError, ex);
      }
    }

    /// <summary>Upload company image</summary>
    [HttpPost ("company/{id}")]
    [RequestSizeLimit (268435456)]
    public async Task<IActionResult> UploadCompanyImage (int? id) {
      if (id == null) {
        return BadRequest (new Exception ("Invalid Company"));
      }
      try {
        var updatedCompany = _context.MCompany
          .FirstOrDefault (x => x.Id == id);
        if (updatedCompany == null) {
          return NotFound (new Exception ("Company not found"));
        }
        var file = Request.Form.Files[0];
        string path = Path.Combine ("uploads", "comp_imgs");
        string physicalPath = Path.Combine ("wwwroot", path);
        if (!Directory.Exists (physicalPath))
          Directory.CreateDirectory (physicalPath);
        if (file.Length > 0) {
          var extArray = file.FileName.Split ('.');
          string ext = extArray[extArray.Length - 1];
          string fileName = Guid.NewGuid ().ToString ("N") + "." + ext;
          string fullPath = Path.Combine (physicalPath, fileName);
          using (var stream = new FileStream (fullPath, FileMode.Create)) {
            await file.CopyToAsync (stream);
          }
          var request = HttpContext.Request;
          var uriBuilder = new UriBuilder {
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
        } else
          return BadRequest (new Exception ("Invalid File"));
      } catch (System.Exception ex) {
        return StatusCode (StatusCodes.Status500InternalServerError, ex);
      }
    }

    /// <summary>Upload user image</summary>
    [HttpPost ("user/{id}")]
    [RequestSizeLimit (268435456)]
    public async Task<IActionResult> UploadUserImage (int? id) {
      try {
        var updatedUser = _context.MUser
          .FirstOrDefault (x => x.Id == id);
        if (updatedUser == null) {
          return NotFound (new Exception ("User not found"));
        }
        var file = Request.Form.Files[0];
        string path = Path.Combine ("uploads", "usr_imgs");
        string physicalPath = Path.Combine ("wwwroot", path);
        if (!Directory.Exists (physicalPath))
          Directory.CreateDirectory (physicalPath);
        if (file.Length > 0) {
          var extArray = file.FileName.Split ('.');
          string ext = extArray[extArray.Length - 1];
          string fileName = Guid.NewGuid ().ToString ("N") + "." + ext;
          string fullPath = Path.Combine (physicalPath, fileName);
          using (var stream = new FileStream (fullPath, FileMode.Create)) {
            await file.CopyToAsync (stream);
          }
          var request = HttpContext.Request;
          var uriBuilder = new UriBuilder {
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
        } else
          return BadRequest (new Exception ("Invalid File"));
      } catch (System.Exception ex) {
        return StatusCode (StatusCodes.Status500InternalServerError, ex);
      }
    }
  }
}