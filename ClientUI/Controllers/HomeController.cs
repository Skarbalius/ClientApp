using ClientApp.Helper;
using ClientApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ClientApp.Controllers;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ClientApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ClientAPI _api = new ClientAPI();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            List<Client> clientsList = new List<Client>();
            HttpClient httpClient = _api.Initial();
            HttpResponseMessage response = await httpClient.GetAsync("api/Client");

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                clientsList = JsonConvert.DeserializeObject<List<Client>>(result);
            }

            return View(clientsList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            Client client = new Client();
            HttpClient httpClient = _api.Initial();
            HttpResponseMessage response = await httpClient.GetAsync($"api/Client/{ id }");

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                client = JsonConvert.DeserializeObject<Client>(result);
            }

            return View(client);
        }

        public ActionResult Create(Client client)
        {
            HttpClient httpClient = _api.Initial();            
            var post = httpClient.PostAsJsonAsync<Client>("api/Client", client);
            post.Wait();
            var result = post.Result;

            if (result.IsSuccessStatusCode && client.Name != null)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        public async Task<ActionResult> Edit(Client client)
        {
            HttpClient httpClient = _api.Initial();

            if (client.Name == null)
            {
                HttpResponseMessage response1 = await httpClient.GetAsync($"api/Client/{ client.Id }");
                Client newClient = new Client();
                if (response1.IsSuccessStatusCode)
                {
                    var result = response1.Content.ReadAsStringAsync().Result;
                    newClient = JsonConvert.DeserializeObject<Client>(result);
                    if (client != newClient)
                    {
                        return View(newClient);
                    }
                }
            }

            var response = httpClient.PutAsJsonAsync($"api/Client/{ client.Id }", client).Result;

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                client = JsonConvert.DeserializeObject<Client>(result);
            }

            return View(client);
        }

        public async Task<ActionResult> AddFromFile()
        {
            HttpClient httpClient = _api.Initial();
            HttpResponseMessage response = await httpClient.GetAsync("api/Client/importuoti_klientus");

            return RedirectToAction("Index");
        }
        public async Task<ActionResult> PostCodeUpdate()
        {
            HttpClient httpClient = _api.Initial();
            HttpResponseMessage response = await httpClient.GetAsync("api/Client/atnaujinti_pašto_indeksus");

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int id)
        {
            HttpClient httpClient = _api.Initial();
            HttpResponseMessage response = await httpClient.DeleteAsync($"api/Client/{ id }");

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
