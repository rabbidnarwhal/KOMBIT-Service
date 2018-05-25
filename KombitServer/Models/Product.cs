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
    public ICollection<FotoUpload> FotoUpload { get; set; }

    [ForeignKey ("ProductId")]
    public ICollection<Interaction> Interaction { get; set; }

    [ForeignKey ("CategoryId")]
    public MCategory Category { get; set; }

    public static Product ProductMapping (ProductRequest productRequest)
    {
      var product = new Product ();
      product.HoldingId = productRequest.HoldingId;
      product.CategoryId = productRequest.CategoryId;
      product.CompanyId = productRequest.CompanyId;
      product.Credentials = productRequest.Credentials;
      product.ProductName = productRequest.ProductName;
      product.Description = productRequest.Description;
      product.IsIncludePrice = productRequest.IsIncludePrice;
      product.Price = productRequest.Price;
      product.VideoPath = productRequest.VideoPath;
      product.UserId = productRequest.UserId;
      return product;
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
    public string FotoPath { get; set; }
    public int TotalLike { get; set; }
    public int TotalComment { get; set; }
    public int TotalView { get; set; }
    public int TotalChat { get; set; }
    public Boolean? IsLike { get; set; }
    public Boolean IsIncludePrice { get; set; }
    public Double? Price { get; set; }

    public static ProductResponse FromData (Product entity, int? id)
    {
      if (entity == null)
      {
        return null;
      }
      var response = new ProductResponse ();
      response.Id = entity.Id;
      response.CompanyName = entity.Company.CompanyName;
      response.HoldingName = entity.Holding.HoldingName;
      response.ProductName = entity.ProductName;
      response.CategoryName = entity.Category.Category;
      response.IsIncludePrice = entity.IsIncludePrice;
      response.Price = entity.Price;
      response.TotalLike = entity.Interaction.Count (x => x.IsLike == true);
      response.TotalChat = entity.Interaction.Count (x => x.IsChat == true);
      response.TotalComment = entity.Interaction.Count (x => x.IsComment == true);
      response.TotalView = entity.Interaction.Count (x => x.IsViewed == true);

      if (entity.FotoUpload.LastOrDefault () == null)
        response.FotoPath = null;
      else
        response.FotoPath = entity.FotoUpload.LastOrDefault ().FotoPath;

      if (entity.Interaction.FirstOrDefault (x => x.LikedBy == id) == null)
        response.IsLike = false;
      else
        response.IsLike = entity.Interaction.FirstOrDefault (x => x.LikedBy == id).IsLike;

      return response;
    }

    public static IEnumerable<ProductResponse> FromArray (List<Product> entity, int? id)
    {
      if (entity == null)
      {
        return null;
      }
      List<ProductResponse> products = new List<ProductResponse> ();
      foreach (var item in entity)
      {
        products.Add (ProductResponse.FromData (item, id));
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
    public string FotoPath { get; set; }
    public Boolean? IsLike { get; set; }
    public int TotalLike { get; set; }
    public int TotalComment { get; set; }
    public int TotalView { get; set; }
    public int TotalChat { get; set; }

    public static ProductDetailResponse FromData (Product entity)
    {
      if (entity == null)
      {
        return null;
      }
      var response = new ProductDetailResponse ();
      response.Id = entity.Id;
      response.CompanyName = entity.Company.CompanyName;
      response.HoldingName = entity.Holding.HoldingName;
      response.ProductName = entity.ProductName;
      response.CategoryName = entity.Category.Category;
      response.Description = entity.Description;
      response.IsIncludePrice = entity.IsIncludePrice;
      response.Price = entity.Price;
      response.Credentials = entity.Credentials;
      response.VideoPath = entity.VideoPath;
      response.TotalLike = entity.Interaction.Count (x => x.IsLike == true);
      response.TotalChat = entity.Interaction.Count (x => x.IsChat == true);
      response.TotalComment = entity.Interaction.Count (x => x.IsComment == true);
      response.TotalView = entity.Interaction.Count (x => x.IsViewed == true);

      if (entity.FotoUpload.LastOrDefault () == null)
        response.FotoPath = null;
      else
        response.FotoPath = entity.FotoUpload.LastOrDefault ().FotoPath;

      if (entity.Interaction.FirstOrDefault (x => x.LikedBy == entity.UserId) == null)
        response.IsLike = false;
      else
        response.IsLike = entity.Interaction.FirstOrDefault (x => x.LikedBy == entity.UserId).IsLike;

      return response;
    }
  }

}