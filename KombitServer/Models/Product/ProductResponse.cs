using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models {
  public class ProductResponse {
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
    public int? Provinsi { get; set; }
    public int? KabKota { get; set; }
    public string Position { get; set; }
    public Boolean? IsLike { get; set; }
    public Boolean IsPromoted { get; set; }
    public Boolean IsIncludePrice { get; set; }
    public string Currency { get; set; }
    public Double? Price { get; set; }
    public ProductResponse (Product product) {
      Id = product.Id;
      CompanyName = product.Company.CompanyName;
      HoldingName = product.Holding.HoldingName;
      HoldingId = product.Holding.Id;
      ProductName = product.ProductName;
      CategoryName = product.Category.Category;
      IsIncludePrice = product.IsIncludePrice;
      IsPromoted = product.IsPromoted;
      Currency = product.Currency;
      Price = product.Price;
      TotalLike = product.Interaction.Count (x => x.IsLike == true);
      TotalChat = product.Interaction.Count (x => x.IsChat == true);
      TotalComment = product.Interaction.Count (x => x.IsComment == true);
      TotalView = product.Interaction.Count (x => x.IsViewed == true);
      UserId = product.UserId;
      Position = product.User.AddressKoordinat;
      Provinsi = product.User.ProvinsiId;
      KabKota = product.User.KabKotaId;
    }
  }
}