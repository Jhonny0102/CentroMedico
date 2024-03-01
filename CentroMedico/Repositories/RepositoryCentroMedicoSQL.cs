using CentroMedico.Data;
using CentroMedico.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
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

//create view V_ALL_PACIENTES
//as
//	SELECT USUARIOS.ID, USUARIOS.NOMBRE, USUARIOS.APELLIDO, USUARIOS.CORREO, USUARIOS.CONTRA, USUARIOS.ID_ESTADOUSUARIO, USUARIOS.ID_TIPOUSUARIO,
//    DATOSEXTRASPACIENTES.TELEFONO, DATOSEXTRASPACIENTES.DIRECCION, DATOSEXTRASPACIENTES.EDAD, DATOSEXTRASPACIENTES.GENERO
//	FROM USUARIOS
//	LEFT JOIN DATOSEXTRASPACIENTES
//	ON USUARIOS.ID = DATOSEXTRASPACIENTES.ID_USUARIO
//	WHERE USUARIOS.ID_TIPOUSUARIO = 4
//go

//create view V_ALL_PACIENTES_DETALLADO
//as
//	SELECT USUARIOS.ID, USUARIOS.NOMBRE, USUARIOS.APELLIDO, USUARIOS.CORREO, USUARIOS.CONTRA,
//    DATOSEXTRASPACIENTES.TELEFONO, DATOSEXTRASPACIENTES.DIRECCION, DATOSEXTRASPACIENTES.EDAD, DATOSEXTRASPACIENTES.GENERO ,
//    ESTADOUSUARIO.ESTADO ,
//    TIPOUSUARIOS.TIPO
//	FROM USUARIOS
//	LEFT JOIN DATOSEXTRASPACIENTES
//	ON USUARIOS.ID = DATOSEXTRASPACIENTES.ID_USUARIO
//	INNER JOIN ESTADOUSUARIO
//	ON USUARIOS.ID_ESTADOUSUARIO = ESTADOUSUARIO.ID
//	INNER JOIN TIPOUSUARIOS
//	ON USUARIOS.ID_TIPOUSUARIO = TIPOUSUARIOS.ID
//	WHERE USUARIOS.ID_TIPOUSUARIO = 4
//go

//create view V_ALL_MEDICOS_DETALLADO
//as
//	SELECT USUARIOS.ID, USUARIOS.NOMBRE, USUARIOS.APELLIDO, USUARIOS.CORREO, USUARIOS.CONTRA, ESPECIALIDADES.ESPECIALIDAD ,
//    ESTADOUSUARIO.ESTADO ,
//    TIPOUSUARIOS.TIPO
//	FROM USUARIOS
//	INNER JOIN MEDICOESPECIALIDAD 
//	ON USUARIOS.ID = MEDICOESPECIALIDAD.ID_MEDICO
//	INNER JOIN ESPECIALIDADES
//	ON MEDICOESPECIALIDAD.ID_ESPECIALIDAD = ESPECIALIDADES.ID
//	INNER JOIN ESTADOUSUARIO
//	ON USUARIOS.ID_ESTADOUSUARIO = ESTADOUSUARIO.ID
//	INNER JOIN TIPOUSUARIOS
//	ON USUARIOS.ID_TIPOUSUARIO = TIPOUSUARIOS.ID
//	WHERE USUARIOS.ID_TIPOUSUARIO = 3
//go

//create view V_ALL_USUARIOS_DETALLADOS
//as
//	SELECT USUARIOS.ID, USUARIOS.NOMBRE, USUARIOS.APELLIDO, USUARIOS.CORREO, USUARIOS.CONTRA,
//    ESTADOUSUARIO.ESTADO,
//    TIPOUSUARIOS.TIPO
//	FROM USUARIOS
//	INNER JOIN ESTADOUSUARIO
//	ON USUARIOS.ID_ESTADOUSUARIO = ESTADOUSUARIO.ID
//	INNER JOIN TIPOUSUARIOS
//	ON USUARIOS.ID_TIPOUSUARIO = TIPOUSUARIOS.ID
//	WHERE USUARIOS.ID_TIPOUSUARIO = 1 OR USUARIOS.ID_TIPOUSUARIO = 2
//go

#endregion


#region PROCEDURES

//create procedure SP_DELETE_PACIENTE
//(@id int)
//as
//    DELETE FROM DATOSEXTRASPACIENTES 
//	WHERE ID_USUARIO=@id

//	DELETE FROM MEDICOPACIENTE
//	WHERE ID_PACIENTE=@id

//	DELETE FROM PETICIONESBAJAS
//	WHERE ID_USUARIO=@id

//	DELETE FROM MEDICAMENTOPACIENTE
//	WHERE ID_PACIENTE=@id

//	DELETE FROM MEDICOPACIENTE
//	WHERE ID_PACIENTE=@id

//	DELETE FROM CITAS
//	WHERE ID_PACIENTE=@id
//	--Esta consulta de accion de ultimo
//	DELETE FROM USUARIOS 
//	WHERE ID=@id
//go


//create procedure SP_DELETE_MEDICO
//(@id int)
//as
//    DELETE FROM MEDICOPACIENTE
//	WHERE ID_MEDICO=@id

//	DELETE FROM PETICIONESBAJAS
//	WHERE ID_USUARIO=@id

//	DELETE FROM MEDICAMENTOPACIENTE
//	WHERE ID_MEDICO=@id

//	DELETE FROM MEDICOPACIENTE
//	WHERE ID_MEDICO=@id

//	DELETE FROM CITAS
//	WHERE ID_MEDICO=@id

//	DELETE FROM MEDICOESPECIALIDAD
//	WHERE ID_MEDICO=@id
//	--Esta consulta de accion de ultimo
//	DELETE FROM USUARIOS 
//	WHERE ID=@id
//go


//create procedure SP_DELETE_USUARIO
//(@id int)
//as
//    DELETE FROM PETICIONESBAJAS
//	WHERE ID_USUARIO=@id

//	--Esta consulta de accion de ultimo
//	DELETE FROM USUARIOS 
//	WHERE ID=@id
//go

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

        //Metodo para encontrar un PACIENTE DETALLADO(Solo muestra string)
        public PacienteDetallado FindPacienteDetallado(int id)
        {
            var consulta = from datos in this.context.PacienteDetallado
                           where datos.Id == id
                           select datos;
            return consulta.FirstOrDefault();
        }

        //No usado aun
        public DatosExtrasPacientes FindDatosExtrasPacientes(int id)
        {
            var consulta = from datos in this.context.DatosExtrasPacientes
                           where datos.Id_Usuario == id
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

        //Metodo para encontrar un MEDICO DETALLADO (Solo muestra string)
        public MedicoDetallado FindMedicoDetallado(int id)
        {
            var consulta = from datos in this.context.MedicoDetallado
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

        //Metodo para encontrar un USUARIO DETALLADO (Solo muestra string)
        public UsuarioDetallado FindUsuarioDetallado(int id)
        {
            var consulta = from datos in this.context.UsuarioDetallado
                           where datos.Id == id
                           select datos;
            return consulta.FirstOrDefault();
        }

        //Metodo para eliminar un USUARIO (Cuidado con el namesepace (Si -> Microsoft.Data.SqlCliente, No -> System.Data.SqlCLiente))
        public void DeleteUsuario(int id , int tipo)
        {
            if (tipo == 4)
            {
                string sql = "SP_DELETE_PACIENTE @id";
                SqlParameter pamId = new SqlParameter("@id",id);
                this.context.Database.ExecuteSqlRaw(sql,pamId);
            }
            else if (tipo == 3)
            {
                string sql = "SP_DELETE_MEDICO @id";
                SqlParameter pamId = new SqlParameter("@id", id);
                this.context.Database.ExecuteSqlRaw(sql, pamId);
            }
            else
            {
                string sql = "SP_DELETE_USUARIO @id";
                SqlParameter pamId = new SqlParameter("@id", id);
                this.context.Database.ExecuteSqlRaw(sql, pamId);
            }

        }

        //Metodo para editar un USUARIO
        public void EditUsuario(int id, int tipo)
        {
            //Queremos primero, saber que tipo de usuario es el que queremos editar
            //Segundo, mostrar los datos de este en un formulario , ver si con model detallado o no . Y ver que necesitamos
            //Tercero, guardar los datos nuevos y redirigirlo de nuevo a la zonaAdminUsuarios
            throw new NotImplementedException();
        }

        // 

        //Metodo para obtener todos los tipos de usuario que puede haber (Admin, Recepcionista,Medico,Paciente)
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
