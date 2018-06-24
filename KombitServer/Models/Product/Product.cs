using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models
{
  public partial class Product
  {
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int HoldingId { get; set; }
    public int CategoryId { get; set; }
    public string ProductName { get; set; }
    public string Description { get; set; }
    public Boolean IsIncludePrice { get; set; }
    public string Currency { get; set; }
    public double? Price { get; set; }
    public string Credentials { get; set; }
    public string VideoPath { get; set; }
    public int UserId { get; set; }

    [ForeignKey ("UserId")]
    public MUser User { get; set; }

    [ForeignKey ("HoldingId")]
    public MHolding Holding { get; set; }

    [ForeignKey ("CompanyId")]
    public MCompany Company { get; set; }

    [ForeignKey ("ProductId")]
    public ICollection<FotoUpload> FotoUpload { get; set; }

    [ForeignKey ("ProductId")]
    public ICollection<Interaction> Interaction { get; set; }

    [ForeignKey ("CategoryId")]
    public MCategory Category { get; set; }

    public static Product NewProductMapping (ProductRequest productRequest)
    {
      var product = new Product ()
      {
        HoldingId = productRequest.HoldingId,
        CategoryId = productRequest.CategoryId,
        CompanyId = productRequest.CompanyId,
        Credentials = productRequest.Credentials,
        ProductName = productRequest.ProductName,
        Description = productRequest.Description,
        IsIncludePrice = productRequest.IsIncludePrice,
        Currency = productRequest.Currency,
        Price = productRequest.Price,
        VideoPath = productRequest.VideoPath,
        UserId = productRequest.UserId
      };
      return product;
    }
    public static Product UpdateProductMapping (Product product, ProductRequest productRequest)
    {
      product.HoldingId = productRequest.HoldingId;
      product.CategoryId = productRequest.CategoryId;
      product.CompanyId = productRequest.CompanyId;
      product.Credentials = productRequest.Credentials;
      product.ProductName = productRequest.ProductName;
      product.Description = productRequest.Description;
      product.IsIncludePrice = productRequest.IsIncludePrice;
      product.Currency = productRequest.Currency;
      product.Price = productRequest.Price;
      product.VideoPath = productRequest.VideoPath;
      product.UserId = productRequest.UserId;
      return product;
    }
  }
}