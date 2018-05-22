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
    public List<FotoUpload> FotoUpload { get; set; }

    [ForeignKey ("ProductId")]
    public ICollection<Interaction> Interaction { get; set; }

    [ForeignKey ("CategoryId")]
    public ICollection<MCategory> Category { get; set; }

    public static Product ProductMapping (ProductRequest productRequest)
    {
      return new Product
      {
        HoldingId = productRequest.HoldingId,
          CategoryId = productRequest.CategoryId,
          CompanyId = productRequest.CompanyId,
          Credentials = productRequest.Credentials,
          ProductName = productRequest.ProductName,
          Description = productRequest.Description,
          IsIncludePrice = productRequest.IsIncludePrice,
          Price = productRequest.Price,
          VideoPath = productRequest.VideoPath,
          UserId = productRequest.UserId
      };
    }

  }

  public partial class ProductRequest : IValidatableObject
  {
    public int CompanyId { get; set; }
    public int HoldingId { get; set; }
    public int CategoryId { get; set; }
    public string ProductName { get; set; }
    public string Description { get; set; }
    public Boolean IsIncludePrice { get; set; }
    public double? Price { get; set; }
    public string Credentials { get; set; }
    public string VideoPath { get; set; }
    public string FotoName { get; set; }
    public string FotoPath { get; set; }
    public int UserId { get; set; }

    public IEnumerable<ValidationResult> Validate (System.ComponentModel.DataAnnotations.ValidationContext validationContext)
    {
      var validator = new ProductValidator ();
      var result = validator.Validate (this);
      return result.Errors.Select (error => new ValidationResult (error.ErrorMessage, new [] { "errorMessage" }));
    }
  }

  public class ProductValidator : AbstractValidator<ProductRequest>
  {
    public ProductValidator ()
    {
      RuleFor (x => x.CategoryId).NotEmpty ();
      RuleFor (x => x.CompanyId).NotEmpty ();
      RuleFor (x => x.Description).NotEmpty ();
      RuleFor (x => x.FotoName).NotEmpty ();
      RuleFor (x => x.FotoPath).NotEmpty ();
      RuleFor (x => x.HoldingId).NotEmpty ();
      RuleFor (x => x.IsIncludePrice).NotEmpty ();
      RuleFor (x => x.ProductName).NotEmpty ();
      RuleFor (x => x.UserId).NotEmpty ();
    }
  }

  public class ProductResponse
  {
    public int Id { get; set; }
    public string CompanyName { get; set; }
    public string HoldingName { get; set; }
    public string ProductName { get; set; }
    public string CategoryName { get; set; }
    public string PicturePath { get; set; }
    public int TotalLike { get; set; }
    public int TotalComment { get; set; }
    public int TotalView { get; set; }
    public int TotalChat { get; set; }

    public static ProductResponse FromData (Product entity)
    {
      if (entity == null)
      {
        return null;
      }
      return new ProductResponse
      {
        Id = entity.Id,
          CompanyName = entity.Company.CompanyName,
          HoldingName = entity.Holding.HoldingName,
          ProductName = entity.ProductName,
          CategoryName = null,
          PicturePath = entity.FotoUpload[0].FotoPath,
          TotalLike = entity.Interaction.Count (x => x.IsLike == true),
          TotalChat = entity.Interaction.Count (x => x.IsChat == true),
          TotalComment = entity.Interaction.Count (x => x.IsComment == true),
          TotalView = entity.Interaction.Count (x => x.IsViewed == true),
      };
    }

    public static IEnumerable<ProductResponse> FromArray (Product[] entity)
    {
      if (entity == null)
      {
        return null;
      }
      List<ProductResponse> products = new List<ProductResponse> ();
      foreach (var item in entity)
      {
        products.Add (ProductResponse.FromData (item));
      }
      return products;
    }
  }
  public class ProductDetailResponse
  {
    public int Id { get; set; }
    public string CompanyName { get; set; }
    public string HoldingName { get; set; }
    public string ProductName { get; set; }
    public string CategoryName { get; set; }
    public string Description { get; set; }
    public Boolean IsIncludePrice { get; set; }
    public double? Price { get; set; }
    public string Credentials { get; set; }
    public string VideoPath { get; set; }
    public string PicturePath { get; set; }
    public string PictureName { get; set; }

    public static ProductDetailResponse FromData (Product entity)
    {
      if (entity == null)
      {
        return null;
      }
      return new ProductDetailResponse
      {
        Id = entity.Id,
          CompanyName = entity.Company.CompanyName,
          HoldingName = entity.Holding.HoldingName,
          ProductName = entity.ProductName,
          CategoryName = null,
          Description = entity.Description,
          IsIncludePrice = entity.IsIncludePrice,
          Price = entity.Price,
          Credentials = entity.Credentials,
          VideoPath = entity.VideoPath,
          PictureName = entity.FotoUpload[0].FotoName,
          PicturePath = entity.FotoUpload[0].FotoPath
      };
    }
  }

}