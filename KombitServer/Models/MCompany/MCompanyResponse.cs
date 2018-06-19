using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KombitServer.Models
{
  public class CompanyResponse
  {
    public int Id { get; set; }
    public string CompanyName { get; set; }
    public string HoldingName { get; set; }
    public string Address { get; set; }
    public string AddressKoordinat { get; set; }
    public string FixedCall { get; set; }
    public string Image { get; set; }

    public static CompanyResponse FromData (MCompany entity)
    {
      if (entity == null)
      {
        return null;
      }
      return new CompanyResponse
      {
        Id = entity.Id,
          CompanyName = entity.CompanyName,
          HoldingName = entity.Holding.HoldingName,
          Address = entity.Address,
          AddressKoordinat = entity.AddressKoordinat,
          Image = entity.Image,
          FixedCall = entity.FixedCall
      };
    }
  }
}