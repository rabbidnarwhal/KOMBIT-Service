using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using KombitServer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace KombitServer.Controllers
{
  [Route ("api/push")]
  public class PushNotificationController : Controller
  {
    [HttpPost ("topics/{topic}")]
    public IActionResult PushToTopics ([FromBody] PushNotification push, string topic)
    {
      PushNotificationRequestToTopic body = PushNotificationRequestToTopic.init (push, topic);
      string jsonBody = JsonConvert.SerializeObject (body);
      return Ok (Push (jsonBody));
    }

    [HttpPost]
    public IActionResult PushToUser ([FromBody] PushNotification push)
    {
      try
      {
        if (push.To == null || push.To.Count == 0)
        {
          return BadRequest ();
        }

      }
      catch
      {
        return BadRequest ();
      }
      PushNotificationRequestToUser body = PushNotificationRequestToUser.From (push);
      string jsonBody = JsonConvert.SerializeObject (body);

      return Ok (Push (jsonBody));

    }

    private string Push (string jsonBody)
    {
      string key = "AIzaSyAab6I-ig-VWIEABDcKaVDxJNE-W723uWo";
      string url = "https://fcm.googleapis.com/fcm/send";

      RestClient client = new RestClient (url);
      RestRequest request = new RestRequest ();

      request.Method = Method.POST;
      request.AddHeader ("Content-Type", "application/json");
      request.AddHeader ("Authorization", "key=" + key);
      request.AddParameter ("application/json", jsonBody, ParameterType.RequestBody);
      IRestResponse response = client.Execute (request);
      return response.Content;
    }

  }

}