using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KombitServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KombitServer.Controllers {
  [Route ("api/users")]
  public class MUserController : Controller {
    private readonly KombitDBContext _context;
    public MUserController (KombitDBContext context) {
      _context = context;
    }
    /// <summary>Get all user</summary>
    [HttpGet]
    [ProducesResponseType (typeof (MUser), 200)]
    public IEnumerable<MUser> Get () {
      var user = _context.MUser
        .Include (x => x.Company).DefaultIfEmpty ()
        .Include (x => x.Type).DefaultIfEmpty ()
        .Include (x => x.Company.Holding).DefaultIfEmpty ()
        .ToList ();
      return user;
    }

    /// <summary>Get user by user id</summary>
    [HttpGet ("{id}")]
    [ProducesResponseType (typeof (MUserResponse), 200)]
    public IActionResult Get (int id) {
      var user = _context.MUser
        .Include (x => x.Company).DefaultIfEmpty ()
        .Include (x => x.Type).DefaultIfEmpty ()
        .Include (x => x.Company.Holding).DefaultIfEmpty ()
        .FirstOrDefault (x => x.Id == id);
      if (user == null) {
        return NotFound (new Exception ("User not found"));
      }
      MUserResponse response = MUserMapping.ResponseMapping (user);
      if (response.ProvinsiId > 0) {
        response.ProvinsiName = _context.MProvinsi.FirstOrDefault (x => x.Id == response.ProvinsiId).Name;
      }
      if (response.KabKotaId > 0) {
        response.KabKotaName = _context.MKabKota.FirstOrDefault (x => x.Id == response.KabKotaId).Name;
      }
      return Ok (response);
    }
    /// <summary>Get user login from id and id card</summary>
    [HttpGet ("{id}/{idCard}")]
    [ProducesResponseType (typeof (LoginResponse), 200)]
    public IActionResult reAuth (int id, string idCard) {
      if (idCard == null || idCard == "") {
        return BadRequest (new Exception ("Invalid token"));
      }
      var user = _context.MUser
        .Include (x => x.Company)
        .Include (x => x.Type)
        .Include (x => x.Company.Holding)
        .FirstOrDefault (x => x.IdNumber == idCard && x.Id == id);
      if (user == null) {
        return Unauthorized ();
      }
      return Ok (LoginResponse.FromData (user));
    }

    /// <summary>Add new user</summary>
    [HttpPost ("register")]
    public IActionResult Register ([FromBody] RegisterRequest registerRequest) {
      if (!ModelState.IsValid) { return BadRequest (ModelState); }
      var user = _context.MUser.FirstOrDefault (x => x.Username == registerRequest.Username);
      if (user != null) {
        return BadRequest (new Exception ("Username already used"));
      }
      var newUser = RegisterMapping.RequestMapping (registerRequest);
      _context.MUser.Add (newUser);
      _context.Commit ();
      return Ok ();
    }

    /// <summary>User login</summary>
    [ProducesResponseType (typeof (LoginResponse), 200)]
    [HttpPost ("login")]
    public IActionResult Login ([FromBody] LoginRequest loginRequest) {
      if (!ModelState.IsValid) { return BadRequest (ModelState); }
      var user = _context.MUser
        .Include (x => x.Company).DefaultIfEmpty ()
        .Include (x => x.Type).DefaultIfEmpty ()
        .Include (x => x.Company.Holding).DefaultIfEmpty ()
        .FirstOrDefault (x => x.Username == loginRequest.Username && x.Password == loginRequest.Password);
      if (user == null) {
        return NotFound (new Exception ("Username or password is missmatch"));
      }
      return Ok (LoginResponse.FromData (user));
    }

    /// <summary>Update user info</summary>
    [HttpPost ("{id}")]
    public IActionResult UpdateUser ([FromBody] MUser updateUser, int id) {
      var oldUser = _context.MUser
        .FirstOrDefault (x => x.Id == id);
      if (oldUser == null) {
        return NotFound (new Exception ("User not found"));
      }
      oldUser = MUserMapping.UpdatedUserMapping (oldUser, updateUser);
      _context.Update (oldUser);
      _context.Commit ();
      return Ok ();
    }

    /// <summary>Add push notification id</summary>
    [HttpPost ("{id}/subscribe/{pushId}")]
    public IActionResult SubscribePush (int id, string pushId) {
      if (pushId == null || id == 0) {
        return BadRequest ();
      }
      MUser updatedUser = new MUser ();
      updatedUser = _context.MUser
        .FirstOrDefault (x => x.Id == id);
      if (updatedUser == null) {
        return NotFound (new Exception ("User not found"));
      }
      updatedUser.PushId = pushId;
      _context.Update (updatedUser);
      _context.Commit ();
      return Ok ();
    }

    /// <summary>Remove push notification id</summary>
    [HttpPost ("{id}/unsubscribe/")]
    public IActionResult UnsubscribePush (int id) {
      if (id == 0) {
        return BadRequest ();
      }
      MUser updatedUser = new MUser ();
      updatedUser = _context.MUser
        .FirstOrDefault (x => x.Id == id);
      if (updatedUser == null) {
        return Ok ();
      }
      updatedUser.PushId = null;
      _context.Update (updatedUser);
      _context.Commit ();
      return Ok ();
    }

  }

}