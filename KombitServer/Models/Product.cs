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
      var product = new Product ()
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
      RuleFor (x => x.Description).NotEmpty ().Length (0, 255).WithMessage ("'Description' cannot be more than 250 characters.");
      RuleFor (x => x.Credentials).Length (0, 255).WithMessage ("Success story cannot be more than 250 characters.");
      RuleFor (x => x.FotoName).NotEmpty ();
      RuleFor (x => x.FotoPath).NotEmpty ();
      RuleFor (x => x.HoldingId).NotEmpty ();
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
    public ProductContact Contact { get; set; }
    public ProductInteraction Interaction { get; set; }
    public static ProductDetailResponse FromData (Product entity, ICollection<Interaction> interaction)
    {
      if (entity == null)
      {
        return null;
      }
      var response = new ProductDetailResponse ()
      {
        Id = entity.Id,
        CompanyName = entity.Company.CompanyName,
        HoldingName = entity.Holding.HoldingName,
        ProductName = entity.ProductName,
        CategoryName = entity.Category.Category,
        Description = entity.Description,
        IsIncludePrice = entity.IsIncludePrice,
        Price = entity.Price,
        Credentials = entity.Credentials,
        VideoPath = entity.VideoPath,
        FotoPath = null,
        Contact = ProductContact.FromData (entity.User),
        Interaction = ProductInteraction.FromData (interaction, entity.UserId),
      };

      if (entity.FotoUpload.LastOrDefault () != null)
        response.FotoPath = entity.FotoUpload.LastOrDefault ().FotoPath;

      return response;
    }
  }

  public class ProductContact
  {
    public string Name { get; set; }
    public string Email { get; set; }
    public string JobTitle { get; set; }
    public string Occupation { get; set; }
    public string Handphone { get; set; }
    public string Address { get; set; }
    public string Foto { get; set; }

    public static ProductContact FromData (MUser user)
    {
      var productContact = new ProductContact ()
      {
        Name = user.Name,
        Email = user.Email,
        JobTitle = user.JobTitle,
        Occupation = user.Occupation,
        Handphone = user.Handphone,
        Address = user.Address,
        Foto = "",
      };
      return productContact;
    }

  }

  public class ProductInteraction
  {
    public Boolean? IsLike { get; set; }
    public int TotalLike { get; set; }
    public int TotalComment { get; set; }
    public int TotalView { get; set; }
    public int TotalChat { get; set; }
    public ICollection<ProductComment> Comment { get; set; }
    public static ProductInteraction FromData (ICollection<Interaction> interaction, int? userId)
    {
      var productInteraction = new ProductInteraction ()
      {
        TotalLike = interaction.Count (x => x.IsLike == true),
        TotalChat = interaction.Count (x => x.IsChat == true),
        TotalComment = interaction.Count (x => x.IsComment == true),
        TotalView = interaction.Count (x => x.IsViewed == true),
        IsLike = false,
        Comment = ProductComment.FromData (interaction.Where (x => x.IsComment == true).ToList ()),
      };
      if (interaction.FirstOrDefault (x => x.LikedBy == userId) != null)
        productInteraction.IsLike = interaction.FirstOrDefault (x => x.LikedBy == userId).IsLike;

      return productInteraction;
    }
  }

  public class ProductComment
  {
    public Boolean? IsComment { get; set; }
    public string CommentBy { get; set; }
    public string Content { get; set; }
    public DateTime? CommentDate { get; set; }

    public static ICollection<ProductComment> FromData (ICollection<Interaction> interaction)
    {
      List<ProductComment> productComment = new List<ProductComment> ();
      if (interaction == null)
        return null;

      foreach (var item in interaction)
      {
        if (item.IsComment != null)
        {

          var comment = new ProductComment ()
          {
          IsComment = item.IsComment,
          CommentBy = item.CommentUser.Name,
          CommentDate = item.CommentDate,
          Content = item.Comment,
          };

          productComment.Add (comment);
        }
      }

      return productComment;
    }
  }

}