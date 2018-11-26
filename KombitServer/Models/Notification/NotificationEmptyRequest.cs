using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KombitServer.Models {
  public class NotificationEmptyRequest {
    public string to { get; set; }
    public string priority { get; set; }
    public object data { get; set; }

    public NotificationEmptyRequest () {
      this.to = "/topics/combits";
      this.priority = "normal";
      this.data = new { newPost = "True" };
    }
  }
}