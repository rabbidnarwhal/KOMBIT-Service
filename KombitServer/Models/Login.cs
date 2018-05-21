using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models
{
  public class LoginRequest : IValidatableObject
  {
    public string Username { get; set; }

    public string Password { get; set; }

    public IEnumerable<ValidationResult> Validate (System.ComponentModel.DataAnnotations.ValidationContext validationContext)
    {
      var validator = new LoginValidator ();
      var result = validator.Validate (this);
      return result.Errors.Select (error => new ValidationResult (error.ErrorMessage, new [] { "errorMessage" }));
    }
  }

  public class LoginValidator : AbstractValidator<LoginRequest>
  {
    public LoginValidator ()
    {
      RuleFor (x => x.Username).NotEmpty ();
      RuleFor (x => x.Password).NotEmpty ();
    }
  }

  public class LoginResponse
  {
    public int Id { get; set; }
    public string Username { get; set; }
    public string IdNumber { get; set; }
    public string IdType { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string Occupation { get; set; }
    public string Handphone { get; set; }
    public string JobTitle { get; set; }
    public string CompanyName { get; set; }

    public static LoginResponse FromData (MUser entity)
    {
      if (entity == null)
      {
        return null;
      }
      return new LoginResponse
      {
        Id = entity.Id,
          Username = entity.Username,
          IdNumber = entity.IdNumber,
          IdType = entity.Type.DescType,
          Name = entity.Name,
          Email = entity.Email,
          Address = entity.Address,
          Occupation = entity.Occupation,
          Handphone = entity.Handphone,
          JobTitle = entity.JobTitle,
          CompanyName = entity.Company.CompanyName
      };
    }
  }

}