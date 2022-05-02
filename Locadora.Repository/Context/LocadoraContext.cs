using Locadora.Domain.Entities;
using Locadora.Repository.Map;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Repository.Context
{
    public class LocadoraContext : DbContext
    {
        public LocadoraContext(DbContextOptions options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Locacao> Locacoes { get; set; }
        public DbSet<Filme> Filmes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClienteMapping());
            modelBuilder.ApplyConfiguration(new LocacaoMapping());
            modelBuilder.ApplyConfiguration(new FilmeMapping());
        }
    }
}
