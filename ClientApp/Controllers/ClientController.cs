using ClientApp.Models;
using ClientApp.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ClientApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IClientRepository _clientRepository;
        private readonly IConfiguration configuration;
        private readonly string connectionString;
        private readonly string postitUrl;
        private readonly string postitKey;
        private readonly string clientFile;

        public ClientController(IClientRepository clientRepository, IConfiguration config, ILogger<ClientController> logger)
        {
            _clientRepository = clientRepository;
            _logger = logger;
            configuration = config;
            connectionString = configuration.GetConnectionString("DefaultConnectionString");
            postitUrl = configuration.GetConnectionString("postitUrl");
            postitKey = configuration.GetConnectionString("postitKey");
            clientFile = configuration.GetConnectionString("clientFile");
        }

        [HttpGet]
        public async Task<IEnumerable<Client>> GetClients()
        {
            return await _clientRepository.GetClients();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            return await _clientRepository.Get(id);
        }
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient([FromBody] Client client)
        {
            var newClient = await _clientRepository.Create(client);
            return CreatedAtAction(nameof(GetClient), new { id = newClient.Id }, newClient);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutClient(int id, [FromBody] Client client)
        {
            if (id != client.Id)
            {
                return BadRequest();
            }
            await _clientRepository.Update(client);
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var clientToDelete = await _clientRepository.Get(id);
            if (clientToDelete == null)
            {
                return NotFound();
            }
            await _clientRepository.Delete(clientToDelete.Id);
            return NoContent();
        }

        [Route("importuoti_klientus")]
        [HttpGet]
        public async Task<ActionResult> LoadJson()
        {
            using (StreamReader r = new StreamReader(clientFile))
            {
                string json = r.ReadToEnd();
                List<Client> items = JsonConvert.DeserializeObject<List<Client>>(json);
                foreach (Client client in items)
                {
                    if (_clientRepository.Exists(client) == false)
                    {
                        await _clientRepository.Create(client);
                    }
                }
            }
            return NoContent();
        }

        [Route("atnaujinti_pašto_indeksus")]
        [HttpGet]
        public async Task<ActionResult> GetPostCodes()
        {
            
            var clients = await _clientRepository.GetClients();
            foreach (Client client in clients)
            {
                HttpClient httpclient = new HttpClient();
                string address = client.Address;
                string url = postitUrl + address + postitKey;
                HttpResponseMessage response = await httpclient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                JObject joResponse = JObject.Parse(responseBody);
                JArray jArr = (JArray)joResponse["data"];
                JObject jParsed = (JObject)jArr[0];
                int postCode = Convert.ToInt32(jParsed["post_code"].ToString());
                Client newClient = client;
                newClient.PostCode = postCode;
                await _clientRepository.Update(newClient);
            }
            return NoContent();
        }
    }
}
