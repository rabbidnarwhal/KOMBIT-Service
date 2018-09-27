using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models {
  public class MUserUpdateRequest {
    public string Address { get; set; }
    public string AddressKoordinat { get; set; }
    public int CompanyId { get; set; }
    public string JobTitle { get; set; }
    public string Occupation { get; set; }
    public int? ProvinsiId { get; set; }
    public int? KabKotaId { get; set; }
  }
}