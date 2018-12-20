using System;
using System.Collections.Generic;

namespace KombitServer.Models {
  public partial class ChatMessagesResponse {
    public DateTime Date { get; set; }
    public string LatestChat { get; set; }
    public string LatestUser { get; set; }
    public int ReceiverId { get; set; }
    public string ReceiverName { get; set; }
    public string ReceiverImage { get; set; }
    public string RoomId { get; set; }
    public int UnRead { get; set; }
    public ChatMessagesResponse(ChatMessages chatMessages, int count, int userId) {
      this.Date = chatMessages.Date;
      this.LatestChat = chatMessages.Message;
      this.LatestUser = chatMessages.Sender.Name;
      if (chatMessages.ReceiverId == userId) {
        this.ReceiverId = chatMessages.SenderId;
        this.ReceiverName = chatMessages.Sender.Name;
        this.ReceiverImage = chatMessages.Sender.Image;
      } else {
        this.ReceiverId = chatMessages.ReceiverId;
        this.ReceiverName = chatMessages.Receiver.Name;
        this.ReceiverImage = chatMessages.Receiver.Image;
      }
      this.RoomId = chatMessages.RoomId;
      this.UnRead = count;
    }
  }
}