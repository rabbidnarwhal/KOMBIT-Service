using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using KombitServer.Models;
using KombitServer.Services.PushNotification;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace KombitServer.Controllers {
    [Route ("api/product")]
    public class ProductController : Controller {
        private readonly KombitDBContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ProductController (KombitDBContext context, IHostingEnvironment hostingEnvironment) {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>Get All active product</summary>
        [HttpGet]
        [ProducesResponseType (typeof (List<ProductResponse>), 200)]

        public IEnumerable<ProductResponse> GetAllActive () {
          return GetAllWithLikedIndicator(null);
        }

        /// <summary>Get All product</summary>

        [HttpGet ("all")]
        public IEnumerable<ProductResponse> GetAllProducts () {
            var product = _context.Product
                .Include (x => x.Holding)
                .Include (x => x.Company)
                .Include (x => x.Contact)
                .Include (x => x.FotoUpload)
                .Include (x => x.Interaction)
                .Include (x => x.Category)
                .ToList ();
            return ProductMapping.ListResponseMapping (product, null).OrderByDescending (x => x.Id);
        }

        /// <summary>Get All product with liked indicator by user</summary>
        [HttpGet ("like/user/{id}")]
        [ProducesResponseType (typeof (List<ProductResponse>), 200)]
        [ResponseCache (Location = ResponseCacheLocation.None, NoStore = true)]
        public IEnumerable<ProductResponse> GetAllWithLikedIndicator (int? id) {
            var product = _context.Product
                .Include (x => x.Holding)
                .Include (x => x.Company)
                .Include (x => x.Contact)
                .Include (x => x.FotoUpload)
                .Include (x => x.Interaction)
                .Include (x => x.Category)
                .Where(x => x.IsActive)
                .ToList ();
            return ProductMapping.ListResponseMapping (product, id).OrderByDescending (x => x.Id);
        }

        /// <summary>Get all product created by user</summary>
        [HttpGet ("user/{id}")]
        [ProducesResponseType (typeof (List<ProductResponse>), 200)]

        [ResponseCache (Location = ResponseCacheLocation.None, NoStore = true)]
        public IEnumerable<ProductResponse> GetAllByUser (int? id) {
            if (id == null) return GetAllActive ();
            var product = _context.Product
                .Include (x => x.Holding)
                .Include (x => x.Company)
                .Include (x => x.Contact)
                .Include (x => x.FotoUpload)
                .Include (x => x.Interaction)
                .Include (x => x.Category)
                .Where (x => x.PosterId == id)
                .ToList ();
            return ProductMapping.ListResponseMapping (product, id).OrderByDescending (x => x.Id);
        }

        /// <summary>Get product detail with liked indicator by user</summary>
        [HttpGet ("{id}/user/{userId}")]
        [ProducesResponseType (typeof (ProductDetailResponse), 200)]

        [ResponseCache (Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult GetDetail (int id, int userId) {
            var product = _context.Product
                .Include (x => x.Holding)
                .Include (x => x.Company)
                .Include (x => x.Contact)
                .Include (x => x.Poster)
                .Include (x => x.FotoUpload)
                .Include (x => x.Category)
                .Include (x => x.Interaction)
                .Include (x => x.AttachmentFile)
                .FirstOrDefault (x => x.Id == id);
            var interaction = _context.Interaction.Where (x => x.ProductId == id).Include (x => x.CommentUser).ToList ();
            if (product == null) return NotFound (new Exception ("Product not found"));
            return Ok (new ProductDetailResponse (product, interaction, userId));
        }

        [HttpGet ("{id}")]
        [ProducesResponseType (typeof (ProductDetailResponseWeb), 200)]

        [ResponseCache (Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult GetDetailWebProduct (int id) {
            var product = _context.Product
                .Include (x => x.Holding)
                .Include (x => x.Company)
                .Include (x => x.FotoUpload)
                .Include (x => x.Category)
                .Include (x => x.Interaction)
                .FirstOrDefault (x => x.Id == id);
            var interaction = _context.Interaction.Where (x => x.ProductId == id && x.IsLike == true).ToList ();
            if (product == null) return NotFound (new Exception ("Product not found"));
            return Ok (new ProductDetailResponseWeb (product, interaction.Count()));
        }

        /// <summary>Get editable product request</summary>
        [HttpGet ("{id}/edit")]
        [ProducesResponseType (typeof (ProductRequest), 200)]

        [ResponseCache (Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult GetProductEdit (int id) {
            var product = _context.Product
                .Include (x => x.Holding)
                .Include (x => x.Company)
                .Include (x => x.Contact)
                .Include (x => x.FotoUpload)
                .Include (x => x.Category)
                .Include (x => x.Interaction)
                .Include (x => x.AttachmentFile)
                .FirstOrDefault (x => x.Id == id);
            if (product == null) return NotFound (new Exception ("Product not found"));
            return Ok (new ProductRequest (product));
        }

        /// <summary>Get top 10 of popular product based on interaction</summary>

        [HttpGet ("popular")]
        [ProducesResponseType (typeof (ProductMostPopularResponse), 200)]

        public IActionResult GetPopularProducts () {
            var products = _context.Product.Include(x => x.Interaction).ToList();
            var popularProducts = new List<ProductMostPopular>();
            foreach (var product in products)
            {
                popularProducts.Add(new ProductMostPopular(product));
            }
            ProductMostPopularResponse response = new ProductMostPopularResponse(popularProducts.OrderByDescending(x => x.TotalInteraction).Take(10).ToList());
            return Ok(response);
        }

        [HttpGet ("check_update_status")]
        [ProducesResponseType (200)]
        public async Task<IActionResult> CheckUpdateStatus () {
            var interval = _context.SysParam.FirstOrDefault(x => x.ParamCode == "DEFAULT_PRODUCT_INTERVAL").ParamValue;
            var products = _context.Product
                .Include(x => x.Poster)
                .Where(x => x.IsActive && x.UpdatedDate.AddSeconds(int.Parse(interval)).CompareTo(DateTime.UtcNow) < 1)
                .ToList();
            if (products == null) {
                return Ok();
            }
            foreach(var item in products) {
                // var interval = int.Parse(item.UpdateIntervalInSecond);
                // var dateTimeProduct = item.UpdatedDate;
                // var dateTimeNow = DateTime.UtcNow;
                // dateTimeProduct.AddSeconds(interval);
                // var compare = dateTimeProduct.CompareTo(dateTimeNow);
                item.IsActive = false;
                _context.Product.Update(item);
                NotificationRequest notif = new NotificationRequest () {
                    Body = item.ProductName + " is diactivated, please update your product to make it active again",
                    Title = "Update Product",
                };
                Notification newNotif = new Notification(notif) {
                    To = item.Poster.Id,
                    ModuleId = item.Id,
                    ModuleName = "product",
                    ModuleUseCase = "expired",
                    PushDate = DateTime.UtcNow
                };
                _context.Notification.Add (newNotif);
                if (item.Poster.PushId != null) {
                  NotificationRequestToTopic body = new NotificationRequestToTopic (notif, item.Poster.PushId);
                  body.data.notId = item.Id;
                  string jsonBody = JsonConvert.SerializeObject (body);
                  PushNotificationService.sendPushNotification (jsonBody);
                }
            }
            await _context.SaveChangesAsync();
            return Ok(products.Count() + " product need to be updated");
        }

        /// <summary>Add new product</summary>
        [HttpPost]
        public IActionResult NewProduct ([FromBody] ProductRequest productRequest) {
            if (!ModelState.IsValid) return BadRequest (ModelState);
            if (!productRequest.Foto.Any ()) return BadRequest ("At least 1 image required.");

            var interval = _context.SysParam.FirstOrDefault(x => x.ParamCode.Equals("DEFAULT_PRODUCT_INTERVAL"));
            _context.Product.Add (new Product (productRequest, interval.ParamValue));
            _context.Commit ();

            var productId = _context.Product.LastOrDefault (x =>
                x.ProductName == productRequest.ProductName && x.CategoryId == productRequest.CategoryId &&
                x.ContactId == productRequest.ContactId && x.PosterId == productRequest.PosterId).Id;

            foreach (var foto in productRequest.Foto) {
                _context.FotoUpload.Add (new FotoUpload (foto, productId));
                _context.Commit ();
            }

            foreach (var foto in productRequest.ProductCertificate) {
                _context.FotoUpload.Add (new FotoUpload (foto, productId));
                _context.Commit ();
            }

            foreach (var foto in productRequest.ProductClient) {
                _context.FotoUpload.Add (new FotoUpload (foto, productId));
                _context.Commit ();
            }

            foreach (var foto in productRequest.ProductImplementation) {
                _context.FotoUpload.Add (new FotoUpload (foto, productId));
                _context.Commit ();
            }

            if (productRequest.Attachment.Any ()) {
                foreach (var file in productRequest.Attachment) {
                    _context.AttachmentFile.Add (new AttachmentFile (file, productId));
                    _context.Commit ();
                }
            }

            var body = new NotificationEmptyRequest ();
            var jsonBody = JsonConvert.SerializeObject (body);
            PushNotificationService.sendPushNotification (jsonBody);
            return Ok (new { msg = "Post Published" });
        }

        /// <summary>Update product</summary>
        [HttpPost ("{id}")]
        public IActionResult UpdateProduct ([FromBody] ProductRequest productRequest, int id) {
            if (!ModelState.IsValid) return BadRequest (ModelState);
            if (!productRequest.Foto.Any ()) return BadRequest ("At least 1 image required.");

            var product = _context.Product.FirstOrDefault (x => x.Id == id);
            product = ProductMapping.UpdateProductMapping (productRequest, product);
            _context.Product.Update (product);

            var removedFotoPath = new List<string> ();
            foreach (var foto in productRequest.Foto) {
                if (foto.Id == 0) {
                    _context.FotoUpload.Add (new FotoUpload (foto, id));
                } else {
                    var existingFoto = _context.FotoUpload.FirstOrDefault (x => x.Id == foto.Id);
                    if (foto.FotoPath == null) {
                        removedFotoPath.Add (existingFoto.FotoPath);
                        _context.FotoUpload.Remove (existingFoto);
                    }
                }
            }

            foreach (var deletedFoto in removedFotoPath) {
                var webRootPath = _hostingEnvironment.WebRootPath;

                var request = HttpContext.Request;
                var uriBuilder = new UriBuilder {
                    Host = request.Host.Host,
                    Scheme = request.Scheme
                };
                if (request.Host.Port.HasValue)
                    uriBuilder.Port = request.Host.Port.Value;

                var urlPath = uriBuilder.ToString ();

                var path = deletedFoto.Replace (urlPath, webRootPath + @"\\").Replace ("/", @"\\");

                if (System.IO.File.Exists (path))
                    System.IO.File.Delete (path);
            }

            _context.Commit ();

            if (!productRequest.Attachment.Any ()) return Ok (new { msg = "Post Updated" });
            var removedAttachmentPath = new List<string> ();
            foreach (var attachment in productRequest.Attachment) {
                if (attachment.Id == 0) {
                    _context.AttachmentFile.Add (new AttachmentFile (attachment, id));
                } else {
                    var existingAttachment = _context.AttachmentFile.FirstOrDefault (x => x.Id == attachment.Id);
                    if (attachment.FilePath == null) {
                        removedAttachmentPath.Add (existingAttachment.FilePath);
                        _context.AttachmentFile.Remove (existingAttachment);
                    }
                }
            }

            foreach (var deletedAttachment in removedAttachmentPath) {
                var webRootPath = _hostingEnvironment.WebRootPath;

                var request = HttpContext.Request;
                var uriBuilder = new UriBuilder {
                    Host = request.Host.Host,
                    Scheme = request.Scheme
                };
                if (request.Host.Port.HasValue)
                    uriBuilder.Port = request.Host.Port.Value;

                var urlPath = uriBuilder.ToString ();

                var path = deletedAttachment.Replace (urlPath, webRootPath + @"\\").Replace ("/", @"\\");

                if (System.IO.File.Exists (path))
                    System.IO.File.Delete (path);
            }

            _context.Commit ();
            
            if (!productRequest.ProductCertificate.Any ()) return Ok (new { msg = "Post Updated" });
            var removedCertificatePath = new List<string> ();
            foreach (var certificate in productRequest.ProductCertificate) {
                if (certificate.Id == 0) {
                    _context.FotoUpload.Add (new FotoUpload (certificate, id));
                } else {
                    var existingCertificate = _context.FotoUpload.FirstOrDefault (x => x.Id == certificate.Id);
                    if (certificate.FotoPath == null) {
                        removedCertificatePath.Add (existingCertificate.FotoPath);
                        _context.FotoUpload.Remove (existingCertificate);
                    }
                }
            }

            foreach (var deletedCertificate in removedCertificatePath) {
                var webRootPath = _hostingEnvironment.WebRootPath;

                var request = HttpContext.Request;
                var uriBuilder = new UriBuilder {
                    Host = request.Host.Host,
                    Scheme = request.Scheme
                };
                if (request.Host.Port.HasValue)
                    uriBuilder.Port = request.Host.Port.Value;

                var urlPath = uriBuilder.ToString ();

                var path = deletedCertificate.Replace (urlPath, webRootPath + @"\\").Replace ("/", @"\\");

                if (System.IO.File.Exists (path))
                    System.IO.File.Delete (path);
            }

            _context.Commit ();
            if (!productRequest.ProductClient.Any ()) return Ok (new { msg = "Post Updated" });
            var removedClient = new List<string> ();
            foreach (var client in productRequest.ProductClient) {
                if (client.Id == 0) {
                    _context.FotoUpload.Add (new FotoUpload (client, id));
                } else {
                    var existingClient = _context.FotoUpload.FirstOrDefault (x => x.Id == client.Id);
                    if (client.FotoPath == null) {
                        removedClient.Add (existingClient.FotoPath);
                        _context.FotoUpload.Remove (existingClient);
                    }
                }
            }

            foreach (var deletedClient in removedClient) {
                var webRootPath = _hostingEnvironment.WebRootPath;

                var request = HttpContext.Request;
                var uriBuilder = new UriBuilder {
                    Host = request.Host.Host,
                    Scheme = request.Scheme
                };
                if (request.Host.Port.HasValue)
                    uriBuilder.Port = request.Host.Port.Value;

                var urlPath = uriBuilder.ToString ();

                var path = deletedClient.Replace (urlPath, webRootPath + @"\\").Replace ("/", @"\\");

                if (System.IO.File.Exists (path))
                    System.IO.File.Delete (path);
            }

            _context.Commit ();
            if (!productRequest.ProductImplementation.Any ()) return Ok (new { msg = "Post Updated" });
            var removedProductImplementation = new List<string> ();
            foreach (var implementation in productRequest.ProductImplementation) {
                if (implementation.Id == 0) {
                    _context.FotoUpload.Add (new FotoUpload (implementation, id));
                } else {
                    var existingImplementation = _context.FotoUpload.FirstOrDefault (x => x.Id == implementation.Id);
                    if (implementation.FotoPath == null) {
                        removedProductImplementation.Add (existingImplementation.FotoPath);
                        _context.FotoUpload.Remove (existingImplementation);
                    }
                }
            }

            foreach (var deletedProductImplementation in removedProductImplementation) {
                var webRootPath = _hostingEnvironment.WebRootPath;

                var request = HttpContext.Request;
                var uriBuilder = new UriBuilder {
                    Host = request.Host.Host,
                    Scheme = request.Scheme
                };
                if (request.Host.Port.HasValue)
                    uriBuilder.Port = request.Host.Port.Value;

                var urlPath = uriBuilder.ToString ();

                var path = deletedProductImplementation.Replace (urlPath, webRootPath + @"\\").Replace ("/", @"\\");

                if (System.IO.File.Exists (path))
                    System.IO.File.Delete (path);
            }

            _context.Commit ();

            return Ok (new { msg = "Post Updated" });
        }

        /// <summary>Update product interval</summary>
        [HttpPost ("{id}/interval/{interval}")]
        public async Task<IActionResult> UpdateProductInterval (int? id, string interval) {
            if (id == null) return BadRequest ();
            if (!Regex.IsMatch(interval, @"\d")) {
                return BadRequest();
            }

            var product = await _context.Product.FirstOrDefaultAsync (x => x.Id == id);
            product.UpdateIntervalInSecond = interval;
            _context.Update (product);
            await _context.SaveChangesAsync ();
            return Ok ();
        }

        [HttpPost ("{id}/{actionProduct}")]
        public async Task<IActionResult> updateProductStatus (int? id, string interval, string actionProduct) {
            var listAction = new List<string> ();
            listAction.Add("promote");
            listAction.Add("demote");
            listAction.Add("active");
            listAction.Add("deactive");
            if (id == null || !listAction.Contains(actionProduct)) return BadRequest ();

            var product = await _context.Product.FirstOrDefaultAsync (x => x.Id == id);
            switch (actionProduct)
            {
                case("promote"): {
                    product.IsPromoted = true;
                    break;
                }
                case("demote"): {
                    product.IsPromoted = false;
                    break;
                }
                case("active"): {
                    product.IsActive = true;
                    break;
                }
                case("deactive"): {
                    product.IsActive = false;
                    break;
                }
                default:
                    break;
            }
            _context.Update (product);
            await _context.SaveChangesAsync ();
            return Ok ();
        }

        /// <summary>Delete product</summary>
        [HttpDelete ("{id}")]
        public async Task<IActionResult> DeleteProduct (int? id) {
            if (id == null) return BadRequest ();
            var product = await _context.Product.FirstOrDefaultAsync (x => x.Id == id);
            var appointment = _context.Appointment.Where(x => x.ProductId == id);
            var attachment = _context.AttachmentFile.Where(x => x.ProductId == id);
            var foto = _context.FotoUpload.Where (x => x.ProductId == id);
            var interaction = _context.Interaction.Where (x => x.ProductId == id);
            var notification = _context.Notification.Where(x => x.ModuleName.Equals("product") && x.ModuleId == id);
            _context.Remove (product);
            if (appointment != null) {
                _context.RemoveRange (appointment);
            }
            if (attachment != null) {
                _context.RemoveRange (attachment);
            }
            if (foto != null) {
                _context.RemoveRange (foto);
            }
            if (interaction != null) {
                _context.RemoveRange (interaction);
            }
            if (notification != null) {
                _context.RemoveRange (notification);
            }

            var physicalPath = Path.Combine ("assets", "upload", "products", "poster_" + product.PosterId, product.ProductName.ToLower());
            if (Directory.Exists(physicalPath)) 
                Directory.Delete(physicalPath, true);
                
            _context.Commit();
            return Ok (new {msg = "Product Deleted", success = "True"});
        }
    }
}