namespace KombitServer.Models.Email {
  public interface IEmailConfiguration {
    string SmtpServer { get; }
    int SmtpPort { get; }
    string SmtpName { get; set; }
    string SmtpUsername { get; set; }
    string SmtpPassword { get; set; }
  }
}