using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KombitServer.Models
{
    public partial class Appointment
    {
        public int Id { get; set; }
        public int MakerId { get; set; }
        public int RecepientId { get; set; }
        public int ProductId { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public string LocationCoords { get; set; }
        public string LocationName { get; set; }
        public string Note { get; set; }
        public string RejectMessage { get; set; }

        [ForeignKey ("MakerId")]
        public MUser Maker { get; set; }

        [ForeignKey ("RecepientId")]
        public MUser Recepient { get; set; }

        [ForeignKey ("ProductId")]
        public Product Product { get; set; }
        public Appointment () {}
        public Appointment(AppointmentRequest request, string status) {
            this.MakerId = request.MakerId;
            this.RecepientId = request.RecepientId;
            this.ProductId = request.ProductId;
            this.Date = request.Date;
            this.LocationCoords = request.LocationCoords;
            this.LocationName = request.LocationName;
            this.Note = request.Note;
            this.Status = status;
        }
    }

}
