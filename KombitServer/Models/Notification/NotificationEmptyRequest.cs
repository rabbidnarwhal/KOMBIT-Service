using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KombitServer.Models
{
  public class NotificationEmptyRequest
  {
    public string to { get; set; }
    public string priority { get; set; }
    public object data { get; set; }

    public static NotificationEmptyRequest Init ()
    {
      NotificationEmptyRequest request = new NotificationEmptyRequest ()
      {
        to = "/topics/combits",
        priority = "normal",
        data = new { newPost = "True" }
      };
      return request;
    }
  }
}