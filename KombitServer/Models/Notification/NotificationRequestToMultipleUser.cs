using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KombitServer.Models
{
  public class NotificationRequestToMultipleUser
  {
    public string[] registration_ids { get; set; }
    public string priority { get; set; }
    public NotificationRequestData data { get; set; }

    public NotificationRequestToMultipleUser (NotificationRequest push)
    {
      NotificationRequestData data = new NotificationRequestData ()
      {
        title = push.Title,
        body = push.Body,
      };
      
      this.registration_ids = push.To.ToArray ();
      this.priority = "normal";
      this.data = data;
    }
  }
}