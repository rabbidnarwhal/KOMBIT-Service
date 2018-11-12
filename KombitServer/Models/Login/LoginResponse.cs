using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models {
  public class LoginResponse {
    public int Id { get; set; }
    public string Username { get; set; }
    public string IdNumber { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Image { get; set; }
    public int? CompanyId { get; set; }
    public int? HoldingId { get; set; }
    public string Role { get; set; }
    public string PhoneNumber { get; set; }

    public static LoginResponse FromData (MUser entity) {
      if (entity == null) {
        return null;
      }
      var response = new LoginResponse () {
        Id = entity.Id,
        Username = entity.Username,
        IdNumber = entity.IdNumber,
        Name = entity.Name,
        Email = entity.Email,
        Image = entity.Image,
        PhoneNumber = entity.Handphone
      };
      if (entity.IdRole == 1) {
        response.Role = "Customer";
      } else if (entity.IdRole == 2) {
        response.Role = "Supplier";
        response.CompanyId = entity.Company?.Id;
        response.HoldingId = entity.Company?.Holding.Id;
      } else if (entity.IdRole == 3) {
        response.Role = "Administrator";
      }
      return response;
    }
  }

}