using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KombitServer.Models {
    public class ProductRequest : IValidatableObject {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int HoldingId { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string BusinessTarget { get; set; }
        public string Feature { get; set; }
        public string Benefit { get; set; }
        public string Implementation { get; set; }
        public string Faq { get; set; }
        public bool IsPromoted { get; set; }
        public bool IsIncludePrice { get; set; }
        public string Currency { get; set; }
        public double? Price { get; set; }
        public string Credentials { get; set; }
        public string Certificate { get; set; }
        public string VideoPath { get; set; }
        public FotoUpload[] Foto { get; set; }
        public FotoUpload[] ProductImplementation { get; set; }
        public FotoUpload[] ProductCertificate { get; set; }
        public FotoUpload[] ProductClient { get; set; }
        public AttachmentFile[] Attachment { get; set; }
        public int ContactId { get; set; }
        public int PosterId { get; set; }
        public string ContactName { get; set; }
        public string ContactHandphone { get; set; }
        public string ContactEmail { get; set; }

        public ProductRequest (Product product) {
            if (product == null)
                return;

            CategoryId = product.CategoryId;
            Credentials = product.Credentials;
            Certificate = product.Certificate;
            CompanyId = product.CompanyId;
            Description = product.Description;
            HoldingId = product.HoldingId;
            IsIncludePrice = product.IsIncludePrice;
            Currency = product.Currency;
            Price = product.Price;
            ProductName = product.ProductName;
            ContactId = product.ContactId;
            PosterId = product.PosterId;
            VideoPath = product.VideoPath;
            BusinessTarget = product.BusinessTarget;
            Feature = product.Feature;
            Benefit = product.Benefit;
            Implementation = product.Implementation;
            Faq = product.Faq;
            IsPromoted = product.IsPromoted;
            ContactName = product.ContactName;
            ContactHandphone = product.ContactHandphone;

            Foto = product.FotoUpload.Where(x => x.UseCase.Equals("foto")).ToArray ();
            ProductCertificate = product.FotoUpload.Where(x => x.UseCase.Equals("certificate")).ToArray ();
            ProductClient = product.FotoUpload.Where(x => x.UseCase.Equals("client")).ToArray ();
            ProductImplementation = product.FotoUpload.Where(x => x.UseCase.Equals("implementationImage") || x.UseCase.Equals("implementationVideo")).ToArray ();
            Attachment = product.AttachmentFile.ToArray ();
        }
        public IEnumerable<ValidationResult> Validate (ValidationContext validationContext) {
            var validator = new ProductValidator ();
            var result = validator.Validate (this);
            return result.Errors.Select (error => new ValidationResult (error.ErrorMessage, new [] { "errorMessage" }));
        }
    }
}