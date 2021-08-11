using ClientApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientApp.Repositories
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetClients();
        Task<Client> Get(int id);
        Task<Client> Get(string name);
        Task<Client> Create(Client client);
        Task Update(Client client);
        Task Delete(int id);
        bool Exists(Client client);
    }
}
