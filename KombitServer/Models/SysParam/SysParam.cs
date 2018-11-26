using System;
using System.Collections.Generic;

namespace KombitServer.Models {
  public partial class SysParam {
    public int Id { get; set; }
    public string ParamCode { get; set; }
    public string ParamValue { get; set; }
    public string Description { get; set; }
  }
}