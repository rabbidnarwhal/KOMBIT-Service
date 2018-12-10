using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models {
  public class MUserContactResponse {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Handphone { get; set; }
    public MUserContactResponse (MUser user) {
      this.Id = user.Id;
      this.Name = user.Name;
      this.Handphone = user.Handphone;
    }
  }
}