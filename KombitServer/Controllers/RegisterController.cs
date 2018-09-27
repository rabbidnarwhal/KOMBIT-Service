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
  [Route ("api/register")]
  public class RegisterController : Controller {
    private readonly KombitDBContext _context;
    public RegisterController (KombitDBContext context) {
      _context = context;
    }

    /// <summary>Add new user</summary>
    [HttpPost]
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
  }

}