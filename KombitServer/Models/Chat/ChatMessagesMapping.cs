using System;
using System.Collections.Generic;
using System.Linq;

namespace KombitServer.Models {
  public partial class ChatMessagesMapping {
    public static List<ChatMessagesResponse> ChatMessagesResponseMapping (List<ChatMessages> chatMessages, List<int> unreadMessages, int userId) {
      List<ChatMessagesResponse> response = new List<ChatMessagesResponse> ();
      var index = 0;
      foreach (var item in chatMessages){
        var chatResponse = new ChatMessagesResponse(item, unreadMessages[index], userId);
        response.Add(chatResponse);
        index++;
      }
      return response.OrderByDescending(x => x.Date).ToList();
    }
    public static List<ChatMessagesRoomResponse> ChatMessagesRoomResponseMapping (List<ChatMessages> chatMessages) {
      List<ChatMessagesRoomResponse> response = new List<ChatMessagesRoomResponse> ();
      foreach (var item in chatMessages){
        var chatResponse = new ChatMessagesRoomResponse(item);
        response.Add(chatResponse);
      }
      return response;
    }
  }
}