using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KombitServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KombitServer.Controllers {
  [Route ("api/chat")]
  public class ChatController : Controller {
    private readonly KombitDBContext _context;
    public ChatController (KombitDBContext context) {
      _context = context;
    }
    /// <summary>List Chats</summary>
    [HttpGet ("user/{id}")]
    public IActionResult GetListChatByUser (int id) {
      var chats = _context.ChatMessages
        .Include(x => x.Receiver)
        .Include(x => x.Sender)
        .Where(x => x.SenderId == id || x.ReceiverId == id)
        .GroupBy(x => x.RoomId);
      var distinctChats = chats
        .Select(x => x.LastOrDefault())
        .ToList();
      var unreadChats = chats
        .Select(x => x.Count(y => y.ReceiverId == id && !y.IsRead))
        .ToList();
      return Ok (ChatMessagesMapping.ChatMessagesResponseMapping(distinctChats, unreadChats, id));
    }

    /// <summary>List Chat Message</summary>
    [HttpGet ("room/{roomId}/user/{userId}")]
    public IActionResult GetListMessagesByRoomId (string roomId, int userId) {
      var chats = _context.ChatMessages
        .Include(x => x.Receiver)
        .Include(x => x.Sender)
        .Where(x => x.RoomId.Equals(roomId))
        .ToList();

      var unread = chats.Where(x => x.ReceiverId == userId && !x.IsRead).ToList();
      if (unread != null) {
        foreach (var item in unread)
        {
            item.IsRead = true;
            _context.ChatMessages.Update(item);
        }
        _context.Commit();
      }
      return Ok (ChatMessagesMapping.ChatMessagesRoomResponseMapping(chats));
    }

    /// <summary>List Chats</summary>
    [HttpGet ("unread/user/{id}")]
    public IActionResult GetUnreadChatByUser (int id) {
      var unReadChat = _context.ChatMessages
        .Where(x => x.ReceiverId == id)
        .Count(x => !x.IsRead);
      return Ok (new {unRead = unReadChat});
    }

    /// <summary>Add Chat</summary>
    [HttpPost]
    public IActionResult AddChat ([FromBody] ChatMessagesRequest chatRequest) {
      if (!ModelState.IsValid) { return BadRequest (ModelState); }
      var chats = new ChatMessages(chatRequest);
      _context.ChatMessages.Add(chats);
      _context.Commit();
      return Ok (new {msg="Chat added"});
    }

    /// <summary>Read Chat</summary>
    [HttpPost ("{chatId}/read")]
    public IActionResult AddChat (int chatId) {
      if (!ModelState.IsValid) { return BadRequest (ModelState); }
      var chat = _context.ChatMessages.FirstOrDefault(x => x.Id == chatId && !x.IsRead);
      chat.IsRead = true;
      _context.Update(chat);
      _context.Commit();
      return Ok (new {msg="Chat Readed"});
    }


  }
}