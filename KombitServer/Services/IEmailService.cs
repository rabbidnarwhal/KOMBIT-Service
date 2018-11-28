using KombitServer.Models.Email;

namespace KombitServer.Services.Email {
  public interface IEmailService {
    void Send (EmailMessage emailMessage);
  }
}