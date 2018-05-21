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
  [Route ("api/users")]
  public class MUserController : Controller
  {
    private readonly KombitDBContext _context;
    public MUserController (KombitDBContext context)
    {
      _context = context;
    }
    // GET api/user
    [HttpGet]
    public IEnumerable<MUser> Get ()
    {
      var user = _context.MUser.Include (c => c.Company).Include (t => t.Type).ToList ();
      return user;
    }

    // GET api/user/5
    [HttpGet ("{id}")]
    public IActionResult Get (int? id)
    {
      if (id == null)
      {
        return BadRequest ();
      }
      var user = _context.MUser.Include (c => c.Company).Include (t => t.Type).FirstOrDefault (x => x.Id == id);
      if (user == null)
      {
        return NotFound ();
      }
      return Ok (user);
    }

    // POST api/user
    [HttpPost]
    public IActionResult Post ([FromBody] MUser user)
    {
      _context.MUser.Add (user);
      _context.SaveChanges ();
      return Ok ();
    }

    // PUT api/user/5
    [HttpPut ("{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Put (int? id, [Bind ("Id, Address, JobTitle")] MUser user)
    {
      if (id == null)
      {
        return BadRequest ();
      }
      else if (id != user.Id)
      {
        return NotFound ();
      }
      _context.MUser.Update (user);
      _context.SaveChanges ();
      return Ok ();
    }

    // DELETE api/user/5
    [HttpDelete ("{id}")]
    public IActionResult Delete (int? id)
    {
      if (id == null)
      {
        return BadRequest ();
      }
      var user = _context.MUser.FirstOrDefault (x => x.Id == id);
      if (user == null)
      {
        return NotFound ();
      }
      _context.MUser.Remove (user);
      _context.SaveChanges ();
      return Ok ();
    }
  }
}