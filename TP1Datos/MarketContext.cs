using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TP1Entities;


namespace TP1Datos
{
    public class MarketContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Orden> Ordenes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DANIEL\SQLEXPRESS; Initial Catalog=OrdenesDb; Trusted_Connection=true; TrustServerCertificate=true;")
               .EnableSensitiveDataLogging() // Permite ver valores en las consultas
               .LogTo(Console.WriteLine, LogLevel.Information)
               .UseLazyLoadingProxies(false);//habilita Lazy Loading
        }
        //FluentAPI
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("Clientes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Apellido).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Dni).HasMaxLength(100).IsRequired();

                var clienteList = new List<Cliente>
            {
                    new Cliente{Id=1,Nombre = "Daniel", Apellido= "Marhon", Dni = 40377284},
            };
                entity.HasData(clienteList);
            });

            modelBuilder.Entity<Orden>(entity =>
            {
        entity.ToTable("Ordenes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NumeroOrden).HasMaxLength(100).IsRequired();
                entity.Property(e => e.FechaOrden).HasColumnType("Date").IsRequired();
                entity.Property(e => e.Valor).HasMaxLength(100).IsRequired();
                entity.Property(e => e.ClienteId).HasMaxLength(100).IsRequired();
                entity.HasIndex(e => new { e.NumeroOrden, e.ClienteId }, "Orden_Numero_ClienteId").IsUnique();
                entity.HasOne(e => e.Cliente).WithMany(e => e.Ordenes).HasForeignKey(e => e.ClienteId)
                    .OnDelete(DeleteBehavior.ClientNoAction);

                var ordenList = new List<Orden>
            {
                    new Orden{Id=1,NumeroOrden = 1, FechaOrden=DateOnly.FromDateTime(new DateTime(1986,10,30)), ClienteId = 1},
                    new Orden{Id=2,NumeroOrden = 2, FechaOrden=DateOnly.FromDateTime(new DateTime(1953,10,10)), ClienteId = 1}
            };
                entity.HasData(ordenList);
            });
        }
    }
}