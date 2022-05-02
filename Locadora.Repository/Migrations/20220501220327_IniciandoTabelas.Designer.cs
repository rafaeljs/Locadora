﻿// <auto-generated />
using System;
using Locadora.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Locadora.Repository.Migrations
{
    [DbContext(typeof(LocadoraContext))]
    [Migration("20220501220327_IniciandoTabelas")]
    partial class IniciandoTabelas
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Locadora.Domain.Entities.Cliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("varchar(11)");

                    b.Property<DateTime>("DataNascimento")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("CPF");

                    b.HasIndex("Nome");

                    b.ToTable("Cliente", (string)null);
                });

            modelBuilder.Entity("Locadora.Domain.Entities.Filme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ClassificacaoIndicativa")
                        .HasColumnType("int");

                    b.Property<bool>("Lancamento")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("Lancamento");

                    b.HasIndex("Titulo");

                    b.ToTable("Filme", (string)null);
                });

            modelBuilder.Entity("Locadora.Domain.Entities.Locacao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DataDevolucao")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DataLocacao")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("FilmeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("FilmeId");

                    b.ToTable("Locacao", (string)null);
                });

            modelBuilder.Entity("Locadora.Domain.Entities.Locacao", b =>
                {
                    b.HasOne("Locadora.Domain.Entities.Cliente", "Cliente")
                        .WithMany("Locacoes")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Locadora.Domain.Entities.Filme", "Filme")
                        .WithMany("Locacoes")
                        .HasForeignKey("FilmeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Cliente");

                    b.Navigation("Filme");
                });

            modelBuilder.Entity("Locadora.Domain.Entities.Cliente", b =>
                {
                    b.Navigation("Locacoes");
                });

            modelBuilder.Entity("Locadora.Domain.Entities.Filme", b =>
                {
                    b.Navigation("Locacoes");
                });
#pragma warning restore 612, 618
        }
    }
}
