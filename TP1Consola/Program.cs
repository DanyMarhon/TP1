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
            Console.WriteLine("1 - Clientes");
            Console.WriteLine("2 - Ordenes");
            Console.WriteLine("x - Salir");
            Console.Write("Ingresa una opción:");
            var option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    ClientesMenu();
                    break;
                case "2":
                    OrdenesMenu();
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
            Console.WriteLine("4 - Editar orden");
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
                    EditarOrden();
                    break;
                case "r":
                    return;
                default:
                    break;
            }
        } while (true);
    }

    private static void EditarOrden()
    {
        Console.Clear();
        Console.WriteLine("Editando Ordenes");
        Console.WriteLine("Lista de ordenes para editar");
        using (var context = new MarketContext())
        {
            var ordenes = context.Ordenes.OrderBy(o => o.NumeroOrden)
                .Select(o => new
                {
                    o.NumeroOrden,
                    o.Valor,
                    o.Cliente
                }).ToList();
            foreach (var item in ordenes)
            {
                Console.WriteLine(item);
            }

            Console.Write("Ingrese el número de orden a editar (0 para cancelar):");
            int numeroOrden = int.Parse(Console.ReadLine()!);
            if (numeroOrden < 0)
            {
                Console.WriteLine("Número de orden inválido");
                Console.ReadLine();
                return;
            }
            if (numeroOrden == 0)
            {
                Console.WriteLine("Operación cancelada");
                Console.ReadLine();
                return;
            }

            var ordenEnDb = context.Ordenes.Include(o => o.Cliente)
                .FirstOrDefault(b => b.NumeroOrden == numeroOrden);
            if (ordenEnDb == null)
            {
                Console.WriteLine("Orden no existente");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"Fecha actual de la orden: {ordenEnDb.FechaOrden}");
            Console.Write("Ingrese la fecha nueva (o presione ENTER para mantener):");
            var nuevaFecha = Console.ReadLine();
            if (!string.IsNullOrEmpty(nuevaFecha))
            {
                if (!DateOnly.TryParse(nuevaFecha, out DateOnly fechaOrden) ||
                    fechaOrden > DateOnly.FromDateTime(DateTime.Today))
                {
                    Console.WriteLine("Fecha inválida");
                    Console.ReadLine();
                    return;
                }
                ordenEnDb.FechaOrden = fechaOrden;
            }

            Console.WriteLine($"Valor de la orden: {ordenEnDb.Valor}");
            Console.Write("Ingrese el nuevo valor de la orden (o presione ENTER para mantener):");
            var nuevoValor = Console.ReadLine();
            if (!string.IsNullOrEmpty(nuevoValor))
            {
                if (!int.TryParse(nuevoValor, out int ordenValor) || ordenValor <= 0)
                {
                    Console.WriteLine("Valor inválido");
                    Console.ReadLine();
                    return;
                }
                ordenEnDb.Valor = ordenValor;
            }

            Console.WriteLine($"Cliente actual asignado:{ordenEnDb.Cliente}");
            Console.WriteLine("Clientes disponibles");
            var clientes = context.Clientes
                .OrderBy(c => c.Id)
                .ToList();
            foreach (var cliente in clientes)
            {
                Console.WriteLine(cliente);
            }
            Console.Write("Seleccione el ID del cliente (presione ENTER para mantener o 0 para crear uno nuevo):");
            var nuevoCliente = Console.ReadLine();
            if (!string.IsNullOrEmpty(nuevoCliente))
            {
                if (!int.TryParse(nuevoCliente, out int clienteId) || clienteId < 0)
                {
                    Console.WriteLine("ID Inválido");
                    Console.ReadLine();
                    return;
                }
                if (clienteId > 0)
                {
                    var clienteExiste = context.Clientes.Any(a => a.Id == clienteId);
                    if (!clienteExiste)
                    {
                        Console.WriteLine("ID de cliente no encontrado");
                        Console.ReadLine();
                        return;
                    }
                    ordenEnDb.ClienteId = clienteId;

                }
                else
                {
                    Console.WriteLine("Ingresando nuevo Cliente");
                    Console.Write("Ingrese nombre:");
                    var nombre = Console.ReadLine();
                    Console.Write("Ingrese apellido:");
                    var apellido = Console.ReadLine();
                    Console.Write("Ingrese Dni:");
                    if (!int.TryParse(Console.ReadLine(), out int dni))
                    {
                        Console.WriteLine("DNI Inválido");
                        Console.ReadLine();
                        return;
                    }
                    var clienteExistente = context.Clientes.FirstOrDefault(
                            c => c.Dni == dni);

                    if (clienteExistente is not null)
                    {
                        Console.WriteLine($"El cliente {clienteExistente} ya existe");
                        Console.WriteLine("Se asignará el cliente existente a la orden");
                        ordenEnDb.ClienteId = clienteExistente.Id;
                    }
                    else
                    {
                        Cliente Cliente = new Cliente
                        {
                            Nombre = nombre ?? string.Empty,
                            Apellido = apellido ?? string.Empty,
                            Dni = dni
                        };

                        var validationContext = new ValidationContext(Cliente);
                        var errorMessages = new List<ValidationResult>();

                        bool isValid = Validator.TryValidateObject(Cliente, validationContext, errorMessages, true);

                        if (isValid)
                        {
                            //ordenEnDb.Cliente = Cliente;
                            //Alternativa Carlos
                            context.Clientes.Add(Cliente);
                            context.SaveChanges();
                            ordenEnDb.ClienteId = Cliente.Id;
                        }
                        else
                        {
                            foreach (var message in errorMessages)
                            {
                                Console.WriteLine(message);
                            }
                        }

                    }
                }

            }

            var ordenOriginal = context.Ordenes
                .AsNoTracking()
                .FirstOrDefault(o => o.NumeroOrden == ordenEnDb.NumeroOrden);

            Console.Write($"Estás seguro que vas a editar \"{ordenOriginal!}\"? (y/n):");
            var confirm = Console.ReadLine();
            try
            {
                if (confirm?.ToLower() == "y")
                {
                    context.SaveChanges();
                    Console.WriteLine("Orden editada");
                }
                else
                {
                    Console.WriteLine("Operación cancelada");
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
            return;


        }
    }

    private static void AnularOrden()
    {
        Console.Clear();
        Console.WriteLine("Anulando Orden");
        Console.WriteLine("Lista de ordenes para anular");
        using (var context = new MarketContext())
        {
            var ordenes = context.Ordenes
                .OrderBy(o => o.NumeroOrden)
                .Select(o => new
                {
                    o.NumeroOrden,
                    o.Cliente,
                    o.Valor
                }).ToList();
            foreach (var ord in ordenes)
            {
                Console.WriteLine(ord);
            }
            Console.Write("Seleccione número de orden a anular (0 para salir):");
            if (!int.TryParse(Console.ReadLine(), out int numeroOrden) || numeroOrden < 0)
            {
                Console.WriteLine("Número de orden inválido...");
                Console.ReadLine();
                return;
            }
            if (numeroOrden == 0)
            {
                Console.WriteLine("Cancelado por usuario");
                Console.ReadLine();
                return;
            }
            var anularOrden = context.Ordenes.FirstOrDefault(o => o.NumeroOrden == numeroOrden);
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
        int nuevoNumeroOrden;
        using (var context = new MarketContext())
        {
            var ultimaOrden = (context.Ordenes.OrderBy(o => o.NumeroOrden).FirstOrDefault());
            nuevoNumeroOrden = ultimaOrden!.NumeroOrden++;
            Console.Write($"El número de la orden será {nuevoNumeroOrden}");
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
                Console.WriteLine(cliente);
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
                NumeroOrden = nuevoNumeroOrden,
                Valor = valor,
                FechaOrden = fechaOrden,
                ClienteId = clienteId
            };

            var ordenesValidador = new ValidadorOrdenes();
            var validationResult = ordenesValidador.Validate(newOrden);

            if (validationResult.IsValid)
            {
                context.Ordenes.Add(newOrden);
                context.SaveChanges();
                Console.WriteLine("Orden Añadida Exitosamente");
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
                .Include(o => o.Cliente)
                .Select(o => new
                {
                    o.NumeroOrden,
                    o.Cliente
                })
                .ToList();
            foreach (var or in ordenes)
            {
                Console.WriteLine($"Número de orden: {or.NumeroOrden} - Cliente:{or.Cliente}");
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
                Console.WriteLine(cliente);
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