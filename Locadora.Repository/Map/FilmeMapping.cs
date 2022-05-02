using Locadora.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locadora.Repository.Map
{
    public class FilmeMapping : IEntityTypeConfiguration<Filme>
    {
        public void Configure(EntityTypeBuilder<Filme> builder)
        {
            builder.ToTable(nameof(Filme));

            builder.HasKey(f => f.Id);
            builder.Property(f => f.Titulo).HasMaxLength(100).IsRequired();
            builder.Property(f => f.ClassificacaoIndicativa).IsRequired();
            builder.Property(f => f.Lancamento).IsRequired();

            builder.HasIndex(f => f.Titulo);
            builder.HasIndex(f => f.Lancamento);
        }
    }
}
