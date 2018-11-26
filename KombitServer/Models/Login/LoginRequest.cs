using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models {
  public class LoginRequest : IValidatableObject {
    public string Username { get; set; }

    public string Password { get; set; }

    public IEnumerable<ValidationResult> Validate (System.ComponentModel.DataAnnotations.ValidationContext validationContext) {
      var validator = new LoginValidator ();
      var result = validator.Validate (this);
      return result.Errors.Select (error => new ValidationResult (error.ErrorMessage, new [] { "errorMessage" }));
    }
  }
}