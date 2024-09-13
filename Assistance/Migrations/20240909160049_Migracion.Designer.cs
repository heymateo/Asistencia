﻿// <auto-generated />
using System;
using Assistance.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Assistance.Migrations
{
    [DbContext(typeof(AssistanceDbContext))]
    [Migration("20240909160049_Migracion")]
    partial class Migracion
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("Assistance.Models.Admin", b =>
                {
                    b.Property<int>("Id_Admin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.Property<string>("User")
                        .HasColumnType("TEXT");

                    b.HasKey("Id_Admin");

                    b.ToTable("Admin");
                });

            modelBuilder.Entity("Assistance.Models.Centro_Educativo", b =>
                {
                    b.Property<int>("Id_Centro")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Logo")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nombre_Centro")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Tipo_Institucion")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id_Centro");

                    b.ToTable("Centro_Educativo");
                });

            modelBuilder.Entity("Assistance.Models.Estudiante", b =>
                {
                    b.Property<int>("Id_Estudiante")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Encargado_Legal")
                        .HasColumnType("TEXT");

                    b.Property<string>("Especialidad")
                        .HasColumnType("TEXT");

                    b.Property<string>("Grupo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Identificacion")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Nivel")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Seccion")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Telefono_Encargado")
                        .HasColumnType("TEXT");

                    b.HasKey("Id_Estudiante");

                    b.ToTable("Estudiante");
                });

            modelBuilder.Entity("Assistance.Models.Registro_Asistencia", b =>
                {
                    b.Property<int>("Id_Registro")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Asistio")
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("Fecha")
                        .HasColumnType("TEXT");

                    b.Property<TimeOnly>("Hora_Entrada")
                        .HasColumnType("TEXT");

                    b.Property<int>("Id_Estudiante")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id_Registro");

                    b.ToTable("Registro_Asistencia");
                });
#pragma warning restore 612, 618
        }
    }
}
