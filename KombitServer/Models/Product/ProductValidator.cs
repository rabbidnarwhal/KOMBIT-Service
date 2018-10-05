using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models {
  public class ProductValidator : AbstractValidator<ProductRequest> {
    public ProductValidator () {
      RuleFor (x => x.CategoryId).NotEmpty ();
      RuleFor (x => x.CompanyId).NotEmpty ();
      RuleFor (x => x.Description).NotEmpty ().WithMessage ("'Description' cannot be more than 250 characters.");
      RuleFor (x => x.Foto.Length).GreaterThan (0).WithMessage ("At least 1 photo is required");
      RuleFor (x => x.HoldingId).NotEmpty ();
      RuleFor (x => x.ProductName).NotEmpty ();
      RuleFor (x => x.UserId).NotEmpty ();
    }
  }
}