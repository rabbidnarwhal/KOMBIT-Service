using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models
{
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
      RuleFor (x => x.Handphone).NotEmpty ();
      RuleFor (x => x.CompanyId).NotEmpty ();
    }
  }
}