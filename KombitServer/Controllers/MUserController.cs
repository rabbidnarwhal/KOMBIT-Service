using System;
using System.Collections.Generic;
using System.IO;
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
    //GET All user
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
    //GET User by id
    [HttpGet ("{id}")]
    public IActionResult Get (int id)
    {
      var user = _context.MUser
        .Include (x => x.Company)
        .Include (x => x.Type)
        .Include (x => x.Company.Holding)
        .FirstOrDefault (x => x.Id == id);
      if (user == null)
      {
        return NotFound (new Exception ("User not found"));
      }
      MUserResponse response = MUserResponse.FromData (user);
      if (response.ProvinsiId > 0)
      {
        response.ProvinsiName = _context.MProvinsi.FirstOrDefault (x => x.Id == response.ProvinsiId).Name;
      }
      if (response.KabKotaId > 0)
      {
        response.KabKotaName = _context.MKabKota.FirstOrDefault (x => x.Id == response.KabKotaId).Name;
      }
      return Ok (response);
    }

    //GET User by matching id & idCard
    [HttpGet ("{id}/{idCard}")]
    public IActionResult reAuth (int id, string idCard)
    {
      if (idCard == null || idCard == "")
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

    //POST register user
    [HttpPost ("register")]
    public IActionResult Register ([FromBody] RegisterRequest registerRequest)
    {
      if (!ModelState.IsValid) { return BadRequest (ModelState); }
      var user = _context.MUser.FirstOrDefault (x => x.Username == registerRequest.Username);
      if (user != null)
      {
        return BadRequest (new Exception ("Username already used"));
      }
      var newUser = RegisterRequest.RegisterMapping (registerRequest);
      _context.MUser.Add (newUser);
      _context.Commit ();
      return Ok ();
    }

    //Post login user
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

    [HttpPost ("{id}")]
    public IActionResult UpdateUser ([FromBody] MUser user, int id)
    {
      var updatedUser = _context.MUser
        .FirstOrDefault (x => x.Id == id);
      if (updatedUser == null)
      {
        return NotFound (new Exception ("User not found"));
      }
      updatedUser = MUser.UserMapping (updatedUser, user);
      _context.Update (updatedUser);
      _context.Commit ();
      return Ok ();
    }

    [HttpPost ("{id}/subscribe/{pushId}")]
    public IActionResult SubscribePush (int id, string pushId)
    {
      if (pushId == null || id == 0)
      {
        return BadRequest ();
      }
      MUser updatedUser = new MUser ();
      updatedUser = _context.MUser
        .FirstOrDefault (x => x.Id == id);
      if (updatedUser == null)
      {
        return NotFound (new Exception ("User not found"));
      }
      updatedUser.PushId = pushId;
      _context.Update (updatedUser);
      _context.Commit ();
      return Ok ();
    }

    [HttpPost ("{id}/unsubscribe/")]
    public IActionResult UnsubscribePush (int id)
    {
      if (id == 0)
      {
        return BadRequest ();
      }
      MUser updatedUser = new MUser ();
      updatedUser = _context.MUser
        .FirstOrDefault (x => x.Id == id);
      if (updatedUser == null)
      {
        return Ok ();
      }
      updatedUser.PushId = null;
      _context.Update (updatedUser);
      _context.Commit ();
      return Ok ();
    }

  }

}