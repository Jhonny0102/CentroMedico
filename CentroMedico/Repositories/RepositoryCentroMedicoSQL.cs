using CentroMedico.Data;
using CentroMedico.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Data;
using Microsoft.VisualBasic;
using System.ComponentModel;
using System;

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

//CREATE VIEW V_CITAS_DETALLADO
//AS
//	SELECT CITAS.ID, CITAS.FECHA, CITAS.HORA, SEGUIMIENTOCITA.ESTADO, USUARIOS.APELLIDO AS MEDICO, CITAS.COMENTARIO
//	FROM CITAS
//	INNER JOIN USUARIOS
//	ON CITAS.ID_MEDICO = USUARIOS.ID
//	INNER JOIN SEGUIMIENTOCITA
//	ON CITAS.ID_SEGUIMIENTOCITA=SEGUIMIENTOCITA.ID
//GO

//CREATE VIEW V_PACIENTESMEDICOS
//AS
//	SELECT ROW_NUMBER() OVER (ORDER BY APELLIDO) AS IDP , MEDICOPACIENTE.ID_MEDICO, USUARIOS.ID, USUARIOS.NOMBRE, USUARIOS.APELLIDO, USUARIOS.CORREO
//	FROM MEDICOPACIENTE
//	INNER JOIN USUARIOS 
//	ON MEDICOPACIENTE.ID_PACIENTE=USUARIOS.ID
//GO

//CREATE VIEW V_DETAILS_PETICIONES
//AS
//	SELECT PETICIONESUSUARIOS.ID AS IDPETICION, PETICIONESUSUARIOS.ID_USUARIO_SOLICITANTE AS IDRECEPCIONISTA, USUARIOS.ID AS IDUSUARIO, USUARIOS.NOMBRE, USUARIOS.APELLIDO, ESTADOUSUARIO.ESTADO  FROM PETICIONESUSUARIOS
//	INNER JOIN USUARIOS
//	ON USUARIOS.ID = PETICIONESUSUARIOS.ID_USUARIO_MODIFICAR
//	INNER JOIN ESTADOUSUARIO
//	ON PETICIONESUSUARIOS.ID_ESTADO_NUEVO=ESTADOUSUARIO.ID
//GO

//CREATE VIEW V_DETAILS_PETICIONES_MEDICAMENTOS
//AS
//	SELECT 
//	PETICIONESMEDICAMENTOS.ID AS IDPETICION, PETICIONESMEDICAMENTOS.ID_MEDICO AS IDMEDICO_SOLICITANTE ,
//    PETICIONESMEDICAMENTOS.NOMBRE AS PETISMEDICAMENTO_NOMBRE, PETICIONESMEDICAMENTOS.DESCRIPCION AS PETISMEDICAMENTO_DESC,
//    PETICIONESMEDICAMENTOS.ID_ESTADO_NUEVO AS PETISMEDICAMENTO_ESTADO,
//    USUARIOS.NOMBRE AS NOMBREMEDICO, USUARIOS.APELLIDO APELLIDOMEDICO,
//    MEDICAMENTOS.ID AS IDMEDICAMENTO, MEDICAMENTOS.NOMBRE,
//    ESTADOUSUARIO.ESTADO
//	FROM PETICIONESMEDICAMENTOS
//	INNER JOIN USUARIOS
//	ON PETICIONESMEDICAMENTOS.ID_MEDICO=USUARIOS.ID
//	LEFT JOIN MEDICAMENTOS
//	ON PETICIONESMEDICAMENTOS.ID_MEDICAMENTO=MEDICAMENTOS.ID
//	INNER JOIN ESTADOUSUARIO
//	ON PETICIONESMEDICAMENTOS.ID_ESTADO_NUEVO=ESTADOUSUARIO.ID
//GO

//CREATE VIEW V_CITASMEDICOS_FILTRO
//AS
//	SELECT CITAS.ID, CITAS.FECHA, CITAS.HORA, CITAS.ID_MEDICO, CITAS.ID_PACIENTE , USUARIOS.NOMBRE , USUARIOS.APELLIDO , CITAS.ID_ESTADOCITA , CITAS.COMENTARIO
//	FROM CITAS 
//	INNER JOIN USUARIOS
//	ON CITAS.ID_PACIENTE = USUARIOS.ID
//GO

//CREATE VIEW V_MEDICAMENTOSYPACIENTES
//AS
//	SELECT  MEDICAMENTOPACIENTE.ID , MEDICAMENTOPACIENTE.ID_MEDICAMENTO AS IDMEDICAMENTO, MEDICAMENTOS.NOMBRE AS NOMBREMEDICAMENTO , MEDICAMENTOS.DESCRIPCION ,
//    MEDICAMENTOPACIENTE.ID_MEDICO AS IDMEDICO, USUARIOS.NOMBRE , USUARIOS.APELLIDO ,
//    MEDICAMENTOPACIENTE.ID_PACIENTE AS IDPACIENTE ,
//    MEDICAMENTOPACIENTE.ID_DISPOMEDICAMENTO AS IDDISPOMEDICAMENTO, DISPONIBILIDADMEDICAMENTOS.ESTADO
//	FROM MEDICAMENTOPACIENTE
//	INNER JOIN USUARIOS
//	ON MEDICAMENTOPACIENTE.ID_MEDICO=USUARIOS.ID
//	INNER JOIN DISPONIBILIDADMEDICAMENTOS
//	ON MEDICAMENTOPACIENTE.ID_DISPOMEDICAMENTO=DISPONIBILIDADMEDICAMENTOS.ID
//	INNER JOIN MEDICAMENTOS
//	ON MEDICAMENTOPACIENTE.ID_MEDICAMENTO= MEDICAMENTOS.ID
//GO
#endregion

#region PROCEDURES

//create procedure sp_delete_paciente
//(@id int)
//as
//    delete from datosextraspacientes 
//	where id_usuario=@id

//	delete from medicopaciente
//	where id_paciente=@id

//	delete from peticionesusuarios
//	where id_usuario_modificar=@id

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

//CREATE VIEW V_DETAILS_PETICIONES
//AS
//	SELECT 
//	PETICIONESUSUARIOS.ID AS IDPETICION,
//    PETICIONESUSUARIOS.ID_USUARIO_SOLICITANTE AS IDRECEPCIONISTA,
//    PETICIONESUSUARIOS.ID_ESTADO_NUEVO AS NUEVOESTADO,
//    USUARIOS.ID AS IDUSUARIO, USUARIOS.NOMBRE, USUARIOS.APELLIDO, ESTADOUSUARIO.ESTADO  FROM PETICIONESUSUARIOS
//	INNER JOIN USUARIOS
//	ON USUARIOS.ID = PETICIONESUSUARIOS.ID_USUARIO_MODIFICAR
//	INNER JOIN ESTADOUSUARIO
//	ON PETICIONESUSUARIOS.ID_ESTADO_NUEVO=ESTADOUSUARIO.ID
//GO

//CREATE PROCEDURE SP_OKNOPETICION_USUARIO
//(@idPeticion int)
//AS
//	DELETE FROM PETICIONESUSUARIOS WHERE ID=@idPeticion;
//GO

//--Puede haber dos opciones al dar de alta un medicamento:
//--Con ID, que solo cambiaria de estado de 2(baja) a 1(alta)
//CREATE PROCEDURE SP_OKPETI_MEDICAMENTO_CON_ID
//(@idPeti int, @idMedicamento int, @idEstado_Nuevo int)
//AS
//	UPDATE MEDICAMENTOS SET ID_ESTADO=@idEstado_Nuevo WHERE ID=@idMedicamento;
//DELETE FROM PETICIONESMEDICAMENTOS WHERE ID=@idPeti;
//GO

//-- Sin ID, que seria de crear un nuevo medicamento , tendremos que generar un ID para ese medicamento y añadir
//-- sus propiedades (Nombre, Descripcion y estado)
//CREATE PROCEDURE SP_OKPETI_MEDICAMENTO_SIN_ID
//(@idPeti int, @nombreMed nvarchar(50), @descMed nvarchar(50), @idEstado_Nuevo int)
//AS
//	DECLARE @idMax int;
//SELECT @idMax = MAX(ID) + 1 FROM MEDICAMENTOS
//	INSERT INTO MEDICAMENTOS (ID, NOMBRE, DESCRIPCION, ID_ESTADO) VALUES (@idMax, @nombreMed, @descMed, @idEstado_Nuevo)
//	DELETE FROM PETICIONESMEDICAMENTOS WHERE ID=@idPeti;
//GO

//CREATE PROCEDURE SP_OKNOPETI_MEDICAMENTO
//(@idPeti int)
//AS
//	DELETE FROM PETICIONESMEDICAMENTOS WHERE ID=@idPeti;
//GO

//CREATE PROCEDURE SP_CREATEPETIMEDIC_CONID
//(@idMedico int , @idMedicamento int , @idEstadoMedicamento int)
//AS
//	DECLARE @idMax int
//	SELECT @idMax = MAX(ID) + 1 from PETICIONESMEDICAMENTOS
//	INSERT INTO PETICIONESMEDICAMENTOS (ID, ID_MEDICO, ID_MEDICAMENTO, ID_ESTADO_NUEVO) VALUES (@idMax, @idMedico, @idMedicamento, @idEstadoMedicamento)
//GO

//CREATE PROCEDURE SP_CREATEPETIMEDIC_SINID
//(@idMedico int , @nombreMedicamento nvarchar(50) , @descMedicamento nvarchar(50) , @idEstadoMedicamento int)
//AS
//	DECLARE @idMax int
//	SELECT @idMax = MAX(ID) + 1 from PETICIONESMEDICAMENTOS
//	INSERT INTO PETICIONESMEDICAMENTOS (ID, ID_MEDICO, NOMBRE, DESCRIPCION, ID_ESTADO_NUEVO) VALUES (@idMax, @idMedico, @nombreMedicamento, @descMedicamento, @idEstadoMedicamento)
//GO

//create procedure sp_createcitapaciente
//(@fecha date , @hora time, @idmedico int , @idpaciente int)
//as
//	declare @idmax int 
//	select @idmax = max(id) + 1 from citas
//	insert into citas(id, fecha, hora, id_seguimientocita, id_medico, id_paciente, id_estadocita) values (@idmax, @fecha, @hora,3, @idmedico, @idpaciente, 1)
//go

//CREATE PROCEDURE SP_UPDATECITAMEDICA
//(@idcita int, @comentario nvarchar(Max), @seguimiento int)
//AS
//	UPDATE CITAS SET COMENTARIO=@comentario , ID_SEGUIMIENTOCITA = @seguimiento, ID_ESTADOCITA = 2
//	WHERE ID=@idcita
//GO

//CREATE PROCEDURE SP_UPDATECITAMEDICA_SINCOMENTARIO
//(@idcita int, @seguimiento int)
//AS
//	UPDATE CITAS SET  ID_SEGUIMIENTOCITA=@seguimiento , ID_ESTADOCITA = 2
//	WHERE ID=@idcita
//GO

//CREATE PROCEDURE SP_INSERTMEDICAMENTOPACIENTE
//(@idmedicamento int, @idmedico int, @idpaciente int)
//AS
//	DECLARE @maxid int
//	SELECT @maxid = MAX(ID) + 1 FROM MEDICAMENTOPACIENTE
//	INSERT INTO MEDICAMENTOPACIENTE (ID, ID_MEDICAMENTO, ID_MEDICO, ID_PACIENTE, ID_DISPOMEDICAMENTO) VALUES (@maxid, @idmedicamento, @idmedico, @idpaciente,1)
//GO

//CREATE PROCEDURE SP_CREATEPETICIONUSUARIO
//(@idsolicitante int, @idmodificado int, @idestadonuevo int)
//AS
//	DECLARE @max int
//	SELECT @max = MAX(ID) + 1 FROM PETICIONESUSUARIOS
//	INSERT INTO PETICIONESUSUARIOS (ID, ID_USUARIO_SOLICITANTE, ID_USUARIO_MODIFICAR, ID_ESTADO_NUEVO) VALUES (@max, @idsolicitante, @idmodificado, @idestadonuevo)
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
            var idMedicoResultado = (int)idMedicoParametro.Value;

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

        //Metodo para editar un PACIENTE
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
        public List<CitaDetallado> GetAllCitas()
        {
            var consulta = from datos in this.context.CitaDetallado
                           select datos;
            return consulta.ToList();
        }

        //Metodo para encontrar una CITA
        public Cita FindCita(int idCita)
        {
            return this.context.Citas.FirstOrDefault(z => z.Id == idCita);
        }

        //Metodo para eliminar una CITA
        public void DeleteCita(int idCita)
        {
            Cita cita = this.FindCita(idCita);
            this.context.Remove(cita);
            this.context.SaveChanges();
        }

        //Metodo para editar una CITA
        public void EditCita(int idCita, DateTime fecha, TimeSpan hora, int idSeguimientoCita, int idMedico, string comentario)
        {
            Cita cita = this.FindCita(idCita);
            cita.Fecha = fecha;
            cita.Hora = hora;
            cita.SeguimientoCita = idSeguimientoCita;
            cita.Medico = idMedico;
            cita.Comentario = comentario;
            this.context.SaveChanges();
        }

        //Metodo para mostrar todos los estado de un SeguimientoCita
        public List<SeguimientoCita> GetAllSeguimientoCita()
        {
            return this.context.SeguimientoCita.ToList();
        }

        //Metodo para obtener los datos de la tabla SeguimientoCita
        public SeguimientoCita GetSeguimientoCita(int idSeguimiento)
        {
            var consulta = from datos in this.context.SeguimientoCita
                           where datos.Id == idSeguimiento
                           select datos;
            return consulta.FirstOrDefault();
        }

        //Metodo para obtener los pacientes de un MEDICO
        public List<MedicosPacientes> MisPacientes(int idMedico)
        {
            var consulta = from datos in this.context.MedicosPacientes
                           where datos.Medico == idMedico
                           select datos;
            return consulta.ToList();
        }

        //Metodo para obtener todas las PETICIONES de forma DETALLADO       
        public List<PeticionesDetallado> GetPeticionesDetallado()
        {
            var consulta = from datos in this.context.PeticionesDetallado
                           select datos;
            return consulta.ToList();
        }

        //Metodo para aceptar la PETICION USUARIOS
        public void OkPetcion(int idPeticion , int idUsuario , int idEstadoNuevo)
        {
            string sql = "SP_OKPETICION_USUARIO @idPeticion, @idUsuario, @idEstadoUsuario";
            SqlParameter pamIdPeticion = new SqlParameter("@idPeticion", idPeticion);
            SqlParameter pamIdUsuario = new SqlParameter("@idUsuario", idUsuario);
            SqlParameter pamIdEstadoNuevo = new SqlParameter("@idEstadoUsuario", idEstadoNuevo);
            this.context.Database.ExecuteSqlRaw(sql, pamIdPeticion , pamIdUsuario, pamIdEstadoNuevo);
        }

        //Metodo para rechazar y eliminar la PETICION USUARIOS
        public void OkNoPeticion(int idPeticion)
        {
            string sql = "SP_OKNOPETICION_USUARIO @idPeticion";
            SqlParameter pamIdPeticion = new SqlParameter("@idPeticion", idPeticion);
            this.context.Database.ExecuteSqlRaw(sql, pamIdPeticion);
        }

        //Metodo para listar todas las peticiones de MEDICAMENTOS DETALLADAS
        public List<PeticionesMedicamentoDetallado> GetPeticionesMedicametentosDetallado()
        {
            return this.context.PeticionesMedicamentoDetallado.ToList();
        }

        //Metodo para aceptar las peticiones de MEDICAMENTOS, dependiendo de si es solo actualizar(Con ID) o crearla(Sin ID)
        public void OkPeticionMedicamento(int idPeti, int? idMedicamento, string nombre, string? descripcion, int estado)
        {
            //Si recibe un ID
            if (idMedicamento != null)
            {
                string sql = "SP_OKPETI_MEDICAMENTO_CON_ID @idPeti , @idMedicamento, @idEstado_Nuevo";
                SqlParameter pamIdPeti = new SqlParameter("@idPeti", idPeti);
                SqlParameter pamIdMedicamento = new SqlParameter("@idMedicamento", idMedicamento);
                SqlParameter pamIdEstado = new SqlParameter("@idEstado_Nuevo", estado);
                this.context.Database.ExecuteSqlRaw(sql, pamIdPeti, pamIdMedicamento, pamIdEstado);
            }
            //Sino recibe un ID
            else
            {
                string sql = "SP_OKPETI_MEDICAMENTO_SIN_ID @idpeti, @nombreMed , @descMed , @idEstado_Nuevo";
                SqlParameter pamIdPeti = new SqlParameter("@idPeti", idPeti);
                SqlParameter pamNombre = new SqlParameter("@nombreMed", nombre);
                SqlParameter pamDescripcion = new SqlParameter("@descMed", descripcion);
                SqlParameter pamIdEstado = new SqlParameter("@idEstado_Nuevo", estado);
                this.context.Database.ExecuteSqlRaw(sql, pamIdPeti , pamNombre , pamDescripcion ,pamIdEstado);
            }
        }

        //Metodo para recharzar una peticion de MEDICAMENTOS
        public void OkNoPeticionMedicamento(int idPeti)
        {
            string sql = "SP_OKNOPETI_MEDICAMENTO @idPeti";
            SqlParameter pamIdPeti = new SqlParameter("@idPeti", idPeti);
            this.context.Database.ExecuteSqlRaw(sql, pamIdPeti);
        }

        // *** Un formulario que me permita ver los medicamentos en estado de alta o baja (Formulario Peticion Medicamentos con ID)

        //Metodo para create una peticion con id(Solo acatualizara el estado del medicamento)
        public void CreatePeticionMedicamentoConId(int idMedico, int idMedicamento , int estadoMedicamento)
        {
            string sql = "SP_CREATEPETIMEDIC_CONID @idMedico , @idMedicamento , @idEstadoMedicamento";
            SqlParameter pamIdMedico = new SqlParameter("@idMedico", idMedico);
            SqlParameter pamIdMedicamento = new SqlParameter("@idMedicamento", idMedicamento);
            SqlParameter pamIdEstadoMedicamento = new SqlParameter("@idEstadoMedicamento", estadoMedicamento);
            this.context.Database.ExecuteSqlRaw(sql, pamIdMedico, pamIdMedicamento, pamIdEstadoMedicamento);
        }

        //Metodo para crear una peticion sin id(Solicitara un nuevo medicamento donde tendremos que insertar los datos)
        public void CreatePeticionMedicamentoSinId(int idMedico, string nombreMedicamento, string descripcionMedicamento)
        {
            string sql = "SP_CREATEPETIMEDIC_SINID @idMedico , @nombreMedicamento , @descMedicamento , @idEstadoMedicamento";
            SqlParameter pamIdMedico = new SqlParameter("@idMedico", idMedico);
            SqlParameter pamNombreMed = new SqlParameter("@nombreMedicamento", nombreMedicamento);
            SqlParameter pamDescMedicamento = new SqlParameter("@descMedicamento", descripcionMedicamento);
            SqlParameter pamEstadoMedicamento = new SqlParameter("@idEstadoMedicamento", 1);
            this.context.Database.ExecuteSqlRaw(sql, pamIdMedico, pamNombreMed ,pamDescMedicamento, pamEstadoMedicamento);
        }

        //Metodo para devolver los datos de los medicamentos
        public List<Medicamentos> GetMedicamentos()
        {
            var consulta = from datos in this.context.Medicamentos
                           select datos;
            return consulta.ToList();
        }

        //Metodo para buscar Medicamentos por ID
        public Medicamentos FindMedicamento(int idMedicamento)
        {
            var consulta = from datos in this.context.Medicamentos
                           where datos.Id == idMedicamento
                           select datos;
            return consulta.FirstOrDefault();
        }

        //Metodo para devolver todos los tipos de estado
        public List<Estados> GetEstados()
        {
            return context.Estados.ToList();
        }

        //Metodo para encontrar el nombre del id estado
        public string FindNombreEstado(int idEstado)
        {
            var consulta = from datos in this.context.Estados
                           where datos.Id == idEstado
                           select datos.Estado;
            return consulta.FirstOrDefault();
        }

        //Metodo para crear una cita siendo paciente.
        public void CreateCitaPaciente(DateTime fecha, TimeSpan hora, int idmedico , int idpaciente)
        {
            string sql = "SP_CREATECITAPACIENTE @fecha , @hora , @idmedico , @idpaciente";
            SqlParameter pamFecha = new SqlParameter("@fecha",fecha);
            SqlParameter pamHora = new SqlParameter("@hora", hora);
            SqlParameter pamMedico = new SqlParameter("@idmedico", idmedico);
            SqlParameter pamPaciente = new SqlParameter("@idpaciente", idpaciente);
            this.context.Database.ExecuteSqlRaw(sql, pamFecha, pamHora, pamMedico , pamPaciente);
        }

        //Metodo para encontrar una cita (Que este en proceso porque si ya finalizo la cita da igual)
        public int FindCitaDispo(int idmedico , DateTime fecha , TimeSpan hora)
        {
            var consulta = from datos in this.context.Citas
                           where datos.Medico == idmedico && datos.Fecha == fecha && datos.Hora == hora
                           && datos.SeguimientoCita == 3 && datos.IdEstado == 1
                           select datos;
            if (consulta != null)
            {
                return 0;
            }
            else
            {
                return 1;
            }
            
        }

        //Metodo para encontrar citas que tiene un MEDICO
        public List<CitaDetalladaMedicos> GetCitasDetalladasMedico(int idmedico)
        {
            return this.context.CitaDetalladaMedicos.Where(z=>z.IdMedico == idmedico).ToList();
        }

        //Metodo para filtrar por fecha las citas de los MEDICOS
        public List<CitaDetalladaMedicos> FindCitasDetalladasMedicos(int idmedico , DateTime fecha)
        {
            return this.context.CitaDetalladaMedicos.Where(z=>z.IdMedico == idmedico && z.Fecha == fecha).ToList();
        }

        //Metodo para filtrar la cita seleccinada
        public CitaDetalladaMedicos FindCitasDetalladasMedicosSinFiltro(int idcita)
        {
            return this.context.CitaDetalladaMedicos.Where(z => z.Id == idcita).FirstOrDefault();
        }

        //Metodo para actualizar una cita MEDICA
        public void UpdateCitaMedica(int idmedico , int idpaciente , int idcita, string comentario , int seguimiento, List<int> medicamentos)
        {
            if (comentario != null)
            {
                string sql = "SP_UPDATECITAMEDICA @idcita , @comentario , @seguimiento";
                SqlParameter pamIdCita = new SqlParameter("@idcita", idcita);
                SqlParameter pamComentario = new SqlParameter("@comentario", comentario);
                SqlParameter pamSeguimiento = new SqlParameter("@seguimiento", seguimiento);
                this.context.Database.ExecuteSqlRaw(sql, pamIdCita, pamComentario, pamSeguimiento);
            }
            else
            {
                string sql = "SP_UPDATECITAMEDICA_SINCOMENTARIO @idcita, @seguimiento";
                SqlParameter pamIdCita = new SqlParameter("@idcita", idcita);
                SqlParameter pamSeguimiento = new SqlParameter("@seguimiento", seguimiento);
                this.context.Database.ExecuteSqlRaw(sql, pamIdCita, pamSeguimiento);
            }
            if (medicamentos != null)
            {
                foreach (int medis in medicamentos)
                {
                    string sql = "SP_INSERTMEDICAMENTOPACIENTE @idmedicamento, @idmedico, @idpaciente";
                    SqlParameter pamIdMedicamento = new SqlParameter("@idmedicamento", medis);
                    SqlParameter pamIdMedico = new SqlParameter("@idmedico", idmedico);
                    SqlParameter pamIdPaciente = new SqlParameter("@idpaciente", idpaciente);
                    this.context.Database.ExecuteSqlRaw(sql, pamIdMedicamento, pamIdMedico, pamIdPaciente);
                }
            }
        }

        //Metodo para obtener todos los medicamentos
        public List<Medicamentos> GetAllMedicamentos()
        {
            return this.context.Medicamentos.Where(z => z.Id_Estado == 1).ToList();
        }

        //Metodo para buscar todas las citas de un PACIENTE
        public List<CitaDetalladaMedicos> FindCitasPaciente(int idpaciente)
        {
            return this.context.CitaDetalladaMedicos.Where(z => z.IdPaciente == idpaciente).ToList();
        }

        //Metodo para encontrar una cita detallada siendo Paciente
        public List<CitaDetalladaMedicos> FindCitasDetalladasPAciente(int idpaciente, DateTime fechadesde, DateTime? fechahasta)
        {
            if (fechahasta == null)
            {
                return this.context.CitaDetalladaMedicos.Where(z => z.IdPaciente == idpaciente && z.Fecha >= fechadesde).ToList();
            }
            else
            {
                return this.context.CitaDetalladaMedicos.Where(z => z.IdPaciente == idpaciente && z.Fecha >= fechadesde && z.Fecha <= fechahasta).ToList();
            }
            
        }

        //Metodo para cambiar los datos de la cita siendo paciente
        public void UpdateCitaDetalladaPaciente(int idcita, DateTime fecha , TimeSpan hora)
        {
            Cita micita = this.FindCita(idcita);
            micita.Fecha = fecha;
            micita.Hora = hora;
            this.context.SaveChanges();
        }

        //Metodo para obtener todos los medicamentos de un paciente
        public List<MedicamentoYPaciente> GetAllMedicamentosPaciente(int idpaciente)
        {
            return this.context.MedicamentoYPacientes.Where(z => z.IdPaciente == idpaciente && z.IdDispoMedicamento == 1).ToList();
        }

        //Metodo para encontrar un dato de MedicamentoYPaciente
        public MedicamentoYPaciente FindMedicamentoYPaciente(int id)
        {
            return this.context.MedicamentoYPacientes.Where(z => z.Id == id).FirstOrDefault();
        }

        //Metodo para actualizar los datos de MedicamentoYPaciente
        public void UpdateMedicamentoYPaciente(int id)
        {
            MedicamentoYPaciente misdatos = this.FindMedicamentoYPaciente(id);
            misdatos.IdDispoMedicamento = 2;
            this.context.SaveChanges();
        }

        //Metodo para encontrar un usuario por datos de nombre, apellido y correo
        public Paciente FindPacienteDistintoDetallado(string nombre, string apellido , string correo)
        {
            return this.context.Paciente.Where(z => z.Apellido == apellido && z.Nombre == nombre && z.Correo == correo).FirstOrDefault();
        }

        //metodo para encontrar el id medico de un paciente
        public MedicosPacientes GetMedicoPaciente(int idpaciente)
        {
            return this.context.MedicosPacientes.Where(z => z.Paciente == idpaciente).FirstOrDefault();
        }

        //Metodo para devolver todos los usuarios pacientes en estado de baja
        public List<Paciente> GetAllPacientesBaja()
        {
            return this.context.Paciente.Where(z => z.EstadoUsuario == 2).ToList();
        }
        public List<Paciente> GetAllPacientes()
        {
            return this.context.Paciente.ToList();
        }

        //Metodo para insertar una peticion en usuarios
        public void CreatePeticionUsuarios(int idsolicitante, int idmodificado, int nuevoestado)
        {
            string sql = "SP_CREATEPETICIONUSUARIO @idsolicitante, @idmodificado, @idestadonuevo";
            SqlParameter pamIdSoli = new SqlParameter("idsolicitante",idsolicitante);
            SqlParameter pamIdModi = new SqlParameter("idmodificado", idmodificado);
            SqlParameter pamIdEstado = new SqlParameter("idestadonuevo", nuevoestado);
            this.context.Database.ExecuteSqlRaw(sql, pamIdSoli, pamIdModi, pamIdEstado);
        }
    }
}
