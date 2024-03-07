using CentroMedico.Data;
using CentroMedico.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Data;

#region VISTAS

//create view V_ALL_MEDICOS
//as
//	SELECT USUARIOS.ID, USUARIOS.NOMBRE, USUARIOS.APELLIDO, USUARIOS.CORREO, USUARIOS.CONTRA, USUARIOS.ID_ESTADOUSUARIO, USUARIOS.ID_TIPOUSUARIO,
//    MEDICOESPECIALIDAD.ID_ESPECIALIDAD
//	FROM USUARIOS
//	INNER JOIN MEDICOESPECIALIDAD 
//	ON USUARIOS.ID = MEDICOESPECIALIDAD.ID_MEDICO
//	WHERE USUARIOS.ID_TIPOUSUARIO = 3
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

//create procedure sp_delete_paciente
//(@id int)
//as
//    delete from datosextraspacientes 
//	where id_usuario=@id

//	delete from medicopaciente
//	where id_paciente=@id

//	delete from peticionesbajas
//	where id_usuario=@id

//	delete from medicamentopaciente
//	where id_paciente=@id

//	delete from medicopaciente
//	where id_paciente=@id

//	delete from citas
//	where id_paciente=@id
//	--esta consulta de accion de ultimo
//	delete from usuarios 
//	where id=@id
//go


//create procedure sp_delete_medico
//(@id int)
//as
//    delete from medicopaciente
//	where id_medico=@id

//	delete from peticionesbajas
//	where id_usuario=@id

//	delete from medicamentopaciente
//	where id_medico=@id

//	delete from medicopaciente
//	where id_medico=@id

//	delete from citas
//	where id_medico=@id

//	delete from medicoespecialidad
//	where id_medico=@id
//	--esta consulta de accion de ultimo
//	delete from usuarios 
//	where id=@id
//go


//create procedure sp_delete_usuario
//(@id int)
//as
//    delete from peticionesbajas
//	where id_usuario=@id

//	--esta consulta de accion de ultimo
//	delete from usuarios 
//	where id=@id
//go

//create procedure sp_edit_medico
//(@id int, @nombre nvarchar(50), @apellido nvarchar(50), @correo nvarchar(50), @contra nvarchar(50), @estado int, @tipo int , @especialidad int)
//as
//	update usuarios
//	set id=@id, nombre = @nombre, apellido = @apellido, correo = @correo, contra = @contra, id_estadousuario = @estado, id_tipousuario = @tipo
//	where id=@id
//	update medicoespecialidad
//	set id_especialidad=@especialidad
//	where id_medico=@id
//go

//create procedure sp_edit_paciente
//(@id int, @nombre nvarchar(50), @apellido nvarchar(50), @correo nvarchar(50), @contra nvarchar(50), @estado int, @tipo int , @telefono int, @direccion nvarchar(50), @edad int, @genero nvarchar(50))
//as
//	update usuarios
//	set id=@id, nombre = @nombre, apellido = @apellido, correo = @correo, contra = @contra, id_estadousuario = @estado, id_tipousuario = @tipo
//	where id=@id
//	update datosextraspacientes
//	set telefono=@telefono, direccion = @direccion, edad = @edad, genero = @genero
//	where id_usuario=@id
//go

//create procedure sp_insert_medico
//(@nombre nvarchar(50), @apellido nvarchar(50), @correo nvarchar(50), @contra nvarchar(50), @especialidad int , @estado int, @tipo int )
//as
//	DECLARE @idMaxUsuario INT
//	SELECT @idMaxUsuario = MAX(ID) + 1 FROM USUARIOS
//	INSERT INTO USUARIOS (ID, NOMBRE, APELLIDO, CORREO, CONTRA, ID_ESTADOUSUARIO, ID_TIPOUSUARIO) VALUES (@idMaxUsuario, @nombre, @apellido, @correo, @contra, @estado, @tipo)
//	DECLARE @idMaxMedicoEspecialidad int
//	SELECT @idMaxMedicoEspecialidad = MAX(ID) + 1 FROM MEDICOESPECIALIDAD
//	INSERT INTO MEDICOESPECIALIDAD (ID, ID_MEDICO, ID_ESPECIALIDAD) VALUES (@idMaxMedicoEspecialidad, @idMaxUsuario, @especialidad)
//go

//create procedure sp_insert_paciente
//( @nombre nvarchar(50), @apellido nvarchar(50), @correo nvarchar(50), @contra nvarchar(50), @telefono int, @direccion nvarchar(50), @edad int, @genero nvarchar(50), @medico int)
//as
//	DECLARE @idMaxUsuario INT
//	SELECT @idMaxUsuario = MAX(ID) + 1 FROM USUARIOS
//	INSERT INTO USUARIOS (ID, NOMBRE, APELLIDO, CORREO, CONTRA, ID_ESTADOUSUARIO, ID_TIPOUSUARIO) VALUES (@idMaxUsuario, @nombre, @apellido, @correo, @contra,1,4)

//	DECLARE @idMaxDatosExtras int
//	SELECT @idMaxDatosExtras = MAX(ID) + 1 FROM DATOSEXTRASPACIENTES
//	INSERT INTO DATOSEXTRASPACIENTES (ID, ID_USUARIO, TELEFONO, DIRECCION, EDAD, GENERO) VALUES (@idMaxDatosExtras, @idMaxUsuario, @telefono, @direccion, @edad, @genero)

//	DECLARE @idMaxMedicoPaciente int
//	SELECT @idMaxMedicoPaciente = MAX(ID) + 1 FROM MEDICOPACIENTE
//	INSERT INTO MEDICOPACIENTE (ID, ID_MEDICO, ID_PACIENTE) VALUES (@idMaxMedicoPaciente, @medico, @idMaxUsuario)
//go

//CREATE PROCEDURE SP_DETALLES_TUMEDICO
//(@idPaciente int, @idMedico int out)
//AS
//	select @idMedico = USUARIOS.ID from USUARIOS 
//	where ID = 
//	( 
//		select ID_MEDICO from MEDICOPACIENTE where ID_PACIENTE=@idPaciente
//	)	
//GO

//CREATE PROCEDURE SP_IDS_MEDICOPACIENTE
//(@idCita int , @idMedico int OUT , @idPaciente int OUT)
//AS
//	SELECT @idMedico = ID_MEDICO FROM CITAS WHERE ID=@idCita
//	SELECT @idPaciente = ID_PACIENTE FROM CITAS WHERE ID=@idCita
//GO



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

        //Metodo para crear PACIENTE
        public void CreatePaciente(string nombre, string apellido, string correo, string contra , int telefono , string direccion,int edad , string genero, int medico)
        {
            string sql = "sp_insert_paciente  @nombre, @apellido, @correo, @contra, @telefono, @direccion, @edad, @genero, @medico";
            SqlParameter pamNombre = new SqlParameter("@nombre", nombre);
            SqlParameter pamApellido = new SqlParameter("@apellido", apellido);
            SqlParameter pamCorreo = new SqlParameter("@correo", correo);
            SqlParameter pamContra = new SqlParameter("@contra", contra);
            SqlParameter pamTelefono = new SqlParameter("@telefono", telefono);
            SqlParameter pamDireccion = new SqlParameter("@direccion", direccion);
            SqlParameter pamEdad = new SqlParameter("@edad", edad);
            SqlParameter pamGenero = new SqlParameter("@genero", genero);
            SqlParameter pamMedico = new SqlParameter("@medico", medico);
            this.context.Database.ExecuteSqlRaw(sql, pamNombre, pamApellido, pamCorreo, pamContra, pamTelefono, pamDireccion, pamEdad, pamGenero, pamMedico);
        }

        //Metodo para crear MEDICO
        public void CreateMedico(string nombre, string apellido, string correo, string contra, int especialidad)
        {
            string sql = "sp_insert_medico @nombre, @apellido, @correo, @contra, @especialidad";
            SqlParameter pamNombre = new SqlParameter("@nombre", nombre);
            SqlParameter pamApellido = new SqlParameter("@apellido", apellido);
            SqlParameter pamCorreo = new SqlParameter("@correo", correo);
            SqlParameter pamContra = new SqlParameter("@contra", contra);
            SqlParameter pamEspecialidad = new SqlParameter("@especialidad", especialidad);
            this.context.Database.ExecuteSqlRaw(sql, pamNombre, pamApellido, pamCorreo, pamContra, pamEspecialidad);
        }

        //Metodo para crear USUARIO
        public void CreateUsuario(string nombre, string apellido, string correo, string contra , int tipo)
        {
            var consulta = from datos in this.context.Usuarios select datos;
            int maxId = (consulta.Max(a => a.Id)) + 1;
            Usuario usuario = new Usuario();
            usuario.Id = maxId;
            usuario.Nombre = nombre;
            usuario.Apellido = apellido;
            usuario.Correo = correo;
            usuario.Contra = contra;
            usuario.Id_EstadoUsuario = 1;
            usuario.Id_TipoUsuario = tipo;
            this.context.Usuarios.Add(usuario);
            this.context.SaveChanges();
        }

        //Metodo para encontrar MEDICOS segun la especialidad y devolver de forma aleatoria un medico
        public int GetIdMedico(int especialidad)
        {
            var consulta = from datos in this.context.MedicoEspecialidad
                           where datos.Id_Especialidad == especialidad
                           select datos.Id_Medico;
            var medicoAleatorio = consulta.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
            return medicoAleatorio;
        }

        //Metodo para devolver la informacion de TU MEDICO
        public MedicoDetallado GetMiMedico(int idPaciente)
        {
            // Define los parámetros
            var idMedicoParametro = new SqlParameter("@idMedico", SqlDbType.Int);
            idMedicoParametro.Direction = ParameterDirection.Output;

            var idPacienteParametro = new SqlParameter("@idPaciente", idPaciente);

            // Ejecuta el procedimiento almacenado
            this.context.Database.ExecuteSqlRaw("EXEC SP_DETALLES_TUMEDICO @idPaciente, @idMedico OUTPUT", idPacienteParametro, idMedicoParametro);

            // Obtiene el valor de @idMedico después de ejecutar el procedimiento almacenado
            var idMedicoResultado = (int)idMedicoParametro.Value; //Da error si no tiene un medico asignado , ver si es necesario corregirlo

            MedicoDetallado medico = this.FindMedicoDetallado(idMedicoResultado);
            return medico;
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
        public void EditUsuario(int id,string nombre, string apellido, string correo, string contra,int estado, int tipo)
        {
            Usuario usuario = this.FindUsuario(id);
            usuario.Id = id;
            usuario.Nombre = nombre;
            usuario.Apellido = apellido;
            usuario.Correo = correo;
            usuario.Contra = contra;
            usuario.Id_EstadoUsuario = estado;
            usuario.Id_TipoUsuario = tipo;
            this.context.SaveChanges();

        }

        //Metodo para editar un MEDICO
        public void EditMedico(int id, string nombre, string apellido, string correo, string contra, int estado, int tipo , int especialidad)
        {
            string sql = "sp_edit_medico @id, @nombre, @apellido, @correo, @contra, @estado, @tipo, @especialidad";
            SqlParameter pamId = new SqlParameter("@id", id);
            SqlParameter pamNombre = new SqlParameter("@nombre", nombre);
            SqlParameter pamApellido = new SqlParameter("@apellido", apellido);
            SqlParameter pamCorreo = new SqlParameter("@correo", correo);
            SqlParameter pamContra = new SqlParameter("@contra", contra);
            SqlParameter pamEspecialidad = new SqlParameter("@especialidad", especialidad);
            SqlParameter pamEstado = new SqlParameter("@estado", estado);
            SqlParameter pamTipo = new SqlParameter("@tipo", tipo);
            this.context.Database.ExecuteSqlRaw(sql, pamId,pamNombre,pamApellido,pamCorreo,pamContra,pamEspecialidad,pamEstado,pamTipo);
        }

        //Metodo para editar un PACIENTE (*** Problema, si el paciente no tiene toda la informacion completa no lo actualiza)
        public void EditPaciente(int id, string nombre, string apellido, string correo, string contra, int telefono, string direccion, int edad, string genero, int estado, int tipo)
        {
            string sql = "sp_edit_paciente @id, @nombre, @apellido, @correo, @contra, @estado, @tipo, @telefono, @direccion, @edad, @genero";
            SqlParameter pamId = new SqlParameter("@id", id);
            SqlParameter pamNombre = new SqlParameter("@nombre", nombre);
            SqlParameter pamApellido = new SqlParameter("@apellido", apellido);
            SqlParameter pamCorreo = new SqlParameter("@correo", correo);
            SqlParameter pamContra = new SqlParameter("@contra", contra);
            SqlParameter pamEstado = new SqlParameter("@estado", estado);
            SqlParameter pamTipo = new SqlParameter("@tipo", tipo);
            SqlParameter pamTelefono = new SqlParameter("@telefono", telefono);
            SqlParameter pamDireccion = new SqlParameter("@direccion", direccion);
            SqlParameter pamEdad = new SqlParameter("@edad", edad);
            SqlParameter pamGenero = new SqlParameter("@genero", genero);
            this.context.Database.ExecuteSqlRaw(sql, pamId, pamNombre, pamApellido, pamCorreo, pamContra, pamEstado, pamTipo, pamTelefono ,pamDireccion, pamEdad, pamGenero);
        }

        // 

        //Metodo para obtener todos los tipos de usuario que puede haber (Admin, Recepcionista,Medico,Paciente)
        public List<UsuariosTipo> GetTipoUsuarios()
        {
            var consulta = from datos in this.context.UsuariosTipos
                           select datos;
            return consulta.ToList();
        }

        //Metodo para obtener todas las especialidades de los medicos
        public List<Especialidades> GetEspecialidades()
        {
            var consulta = from datos in this.context.Especialidades
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

        //Metodo para obtener todas las citas
        public List<Cita> GetAllCitas()
        {
            var consulta = from datos in this.context.Citas
                           select datos;
            return consulta.ToList();
        }

        //Metodo para encontrar una CITA
        public Cita FindCita(int idCita)
        {
            return this.context.Citas.FirstOrDefault(z => z.Id == idCita);
        }

        public void DeleteCita(int idCita)
        {
            Cita cita = this.FindCita(idCita);
            this.context.Remove(cita);
            this.context.SaveChanges();
        }

        public void EditCita(int idCita, DateTime fecha, TimeSpan hora, int idEstadoCita, int idMedico, string comentario)
        {
            Cita cita = this.FindCita(idCita);
            cita.Fecha = fecha;
            cita.Hora = hora;
            cita.EstadoCita = idEstadoCita;
            cita.Medico = idMedico;
            cita.Comentario = comentario;
            this.context.SaveChanges();
        }
    }
}
