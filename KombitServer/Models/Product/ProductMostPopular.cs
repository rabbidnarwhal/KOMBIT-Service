using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models {
  public class ProductMostPopular {
    public string ProductName { get; set; }
    public int TotalLike { get; set; }
    public int TotalComment { get; set; }
    public int TotalView { get; set; }
    public int TotalChat { get; set; }
    public int TotalInteraction { get; set; }
    public ProductMostPopular (Product product) {
      ProductName = product.ProductName;
      TotalLike = product.Interaction.Count (x => x.IsLike == true);
      TotalChat = product.Interaction.Count (x => x.IsChat == true);
      TotalComment = product.Interaction.Count (x => x.IsComment == true);
      TotalView = product.Interaction.Count (x => x.IsViewed == true);
      TotalInteraction = TotalLike + TotalComment + TotalChat;
    }
  }
}