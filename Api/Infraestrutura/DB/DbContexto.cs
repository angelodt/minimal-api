using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.Entidades;

namespace MinimalApi.Infraestrutura.DB
{
    public class DbContexto : DbContext
    {
        public DbSet<Administrador> Administradores { get; set; } = default;

        public DbSet<Veiculo> Veiculos { get; set; } = default;

        private readonly IConfiguration _configurationAppSettings;

        public DbContexto(IConfiguration config) {
            _configurationAppSettings = config;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<Administrador>().HasData(new Administrador {
                Id = 1,
                Email = "administrador@teste.com",
                Senha = "123456",
                Perfil = "Administrador"
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                var connectionStr = _configurationAppSettings.GetConnectionString("mysql")?.ToString();
                
                if(!string.IsNullOrEmpty(connectionStr)) {
                    optionsBuilder.UseMySql(connectionStr,
                    ServerVersion.AutoDetect(connectionStr));
                }
            }            
        }
    }
}