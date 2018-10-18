using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models {
  public partial class ProductMapping {
    public static ProductResponse ResponseMapping (Product product, int? id) {
      if (product == null)
        return null;

      var response = new ProductResponse (product);

      if (product.FotoUpload.FirstOrDefault () == null)
        response.FotoPath = null;
      else
        response.FotoPath = product.FotoUpload.FirstOrDefault ().FotoPath;

      if (product.Interaction.FirstOrDefault (x => x.LikedBy == id) == null)
        response.IsLike = false;
      else
        response.IsLike = product.Interaction.FirstOrDefault (x => x.LikedBy == id).IsLike;

      return response;
    }

    public static IEnumerable<ProductResponse> ListResponseMapping (List<Product> product, int? id) {
      if (product == null) {
        return null;
      }
      List<ProductResponse> products = new List<ProductResponse> ();
      foreach (var item in product) {
        products.Add (ProductMapping.ResponseMapping (item, id));
      }
      return products;
    }

    public static Product UpdateProductMapping (ProductRequest request, Product existing) {
      existing.Benefit = request.Benefit;
      existing.BusinessTarget = request.BusinessTarget;
      existing.Faq = request.Faq;
      existing.Feature = request.Feature;
      existing.Implementation = request.Implementation;
      existing.HoldingId = request.HoldingId;
      existing.CategoryId = request.CategoryId;
      existing.CompanyId = request.CompanyId;
      existing.Credentials = request.Credentials;
      existing.ProductName = request.ProductName;
      existing.Description = request.Description;
      existing.IsIncludePrice = request.IsIncludePrice;
      existing.Currency = request.Currency;
      existing.Price = request.Price;
      existing.VideoPath = request.VideoPath;
      existing.UserId = request.UserId;
      return existing;
    }
    public static ICollection<ProductComment> CommentMapping (ICollection<Interaction> interaction) {
      List<ProductComment> productComment = new List<ProductComment> ();
      if (interaction == null)
        return null;
      foreach (var item in interaction) {
        if (item.IsComment != null)
          productComment.Add (new ProductComment (item));
      }
      return productComment.OrderBy (x => x.CommentDate).ToList ();
    }

    public static ProductInteraction InteractionMapping (ICollection<Interaction> interaction, int? userId) {
      var productInteraction = new ProductInteraction (interaction);
      if (interaction.FirstOrDefault (x => x.LikedBy == userId) != null)
        productInteraction.IsLike = interaction.FirstOrDefault (x => x.LikedBy == userId).IsLike;

      return productInteraction;
    }

  }
}