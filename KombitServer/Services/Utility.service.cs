using Microsoft.Extensions.Options;
using RestSharp;

namespace KombitServer.Services
{
  public class Utility
  {
    public static string sendPushNotification (string jsonBody)
    {
      string key = "AIzaSyDgpko0W1nEYN0vtAONSwrVTYux5ngsZHk";
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