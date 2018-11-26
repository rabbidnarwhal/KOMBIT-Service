using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models {
  public class ActiveCustomer {
    public string Name { get; set; }
    public int TotalLike { get; set; }
    public int TotalComment { get; set; }
    public int TotalView { get; set; }
    public int TotalChat { get; set; }
    public int TotalInteraction { get; set; }
    public ActiveCustomer (MUser user, List<Interaction> interactions) {
      Name = user.Name;
      TotalLike = interactions.Count (x => x.IsLike == true && x.LikedBy == user.Id);
      TotalChat = interactions.Count (x => x.IsChat == true && x.ChatBy == user.Id);
      TotalComment = interactions.Count (x => x.IsComment == true && x.CommentBy == user.Id);
      TotalView = interactions.Count (x => x.IsViewed == true && x.ViewedBy == user.Id);
      TotalInteraction = TotalChat + TotalComment + TotalLike;
    }
  }
}