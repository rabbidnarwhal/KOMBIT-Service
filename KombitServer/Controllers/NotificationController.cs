﻿using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using KombitServer.Models;
using KombitServer.Services.PushNotification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace KombitServer.Controllers {
  [Route ("api/notification")]
  public class NotificationController : Controller {

    private readonly KombitDBContext _context;
    public NotificationController (KombitDBContext context) {
      _context = context;
    }

    [HttpGet ("user/{id}")]
    public IActionResult GetNotification (int? id) {
      if (id == null || id == 0) {
        return BadRequest ();
      }
      var notif = _context.Notification.Where (x => x.Topic == "combits" || x.To == id).ToList ().OrderByDescending (x => x.Id);
      foreach (var item in notif.Where (x => x.IsRead == false).ToList ()) {
        item.IsRead = true;
        _context.Notification.Update (item);
      }
      _context.Commit ();
      return Ok (notif);
    }

    [HttpGet ("user/{id}/unread")]
    public IActionResult GetNotificationUnreadCount (int? id) {
      if (id == null || id == 0) {
        return BadRequest ();
      }
      var notifCount = _context.Notification.Where (x => x.To == id && x.IsRead == false).ToList ().Count;
      return Ok (new { unRead = notifCount });
    }

    [HttpPost ("topics/{topic}")]
    public IActionResult NotificationToTopics ([FromBody] NotificationRequest notif, string topic) {
      var newNotif = new Notification (notif) {
        Topic = topic,
        PushDate = DateTime.UtcNow
      };
      _context.Notification.Add (newNotif);
      _context.Commit ();

      var toTopic = "/topics/" + topic;

      NotificationRequestToTopic body = new NotificationRequestToTopic (notif, toTopic);
      string jsonBody = JsonConvert.SerializeObject (body);
      return Ok (PushNotificationService.sendPushNotification (jsonBody));

    }

    [HttpPost]
    public IActionResult NotificationToUser ([FromBody] NotificationRequest notif) {
      try {
        if (notif.To == null || notif.To.Count == 0)
          return BadRequest ();
      } catch {
        return BadRequest ();
      }
      notif.To.ForEach (item => {
        var userId = _context.MUser.FirstOrDefault (x => x.PushId == item).Id;
        var newNotif = new Notification (notif) {
          To = userId,
          PushDate = DateTime.UtcNow
        };
        _context.Notification.Add (newNotif);
      });

      _context.Commit ();
      NotificationRequestToMultipleUser body = new NotificationRequestToMultipleUser (notif);
      string jsonBody = JsonConvert.SerializeObject (body);
      return Ok (PushNotificationService.sendPushNotification (jsonBody));
    }

  }

}