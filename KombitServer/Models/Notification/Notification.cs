using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KombitServer.Models {
  public partial class Notification {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Topic { get; set; }
    public int? To { get; set; }
    public Boolean IsRead { get; set; }
    public DateTime PushDate { get; set; }
    public int? ModuleId { get; set; }
    public string ModuleName { get; set; }
    public string ModuleUseCase { get; set; }
    public Notification () {

    }
    public Notification (NotificationRequest request) {
      this.Title = request.Title;
      this.Content = request.Body;
    }
  }
}