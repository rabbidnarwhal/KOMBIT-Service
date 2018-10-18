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
      var response = new MUserResponse ();

      response.Id = entity.Id;
      response.Username = entity.Username;
      response.IdNumber = entity.IdNumber;
      response.IdType = entity.Type?.DescType;
      response.Name = entity.Name;
      response.Email = entity.Email;
      response.Address = entity.Address;
      response.AddressKoordinat = entity.AddressKoordinat;
      response.Occupation = entity.Occupation;
      response.Handphone = entity.Handphone;
      response.JobTitle = entity.JobTitle;
      response.CompanyCoordinate = entity.Company?.AddressKoordinat;
      response.CompanyFixedCall = entity.Company?.FixedCall;
      response.CompanyAddress = entity.Company?.Address;
      response.CompanyName = entity.Company?.CompanyName;
      response.CompanyId = entity.Company?.Id;
      response.HoldingName = entity.Company?.Holding.HoldingName;
      response.HoldingId = entity.Company?.Holding.Id;
      response.Image = entity.Image;
      response.ProvinsiId = entity.ProvinsiId;
      response.KabKotaId = entity.KabKotaId;
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