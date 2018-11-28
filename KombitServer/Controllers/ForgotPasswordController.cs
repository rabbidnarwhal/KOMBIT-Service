using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KombitServer.Models;
using KombitServer.Models.Email;
using KombitServer.Services.Email;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KombitServer.Controllers {
  [Route ("api/forgot-password")]
  public class ForgotPasswordController : Controller {
    private readonly KombitDBContext _context;
    private readonly IEmailService _emailService;
    public ForgotPasswordController (KombitDBContext context, IEmailService emailService) {
      _context = context;
      _emailService = emailService;
    }
    /// <summary>Reset password</summary>
    [ProducesResponseType (200)]
    [HttpPost]
    public IActionResult ResetPassword ([FromBody] ForgotPassword request) {
      var user = _context.MUser
        .FirstOrDefault (x => x.Email == request.Email);
      if (user == null) {
        return NotFound (new Exception ("Email not found!"));
      }
      // var mask = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
      var mask = "0123456789";
      var newPassword = "";
      Random random = new Random();
      for (int i = 0; i < 8; i++)
      {
          var index = random.Next(mask.Length);
          newPassword += mask[index];
      }

      user.Password = newPassword;
      _context.Update(user);
      _context.Commit();

      var toAddress = new EmailAddress() {
        Address = user.Email,
        Name = user.Name
      };

      var emailMessage = new EmailMessage() {
        Subject = "Password Reset",
        Content = "<p>Your new password is: <b>" + newPassword + "</b></p>",
      };

      emailMessage.ToAddresses.Add(toAddress);

      _emailService.Send(emailMessage);
      return Ok ();
    }
  }
}