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

    public static Product UpdateProductMapping (ProductRequest request, Product response) {
      response.HoldingId = request.HoldingId;
      response.CategoryId = request.CategoryId;
      response.CompanyId = request.CompanyId;
      response.Credentials = request.Credentials;
      response.ProductName = request.ProductName;
      response.Description = request.Description;
      response.IsIncludePrice = request.IsIncludePrice;
      response.Currency = request.Currency;
      response.Price = request.Price;
      response.VideoPath = request.VideoPath;
      response.UserId = request.UserId;
      return response;
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