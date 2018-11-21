using System;
using System.Collections.Generic;

namespace KombitServer.Models
{
    public partial class AppointmentResponse
    {
        public int Id { get; set; }
        // public string MakerName { get; set; }
        // public string RecepientName { get; set; }
        public string UserName { get; set; }
        public string ProductName { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        
        public AppointmentResponse(Appointment appointment, MUser user) {
            this.Id = appointment.Id;
            // this.MakerName = appointment.Maker.Name;
            // this.RecepientName = appointment.Recepient.Name;
            this.UserName = user.Name;
            this.ProductName = appointment.Product.ProductName;
            this.Status = appointment.Status;
            this.Date = appointment.Date;
        } 
    }
}
