using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KombitServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KombitServer.Controllers {
  [Route ("api/appointment")]
  public class AppointmentController : Controller {
    private readonly KombitDBContext _context;
    public AppointmentController (KombitDBContext context) {
      _context = context;
    }

    /// <summary>Get list of appointment available for user</summary>
    [HttpGet ("user/{id}")]
    [ProducesResponseType (typeof (List<AppointmentResponse>), 200)]
    public IActionResult GetAppointments (int id) {
      List<string> status = new List<string> ();
      status.Add ("DONE");
      status.Add ("REJECT");

      var appointment = _context.Appointment
        .OrderByDescending (x => x.Date)
        .Include (x => x.Maker)
        .Include (x => x.Recepient)
        .Include(x => x.Product)
        .Where (x => x.MakerId == id || (x.RecepientId == id && !status.Contains (x.Status))).ToList ();

      List<AppointmentResponse> listResponse = new List<AppointmentResponse> ();
      foreach (var item in appointment) {
        var user = item.MakerId == id ? item.Recepient : item.Maker;
        listResponse.Add (new AppointmentResponse (item, user));
      }
      return Ok (listResponse);
    }

    /// <summary>Get appointment detail</summary>
    [HttpGet ("{appointmentId}")]
    [ProducesResponseType (typeof (Appointment), 200)]
    public IActionResult GetAppointmentsFromUser (int appointmentId) {
      var appointment = _context.Appointment
        .Include (x => x.Maker)
        .Include (x => x.Recepient)
        .Include (x => x.Product).ThenInclude(x => x.Company)
        .Include (x => x.Product).ThenInclude(x => x.Category)
        .Include (x => x.Product).ThenInclude(x => x.Holding)
        .FirstOrDefault (x => x.Id == appointmentId);
      if (appointment == null) return Ok (new object());
      else {
        return Ok (new AppointmentDetailResponse(appointment));
      }
    }

    /// <summary>Create appointment</summary>
    [HttpPost ("")]
    [ProducesResponseType (200)]
    public IActionResult CreateAppointment ([FromBody] AppointmentRequest request) {
      var appointment = new Appointment (request, "PENDING");
      _context.Appointment.Add (appointment);
      _context.Commit ();
      return Ok ();
    }

    /// <summary>Update existing status and message appointment</summary>
    [HttpPost ("{appointmentId}")]
    [ProducesResponseType (200)]
    public IActionResult ChangeAppointmentStatus ([FromBody] AppointmentUpdateStatusRequest statusRequest, int appointmentId) {
      if (statusRequest.Status.ToLower().Contains("reject") || statusRequest.Status.ToLower().Contains("approve") || statusRequest.Status.ToLower().Contains("done") || statusRequest.Status.ToLower().Contains("delete")) {
        var appointment = _context.Appointment.FirstOrDefault (x => x.Id == appointmentId);
        appointment.Status = statusRequest.Status.ToUpper();
        appointment.RejectMessage = statusRequest.RejectMessage;
        _context.Appointment.Update (appointment);
        _context.Commit ();
        return Ok ();
      } else {
        return NotFound();
      }
    }
  }
}