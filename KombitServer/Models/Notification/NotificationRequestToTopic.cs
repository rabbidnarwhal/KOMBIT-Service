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

    public static NotificationRequestToTopic init (NotificationRequest push, string topic)
    {
      NotificationRequestData data = new NotificationRequestData ()
      {
        title = push.Title,
        body = push.Body,
        style = "inbox",
        summaryText = "There are %n% notifications"
      };

      NotificationRequestToTopic request = new NotificationRequestToTopic ()
      {
        to = "/topics/" + topic,
        priority = "normal",
        data = data
      };
      return request;
    }

    public static NotificationRequestToTopic initComment (NotificationRequest notif, string to)
    {
      NotificationRequestData data = new NotificationRequestData ()
      {
        title = notif.Title,
        body = notif.Body,
      };

      NotificationRequestToTopic request = new NotificationRequestToTopic ()
      {
        to = to,
        priority = "normal",
        data = data
      };
      return request;
    }
  }

}