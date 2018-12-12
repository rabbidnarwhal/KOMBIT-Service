using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models {
  public class ProductDetailResponseWeb {
    public int Id { get; set; }
    public string CompanyName { get; set; }
    public string HoldingName { get; set; }
    public string ProductName { get; set; }
    public string CategoryName { get; set; }
    public string Description { get; set; }
    public Boolean IsIncludePrice { get; set; }
    public string FotoPath { get; set; }
    public string Currency { get; set; }
    public double? Price { get; set; }
    public int Like {get; set;}

    public ProductDetailResponseWeb (Product product, int count) {
      Id = product.Id;
      CompanyName = product.Company.CompanyName;
      HoldingName = product.Holding.HoldingName;
      ProductName = product.ProductName;
      CategoryName = product.Category.Category;
      Description = product.Description;
      IsIncludePrice = product.IsIncludePrice;
      FotoPath = product.FotoUpload.First().FotoPath;
      Currency = product.Currency;
      Price = product.Price;
      Like = count;
    }
  }
}