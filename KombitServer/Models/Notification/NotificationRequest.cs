using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KombitServer.Models
{
  public class NotificationRequest
  {
    public string Title { get; set; }
    public string Body { get; set; }
    public List<string> To { get; set; }
  }
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
  }
  public class NotificationRequestToUser
  {
    public string[] to { get; set; }
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
        to = push.To.ToArray (),
        priority = "normal",
        data = data
      };
      return request;
    }
  }
  public class NotificationRequestData
  {
    public string title { get; set; }
    public string body { get; set; }
    public string style { get; set; }
    public string summaryText { get; set; }
  }

}