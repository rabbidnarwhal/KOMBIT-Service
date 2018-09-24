using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models {
  public class RegisterRequest : IValidatableObject {
    public string Username { get; set; }
    public string Password { get; set; }
    public string IdNumber { get; set; }
    public int IdType { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string AddressKoordinat { get; set; }
    public string Occupation { get; set; }
    public string Handphone { get; set; }
    public string JobTitle { get; set; }
    public int CompanyId { get; set; }
    public int ProvinsiId { get; set; }
    public int KabKotaId { get; set; }

    public string Role { get; set; }

    public IEnumerable<ValidationResult> Validate (System.ComponentModel.DataAnnotations.ValidationContext validationContext) {
      var validator = new RegisterValidator ();
      var result = validator.Validate (this);
      return result.Errors.Select (error => new ValidationResult (error.ErrorMessage, new [] { "errorMessage" }));
    }

    public static MUser RegisterMapping (RegisterRequest registerRequest) {
      var user = new MUser () {
        Username = registerRequest.Username,
        Password = registerRequest.Password,
        IdNumber = registerRequest.IdNumber,
        IdType = registerRequest.IdType,
        Name = registerRequest.Name,
        Email = registerRequest.Email,
        Address = registerRequest.Address,
        AddressKoordinat = registerRequest.AddressKoordinat,
        Occupation = registerRequest.Occupation,
        Handphone = registerRequest.Handphone,
        JobTitle = registerRequest.JobTitle,
        CompanyId = registerRequest.CompanyId,
        ProvinsiId = registerRequest.ProvinsiId,
        KabKotaId = registerRequest.KabKotaId,
        Role = registerRequest.Role
      };
      return user;
    }
  }
}