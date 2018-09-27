using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models {
  public class MUser {
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string PushId { get; set; }
    public string IdNumber { get; set; }
    public int IdType { get; set; }
    public int IdRole { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public int? ProvinsiId { get; set; }
    public int? KabKotaId { get; set; }
    public string AddressKoordinat { get; set; }
    public string Occupation { get; set; }
    public string Handphone { get; set; }
    public string JobTitle { get; set; }
    public string Image { get; set; }
    public int CompanyId { get; set; }

    [ForeignKey ("IdType")]
    public virtual MTypeId Type { get; set; }

    [ForeignKey ("CompanyId")]
    public virtual MCompany Company { get; set; }

    [ForeignKey ("ProvinsiId")]
    public virtual MProvinsi Provinsi { get; set; }

    [ForeignKey ("KabKotaId")]
    public virtual MKabKota KabKota { get; set; }
  }
}