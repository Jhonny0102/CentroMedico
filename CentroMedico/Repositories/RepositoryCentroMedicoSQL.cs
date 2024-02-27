using CentroMedico.Data;
using CentroMedico.Models;
using System.Diagnostics.Contracts;

namespace CentroMedico.Repositories
{
    public class RepositoryCentroMedicoSQL : IRepositoryCentroMedico
    {
        private CentroMedicoContext context;
        public RepositoryCentroMedicoSQL(CentroMedicoContext context)
        {
            this.context = context;
        }

        public void AltaPaciente(string nombre, string apellido, string correo, string contra)
        {
            var consulta = from datos in this.context.Usuarios select datos;
            int maxId = (consulta.Max(a => a.Id)) + 1; //Punto de interrupcion para ver que hace.
            Usuario usuario = new Usuario();
            usuario.Id = maxId;
            usuario.Nombre = nombre;
            usuario.Apellido = apellido;
            usuario.Correo = correo;
            usuario.Contra = contra;
            usuario.Id_EstadoUsuario = 1;
            usuario.Id_TipoUsuario = 4;
            this.context.Usuarios.Add(usuario);
            this.context.SaveChanges();
        }

        public Usuario Login(string correo, string contra)
        {
            var consulta = from datos in this.context.Usuarios 
                           where datos.Correo == correo && datos.Contra == contra && datos.Id_EstadoUsuario == 1
                           select datos;
            return consulta.FirstOrDefault();
        }
        public Usuario FindUsuario(int id)
        {
            var consulta = from datos in this.context.Usuarios
                           where datos.Id == id
                           select datos;
            return consulta.FirstOrDefault();
        }

        public void EliminarUsuario(int id)
        {
            throw new NotImplementedException();
        }

        public void EditarUsuario(int id)
        {
            throw new NotImplementedException();
        }
    }
}
