using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KombitServer.Models;
using KombitServer.Services;
using KombitServer.Services.PushNotification;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace KombitServer.Hubs {
  public class ChatHub : Hub {

    private readonly KombitDBContext _dbcontext;

    public ChatHub (KombitDBContext dbcontext) {
      _dbcontext = dbcontext;
    }
    public void SendChatMessage (ChatMessagesRequest data) {
      List<int> listId = new List<int> ();
      listId.Add (data.SenderId);
      listId.Add (data.ReceiverId);

      List<MUser> listUser = _dbcontext.MUser.Where (x => listId.Contains (x.Id)).ToList ();
      var sender = listUser.FirstOrDefault (x => x.Id == data.SenderId);
      var receiver = listUser.FirstOrDefault (x => x.Id ==data.ReceiverId);

      ChatMessages chatMessage = new ChatMessages () {
        Date = DateTime.UtcNow,
        ReceiverId = receiver.Id,
        SenderId = sender.Id,
        RoomId = data.RoomId,
        Message = data.Message,
        IsRead = false,
      };
      _dbcontext.ChatMessages.Add (chatMessage);
      _dbcontext.Commit ();

      if (receiver.SocketId == null && sender.PushId != null) {
        NotificationRequest body = new NotificationRequest () {
          Body = "New chat message from " + sender.Name,
          Title = "New chat message",
        };

        NotificationRequestToTopic request = new NotificationRequestToTopic (body, receiver.PushId);
        request.data.newChat = "True";
        string jsonBody = JsonConvert.SerializeObject (request);
        PushNotificationService.sendPushNotification (jsonBody);
      } else if (receiver.SocketId != null) {
        Clients.Client (receiver.SocketId).SendAsync ("send:chat", new {msg = "Chat Received", roomId = data.RoomId, chat= data.Message, dateTime = chatMessage.Date});
      }
    }

    public void Register (int userId) {
      var connectionId = Context.ConnectionId;
      var user = _dbcontext.MUser.FirstOrDefault (x => x.Id == userId);
      if (user.SocketId == null || !user.SocketId.Equals(connectionId)) {
        user.SocketId = connectionId;
        _dbcontext.MUser.Update (user);
        _dbcontext.Commit ();
        Clients.Caller.SendAsync ("register", connectionId);
      } else {
        Clients.Caller.SendAsync ("register", "Connetion are already made");
      }
    }

    public async Task UnRegister (string connectionId) {
      var user = _dbcontext.MUser.FirstOrDefault (x => x.SocketId == connectionId);
      if (user != null) {
        user.SocketId = null;
        _dbcontext.MUser.Update (user);
        await _dbcontext.SaveChangesAsync ();
      }
    }

    public override Task OnConnectedAsync()
    {
      return base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
      await this.UnRegister(Context.ConnectionId);
      await base.OnDisconnectedAsync(exception);
    }
  }
}