using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models
{
  public class LoginResponse
  {
    public int Id { get; set; }
    public string Username { get; set; }
    public string IdNumber { get; set; }
    // public string IdType { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    // public string Address { get; set; }
    // public string Occupation { get; set; }
    // public string Handphone { get; set; }
    // public string JobTitle { get; set; }
    // public string CompanyName { get; set; }
    public string Image { get; set; }
    public int CompanyId { get; set; }
    public int HoldingId { get; set; }
    // public string HoldingName { get; set; }

    public static LoginResponse FromData (MUser entity)
    {
      if (entity == null)
      {
        return null;
      }
      var response = new LoginResponse ()
      {
        Id = entity.Id,
        Username = entity.Username,
        IdNumber = entity.IdNumber,
        Name = entity.Name,
        Email = entity.Email,
        CompanyId = entity.Company.Id,
        HoldingId = entity.Company.Holding.Id,
        Image = entity.Image
      };
      return response;
    }
  }

}