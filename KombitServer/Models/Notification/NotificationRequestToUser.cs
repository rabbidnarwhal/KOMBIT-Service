using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KombitServer.Models
{
  public class NotificationRequestToUser
  {
    public string[] registration_ids { get; set; }
    public string priority { get; set; }
    public NotificationRequestData data { get; set; }

    public static NotificationRequestToUser From (NotificationRequest push)
    {
      NotificationRequestData data = new NotificationRequestData ()
      {
        title = push.Title,
        body = push.Body,
        style = "inbox",
        summaryText = "There are %n% notifications"
      };

      NotificationRequestToUser request = new NotificationRequestToUser ()
      {
        registration_ids = push.To.ToArray (),
        priority = "normal",
        data = data
      };
      return request;
    }
  }
}