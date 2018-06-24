using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KombitServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KombitServer.Controllers
{
  [Route ("api/interaction")]
  public class InteractionController : Controller
  {
    private readonly KombitDBContext _context;
    public InteractionController (KombitDBContext context)
    {
      _context = context;
    }

    [HttpPost ("view")]
    public IActionResult Viewed ([FromBody] Interaction interaction)
    {
      if (interaction.IsViewed == null || interaction.ViewedBy == null || interaction.ProductId == 0)
      {
        return BadRequest (new Exception ("Invalid View"));
      }
      interaction.ViewedDate = DateTime.Now.ToUniversalTime ();
      _context.Interaction.Add (interaction);
      _context.Commit ();

      return Ok ();
    }

    [HttpPost ("like")]
    public IActionResult Liked ([FromBody] Interaction interaction)
    {
      if (interaction.IsLike == null || interaction.LikedBy == null || interaction.ProductId == 0)
      {
        return BadRequest (new Exception ("Invalid Like"));
      }
      interaction.LikedDate = DateTime.Now.ToUniversalTime ();
      var like = _context.Interaction.FirstOrDefault (x => x.LikedBy == interaction.LikedBy && x.ProductId == interaction.ProductId);
      if (like == null) _context.Interaction.Add (interaction);
      else
      {
        like.IsLike = interaction.IsLike;
        like.LikedBy = interaction.LikedBy;
        like.LikedDate = interaction.LikedDate;
        _context.Interaction.Update (like);
      }
      _context.Commit ();

      return Ok ();
    }

    [HttpPost ("comment")]
    public IActionResult Commented ([FromBody] Interaction interaction)
    {
      if (interaction.IsComment == null || interaction.CommentBy == null || interaction.ProductId == 0)
      {
        return BadRequest (new Exception ("Invalid Comment"));
      }
      interaction.CommentDate = DateTime.UtcNow;
      _context.Interaction.Add (interaction);
      _context.Commit ();

      return Ok ();
    }

    [HttpPost ("chat")]
    public IActionResult Chatted ([FromBody] Interaction interaction)
    {
      if (interaction.IsChat == null || interaction.ChatBy == null || interaction.ProductId == 0)
      {
        return BadRequest (new Exception ("Invalid Chat"));
      }
      interaction.ChatDate = DateTime.Now.ToUniversalTime ();
      _context.Interaction.Add (interaction);
      _context.Commit ();

      return Ok ();
    }

  }

}