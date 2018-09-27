using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;

namespace KombitServer.Models {
  public class MUserMapping {
    public static MUserResponse ResponseMapping (MUser entity) {
      if (entity == null) {
        return null;
      }
      var response = new MUserResponse () {
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
        Image = entity.Image,
        ProvinsiId = entity.ProvinsiId,
        KabKotaId = entity.KabKotaId,
      };
      return response;
    }

    public static MUser UpdatedUserMapping (MUser oldUser, MUser updateUser) {
      oldUser.Address = updateUser.Address;
      oldUser.AddressKoordinat = updateUser.AddressKoordinat;
      oldUser.CompanyId = updateUser.CompanyId;
      oldUser.JobTitle = updateUser.JobTitle;
      oldUser.Occupation = updateUser.Occupation;
      oldUser.ProvinsiId = updateUser.ProvinsiId;
      oldUser.KabKotaId = updateUser.KabKotaId;
      return oldUser;
    }
  }

}