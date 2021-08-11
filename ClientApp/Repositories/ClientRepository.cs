using ClientApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ClientApp.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ClientContext _context;

        public ClientRepository(ClientContext context)
        {
            _context = context;
        }

        public async Task<Client> Create(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task Delete(int id)
        {
            var clientToDelete = await _context.Clients.FindAsync(id);
            _context.Clients.Remove(clientToDelete);
            await _context.SaveChangesAsync();
        }

        public bool Exists(Client client)
        {
            bool found = _context.Clients.Any(
                c =>
                c.Name == client.Name
                && c.Address == client.Address
                );
            return found;
        }

        public async Task<Client> Get(int id)
        {
            return await _context.Clients.FindAsync(id);
        }
        public async Task<Client> Get(string name)
        {
            return await _context.Clients.FindAsync(name);
        }
        public async Task<IEnumerable<Client>> GetClients()
        {
            return await _context.Clients.ToListAsync();
        }

        public async Task Update(Client client)
        {
            _context.Entry(client).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
