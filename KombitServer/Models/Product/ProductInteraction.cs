using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models
{
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
}