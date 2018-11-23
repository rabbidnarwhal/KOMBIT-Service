using System;
using System.Collections.Generic;

namespace KombitServer.Models
{
    public partial class AppointmentDetailResponse
    {
        public int Id { get; set; }
        public string MakerName { get; set; }
        public string RecepientName { get; set; }
        public int ProductId {get; set;}
        public string ProductName { get; set; }
        public string ProductSolution { get; set; }
        public string ProductCompany { get; set; }
        public string ProductHolding { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string RejectMessage { get; set; }
        public string LocationCoords { get; set; }
        public string LocationName { get; set; }
        public DateTime Date { get; set; }
        
        public AppointmentDetailResponse(Appointment appointment) {
            this.Id = appointment.Id;
            this.ProductId = appointment.ProductId;
            this.MakerName = appointment.Maker.Name;
            this.RecepientName = appointment.Recepient.Name;
            this.ProductName = appointment.Product.ProductName;
            this.ProductCompany = appointment.Product.Company.CompanyName;
            this.ProductHolding = appointment.Product.Holding.HoldingName;
            this.ProductSolution = appointment.Product.Category.Category;
            this.Status = appointment.Status;
            this.Note = appointment.Note;
            this.RejectMessage = appointment.RejectMessage;
            this.Date = appointment.Date;
            this.LocationCoords = appointment.LocationCoords;
            this.LocationName = appointment.LocationName;
        } 
    }
}
