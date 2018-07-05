using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models
{
  public class ProductDetailResponse
  {
    public int Id { get; set; }
    public string CompanyName { get; set; }
    public string HoldingName { get; set; }
    public string ProductName { get; set; }
    public string CategoryName { get; set; }
    public string Description { get; set; }
    public Boolean IsIncludePrice { get; set; }
    public string Currency { get; set; }
    public double? Price { get; set; }
    public string Credentials { get; set; }
    public string VideoPath { get; set; }
    public IEnumerable<FotoUpload> Foto { get; set; }
    public ProductContact Contact { get; set; }
    public ProductInteraction Interaction { get; set; }
    public static ProductDetailResponse FromData (Product product, ICollection<Interaction> interaction, int userId)
    {
      if (product == null)
      {
        return null;
      }
      var response = new ProductDetailResponse ()
      {
        Id = product.Id,
        CompanyName = product.Company.CompanyName,
        HoldingName = product.Holding.HoldingName,
        ProductName = product.ProductName,
        CategoryName = product.Category.Category,
        Description = product.Description,
        IsIncludePrice = product.IsIncludePrice,
        Currency = product.Currency,
        Price = product.Price,
        Credentials = product.Credentials,
        VideoPath = product.VideoPath,
        Foto = product.FotoUpload,
        Contact = ProductContact.FromData (product.User),
        Interaction = ProductInteraction.FromData (interaction, userId),
      };
      return response;
    }
  }
}