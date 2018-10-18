using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KombitServer.Models {
  public partial class Notification {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Topic { get; set; }
    public int? To { get; set; }
    public Boolean IsRead { get; set; }
    public DateTime PushDate { get; set; }

    public static Notification newNotificationToTopic (NotificationRequest notif, string topic) {
      var newNotif = new Notification () {
        Title = notif.Title,
        Content = notif.Body,
        Topic = topic
      };
      return newNotif;
    }
    public static Notification newNotificationToUser (NotificationRequest notif, int userId) {
      var newNotif = new Notification () {
        Title = notif.Title,
        Content = notif.Body,
        To = userId,
      };
      return newNotif;
    }

  }
}