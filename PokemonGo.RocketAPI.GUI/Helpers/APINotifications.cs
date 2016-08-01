using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.GUI.Helpers
{
    public static class APINotifications
    {
        private static string API_KEY = "70924d828df79feebc5d3c59fdda4c36fc7ccfa12dbdc9d7";
        private static string API_ENDPOINT = "https://www.notifymyandroid.com/publicapi/notify";
        private static string APP = "PoGo Bot";

        public static void SendNotification(string description, string title, int priority)
        {
            WebRequest request = WebRequest.Create(API_ENDPOINT);
            request.Method = "POST";

            byte[] data = Encoding.UTF8.GetBytes(
                $"apikey={API_KEY}&application={APP}&event={title}&description={description}");

            request.ContentLength = data.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(data, 0, data.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();
        }
    }
}
