using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KombitServer.Models
{
  public partial class Interaction
  {
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Boolean? IsLike { get; set; }
    public int? LikedBy { get; set; }
    public DateTime? LikedDate { get; set; }
    public Boolean? IsComment { get; set; }
    public int? CommentBy { get; set; }
    public DateTime? CommentDate { get; set; }
    public Boolean? IsChat { get; set; }
    public int? ChatBy { get; set; }
    public DateTime? ChatDate { get; set; }
    public Boolean? IsViewed { get; set; }
    public int? ViewedBy { get; set; }
    public DateTime? ViewedDate { get; set; }

    [ForeignKey ("ProductId")]
    public Product Product { get; set; }

    [ForeignKey ("CommentBy")]
    public MUser CommentUser { get; set; }
  }
}