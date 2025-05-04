using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP1Entities;

namespace TP1Datos.Interfaces
{
    public interface IClienteRepositorio
    {
        void Add(Cliente cliente);
        void Delete(int clienteId);
        void Edit(Cliente cliente);
        bool Exist(string firstName, string lastName, int? excludeId = null);
        List<Cliente> GetAll(string sortedBy = "LastName");
        Cliente? GetById(int authorId, bool tracked = false);
        bool HasBooks(int authorId);
        void LoadBooks(Cliente cliente);
        List<Cliente> GetAllWithBooks();
        List<IGrouping<int, Orden>> AuthorsGroupIdBooks();
    }
}
