using Locadora.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locadora.Repository.Map
{
    public class LocacaoMapping : IEntityTypeConfiguration<Locacao>
    {
        public void Configure(EntityTypeBuilder<Locacao> builder)
        {
            builder.ToTable(nameof(Locacao));

            builder.HasKey(l => l.Id);
            builder.Property(l => l.DataLocacao).IsRequired();
            builder.Property(l => l.DataDevolucao);

            builder.HasOne(l => l.Cliente)
                .WithMany(c => c.Locacoes)
                .HasForeignKey(c => c.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(l => l.Filme)
                .WithMany(c => c.Locacoes)
                .HasForeignKey(c => c.FilmeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
