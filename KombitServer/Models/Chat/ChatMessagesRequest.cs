using System;
using System.Collections.Generic;

namespace KombitServer.Models {
  public partial class ChatMessagesRequest {
    public string RoomId { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public string Date { get; set; }
    public string Message { get; set; }
  }
}