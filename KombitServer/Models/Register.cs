using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models
{
  public class RegisterRequest : IValidatableObject
  {
    public string Username { get; set; }
    public string Password { get; set; }
    public string IdNumber { get; set; }
    public int IdType { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string Occupation { get; set; }
    public string Handphone { get; set; }
    public string JobTitle { get; set; }
    public int CompanyId { get; set; }

    public IEnumerable<ValidationResult> Validate (System.ComponentModel.DataAnnotations.ValidationContext validationContext)
    {
      var validator = new RegisterValidator ();
      var result = validator.Validate (this);
      return result.Errors.Select (error => new ValidationResult (error.ErrorMessage, new [] { "errorMessage" }));
    }
  }

  public class RegisterValidator : AbstractValidator<RegisterRequest>
  {
    public RegisterValidator ()
    {
      RuleFor (x => x.Username).NotEmpty ();
      RuleFor (x => x.Password).NotEmpty ();
      RuleFor (x => x.IdNumber).NotEmpty ();
      RuleFor (x => x.IdType).NotEmpty ();
      RuleFor (x => x.Name).NotEmpty ();
      RuleFor (x => x.Email).NotEmpty ();
      RuleFor (x => x.Address).NotEmpty ();
      RuleFor (x => x.Occupation).NotEmpty ();
      RuleFor (x => x.Handphone).NotEmpty ();
      RuleFor (x => x.JobTitle).NotEmpty ();
      RuleFor (x => x.CompanyId).NotEmpty ();
    }
  }

  public class RegisterResponse
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

    public static RegisterResponse FromData (MUser entity)
    {
      if (entity == null)
      {
        return null;
      }
      var response = new RegisterResponse ();
      response.Id = entity.Id;
      response.Username = entity.Username;
      response.IdNumber = entity.IdNumber;
      response.IdType = entity.Type.DescType;
      response.Name = entity.Name;
      response.Email = entity.Email;
      response.Address = entity.Address;
      response.Occupation = entity.Occupation;
      response.Handphone = entity.Handphone;
      response.JobTitle = entity.JobTitle;
      response.CompanyName = entity.Company.CompanyName;
      return response;
    }
  }

}