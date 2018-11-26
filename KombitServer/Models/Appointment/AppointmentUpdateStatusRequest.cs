using System;
using System.Collections.Generic;

namespace KombitServer.Models {
  public partial class AppointmentUpdateStatusRequest {
    public string RejectMessage { get; set; }
    public string Status { get; set; }
  }
}