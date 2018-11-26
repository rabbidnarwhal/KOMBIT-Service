using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models {
  public class ActiveSupplierResponse {
    public List<string> Name { get; set; }
    public List<int> TotalProduct { get; set; }
    public ActiveSupplierResponse (List<ActiveSupplier> suppliers) {
      Name = new List<string> ();
      TotalProduct = new List<int> ();
      foreach (var supplier in suppliers) {
        Name.Add (supplier.Name);
        TotalProduct.Add (supplier.TotalProduct);
      }
    }
  }
}