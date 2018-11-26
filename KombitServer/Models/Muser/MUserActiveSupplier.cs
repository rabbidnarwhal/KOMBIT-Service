using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models {
  public class ActiveSupplier {
    public string Name { get; set; }
    public int TotalProduct { get; set; }
    public ActiveSupplier (MUser user, List<Product> products) {
      Name = user.Name;
      TotalProduct = products.Count (x => x.PosterId == user.Id);
    }
  }
}