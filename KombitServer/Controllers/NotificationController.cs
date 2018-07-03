using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using KombitServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;

namespace KombitServer.Controllers
{
  [Route ("api/notification")]
  public class NotificationController : Controller
  {

    private readonly KombitDBContext _context;
    public NotificationController (KombitDBContext context)
    {
      _context = context;
    }

    [HttpGet ("user/{id}")]
    public IActionResult GetNotification (int? id)
    {
      if (id == null || id == 0)
      {
        return BadRequest ();
      }
      var notif = _context.Notification.Where (x => x.Topic == "combits" || x.To == id).ToList ().OrderByDescending (x => x.Id);
      return Ok (notif);
    }

    [HttpPost ("topics/{topic}")]
    public IActionResult NotificationToTopics ([FromBody] NotificationRequest notif, string topic)
    {
      var newNotif = Notification.newNotificationToTopic (notif, topic);
      newNotif.PushDate = DateTime.UtcNow;
      _context.Notification.Add (newNotif);
      _context.Commit ();

      NotificationRequestToTopic body = NotificationRequestToTopic.init (notif, topic);
      string jsonBody = JsonConvert.SerializeObject (body);
      return Ok (Push (jsonBody));
    }

    [HttpPost ("topics/{topic}/nopush")]
    public IActionResult NotificationToTopicsWithoutPush ([FromBody] NotificationRequest notif, string topic)
    {
      var newNotif = Notification.newNotificationToTopic (notif, topic);
      newNotif.PushDate = DateTime.UtcNow;
      _context.Notification.Add (newNotif);
      _context.Commit ();
      return Ok ();
    }

    [HttpPost]
    public IActionResult NotificationToUser ([FromBody] NotificationRequest notif)
    {
      try
      {
        if (notif.To == null || notif.To.Count == 0)
          return BadRequest ();
      }
      catch
      {
        return BadRequest ();
      }
      notif.To.ForEach (item =>
      {
        var userId = _context.MUser.FirstOrDefault (x => x.PushId == item).Id;
        var newNotif = Notification.newNotificationToUser (notif, userId);
        newNotif.PushDate = DateTime.UtcNow;
        _context.Notification.Add (newNotif);
      });

      _context.Commit ();
      NotificationRequestToUser body = NotificationRequestToUser.From (notif);
      string jsonBody = JsonConvert.SerializeObject (body);

      return Ok (Push (jsonBody));
    }

    [HttpPost ("nopush")]
    public IActionResult NotificationToUserWithoutPush ([FromBody] NotificationRequest notif)
    {
      try
      {
        if (notif.To == null || notif.To.Count == 0)
          return BadRequest ();
      }
      catch
      {
        return BadRequest ();
      }
      notif.To.ForEach (item =>
      {
        var userId = _context.MUser.FirstOrDefault (x => x.PushId == item).Id;
        var newNotif = Notification.newNotificationToUser (notif, userId);
        newNotif.PushDate = DateTime.UtcNow;
        _context.Notification.Add (newNotif);
      });
      _context.Commit ();
      return Ok ();
    }

    private string Push (string jsonBody)
    {
      string key = "AIzaSyDgpko0W1nEYN0vtAONSwrVTYux5ngsZHk";
      string url = "https://fcm.googleapis.com/fcm/send";

      RestClient client = new RestClient (url);
      RestRequest request = new RestRequest ();

      request.Method = Method.POST;
      request.AddHeader ("Content-Type", "application/json");
      request.AddHeader ("Authorization", "key=" + key);
      request.AddParameter ("application/json", jsonBody, ParameterType.RequestBody);
      IRestResponse response = client.Execute (request);
      return response.Content;
    }

  }

}