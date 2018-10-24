using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models {
  public class ProductMostPopularResponse {
    public List<string> ProductName { get; set; }
    public List<int> TotalLike { get; set; }
    public List<int> TotalComment { get; set; }
    public List<int> TotalView { get; set; }
    public List<int> TotalChat { get; set; }
    public List<int> TotalInteraction { get; set; }
    public ProductMostPopularResponse (List<ProductMostPopular> products) {
      ProductName = new List<string>();
      TotalChat = new List<int>();
      TotalComment = new List<int>();
      TotalInteraction = new List<int>();
      TotalLike = new List<int>();
      TotalView = new List<int>();
      foreach (var product in products)
      {
        ProductName.Add(product.ProductName);
        TotalChat.Add(product.TotalChat);
        TotalComment.Add(product.TotalComment);
        TotalInteraction.Add(product.TotalInteraction);
        TotalLike.Add(product.TotalLike);
        TotalView.Add(product.TotalView);
      }
    }
  }
}