using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

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
    public Boolean IsPromoted { get; set; }
    public Boolean IsIncludePrice { get; set; }
    public string Currency { get; set; }
    public double? Price { get; set; }
    public string Credentials { get; set; }
    public string VideoPath { get; set; }
    public FotoUpload[] Foto { get; set; }
    public AttachmentFile[] Attachment { get; set; }
    public string ContactName { get; set; }
    public string ContactHandphone { get; set; }
    public string ContactEmail { get; set; }
    public int UserId { get; set; }

    public ProductRequest (Product product) {
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
      VideoPath = product.VideoPath;
      Foto = product.FotoUpload.ToArray ();
    }
    public IEnumerable<ValidationResult> Validate (System.ComponentModel.DataAnnotations.ValidationContext validationContext) {
      var validator = new ProductValidator ();
      var result = validator.Validate (this);
      return result.Errors.Select (error => new ValidationResult (error.ErrorMessage, new [] { "errorMessage" }));
    }
  }
}