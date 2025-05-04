using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP1Datos.Interfaces;
using TP1Entities;
using TP1Servicios.Interfaces;

namespace TP1Servicios.Servicios
{
    class ClienteServicio : IClienteServicio
    {
        private readonly IClienteRepositorio _repository;

        public ClienteServicio(IClienteRepositorio repository)
        {
            _repository = repository;
        }
        public void Anular(int clienteId)
        {
            throw new NotImplementedException();
        }

        public List<IGrouping<int, Orden>> AuthorsGroupIdBooks()
        {
            throw new NotImplementedException();
        }

        public void Cargarordenes(Cliente cliente)
        {
            throw new NotImplementedException();
        }

        public bool Existe(string nombre, string apellido, int? idExcuido = null)
        {
            throw new NotImplementedException();
        }

        public List<Cliente> GetAll(string sortedBy = "Nombre")
        {
            throw new NotImplementedException();
        }

        public Cliente? GetById(int clienteId, bool tracked = false)
        {
            throw new NotImplementedException();
        }

        public List<Cliente> GetTodosConOrdenes()
        {
            throw new NotImplementedException();
        }

        public void Guardar(Cliente cliente)
        {
            throw new NotImplementedException();
        }

        public bool TieneOrdenes(int clienteId)
        {
            throw new NotImplementedException();
        }
    }
}
