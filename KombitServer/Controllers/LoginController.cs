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
  [Route ("api/login")]
  public class LoginController : Controller {
    private readonly KombitDBContext _context;
    public LoginController (KombitDBContext context) {
      _context = context;
    }
    /// <summary>User login</summary>
    [ProducesResponseType (typeof (LoginResponse), 200)]
    [HttpPost]
    public IActionResult Login ([FromBody] LoginRequest loginRequest) {
      if (!ModelState.IsValid) { return BadRequest (ModelState); }
      var user = _context.MUser
        .Include (x => x.Company).DefaultIfEmpty ()
        .Include (x => x.Type).DefaultIfEmpty ()
        .Include (x => x.Company.Holding).DefaultIfEmpty ()
        .FirstOrDefault (x => x.Username == loginRequest.Username && x.Password == loginRequest.Password);
      if (user == null) {
        return NotFound (new Exception ("Username or password is not found"));
      }
      return Ok (LoginResponse.FromData (user));
    }
  }
}