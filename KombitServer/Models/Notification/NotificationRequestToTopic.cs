using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KombitServer.Models
{
  public class NotificationRequestToTopic
  {
    public string to { get; set; }
    public string priority { get; set; }
    public NotificationRequestData data { get; set; }
    public NotificationRequestToTopic (NotificationRequest request, string to)
    {
      NotificationRequestData data = new NotificationRequestData ()
      {
        title = request.Title,
        body = request.Body,
      };

      this.to = to;
      this.priority = "normal";
      this.data = data;
    }
  }

}