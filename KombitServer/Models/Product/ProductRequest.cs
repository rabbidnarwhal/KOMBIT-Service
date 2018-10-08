using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KombitServer.Models {
    public class ProductRequest : IValidatableObject {
        public ProductRequest (Product product) {
            if (product == null)
                return;

            CategoryId = product.CategoryId;
            Credentials = product.Credentials;
            CompanyId = product.CompanyId;
            Description = product.Description;
            HoldingId = product.HoldingId;
            IsIncludePrice = product.IsIncludePrice;
            Currency = product.Currency;
            Price = product.Price;
            ProductName = product.ProductName;
            UserId = product.UserId;
            PosterId = product.PosterId;
            VideoPath = product.VideoPath;
            BusinessTarget = product.BusinessTarget;
            Feature = product.Feature;
            Benefit = product.Benefit;
            Implementation = product.Implementation;
            Faq = product.Faq;
            IsPromoted = product.IsPromoted;

            Foto = product.FotoUpload.ToArray ();
            Attachment = product.AttachmentFile.ToArray ();
        }

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
        public string VideoPath { get; set; }
        public FotoUpload[] Foto { get; set; }
        public AttachmentFile[] Attachment { get; set; }
        public int UserId { get; set; }
        public int PosterId { get; set; }

        public IEnumerable<ValidationResult> Validate (ValidationContext validationContext) {
            var validator = new ProductValidator ();
            var result = validator.Validate (this);
            return result.Errors.Select (error => new ValidationResult (error.ErrorMessage, new [] { "errorMessage" }));
        }
    }
}