using System.Collections.Generic;

namespace KombitServer.Models.Email {
  public class EmailMessage {
    public EmailMessage () {
      ToAddresses = new List<EmailAddress> ();
    }
    public List<EmailAddress> ToAddresses { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
  }
}