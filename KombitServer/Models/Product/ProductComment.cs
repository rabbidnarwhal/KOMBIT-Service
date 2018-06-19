using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models
{
  public class ProductComment
  {
    public Boolean? IsComment { get; set; }
    public string CommentBy { get; set; }
    public string Content { get; set; }
    public DateTime? CommentDate { get; set; }

    public static ICollection<ProductComment> FromData (ICollection<Interaction> interaction)
    {
      List<ProductComment> productComment = new List<ProductComment> ();
      if (interaction == null)
        return null;

      foreach (var item in interaction)
      {
        if (item.IsComment != null)
        {

          var comment = new ProductComment ()
          {
          IsComment = item.IsComment,
          CommentBy = item.CommentUser.Name,
          CommentDate = item.CommentDate,
          Content = item.Comment,
          };

          productComment.Add (comment);
        }
      }

      return productComment;
    }
  }

}