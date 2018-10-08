using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models {
  public partial class Product {
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
    public int PosterId { get; set; }
    public int UserId { get; set; }

    [ForeignKey ("PosterId")]
    public MUser Poster { get; set; }

    [ForeignKey ("UserId")]
    public MUser User { get; set; }

    [ForeignKey ("HoldingId")]
    public MHolding Holding { get; set; }

    [ForeignKey ("CompanyId")]
    public MCompany Company { get; set; }

    [ForeignKey ("ProductId")]
    public ICollection<FotoUpload> FotoUpload { get; set; }

    [ForeignKey ("ProductId")]
    public ICollection<AttachmentFile> AttachmentFile { get; set; }

    [ForeignKey ("ProductId")]
    public ICollection<Interaction> Interaction { get; set; }

    [ForeignKey ("CategoryId")]
    public MCategory Category { get; set; }

    public Product () { }
    public Product (ProductRequest productRequest = null) {
      if (productRequest != null) {
        Benefit = productRequest.Benefit;
        BusinessTarget = productRequest.BusinessTarget;
        CategoryId = productRequest.CategoryId;
        CompanyId = productRequest.CompanyId;
        Credentials = productRequest.Credentials;
        Currency = productRequest.Currency;
        Description = productRequest.Description;
        Faq = productRequest.Implementation;
        Feature = productRequest.Feature;
        HoldingId = productRequest.HoldingId;
        Implementation = productRequest.Implementation;
        IsIncludePrice = productRequest.IsIncludePrice;
        PosterId = productRequest.PosterId;
        Price = productRequest.Price;
        ProductName = productRequest.ProductName;
        UserId = productRequest.UserId;
        VideoPath = productRequest.VideoPath;
      }
    }
  }
}