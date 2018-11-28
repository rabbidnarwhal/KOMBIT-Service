using System;
using System.Collections.Generic;

namespace KombitServer.Models {
  public partial class ChangePassword {
    public string UserName { get; set; }
    public string Current { get; set; }
    public string New { get; set; }
  }
}