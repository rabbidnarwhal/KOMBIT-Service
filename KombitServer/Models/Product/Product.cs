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
    public Boolean IsActive { get; set; }
    public Boolean PosterAsContact { get; set; }
    public string Currency { get; set; }
    public double? Price { get; set; }
    public string Credentials { get; set; }
    public string Certificate { get; set; }
    public string VideoPath { get; set; }
    public int PosterId { get; set; }
    public int ContactId { get; set; }
    public string ContactName { get; set; }
    public string ContactHandphone { get; set; }
    public string ContactEmail { get; set; }
    public string UpdateIntervalInSecond { get; set; }
    public DateTime UpdatedDate { get; set; }

    [ForeignKey ("PosterId")]
    public MUser Poster { get; set; }

    [ForeignKey ("ContactId")]
    public MUser Contact { get; set; }

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
    public Product (ProductRequest productRequest, string inteval) {
      if (productRequest != null) {
        Benefit = productRequest.Benefit;
        BusinessTarget = productRequest.BusinessTarget;
        CategoryId = productRequest.CategoryId;
        CompanyId = productRequest.CompanyId;
        Credentials = productRequest.Credentials;
        Certificate = productRequest.Certificate;
        Currency = productRequest.Currency;
        Description = productRequest.Description;
        Faq = productRequest.Faq;
        Feature = productRequest.Feature;
        HoldingId = productRequest.HoldingId;
        Implementation = productRequest.Implementation;
        IsIncludePrice = productRequest.IsIncludePrice;
        PosterId = productRequest.PosterId;
        Price = productRequest.Price;
        ProductName = productRequest.ProductName;
        ContactId = productRequest.ContactId;
        VideoPath = productRequest.VideoPath;
        ContactEmail = productRequest.ContactEmail;
        ContactName = productRequest.ContactName;
        ContactHandphone = productRequest.ContactHandphone;
        PosterAsContact = productRequest.PosterAsContact;
        UpdatedDate = DateTime.Now.ToUniversalTime ();
        IsActive = true;
        UpdateIntervalInSecond = inteval;
      }
    }
  }
}