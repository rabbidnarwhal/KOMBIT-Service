using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KombitServer.Models;
using KombitServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace KombitServer.Controllers {
  [Route ("api/interaction")]
  public class InteractionController : Controller {
    private readonly KombitDBContext _context;
    public InteractionController (KombitDBContext context) {
      _context = context;
    }

    [HttpPost ("view")]
    public IActionResult Viewed ([FromBody] Interaction interaction) {
      if (interaction.IsViewed == null || interaction.ViewedBy == null || interaction.ProductId == 0) {
        return BadRequest (new Exception ("Invalid View"));
      }
      interaction.ViewedDate = DateTime.Now.ToUniversalTime ();
      _context.Interaction.Add (interaction);
      _context.Commit ();

      return Ok ();
    }

    [HttpPost ("like")]
    public IActionResult Liked ([FromBody] Interaction interaction) {
      if (interaction.IsLike == null || interaction.LikedBy == null || interaction.ProductId == 0) {
        return BadRequest (new Exception ("Invalid Like"));
      }
      interaction.LikedDate = DateTime.Now.ToUniversalTime ();
      var like = _context.Interaction.FirstOrDefault (x => x.LikedBy == interaction.LikedBy && x.ProductId == interaction.ProductId);
      if (like == null) _context.Interaction.Add (interaction);
      else {
        like.IsLike = interaction.IsLike;
        like.LikedBy = interaction.LikedBy;
        like.LikedDate = interaction.LikedDate;
        _context.Interaction.Update (like);
      }
      Product post = _context.Product.Include (x => x.Poster).FirstOrDefault (x => x.Id == interaction.ProductId);

      if (interaction.LikedBy != post.UserId) {

        string likedBy = _context.MUser.FirstOrDefault (x => x.Id == interaction.LikedBy).Name;
        string postOwner = post.Poster.Name;
        string postName = post.ProductName;
        string likeText = interaction.IsLike == true ? "liked" : "unliked";
        NotificationRequest notif = new NotificationRequest () {
          Body = "Hi " + postOwner + ", " + likedBy + " " + likeText + " your post.",
          Title = "Post " + likeText
        };

        Notification newNotif = Notification.newNotificationToUser (notif, post.Poster.Id);
        newNotif.PushDate = DateTime.UtcNow;
        _context.Notification.Add (newNotif);

        if (post.Poster.PushId != null) {
          NotificationRequestToTopic body = NotificationRequestToTopic.initComment (notif, post.Poster.PushId);
          string jsonBody = JsonConvert.SerializeObject (body);
          Utility.sendPushNotification (jsonBody);
        }
      }
      _context.Commit ();
      return Ok ();
    }

    [HttpPost ("comment")]
    public IActionResult Commented ([FromBody] Interaction interaction) {
      if (interaction.IsComment == null || interaction.CommentBy == null || interaction.ProductId == 0) {
        return BadRequest (new Exception ("Invalid Comment"));
      }
      interaction.CommentDate = DateTime.UtcNow;
      _context.Interaction.Add (interaction);

      Product post = _context.Product.Include (x => x.Poster).FirstOrDefault (x => x.Id == interaction.ProductId);
      if (interaction.CommentBy != post.UserId) {

        string commentBy = _context.MUser.FirstOrDefault (x => x.Id == interaction.CommentBy).Name;
        string postOwner = post.Poster.Name;
        string postName = post.ProductName;
        NotificationRequest notif = new NotificationRequest () {
          Body = "Hi " + postOwner + ", " + commentBy + " commented your post.",
          Title = "Post Commented"
        };

        Notification newNotif = Notification.newNotificationToUser (notif, post.Poster.Id);
        newNotif.PushDate = DateTime.UtcNow;
        _context.Notification.Add (newNotif);
        if (post.Poster.PushId != null) {
          NotificationRequestToTopic body = NotificationRequestToTopic.initComment (notif, post.Poster.PushId);
          string jsonBody = JsonConvert.SerializeObject (body);
          Utility.sendPushNotification (jsonBody);
        }
      }
      _context.Commit ();
      return Ok ();
    }

    [HttpPost ("chat")]
    public IActionResult Chatted ([FromBody] Interaction interaction) {
      if (interaction.IsChat == null || interaction.ChatBy == null || interaction.ProductId == 0) {
        return BadRequest (new Exception ("Invalid Chat"));
      }
      interaction.ChatDate = DateTime.Now.ToUniversalTime ();
      _context.Interaction.Add (interaction);
      _context.Commit ();

      return Ok ();
    }

  }

}