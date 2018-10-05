using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models {
  public class ProductComment {
    public Boolean? IsComment { get; set; }
    public string CommentBy { get; set; }
    public string Content { get; set; }
    public DateTime? CommentDate { get; set; }
    public ProductComment (Interaction interaction) {
      IsComment = interaction.IsComment;
      CommentBy = interaction.CommentUser.Name;
      CommentDate = interaction.CommentDate;
      Content = interaction.Comment;
    }
  }

}