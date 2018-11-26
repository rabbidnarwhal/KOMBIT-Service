using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models {
  public class LoginValidator : AbstractValidator<LoginRequest> {
    public LoginValidator () {
      RuleFor (x => x.Username).NotEmpty ();
      RuleFor (x => x.Password).NotEmpty ();
    }
  }
}