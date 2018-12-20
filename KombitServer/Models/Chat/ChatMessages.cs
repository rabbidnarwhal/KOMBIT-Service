using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KombitServer.Models {
  public partial class ChatMessages {
    public int Id { get; set; }
    public string RoomId { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public DateTime Date { get; set; }
    public string Message { get; set; }
    public Boolean IsRead { get; set; }
    [ForeignKey ("SenderId")]
    public virtual MUser Sender {set; get;}
    [ForeignKey ("ReceiverId")]
    public virtual MUser Receiver {set; get;}
    public ChatMessages(){}
    public ChatMessages(ChatMessagesRequest request) {
      this.RoomId = request.RoomId;
      this.SenderId = request.SenderId;
      this.ReceiverId = request.ReceiverId;
      this.Date = DateTime.UtcNow;
      this.Message = request.Message;
      this.IsRead = false;
    }
  }
}