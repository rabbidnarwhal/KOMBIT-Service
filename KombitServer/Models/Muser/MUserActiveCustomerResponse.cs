using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models {
  public class ActiveCustomerResponse {
    public List<string> Name { get; set; }
    public List<int> TotalLike { get; set; }
    public List<int> TotalComment { get; set; }
    public List<int> TotalView { get; set; }
    public List<int> TotalChat { get; set; }
    public List<int> TotalInteraction { get; set; }
    public ActiveCustomerResponse (List<ActiveCustomer> customers) {
      Name = new List<string> ();
      TotalChat = new List<int> ();
      TotalComment = new List<int> ();
      TotalLike = new List<int> ();
      TotalView = new List<int> ();
      TotalInteraction = new List<int> ();
      foreach (var customer in customers) {
        Name.Add (customer.Name);
        TotalChat.Add (customer.TotalChat);
        TotalComment.Add (customer.TotalComment);
        TotalInteraction.Add (customer.TotalInteraction);
        TotalLike.Add (customer.TotalLike);
        TotalView.Add (customer.TotalView);
      }
    }
  }
}