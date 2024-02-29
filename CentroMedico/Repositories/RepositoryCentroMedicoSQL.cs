using CentroMedico.Data;
using CentroMedico.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics.Contracts;

#region VISTAS

//create view V_ALL_MEDICOS
//as
//	SELECT USUARIOS.ID, USUARIOS.NOMBRE, USUARIOS.APELLIDO, USUARIOS.CORREO, USUARIOS.CONTRA, USUARIOS.ID_ESTADOUSUARIO, USUARIOS.ID_TIPOUSUARIO, ESPECIALIDADES.ESPECIALIDAD
//	FROM USUARIOS
//	INNER JOIN MEDICOESPECIALIDAD 
//	ON USUARIOS.ID = MEDICOESPECIALIDAD.ID_MEDICO
//	INNER JOIN ESPECIALIDADES
//	ON MEDICOESPECIALIDAD.ID_ESPECIALIDAD = ESPECIALIDADES.ID
//go

#endregion


#region PROCEDURES

#endregion

namespace CentroMedico.Repositories
{
    public class RepositoryCentroMedicoSQL : IRepositoryCentroMedico
    {
        private CentroMedicoContext context;
        public RepositoryCentroMedicoSQL(CentroMedicoContext context)
        {
            this.context = context;
        }

        // 
        public Usuario GetLogin(string correo, string contra)
        {
            var consulta = from datos in this.context.Usuarios 
                           where datos.Correo == correo && datos.Contra == contra && datos.Id_EstadoUsuario == 1
                           select datos;
            return consulta.FirstOrDefault();
        }

        // 
        public void CreatePaciente(string nombre, string apellido, string correo, string contra)
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

        //Metodo para encontrar un PACIENTE
        public Paciente FindPaciente(int id)
        {
            var consulta = from datos in this.context.Paciente
                           where datos.Id == id
                           select datos;
            return consulta.FirstOrDefault();
        }

        //Metodo para encontrar un MEDICO
        public Medico FindMedico(int id)
        {
            var consulta = from datos in this.context.Medico
                           where datos.Id == id
                           select datos;
            return consulta.FirstOrDefault();
        }

        //Metodo para encontrar un USUARIO
        public Usuario FindUsuario(int id)
        {
            var consulta = from datos in this.context.Usuarios
                           where datos.Id == id
                           select datos;
            return consulta.FirstOrDefault();
        }

        //Metodo para eliminar un USUARIO
        public void DeleteUsuario(int id)
        {
            throw new NotImplementedException();
        }

        //MEtodo para editar un USUARIO
        public void EditUsuario(int id)
        {
            throw new NotImplementedException();
        }

        // 

        //Metodo para obtener todos los tipos de usuario que puede haber (Admin, Recepcionista,....)
        public List<UsuariosTipo> GetTipoUsuarios()
        {
            var consulta = from datos in this.context.UsuariosTipos
                           select datos;
            return consulta.ToList();
        }

        //Metodo para obtener los usuarios de un tipo especifico
        public List<Usuario> GetUsuariosTipo(int tipo)
        {
            var consulta = from datos in this.context.Usuarios
                           where datos.Id_TipoUsuario == tipo
                           select datos;
            return consulta.ToList();
        }

        //Metodo para obtener todos los usuarios que haya en la bbdd
        public List<Usuario> GetUsuarios()
        {
            var consulta = from datos in this.context.Usuarios
                           select datos;
            return consulta.ToList();
        }

    }
}
