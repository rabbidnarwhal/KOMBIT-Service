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
    public double? Price { get; set; }
    public string Credentials { get; set; }
    public string VideoPath { get; set; }
    public IEnumerable<FotoUpload> Foto { get; set; }
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
        Foto = entity.FotoUpload,
        Contact = ProductContact.FromData (entity.User),
        Interaction = ProductInteraction.FromData (interaction, entity.UserId),
      };
      return response;
    }
  }
}