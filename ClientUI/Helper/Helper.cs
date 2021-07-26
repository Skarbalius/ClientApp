using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClientUI.Helper
{
    public class ClientAPI
    {
        public HttpClient Initial()
        {
            var client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost:50771");
            client.BaseAddress = new Uri("https://localhost:44336/");
            return client;
        }
    }
}
