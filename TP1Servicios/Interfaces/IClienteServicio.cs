using TP1Entities;

namespace TP1Servicios.Interfaces
{
    public interface IClienteServicio
    {
        void Guardar(Cliente cliente);
        void Anular(int clienteId);
        bool Existe(string nombre, string apellido, int? idExcuido = null);
        List<Cliente> GetAll(string sortedBy = "Nombre");
        Cliente? GetById(int clienteId, bool tracked = false);
        bool TieneOrdenes(int clienteId);
        void Cargarordenes(Cliente cliente);
        List<Cliente> GetTodosConOrdenes();
        List<IGrouping<int, Orden>> AuthorsGroupIdBooks();

    }
}
