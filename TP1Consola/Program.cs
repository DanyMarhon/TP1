using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TP1Consola.Validador;
using TP1Datos;
using TP1Entities;

internal class Program
{
    static void Main(string[] args)
    {
        CreateDb();
        do
        {
            Console.Clear();
            Console.WriteLine("Menu Principal");
            Console.WriteLine("1 - Ordenes");
            Console.WriteLine("2 - Clientes");
            Console.WriteLine("x - Salir");
            Console.Write("Ingresa una opción:");
            var option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    OrdenesMenu();
                    break;
                case "2":
                    ClientesMenu();
                    break;
                case "x":
                    Console.WriteLine("Fin del Programa");
                    return;
                default:
                    break;
            }

        } while (true);
    }

    private static void OrdenesMenu()
    {
        do
        {
            Console.Clear();
            Console.WriteLine("Ordenes");
            Console.WriteLine("1 - Tomar una orden");
            Console.WriteLine("2 - Historial de ordenes");
            Console.WriteLine("3 - Anular orden");
            Console.WriteLine("4 - X Nada X");
            Console.WriteLine("r - Volver");
            Console.Write("Ingrese una opción:");
            var option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    TomarOrden();
                    break;
                case "2":
                    HistorialDeOrdenes();
                    break;
                case "3":
                    AnularOrden();
                    break;
                case "4":
                    //EditarOrden();
                    break;
                case "r":
                    return;
                default:
                    break;
            }
        } while (true);
    }

    private static void AnularOrden()
    {
        Console.Clear();
        Console.WriteLine("Anulando Orden");
        Console.WriteLine("Lista de ordenes para anular");
        using (var context = new MarketContext())
        {
            var ordenes = context.Ordenes
                .OrderBy(o => o.Id)
                .Select(o => new
                {
                    o.NumeroOrden,
                    o.Cliente,
                    o.Valor
                }).ToList();
            foreach (var ord in ordenes)
            {
                Console.WriteLine($"Número de orden: {ord.NumeroOrden} - Cliente: {ord.Cliente.Nombre}" +
                    $" - {ord.Cliente.Apellido} - Valor: {ord.Valor}");
            }
            Console.Write("Seleccione número de orden a anular (0 para salir):");
            if (!int.TryParse(Console.ReadLine(), out int ordenId) || ordenId < 0)
            {
                Console.WriteLine("Número de orden inválido...");
                Console.ReadLine();
                return;
            }
            if (ordenId == 0)
            {
                Console.WriteLine("Cancelado por usuario");
                Console.ReadLine();
                return;
            }
            var anularOrden = context.Ordenes.Find(ordenId);
            if (anularOrden is null)
            {
                Console.WriteLine("Orden no existente");
            }
            else
            {
                context.Ordenes.Remove(anularOrden);
                context.SaveChanges();
                Console.WriteLine("Orden anulada exitosamente");
            }
            Console.ReadLine();
            return;
        }

    }

    private static void TomarOrden()
    {
        Console.Clear();
        Console.WriteLine("Añadiendo nueva orden");

        Console.Write("Ingrese el numero de la orden:");
        if (!int.TryParse(Console.ReadLine(), out var numeroOrden))
        {
            Console.WriteLine("Número inválido");
            Console.ReadLine();
            return;
        }

        Console.Write("Ingrese la fecha de la orden (dd/mm/yyyy):");
        if (!DateOnly.TryParse(Console.ReadLine(), out var fechaOrden))
        {
            Console.WriteLine("Fecha errónea");
            Console.ReadLine();
            return;
        }

        Console.Write("Ingrese el valor de la orden:");
        if (!int.TryParse(Console.ReadLine(), out var valor))
        {
            Console.WriteLine("Valor inválido");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Lista de clientes para seleccionar");
        using (var context = new MarketContext())
        {
            var clientesList = context.Clientes
                .OrderBy(a => a.Id)
                .ToList();
            foreach (var cliente in clientesList)
            {
                Console.WriteLine($"Código: {cliente.Id} - Nombre: {cliente.Nombre} - {cliente.Apellido}");
            }
            Console.Write("Ingrese ID del cliente (0 Nuevo cliente):");
            if (!int.TryParse(Console.ReadLine(), out var clienteId) || clienteId < 0)
            {
                Console.WriteLine("ID inválido");
                Console.ReadLine();
                return;
            }
            var selectedCliente = context.Clientes.Find(clienteId);
            if (selectedCliente is null)
            {
                Console.WriteLine("Cliente no encontrado");
                Console.ReadLine();
                return;
            }
            var newOrden = new Orden
            {
                NumeroOrden = numeroOrden,
                Valor = valor,
                FechaOrden = fechaOrden,
                ClienteId = clienteId
            };

            var ordenesValidador = new ValidadorOrdenes();
            var validationResult = ordenesValidador.Validate(newOrden);

            if (validationResult.IsValid)
            {
                var ordenExistente = context.Ordenes.FirstOrDefault(b => b.Valor == valor! &&
                    b.ClienteId == clienteId);

                if (ordenExistente is null)
                {
                    context.Ordenes.Add(newOrden);
                    context.SaveChanges();
                    Console.WriteLine("Orden Añadida Exitosamente");

                }
                else
                {
                    Console.WriteLine("Orden Duplicada");
                }

            }
            else
            {
                foreach (var error in validationResult.Errors)
                {
                    Console.WriteLine(error);
                }
            }
            Console.ReadLine();
            return;
        }
    }

    private static void HistorialDeOrdenes()
    {
        Console.Clear();
        Console.WriteLine("Lista de Ordenes");
        using (var context = new MarketContext())
        {
            var ordenes = context.Ordenes
                .Include(b => b.Cliente)
                .Select(b => new
                {
                    b.NumeroOrden,
                    b.Cliente
                })
                .ToList();
            foreach (var bo in ordenes)
            {
                Console.WriteLine($"Número de orden: {bo.NumeroOrden} - Cliente:{bo.Cliente.Nombre} + {bo.Cliente.Apellido}");
            }
        }
        Console.WriteLine("ENTER para continuar");
        Console.ReadLine();
    }

    private static void ClientesMenu()
    {
        do
        {
            Console.Clear();
            Console.WriteLine("CLIENTES");
            Console.WriteLine("1 - Lista de Clientes");
            Console.WriteLine("2 - Añadir Cliente");
            Console.WriteLine("3 - Anular Cliente");
            Console.WriteLine("4 - Editar Cliente");
            Console.WriteLine("r - Volver");
            Console.Write("Ingrese una opción:");
            var option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    ClientesList();
                    break;
                case "2":
                    AddClientes();
                    break;
                case "3":
                    AnularCliente();
                    break;
                case "4":
                    EditarCliente();
                    break;
                case "r":
                    return;
                default:
                    break;
            }

        } while (true);

    }

    private static void EditarCliente()
    {
        Console.Clear();
        Console.WriteLine("Editar Cliente");
        using (var context = new MarketContext())
        {
            var clientes = context.Clientes
                .OrderBy(a => a.Id)
                .ToList();
            foreach (var cliente in clientes)
            {
                Console.WriteLine($"Id: {cliente.Id} - Nombre: {cliente.Nombre} - {cliente.Apellido}");
            }
            Console.Write("Ingrese un ID para editar:");
            int clienteId;
            if (!int.TryParse(Console.ReadLine(), out clienteId) || clienteId <= 0)
            {
                Console.WriteLine("ID Inválido");
                Console.ReadLine();
                return;
            }

            var clienteInDb = context.Clientes.Find(clienteId);
            if (clienteInDb == null)
            {
                Console.WriteLine("Cliente no existe");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"Nombre actual: {clienteInDb.Nombre}");
            Console.Write("Ingrese un nuevo nombre (o ENTER para mantener el mismo)");
            var nuevoNombre = Console.ReadLine();
            if (!string.IsNullOrEmpty(nuevoNombre))
            {
                clienteInDb.Nombre = nuevoNombre;
            }
            Console.WriteLine($"Apellido actual: {clienteInDb.Apellido}");
            Console.Write("Ingrese un nuevo apellido (o ENTER para mantener el mismo)");
            var nuevoApellido = Console.ReadLine();
            if (!string.IsNullOrEmpty(nuevoApellido))
            {
                clienteInDb.Apellido = nuevoApellido;
            }

            var clienteOriginal = context.Clientes
                .AsNoTracking()
                .FirstOrDefault(a => a.Id == clienteInDb.Id);

            Console.Write($"Seguro que vas a editar el cliente \"{clienteOriginal!.Nombre} {clienteOriginal.Apellido}\"? (y/n):");
            var confirm = Console.ReadLine();
            if (confirm?.ToLower() == "y")
            {
                context.SaveChanges();
                Console.WriteLine("Cliente editado");
            }
            else
            {
                Console.WriteLine("Operación cancelada");
            }
            Console.ReadLine();
            return;
        }
    }

    private static void AnularCliente()
    {
        Console.Clear();
        Console.WriteLine("Anular Cliente");
        using (var context = new MarketContext())
        {
            var clientes = context.Clientes
                .OrderBy(a => a.Id)
                .ToList();
            foreach (var cliente in clientes)
            {
                Console.WriteLine($"Id: {cliente.Id} - Nombre: {cliente.Nombre} - {cliente.Apellido}");
            }
            Console.Write("Ingrese el ID del cliente a Anular:");
            int clienteId;
            if (!int.TryParse(Console.ReadLine(), out clienteId) || clienteId <= 0)
            {
                Console.WriteLine("ID Inválido");
                Console.ReadLine();
                return;
            }

            var clienteInDb = context.Clientes.Find(clienteId);
            if (clienteInDb == null)
            {
                Console.WriteLine("Cliente no existe");
                Console.ReadLine();
                return;
            }
            var tieneOrdenes = context.Ordenes.Any(b => b.ClienteId == clienteInDb.Id);
            if (!tieneOrdenes)
            {
                Console.Write($"Estas seguro de eliminar a \"{clienteInDb.Nombre} {clienteInDb.Apellido}\"? (y/n):");
                var confirm = Console.ReadLine();
                if (confirm?.ToLower() == "y")
                {
                    context.Clientes.Remove(clienteInDb);
                    context.SaveChanges();
                    Console.WriteLine("Cliente anulado");
                }
                else
                {
                    Console.WriteLine("Operación cancelada");
                }

            }
            else
            {
                Console.WriteLine("Cliente tiene ordenes activas, no se puede anular");
            }

            Console.ReadLine();
            return;
        }
    }

    private static void AddClientes()
    {
        Console.Clear();
        Console.WriteLine("Añadiendo cliente");
        Console.Write("Ingrese Nombre:");
        var nombre = Console.ReadLine();
        Console.Write("Ingrese Apellido:");
        var apellido = Console.ReadLine();
        Console.Write("Ingrese DNI:");
        if (!int.TryParse(Console.ReadLine(), out var dni))
        {
            Console.WriteLine("DNI Inválido");
            Console.ReadLine();
            return;
        }

        using (var context = new MarketContext())
        {
            bool exist = context.Clientes.Any(a => a.Nombre == nombre &&
                a.Apellido == apellido);

            if (!exist)
            {
                var cliente = new Cliente
                {
                    Nombre = nombre ?? string.Empty,
                    Apellido = apellido ?? string.Empty,
                    Dni = dni
                };

                var validationContext = new ValidationContext(cliente);
                var errorMessages = new List<ValidationResult>();

                bool isValid = Validator.TryValidateObject(cliente, validationContext, errorMessages, true);

                if (isValid)
                {
                    context.Clientes.Add(cliente);
                    context.SaveChanges();
                    Console.WriteLine("Cliente Añadido");

                }
                else
                {
                    foreach (var message in errorMessages)
                    {
                        Console.WriteLine(message);
                    }
                }
            }
            else
            {
                Console.WriteLine("Cliente ya existe");
            }
        }
        Console.ReadLine();
    }

    private static void ClientesList()
    {
        Console.Clear();
        Console.WriteLine("Lista de clientes");
        using (var context = new MarketContext())
        {
            var clientes = context.Clientes
                .OrderBy(a => a.Nombre)
                .ThenBy(a => a.Apellido)
                .AsNoTracking()
                .ToList();
            foreach (var cliente in clientes)
            {
                Console.WriteLine(cliente);
            }
            Console.WriteLine("ENTER para continuar");
            Console.ReadLine();
        }
    }

    private static void CreateDb()
    {
        using (var context = new MarketContext())
        {
            context.Database.EnsureCreated();
        }
        Console.WriteLine("Database Creada");
    }
}