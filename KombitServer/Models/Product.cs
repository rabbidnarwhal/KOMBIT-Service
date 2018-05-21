using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace KombitServer.Models
{
  public partial class Product
  {
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int HoldingId { get; set; }
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

  }
  public class ProductResponse
  {
    public int Id { get; set; }
    public string CompanyName { get; set; }
    public string HoldingName { get; set; }
    public string ProductName { get; set; }
    public string CategoryName { get; set; }
    public string PicturePath { get; set; }
    public int Like { get; set; }
    public int Comment { get; set; }
    public int View { get; set; }
    public int Chat { get; set; }

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
          Like = entity.Interaction.Count (x => x.IsLike == true),
          Chat = entity.Interaction.Count (x => x.IsChat == true),
          Comment = entity.Interaction.Count (x => x.IsComment == true),
          View = entity.Interaction.Count (x => x.IsViewed == true),
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