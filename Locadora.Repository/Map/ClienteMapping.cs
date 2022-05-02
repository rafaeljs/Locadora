using Locadora.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locadora.Repository.Map
{
    public class ClienteMapping : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable(nameof(Cliente));

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Nome).HasMaxLength(200).IsRequired();
            builder.Property(c => c.CPF).HasMaxLength(11).IsRequired();
            builder.Property(c => c.DataNascimento).IsRequired();

            builder.HasIndex(c => c.Nome);
            builder.HasIndex(c => c.CPF);
        }
    }
}
