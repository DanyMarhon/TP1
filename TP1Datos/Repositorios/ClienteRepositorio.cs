using Microsoft.EntityFrameworkCore;
using TP1Datos;
using TP1Datos.Interfaces;
using TP1Entities;

namespace EFIntro.Data.Repositories
{
    public class ClienteRepositorio : IClienteRepositorio
    {
        private readonly MarketContext _context = null!;

        public ClienteRepositorio(MarketContext context)
        {
            _context = context;
        }
        public List<Cliente> GetAll(string sortedBy = "Nombre")
        {
            IQueryable<Cliente> query = _context.Clientes.AsNoTracking();

            //switch (sortedBy)
            //{
            //    case "LastName":
            //        return query.OrderBy(a => a.LastName)
            //            .ThenBy(a => a.FirstName)
            //            .ToList();
            //    case "FirstName":
            //        return query.OrderBy(a => a.FirstName)
            //        .ThenBy(a => a.LastName)
            //        .ToList();

            //    default:
            //        return query.OrderBy(a => a.Id).ToList();

            //}

            //MODERM FORM--> MAS BANANA
            return sortedBy switch
            {
                "Apellido" => query.OrderBy(a => a.Apellido)
                                        .ThenBy(a => a.Nombre)
                                        .ToList(),
                "Nombre" => query.OrderBy(a => a.Nombre)
                                    .ThenBy(a => a.Apellido)
                                    .ToList(),
                _ => query.OrderBy(a => a.Id).ToList(),
            };
        }

        public Cliente? GetById(int clienteId, bool tracked = false)
        {
            return tracked
                ? _context.Clientes
                    .FirstOrDefault(a => a.Id == clienteId)
                : _context.Clientes
                    .AsNoTracking()
                    .FirstOrDefault(a => a.Id == clienteId);
        }
        public bool Exist(string nombre, string apellido, int? idExpluido = null)
        {
            return idExpluido.HasValue
                ? _context.Clientes.Any(a => a.Nombre == nombre &&
                    a.Apellido == apellido && a.Id != idExpluido)
                : _context.Clientes.Any(a => a.Nombre == nombre &&
                    a.Apellido == apellido);


        }

        public void Add(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            _context.SaveChanges();

        }

        public void Delete(int clienteId)
        {
            var clienteEnDb = GetById(clienteId, true);
            if (clienteEnDb != null)
            {
                _context.Clientes.Remove(clienteEnDb);
                _context.SaveChanges();

            }

        }
        public void Edit(Cliente cliente)
        {
            var clienteEnDb = GetById(cliente.Id, true);
            if (clienteEnDb != null)
            {
                clienteEnDb.Nombre = cliente.Nombre;
                clienteEnDb.Apellido = cliente.Apellido;

                _context.SaveChanges();
            }
        }

        public bool HasBooks(int clienteId)
        {
            return _context.Ordenes.Any(b => b.ClienteId == clienteId);

        }

        public void LoadBooks(Cliente cliente)
        {
            _context.Entry(cliente).Collection(a => a.Ordenes!).Load();

        }

        public List<Cliente> GetAllWithBooks()
        {
            return _context.Clientes.Include(a => a.Ordenes).ToList();
        }

        public List<IGrouping<int, Orden>> AuthorsGroupIdBooks()
        {
            return _context.Ordenes
                    .GroupBy(a => a.ClienteId).ToList();
        }
    }
}