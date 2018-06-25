using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KombitServer.Models
{
  public class PushNotification
  {
    public string Title { get; set; }
    public string Body { get; set; }
    public List<string> To { get; set; }
  }
  public class PushNotificationRequestToTopic
  {
    public string to { get; set; }
    public string priority { get; set; }
    public PushNotificationRequestData data { get; set; }

    public static PushNotificationRequestToTopic init (PushNotification push, string topic)
    {
      PushNotificationRequestData data = new PushNotificationRequestData ()
      {
        title = push.Title,
        body = push.Body,
        style = "inbox",
        summaryText = "There are %n% notifications"
      };

      PushNotificationRequestToTopic request = new PushNotificationRequestToTopic ()
      {
        to = "/topics/" + topic,
        priority = "normal",
        data = data
      };
      return request;
    }
  }
  public class PushNotificationRequestToUser
  {
    public string[] to { get; set; }
    public string priority { get; set; }
    public PushNotificationRequestData data { get; set; }

    public static PushNotificationRequestToUser From (PushNotification push)
    {
      PushNotificationRequestData data = new PushNotificationRequestData ()
      {
        title = push.Title,
        body = push.Body,
        style = "inbox",
        summaryText = "There are %n% notifications"
      };

      PushNotificationRequestToUser request = new PushNotificationRequestToUser ()
      {
        to = push.To.ToArray (),
        priority = "normal",
        data = data
      };
      return request;
    }
  }
  public class PushNotificationRequestData
  {
    public string title { get; set; }
    public string body { get; set; }
    public string style { get; set; }
    public string summaryText { get; set; }
  }

}