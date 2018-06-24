using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models
{
  public class ProductResponse
  {
    public int Id { get; set; }
    public string CompanyName { get; set; }
    public int HoldingId { get; set; }
    public string HoldingName { get; set; }
    public string ProductName { get; set; }
    public string CategoryName { get; set; }
    public string FotoPath { get; set; }
    public int TotalLike { get; set; }
    public int TotalComment { get; set; }
    public int TotalView { get; set; }
    public int TotalChat { get; set; }
    public int UserId { get; set; }
    public Boolean? IsLike { get; set; }
    public Boolean IsIncludePrice { get; set; }
    public string Currency { get; set; }
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
      response.HoldingId = entity.Holding.Id;
      response.ProductName = entity.ProductName;
      response.CategoryName = entity.Category.Category;
      response.IsIncludePrice = entity.IsIncludePrice;
      response.Currency = entity.Currency;
      response.Price = entity.Price;
      response.TotalLike = entity.Interaction.Count (x => x.IsLike == true);
      response.TotalChat = entity.Interaction.Count (x => x.IsChat == true);
      response.TotalComment = entity.Interaction.Count (x => x.IsComment == true);
      response.TotalView = entity.Interaction.Count (x => x.IsViewed == true);
      response.UserId = entity.UserId;

      if (entity.FotoUpload.FirstOrDefault () == null)
        response.FotoPath = null;
      else
        response.FotoPath = entity.FotoUpload.FirstOrDefault ().FotoPath;

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
}