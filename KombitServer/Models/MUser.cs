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
    public string AddressKoordinat { get; set; }
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
      var user = new MUser ()
      {
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
      };
      return user;
    }
  }

  public class MUserResponse
  {
    public int Id { get; set; }
    public string Username { get; set; }
    public string IdNumber { get; set; }
    public string IdType { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string AddressKoordinat { get; set; }
    public string Occupation { get; set; }
    public string Handphone { get; set; }
    public string JobTitle { get; set; }
    public string CompanyCoordinate { get; set; }
    public string CompanyFixedCall { get; set; }
    public string CompanyAddress { get; set; }
    public string CompanyName { get; set; }
    public int CompanyId { get; set; }
    public int HoldingId { get; set; }
    public string HoldingName { get; set; }

    public static MUserResponse FromData (MUser entity)
    {
      if (entity == null)
      {
        return null;
      }
      var response = new MUserResponse ()
      {
        Id = entity.Id,
        Username = entity.Username,
        IdNumber = entity.IdNumber,
        IdType = entity.Type.DescType,
        Name = entity.Name,
        Email = entity.Email,
        Address = entity.Address,
        AddressKoordinat = entity.AddressKoordinat,
        Occupation = entity.Occupation,
        Handphone = entity.Handphone,
        JobTitle = entity.JobTitle,
        CompanyCoordinate = entity.Company.AddressKoordinat,
        CompanyFixedCall = entity.Company.FixedCall,
        CompanyAddress = entity.Company.Address,
        CompanyName = entity.Company.CompanyName,
        CompanyId = entity.Company.Id,
        HoldingName = entity.Company.Holding.HoldingName,
        HoldingId = entity.Company.Holding.Id,
      };
      return response;
    }
  }

}