using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KombitServer.Models
{
  public class NotificationRequest
  {
    public string Title { get; set; }
    public string Body { get; set; }
    public List<string> To { get; set; }
  }

}