using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models {
  public class ProductContact {
    public string Name { get; set; }
    public string Email { get; set; }
    public string JobTitle { get; set; }
    public string Occupation { get; set; }
    public string Handphone { get; set; }
    public string Address { get; set; }
    public string AddressKoordinat { get; set; }
    public string Image { get; set; }
    public ProductContact (MUser user) {
      Name = user.Name;
      Email = user.Email;
      JobTitle = user.JobTitle;
      Occupation = user.Occupation;
      Handphone = user.Handphone;
      Address = user.Address;
      Image = user.Image;
      AddressKoordinat = user.AddressKoordinat;
    }
  }
}