﻿using CentroMedico.Models;
using Microsoft.EntityFrameworkCore;

namespace CentroMedico.Data
{
    public class CentroMedicoContext : DbContext
    {
        public CentroMedicoContext(DbContextOptions<CentroMedicoContext> options)
            :base(options)
        {}

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UsuariosTipo> UsuariosTipos { get; set; }
        public DbSet<Paciente> Paciente { get; set;}
        public DbSet<PacienteDetallado> PacienteDetallado { get; set; }
        public DbSet<Medico> Medico { get; set;}
        public DbSet<MedicoDetallado> MedicoDetallado { get; set; }
        public DbSet<UsuarioDetallado> UsuarioDetallado { get; set; }
        public DbSet<DatosExtrasPacientes> DatosExtrasPacientes { get; set; }
        public DbSet<Especialidades> Especialidades { get; set;}
        public DbSet<Citas> Citas { get; set;}
        public DbSet<MedicoEspecialidad> MedicoEspecialidad { get; set; }
        public DbSet<MedicoPaciente> MedicoPaciente { get; set; }
    }
}
