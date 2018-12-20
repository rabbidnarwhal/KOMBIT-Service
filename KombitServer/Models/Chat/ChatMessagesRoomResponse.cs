using System;
using System.Collections.Generic;

namespace KombitServer.Models {
  public partial class ChatMessagesRoomResponse {
    public string Chat { get; set; }
    public DateTime Date { get; set; }
    public Boolean IsRead { get; set; }
    public int ReceiverId { get; set; }
    public string ReceiverName { get; set; }
    public int SenderId { get; set; }
    public string SenderName { get; set; }
    public ChatMessagesRoomResponse(ChatMessages chatMessage) {
      this.Chat = chatMessage.Message;
      this.Date = chatMessage.Date;
      this.IsRead = chatMessage.IsRead;
      this.ReceiverId = chatMessage.ReceiverId;
      this.ReceiverName = chatMessage.Receiver.Name;
      this.SenderId = chatMessage.SenderId;
      this.SenderName = chatMessage.Sender.Name;
    }
  }
}