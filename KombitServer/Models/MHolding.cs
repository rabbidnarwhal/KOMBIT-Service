using System;
using System.Collections.Generic;

namespace KombitServer.Models
{
  public partial class MHolding
  {
    public int Id { get; set; }
    public string HoldingName { get; set; }
    public string Address { get; set; }
    public string AddressKoordinat { get; set; }
    public string FixedCall { get; set; }
  }
}
