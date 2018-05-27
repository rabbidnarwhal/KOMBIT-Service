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
    //GET api / user
    [HttpGet]
    public IEnumerable<MUser> Get ()
    {
      var user = _context.MUser
        .Include (x => x.Company)
        .Include (x => x.Type)
        .Include (x => x.Company.Holding)
        .ToList ();
      return user;
    }

    [HttpGet ("{id}")]
    public IActionResult Get (int? id)
    {
      if (id == null)
      {
        return BadRequest ();
      }
      var user = _context.MUser
        .Include (x => x.Company)
        .Include (x => x.Type)
        .Include (x => x.Company.Holding)
        .FirstOrDefault (x => x.Id == id);
      if (user == null)
      {
        return NotFound (new Exception ("User not found"));
      }
      return Ok (MUserResponse.FromData (user));
    }

    [HttpGet ("{id}/{idCard}")]
    public IActionResult reAuth (int? id, string idCard)
    {
      if (id == null || idCard == null || idCard == "")
      {
        return BadRequest (new Exception ("Invalid token"));
      }
      var user = _context.MUser
        .Include (x => x.Company)
        .Include (x => x.Type)
        .Include (x => x.Company.Holding)
        .FirstOrDefault (x => x.IdNumber == idCard && x.Id == id);
      if (user == null)
      {
        return Unauthorized ();
      }
      return Ok (LoginResponse.FromData (user));
    }

    [HttpPost ("register")]
    public IActionResult Register ([FromBody] RegisterRequest registerRequest)
    {
      if (!ModelState.IsValid) { return BadRequest (ModelState); }
      var user = _context.MUser.FirstOrDefault (x => x.Username == registerRequest.Username);
      if (user != null)
      {
        return BadRequest (new Exception ("Username already used"));
      }
      var newUser = MUser.RegisterMapping (registerRequest);
      _context.MUser.Add (newUser);
      _context.Commit ();
      return Ok ();
    }

    [HttpPost ("login")]
    public IActionResult Login ([FromBody] LoginRequest loginRequest)
    {
      if (!ModelState.IsValid) { return BadRequest (ModelState); }
      var user = _context.MUser
        .Include (x => x.Company)
        .Include (x => x.Type)
        .Include (x => x.Company.Holding)
        .FirstOrDefault (x => x.Username == loginRequest.Username && x.Password == loginRequest.Password);
      if (user == null)
      {
        return NotFound (new Exception ("Username or password is missmatch"));
      }
      return Ok (LoginResponse.FromData (user));
    }
  }

}