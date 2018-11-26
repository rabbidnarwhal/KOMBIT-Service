using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KombitServer.Models {
  public class NotificationRequestData {
    public string title { get; set; }
    public string body { get; set; }
    public string style { get; set; }
    public string summaryText { get; set; }
    public int notId { get; set; }
  }

}