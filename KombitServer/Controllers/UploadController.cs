using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KombitServer.Models;
using KombitServer.Models.Upload;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KombitServer.Controllers {
    [Route ("api/[controller]")]
    public class UploadController : Controller {
        private readonly KombitDBContext _context;
        private readonly IHostingEnvironment _env;

        public UploadController (IHostingEnvironment env, KombitDBContext context) {
            _env = env;
            _context = context;
        }

        /// <summary>Upload File</summary>
        [HttpPost ("product")]
        [RequestSizeLimit (268435456)]
        public async Task<IActionResult> UploadProductFile (UploadModel model) {
            try {
                if (!model.Type.Equals ("foto") && !model.Type.Equals ("video") && !model.Type.Equals ("kit"))
                    return BadRequest (new Exception ("Invalid Request " + model.Type));
                var file = model.File;
                var path = Path.Combine ("upload", "products", "poster_" + model.UserId, model.ProductName, model.UseCase);
                var physicalPath = Path.Combine ("assets", path);
                if (!Directory.Exists (physicalPath))
                    Directory.CreateDirectory (physicalPath);
                if (file.Length > 0) {
                    var extArray = file.FileName.Split ('.');
                    var ext = extArray[extArray.Length - 1];
                    var fileName = Guid.NewGuid ().ToString ("N") + "." + ext;
                    var fullPath = Path.Combine (physicalPath, fileName);
                    using (var stream = new FileStream (fullPath, FileMode.Create)) {
                        await file.CopyToAsync (stream);
                    }

                    var request = HttpContext.Request;
                    var uriBuilder = new UriBuilder {
                        Host = request.Host.Host,
                        Scheme = request.Scheme,
                        Path = Path.Combine (path, fileName)
                    };
                    if (request.Host.Port.HasValue)
                        uriBuilder.Port = request.Host.Port.Value;
                    var urlPath = uriBuilder.ToString ();

                    return Ok (new { name = fileName, path = urlPath });
                }
                return BadRequest (new Exception ("Invalid File"));
            } catch (Exception ex) {
                return StatusCode (StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>Upload company image</summary>
        [HttpPost ("company/{id}")]
        [RequestSizeLimit (268435456)]
        public async Task<IActionResult> UploadCompanyImage (int? id) {
            if (id == null) return BadRequest (new Exception ("Invalid Company"));
            try {
                var updatedCompany = _context.MCompany
                    .FirstOrDefault (x => x.Id == id);
                if (updatedCompany == null) return NotFound (new Exception ("Company not found"));
                var file = Request.Form.Files[0];
                var path = Path.Combine ("upload", "companies");
                var physicalPath = Path.Combine ("assets", path, id.ToString());
                if (!Directory.Exists (physicalPath))
                    Directory.CreateDirectory (physicalPath);
                if (file.Length > 0) {
                    var extArray = file.FileName.Split ('.');
                    var ext = extArray[extArray.Length - 1];
                    var fileName = Guid.NewGuid ().ToString ("N") + "." + ext;
                    var fullPath = Path.Combine (physicalPath, fileName);
                    using (var stream = new FileStream (fullPath, FileMode.Create)) {
                        await file.CopyToAsync (stream);
                    }

                    var request = HttpContext.Request;
                    var uriBuilder = new UriBuilder {
                        Host = request.Host.Host,
                        Scheme = request.Scheme,
                        Path = Path.Combine (path, fileName)
                    };
                    if (request.Host.Port.HasValue)
                        uriBuilder.Port = request.Host.Port.Value;
                    var urlPath = uriBuilder.ToString ();
                    updatedCompany.Image = urlPath;
                    _context.Update (updatedCompany);
                    _context.Commit ();
                    return Ok (new { name = fileName, path = urlPath });
                }

                return BadRequest (new Exception ("Invalid File"));
            } catch (Exception ex) {
                return StatusCode (StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>Upload category image</summary>
        [HttpPost ("category/{id}")]
        [RequestSizeLimit (268435456)]
        public async Task<IActionResult> UploadCategoryImage (int? id) {
            if (id == null) return BadRequest (new Exception ("Invalid Category"));
            try {
                var updatedCategpry = _context.MCategory
                    .FirstOrDefault (x => x.Id == id);
                if (updatedCategpry == null) return NotFound (new Exception ("Company not found"));
                var file = Request.Form.Files[0];
                var path = Path.Combine ("upload", "categories");
                var physicalPath = Path.Combine ("assets", path, id.ToString());
                if (!Directory.Exists (physicalPath))
                    Directory.CreateDirectory (physicalPath);
                if (file.Length > 0) {
                    var extArray = file.FileName.Split ('.');
                    var ext = extArray[extArray.Length - 1];
                    var fileName = Guid.NewGuid ().ToString ("N") + "." + ext;
                    var fullPath = Path.Combine (physicalPath, fileName);
                    using (var stream = new FileStream (fullPath, FileMode.Create)) {
                        await file.CopyToAsync (stream);
                    }

                    var request = HttpContext.Request;
                    var uriBuilder = new UriBuilder {
                        Host = request.Host.Host,
                        Scheme = request.Scheme,
                        Path = Path.Combine (path, fileName)
                    };
                    if (request.Host.Port.HasValue)
                        uriBuilder.Port = request.Host.Port.Value;
                    var urlPath = uriBuilder.ToString ();
                    updatedCategpry.Image = urlPath;
                    _context.Update (updatedCategpry);
                    _context.Commit ();
                    return Ok (new { name = fileName, path = urlPath });
                }

                return BadRequest (new Exception ("Invalid File"));
            } catch (Exception ex) {
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
                if (updatedUser == null) return NotFound (new Exception ("User not found"));
                var file = Request.Form.Files[0];
                var path = Path.Combine ("upload", "users");
                var physicalPath = Path.Combine ("assets", path, id.ToString());
                if (!Directory.Exists (physicalPath))
                    Directory.CreateDirectory (physicalPath);
                if (file.Length > 0) {
                    var extArray = file.FileName.Split ('.');
                    var ext = extArray[extArray.Length - 1];
                    var fileName = Guid.NewGuid ().ToString ("N") + "." + ext;
                    var fullPath = Path.Combine (physicalPath, fileName);
                    using (var stream = new FileStream (fullPath, FileMode.Create)) {
                        await file.CopyToAsync (stream);
                    }

                    var request = HttpContext.Request;
                    var uriBuilder = new UriBuilder {
                        Host = request.Host.Host,
                        Scheme = request.Scheme,
                        Path = Path.Combine (path, fileName)
                    };
                    if (request.Host.Port.HasValue)
                        uriBuilder.Port = request.Host.Port.Value;
                    var urlPath = uriBuilder.ToString ();
                    updatedUser.Image = urlPath;
                    _context.Update (updatedUser);
                    _context.Commit ();
                    return Ok (new { name = fileName, path = urlPath });
                }

                return BadRequest (new Exception ("Invalid File"));
            } catch (Exception ex) {
                return StatusCode (StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}