using System;
using System.Collections.Generic;

namespace KombitServer.Models
{
    public partial class AppointmentRequest
    {
        public int MakerId { get; set; }
        public int RecepientId { get; set; }
        public int ProductId { get; set; }
        public string LocationCoords { get; set; }
        public string LocationName { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
    }
}
