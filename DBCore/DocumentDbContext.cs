using Common.BL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DbCore
{
    public class DocumentDbContext : DbContext
    {
        //Constructor sin parametros
        public DocumentDbContext()
        {
        }

        //Constructor con parametros para la configuracion
        public DocumentDbContext(DbContextOptions options) : base(options)
        {
        }

        //Sobreescribimos el metodo OnConfiguring para hacer los ajustes que queramos en caso de
        //llamar al constructor sin parametros
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //En caso de que el contexto no este configurado, lo configuramos mediante la cadena de conexion
            if (!optionsBuilder.IsConfigured)
            {
                //var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));

                IConfigurationRoot configuration = new ConfigurationBuilder()
                   // .SetBasePath(Directory.GetCurrentDirectory
                  .SetBasePath(AppContext.BaseDirectory)
                  .AddJsonFile("appsettings.json")
                  .Build();
                var connectionString = configuration.GetConnectionString("DatabaseConnection");
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
        }

        //Tablas de datos
        public virtual DbSet<Documentos> Documento { get; set; }
        public virtual DbSet<EstadosSII> DocumentoEstado { get; set; }
    }
}

