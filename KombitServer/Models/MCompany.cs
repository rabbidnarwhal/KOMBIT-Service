using System;
using System.Collections.Generic;

namespace KombitServer.Models
{
  public partial class MCompany
  {
    public int Id { get; set; }
    public int HoldingId { get; set; }
    public string CompanyName { get; set; }
    public string Address { get; set; }
    public string AddressKoordinat { get; set; }
    public string FixedCall { get; set; }
  }
}