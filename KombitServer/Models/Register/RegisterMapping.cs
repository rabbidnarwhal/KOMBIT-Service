using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models {
  public class RegisterMapping {

    public static MUser RequestMapping (RegisterRequest registerRequest) {
      var user = new MUser () {
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
        ProvinsiId = registerRequest.ProvinsiId,
        KabKotaId = registerRequest.KabKotaId,
        IdRole = registerRequest.IdRole
      };
      return user;
    }

    public static RegisterResponse ResponseMapping (MUser entity) {
      if (entity == null) {
        return null;
      }
      var response = new RegisterResponse () {
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
        CompanyName = entity.Company.CompanyName
      };
      return response;
    }
  }
}