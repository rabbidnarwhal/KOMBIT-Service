using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;
// using FluentValidation.Results;

namespace KombitServer.Models
{
  public class MUser
  {
    public int Id { get; set; }
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

    [ForeignKey ("IdType")]
    public MTypeId Type { get; set; }

    [ForeignKey ("CompanyId")]
    public MCompany Company { get; set; }

    public static MUser RegisterMapping (RegisterRequest registerRequest)
    {
      return new MUser
      {
        Username = registerRequest.Username,
          Password = registerRequest.Password,
          IdNumber = registerRequest.IdNumber,
          IdType = registerRequest.IdType,
          Name = registerRequest.Name,
          Email = registerRequest.Email,
          Address = registerRequest.Address,
          Occupation = registerRequest.Occupation,
          Handphone = registerRequest.Handphone,
          JobTitle = registerRequest.JobTitle,
          CompanyId = registerRequest.CompanyId,
      };
    }
  }

  public class MUserResponse { }

}