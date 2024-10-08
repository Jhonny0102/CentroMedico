﻿using CentroMedico.Models;
using CentroMedico.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CentroMedico.Controllers
{
    public class CentroMedicoController : Controller
    {
        private RepositoryCentroMedicoSQL repo;
        public CentroMedicoController(RepositoryCentroMedicoSQL repo)
        {
            this.repo = repo;
        }
        public IActionResult Index()
        {
            return View();
        }

        // Alta de Pacientes.
        public IActionResult CreatePaciente()
        {
            ViewData["ESPECIALIDADES"] = this.repo.GetEspecialidades();
            return View();
        }
        [HttpPost]
        public IActionResult CreatePaciente(Paciente paciente, int especialidad)
        {
            ViewData["ESPECIALIDADES"] = this.repo.GetEspecialidades();
            int medico = this.repo.GetIdMedico(especialidad);
            this.repo.CreatePaciente(paciente.Nombre, paciente.Apellido, paciente.Correo,paciente.Contra, paciente.Telefono, paciente.Direccion, paciente.Edad, paciente.Genero,medico);
            return RedirectToAction("Index");
        }

        //Cerrar Session
        public IActionResult CloseSession()
        {
            HttpContext.Session.Clear();
            //Todos los view usado por admin, por si lo necesitas
            //ViewData["IDUSUARILOGUEADO"];
            //ViewData["IDUSUARIOVER"];
            //ViewData["IDUSUARIOEDIT"];
            return RedirectToAction("Index");
        }

        // Login de usuarios.
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string correo, string contra)
        {
            Usuario usuario = this.repo.GetLogin(correo,contra);
            if(usuario != null)
            {
                //Guardamos en la session el id del usuario encontrado.
                HttpContext.Session.SetInt32("IDUSUARILOGUEADO",usuario.Id);
                HttpContext.Session.SetInt32("TIPOUSUARILOGUEADO", usuario.Id_TipoUsuario);
                if (usuario.Id_TipoUsuario == 1)
                {
                    return RedirectToAction("ZonaAdmin");
                }
                else if(usuario.Id_TipoUsuario == 2)
                {
                    return RedirectToAction("ZonaRecepcionista");
                }
                else if (usuario.Id_TipoUsuario == 3)
                {
                    return RedirectToAction("ZonaMedico");
                }
                else
                {
                    return RedirectToAction("ZonaPaciente");
                }
            }
            else
            {
                ViewData["mensaje"] = "Datos Erroneos o Usuario de Baja, vuelve a intentarlo";
                return View();
            }
        }

        // Zona de ADMINISTRADOR ****.

        /// Zona de CRUD USUARIOS ///

        //Controller que recoge un ID de session y muestra su info
        public IActionResult ZonaAdmin()
        {
            int idUsuario = (int)HttpContext.Session.GetInt32("IDUSUARILOGUEADO");
            Usuario usuario = this.repo.FindUsuario(idUsuario);
            return View(usuario);
        }

        public IActionResult ZonaAdminPerfil()
        {
            int idUsuario = (int)HttpContext.Session.GetInt32("IDUSUARILOGUEADO");
            UsuarioDetallado usuario = this.repo.FindUsuarioDetallado(idUsuario);
            return View(usuario);
        }

        //Incluimos nuevo
        public IActionResult PerfilAdmin(int idUsuario)
        {
            UsuarioDetallado usuario = this.repo.FindUsuarioDetallado(idUsuario);
            List<Estados> tiposEstados = this.repo.GetEstados();
            ViewData["ESTADOS"] = tiposEstados;
            return View(usuario);
        }
        [HttpPost]
        public IActionResult PerfilAdmin(Usuario admin)
        {
            this.repo.EditUsuario(admin.Id,admin.Nombre,admin.Apellido,admin.Correo,admin.Contra,admin.Id_EstadoUsuario,admin.Id_TipoUsuario);
            return RedirectToAction("ZonaAdmin");
        }

        //Controller que guarda los tipos de usuarios y muestra una lista de usuarios
        public IActionResult ZonaAdminUsuarios()
        {
            ViewData["TipoUsuarios"] = this.repo.GetTipoUsuarios();
            List<Usuario> usuarios = this.repo.GetUsuarios();
            return View(usuarios);
        }
        [HttpPost]
        public IActionResult ZonaAdminUsuarios(int tipo)
        {
            ViewData["TipoUsuarios"] = this.repo.GetTipoUsuarios();
            List<Usuario> usuarios = this.repo.GetUsuariosTipo(tipo);
            return View(usuarios);
        }

        //Controller dar de alta MEDICOS
        public IActionResult CreateMedico()
        {
            ViewData["ESPECIALIDADES"] = this.repo.GetEspecialidades();
            return View();
        }

        [HttpPost]
        public IActionResult CreateMedico(Medico medico)
        {
            ViewData["ESPECIALIDADES"] = this.repo.GetEspecialidades();
            this.repo.CreateMedico(medico.Nombre,medico.Apellido,medico.Correo,medico.Contra,medico.Especialidad);
            return RedirectToAction("ZonaAdminUsuarios");
        }

        //Controller dar de alta USUARIOS
        public IActionResult CreateUsuario()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateUsuario(Usuario usuario)
        {
            this.repo.CreateUsuario(usuario.Nombre,usuario.Apellido,usuario.Correo,usuario.Contra,usuario.Id_TipoUsuario);
            return RedirectToAction("ZonaAdminUsuarios");
        }

        // Controller que redirije y guarda int en session
        public IActionResult Details(int idUsuario, int idTipo)
        {
            //Guardamos el id del usuario en la session
            HttpContext.Session.SetInt32("IDUSUARIOVER", idUsuario);
            //Si el tipo es 1 o 2 redirije a DetailsUsuario
            if (idTipo == 1 || idTipo == 2)
            {
                return RedirectToAction("DetailsUsuario");
            }
            //Si el tipo es 3 redirije a DetailsMedico
            else if (idTipo == 3)
            {
                return RedirectToAction("DetailsMedico");
            }
            //Si el tipo distinto redirije a DetailsPaciente
            else
            {
                return RedirectToAction("DetailsPaciente");
            }
        }

        //Controller que muestra los detalles de MEDICO
        public IActionResult DetailsMedico(int? idMedico)
        {
            //Si recibimos un idMedico(Este lo recibe de detailsCita ya que queremos mostrar la informacion del medico que esta asiganado a la cita)
            //buscara el medico de forma detallada y devolvera su informacion
            if (idMedico != null)
            {
                MedicoDetallado medico = this.repo.FindMedicoDetallado(idMedico.Value);
                return View(medico);
            }
            else
            {
                int id = (int)HttpContext.Session.GetInt32("IDUSUARIOVER");
                MedicoDetallado medico = this.repo.FindMedicoDetallado(id);
                return View(medico);
            }
            
        }

        //Controller que muestra los detalles de USUARIO
        public IActionResult DetailsUsuario(int? idUsuario)
        {
            if (idUsuario != null)
            {
                UsuarioDetallado usuario = this.repo.FindUsuarioDetallado(idUsuario.Value);
                return View(usuario);
            }
            else
            {
                int id = (int)HttpContext.Session.GetInt32("IDUSUARIOVER");
                UsuarioDetallado usuario = this.repo.FindUsuarioDetallado(id);
                return View(usuario);
            }
        }

        //Controller que muestra los detalles de PACIENTE (Si añadimos una ? seguido del tipo en el model puede aceptar nulos)
        public IActionResult DetailsPaciente(int? idPaciente)
        {
            //Si recibimos un idPaciente(Este lo recibe de detailsCita ya que queremos mostrar la informacion del paciente que esta asiganado a la cita)
            //buscara el paciente de forma detallada y devolvera su informacion
            if (idPaciente != null)
            {
                PacienteDetallado paciente = this.repo.FindPacienteDetallado(idPaciente.Value);
                return View(paciente);
            }
            else
            {
                int id = (int)HttpContext.Session.GetInt32("IDUSUARIOVER");
                PacienteDetallado paciente = this.repo.FindPacienteDetallado(id);
                return View(paciente);
            }
            
        }

        //Controller que elimina DIFERENTES TIPOS USUARIOS
        public IActionResult DeleteUsuario(int id, int tipo)
        {
            //**** Mostrar un Alert o algo de confirmacion, ya que esto borra todos los registros relacionados con ese usuario ****//
            this.repo.DeleteUsuario(id, tipo);
            return RedirectToAction("ZonaAdminUsuarios");
        }

        //Controller que editar
        public IActionResult Edit(int id , int tipo)
        {
            HttpContext.Session.SetInt32("IDUSUARIOEDIT", id);
            if (tipo == 1 | tipo == 2)
            {
                return RedirectToAction("EditUsuario");
            }
            else if(tipo == 3)
            {
                return RedirectToAction("EditMedico");
            }
            else
            {
                return RedirectToAction("EditPaciente");
            }
        }

        //Controller de editar USUARIO
        public IActionResult EditUsuario(int? idUsuario)
        {
            if (idUsuario != null)
            {
                Usuario usuario = this.repo.FindUsuario(idUsuario.Value);
                List<Estados> tiposEstados = this.repo.GetEstados();
                ViewData["ESTADOS"] = tiposEstados;
                return View(usuario);
            }
            else
            {
                int id = (int)HttpContext.Session.GetInt32("IDUSUARIOEDIT");
                Usuario usuario = this.repo.FindUsuario(id);
                List<Estados> tiposEstados = this.repo.GetEstados();
                ViewData["ESTADOS"] = tiposEstados;
                return View(usuario);
            }
        }
        [HttpPost]
        public IActionResult EditUsuario(Usuario usuario)
        {
            this.repo.EditUsuario(usuario.Id,usuario.Nombre,usuario.Apellido,usuario.Correo,usuario.Contra,usuario.Id_EstadoUsuario,usuario.Id_TipoUsuario);
            return RedirectToAction("ZonaAdminUsuarios");
        }

        //Controller de editar MEDICO
        public IActionResult EditMedico(int? idMedico)
        {
            if (idMedico != null)
            {
                Medico medico = this.repo.FindMedico(idMedico.Value);
                ViewData["ESPECIALIDADES"] = this.repo.GetEspecialidades();
                return View(medico);
            }
            else
            {
                int id = (int)HttpContext.Session.GetInt32("IDUSUARIOEDIT");
                Medico medico = this.repo.FindMedico(id);
                ViewData["ESPECIALIDADES"] = this.repo.GetEspecialidades();
                return View(medico);
            }
        }
        [HttpPost]
        public IActionResult EditMedico(Medico medico)
        {
            ViewData["ESPECIALIDADES"] = this.repo.GetEspecialidades();
            this.repo.EditMedico(medico.Id,medico.Nombre,medico.Apellido,medico.Correo,medico.Contra,medico.EstadoUsuario,medico.TipoUsuario, medico.Especialidad);
            return RedirectToAction("ZonaAdminUsuarios");
        }

        //Controller de editar PACIENTE
        public IActionResult EditPaciente()
        {
            int id = (int)HttpContext.Session.GetInt32("IDUSUARIOEDIT");
            Paciente paciente = this.repo.FindPaciente(id);
            return View(paciente);
        }
        [HttpPost]
        public IActionResult EditPaciente(Paciente paciente)
        {
            this.repo.EditPaciente(paciente.Id, paciente.Nombre, paciente.Apellido, paciente.Correo, paciente.Contra, paciente.Telefono, paciente.Direccion, paciente.Edad, paciente.Genero, paciente.EstadoUsuario, paciente.TipoUsuario);
            return RedirectToAction("ZonaAdminUsuarios");
        }

        /// Fin Zona de CRUD USUARIOS ///

        /// Zona CRUD CITAS ///
        
        //Controller para mostrar todas las CITAS Detalladas 
        public IActionResult ZonaAdminCitas(int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            CitaDetalladoModel model = this.repo.GetAllCitas(posicion.Value);
            ViewData["REGISTROS"] = model.Registros;
            return View(model.CitaDetallado);
        }

        //Controller para mostrar los detalles de una CITA
        public IActionResult DetailsCita(int idCita)
        {
            Cita cita = this.repo.FindCita(idCita);

            int idSeguimiento = cita.SeguimientoCita;
            SeguimientoCita seguimiento = this.repo.GetSeguimientoCita(idSeguimiento);
            ViewData["NOMBRESEGUIMIENTO"] = seguimiento.Estado;
            
            //Guardamos el IDMEDICO
            int idMedico = cita.Medico;
            //Buscamos el usuarios con ese id y nos guardamos sus datos
            Usuario medico = this.repo.FindUsuario(idMedico);
            //Guardamos en el view data el nombre y apellido del medico
            ViewData["NOMBREMEDICO"] = medico.Nombre + " " + medico.Apellido;
            
            //Guardamos el IDPACIENTE
            int idPaciente = cita.Paciente;
            //Buscamos el usuarios con ese id y nos guardamos sus datos
            Usuario paciente = this.repo.FindUsuario(idPaciente);
            //Guardamos en el view data el nombre y apellido del paciente
            ViewData["NOMBREPACIENTE"] = paciente.Nombre + " " + paciente.Apellido;
            return View(cita);
        }

        //Controller que muestra la informacion de un medico llamado desde admin citas
        public IActionResult DetailsMedicoCita(int idMedico, int idcita)
        {
            MedicoDetallado medico = this.repo.FindMedicoDetallado(idMedico);
            ViewData["CITASELECCIONADA"] = idcita;
            return View(medico);
        }

        public IActionResult DetailsPacienteCita(int idPaciente , int idcita)
        {
            PacienteDetallado paciente = this.repo.FindPacienteDetallado(idPaciente);
            ViewData["CITASELECCIONADA"] = idcita;
            return View(paciente);
        }

        //Controller para eliminar una CITA
        public IActionResult DeleteCita(int idCita)
        {
            this.repo.DeleteCita(idCita);
            return RedirectToAction("ZonaAdminCitas");
        }

        /// Fin Zona CRUD CITAS ///

        /// Zona PETICIONES ///
        
        //Controller para mostrar PETICIONES DE FORMA DETALLADA
        public IActionResult ZonaAdminPeticiones()
        {
            List<PeticionesDetallado> peticiones = this.repo.GetPeticionesDetallado();
            return View(peticiones);
        }
        
        //Controller que nos permite aceptar una peticion de USUARIOS
        public IActionResult OkPeticiones(int idPeticion, int idUsuario, int idEstadoNuevo)
        {
            this.repo.OkPetcion(idPeticion,idUsuario,idEstadoNuevo);
            return RedirectToAction("ZonaAdminPeticiones");
        }

        //Controller que nos permite denegar una peticion de USUARIOS
        public IActionResult OkNoPeticiones(int idPeticion)
        {
            this.repo.OkNoPeticion(idPeticion);
            return RedirectToAction("ZonaAdminPeticiones");
        }
        
        //Controller que nos devuelve una lista de Peticion de Medicamentos
        public IActionResult ZonaAdminPeticionesMedicamentos()
        {
            List<PeticionesMedicamentoDetallado> petisMedicamentos = this.repo.GetPeticionesMedicametentosDetallado();
            return View(petisMedicamentos);
        }

        //Controller que nos permite aceptar una peticion de MEDICAMENTOS
        public IActionResult OkPeticionMedicamento(int idPeti , int? idMedicamento, string nombre, string? descripcion, int estado)
        {
            this.repo.OkPeticionMedicamento(idPeti, idMedicamento,nombre,descripcion,estado);
            return RedirectToAction("ZonaAdminPeticionesMedicamentos");
        }

        //Controller que nos permite denegar una peticion de MEDICAMENTOS
        public IActionResult OkNoPeticionMedicamento(int idPeti)
        {
            this.repo.OkNoPeticionMedicamento(idPeti);
            return RedirectToAction("ZonaAdminPeticionesMedicamentos");
        }
        
        /// Fin PETICIONES///


        // Zona de RECEPCIONISTA ***.
        public IActionResult ZonaRecepcionista()
        {
            int idUsuario = (int)HttpContext.Session.GetInt32("IDUSUARILOGUEADO");
            Usuario usuario = this.repo.FindUsuario(idUsuario);
            return View(usuario);
        }

        //Controller para actualizar datos de Recepcionista
        public IActionResult UpdateRecepcionsita()
        {
            int id = (int)HttpContext.Session.GetInt32("IDUSUARILOGUEADO");
            Usuario recep = this.repo.FindUsuario(id);
            return View(recep);
        }
        [HttpPost]
        public IActionResult UpdateRecepcionsita(int id, string nombre, string apellido , string correo , string contra , int estado, int tipo)
        {
            this.repo.EditUsuario(id,nombre,apellido,correo,contra,estado,tipo);
            return RedirectToAction("ZonaRecepcionista");
        }

        //Controller cita rapida Recepcionista
        public IActionResult CitaRapidaRecepcionista()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CitaRapidaRecepcionista(string nombre , string apellido , string correo, int? idmedico, int? idpaciente, DateTime? fecha, TimeSpan? hora)
        {
            Paciente paciente = this.repo.FindPacienteDistintoDetallado(nombre, apellido , correo);
            if (paciente != null)
            {
                if (paciente.EstadoUsuario == 1)
                {
                    MedicosPacientes mimedicoid = this.repo.GetMedicoPaciente(paciente.Id); ;
                    ViewData["SUMEDICO"] = mimedicoid.Medico;

                    if (idmedico != null && idpaciente != null && fecha != null && hora != null)
                    {
                        this.repo.CreateCitaPaciente(fecha.Value, hora.Value, idmedico.Value, idpaciente.Value);
                        return RedirectToAction("ZonaRecepcionista");
                    }

                    return View(paciente);
                }
                else
                {
                    ViewData["ERROR"] = "Paciente en estado de BAJA , Solicita una Peticion al Admin";
                    return View();
                }
            }
            else
            {
                ViewData["ERROR"] = "Datos incorrectos , vuelva a preguntar";
                return View();
            }
        }

        //Controller para insertar una nueva peticion de usuario
        public IActionResult CreatePeticionUsuario()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreatePeticionUsuario(string nombre, string apellido, string correo, int? idpaciente , int? estadonuevo)
        {
            int id = (int)HttpContext.Session.GetInt32("IDUSUARILOGUEADO");
            ViewData["ESTADOUSUARIO"] = this.repo.GetEstados();
            Paciente paciente = this.repo.FindPacienteDistintoDetallado(nombre, apellido, correo);
            if (paciente != null)
            {
                if (idpaciente != null & estadonuevo != null)
                {
                    this.repo.CreatePeticionUsuarios(id,idpaciente.Value,estadonuevo.Value);
                    return RedirectToAction("ZonaRecepcionista");
                }
                return View(paciente);
            }
            else
            {
                ViewData["ERROR"] = "Datos incorrectos , vuelva a preguntar";
                return View();
            }
        }


        // Zona de MEDICO ***.

        //Los dos controllers de abajo devuelven lo mismo , solo que el primero guarda un model en la vista y usamos algunas propiedades
        //El otro envia a la vista zonamedicaperfil y carga ahi los datos del medico
        public IActionResult ZonaMedico()
        {
            int idUsuario = (int)HttpContext.Session.GetInt32("IDUSUARILOGUEADO");
            Usuario usuario = this.repo.FindUsuario(idUsuario);
            HttpContext.Session.SetString("NOMBREMEDICOGUARDAR", usuario.Nombre+" "+usuario.Apellido);
            return View(usuario);
        }

        public IActionResult PerfilMedico(int idMedico)
        {
            MedicoDetallado medico = this.repo.FindMedicoDetallado(idMedico);
            return View(medico);
        }

        public IActionResult DetailsPacienteMedico(int idPaciente, int idMedico)
        {
            PacienteDetallado paciente = this.repo.FindPacienteDetallado(idPaciente);
            ViewData["MEDICOSELECCIONADO"] = idMedico;
            return View(paciente);
        }

        //Controller que nos permite obtener todos los pacientes de una medico
        public IActionResult MisPacientes(int idMedico)
        {
            List<MedicosPacientes> mispaciente = this.repo.MisPacientes(idMedico);
            return View(mispaciente);
        }

        //Controller que nos permite crear una peticion (ALTA DE MEDICAMENTO) 
        public IActionResult CreatePeticionSinIdMedico(int idMedico)
        {
            ViewData["NOMBREMEDICO"] = HttpContext.Session.GetString("NOMBREMEDICOGUARDAR");
            ViewData["IDMEDICO"] = idMedico;
            return View();
        }
        [HttpPost]
        public IActionResult CreatePeticionSinIdMedico(int idMedico, string nombreMedicamento, string descMedicamento)
        {
            this.repo.CreatePeticionMedicamentoSinId(idMedico, nombreMedicamento, descMedicamento);
            ViewData["NOMBREMEDICO"] = HttpContext.Session.GetString("NOMBREMEDICOGUARDAR");
            return RedirectToAction("ZonaMedico");
        }

        //Controller que nos permite crear una peticion(ACTUALIZACION DE UN MEDICAMENTO)
        public IActionResult CreatePeticionConIdMedico(int idMedico)
        {
            List<Medicamentos> medicamentos = this.repo.GetMedicamentos();
            ViewData["ESTADOS"] = this.repo.GetEstados();
            ViewData["NOMBREMEDICO"] = HttpContext.Session.GetString("NOMBREMEDICOGUARDAR");
            ViewData["IDMEDICO"] = idMedico;
            return View(medicamentos);
        }
        [HttpPost]
        public IActionResult CreatePeticionConIdMedico(int idMedico, int idMedicamento, int estadoMedicamento)
        {
            ViewData["NOMBREMEDICO"] = HttpContext.Session.GetString("NOMBREMEDICOGUARDAR");
            ViewData["IDMEDICO"] = idMedico;
            List<Medicamentos> medicamentos = this.repo.GetMedicamentos();
            ViewData["ESTADOS"] = this.repo.GetEstados();

            Medicamentos medicamento = this.repo.FindMedicamento(idMedicamento);
            string nombreEstado = this.repo.FindNombreEstado(estadoMedicamento);
            if (medicamento.Id_Estado == estadoMedicamento)
            {
                ViewData["MENSAJEMEDICAMENTO"] = "El medicamento " + medicamento.Nombre  + " ya esta de " + nombreEstado;
                return View(medicamentos);
            }
            else
            {
                this.repo.CreatePeticionMedicamentoConId(idMedico,idMedicamento,estadoMedicamento);
                return RedirectToAction("ZonaMedico");
            }
            
        }

        //Controller para Mostrar y Buscar citas de un medico
        public IActionResult MisCitasMedico()
        {
            int idMedico = (int)HttpContext.Session.GetInt32("IDUSUARILOGUEADO");
            List<CitaDetalladaMedicos> misCitas = this.repo.GetCitasDetalladasMedico(idMedico);
            return View(misCitas);
        }
        [HttpPost]
        public IActionResult MisCitasMedico(DateTime fecha)
        {
            int idMedico = (int)HttpContext.Session.GetInt32("IDUSUARILOGUEADO");
            List<CitaDetalladaMedicos> misCitasFiltrados = this.repo.FindCitasDetalladasMedicos(idMedico,fecha);
            return View(misCitasFiltrados);
        }

        //Controller para actualizar una CITA MEDICA
        public IActionResult CitaMedicaFinal(int idcita)
        {
            ViewData["MEDICAMENTOS"] = this.repo.GetAllMedicamentos();
            ViewData["ESTADOSEGUIMIENTO"] = this.repo.GetAllSeguimientoCita();
            CitaDetalladaMedicos citaseleccionada = this.repo.FindCitasDetalladasMedicosSinFiltro(idcita);
            return View(citaseleccionada);
        }
        [HttpPost]
        public IActionResult CitaMedicaFinal(int idmedico , int idpaciente , int idcita , string? comentario , int seguimiento, List<int>? medicamentos)
        {
            this.repo.UpdateCitaMedica(idmedico,idpaciente,idcita,comentario,seguimiento,medicamentos);
            return RedirectToAction("MisCitasMedico");
        }

        // Zona de PACIENTES ***.
        public IActionResult ZonaPaciente()
        {
            int idUsuario = (int)HttpContext.Session.GetInt32("IDUSUARILOGUEADO");
            Usuario usuario = this.repo.FindUsuario(idUsuario);
            return View(usuario);
        }
        public IActionResult GetMiMedico(int idPaciente)
        {
            MedicoDetallado medicoAsignado = this.repo.GetMiMedico(idPaciente);
            if (medicoAsignado == null)
            {
                ViewData["MENSAJE"] = "No tienes medico Asigando aun";
                return View();
            }
            else
            {
                return View(medicoAsignado);
            } 
        }

        //Controller para crear una cita siendo PACIENTE
        public IActionResult CreateCitaPaciente()
        {
            int idUsuario = (int)HttpContext.Session.GetInt32("IDUSUARILOGUEADO");
            Usuario paciente = this.repo.FindUsuario(idUsuario);
            MedicoDetallado medico = this.repo.GetMiMedico(idUsuario);
            ViewData["IDMEDICO"] = medico.Id;
            ViewData["NOMBREMEDICO"] = medico.Nombre + " " + medico.Apellido;
            return View(paciente);
        }
        [HttpPost]
        public IActionResult CreateCitaPaciente(DateTime fecha, TimeSpan hora, int idmedico, int idpaciente)
        {
            int idUsuario = (int)HttpContext.Session.GetInt32("IDUSUARILOGUEADO");
            Usuario paciente = this.repo.FindUsuario(idUsuario);
            MedicoDetallado medico = this.repo.GetMiMedico(idUsuario);
            ViewData["IDMEDICO"] = medico.Id;
            ViewData["NOMBREMEDICO"] = medico.Nombre + " " + medico.Apellido;
            DateTime hoy = DateTime.Now;
            if (fecha < hoy)
            {
                ViewData["MENSAJE"] = "Esto es regreso al pasado ?? Pon una fecha correcta";
                return View(paciente);
            }
            else
            {
                int dispo = this.repo.FindCitaDispo(idmedico, fecha, hora); 
                if (dispo == 1)
                {
                    ViewData["OTROMENSAJE"] = "Esta Fecha y Hora no esta disponible";
                    return View(paciente);
                }
                else
                {
                    this.repo.CreateCitaPaciente(fecha, hora, idmedico, idpaciente);
                    return RedirectToAction("ZonaPaciente");
                }
            }
        }

        //Controller de editar PACIENTE
        public IActionResult ActualizarPaciente()
        {
            int id = (int)HttpContext.Session.GetInt32("IDUSUARILOGUEADO");
            Paciente paciente = this.repo.FindPaciente(id);
            return View(paciente);
        }
        [HttpPost]
        public IActionResult ActualizarPaciente(Paciente paciente)
        {
            this.repo.EditPaciente(paciente.Id, paciente.Nombre, paciente.Apellido, paciente.Correo, paciente.Contra, paciente.Telefono, paciente.Direccion, paciente.Edad, paciente.Genero, paciente.EstadoUsuario, paciente.TipoUsuario);
            return RedirectToAction("ZonaPaciente");
        }

        //Controller donde mostrarmos todas las citas de un PACIENTE
        public IActionResult CitasPaciente()
        {
            int id = (int)HttpContext.Session.GetInt32("IDUSUARILOGUEADO");
            List<CitaDetalladaMedicos> miscitas = this.repo.FindCitasPaciente(id);
            MedicoDetallado mimedico = this.repo.GetMiMedico(id);
            ViewData["NOMBREMEDICO"] = mimedico.Nombre + " " + mimedico.Apellido;
            return View(miscitas);
        }
        [HttpPost]
        public IActionResult CitasPaciente(DateTime fechadesde, DateTime? fechahasta)
        {
            int id = (int)HttpContext.Session.GetInt32("IDUSUARILOGUEADO");
            MedicoDetallado mimedico = this.repo.GetMiMedico(id);
            ViewData["NOMBREMEDICO"] = mimedico.Nombre + " " + mimedico.Apellido;
            if (fechahasta != null)
            {
                List<CitaDetalladaMedicos> miscitas = this.repo.FindCitasDetalladasPAciente(id, fechadesde, fechahasta.Value);
                return View(miscitas);
            }
            else
            {
                List<CitaDetalladaMedicos> miscitas = this.repo.FindCitasDetalladasPAciente(id,fechadesde,null);
                return View(miscitas);
            }
        }

        //Controller para mostrar los detalles de una cita
        public IActionResult DetallesCitaPaciente(int idcita)
        {
            int id = (int)HttpContext.Session.GetInt32("IDUSUARILOGUEADO");
            MedicoDetallado mimedico = this.repo.GetMiMedico(id);
            ViewData["NOMBREMEDICO"] = mimedico.Nombre + " " + mimedico.Apellido;
            CitaDetalladaMedicos micita = this.repo.FindCitasDetalladasMedicosSinFiltro(idcita);
            return View(micita);
        }

        //Controller para anular una cita siendo PACIENTE
        public IActionResult UpdateCitaPaciente(int idcita)
        {
            CitaDetalladaMedicos micita = this.repo.FindCitasDetalladasMedicosSinFiltro(idcita);
            MedicoDetallado mimedico = this.repo.GetMiMedico(micita.IdPaciente);
            ViewData["MIMEDICO"] = mimedico.Nombre + " " + mimedico.Apellido;
            return View(micita);
        }
        [HttpPost]
        public IActionResult UpdateCitaPaciente(int idcita , DateTime fecha , TimeSpan hora)
        {
            this.repo.UpdateCitaDetalladaPaciente(idcita,fecha,hora);
            return RedirectToAction("CitasPaciente");
        }

        //Controller para Cambiar hora o fecha siendo Paciente
        public IActionResult DeleteCitaPaciente(int idcita)
        {
            this.repo.DeleteCita(idcita);
            return RedirectToAction("CitasPaciente");
        }

        //Controller para mostrar los medicamentos de un PACIENTE
        public IActionResult MedicamentosYPacientes()
        {
            int id = (int)HttpContext.Session.GetInt32("IDUSUARILOGUEADO");
            List<MedicamentoYPaciente> mismedicamentos = this.repo.GetAllMedicamentosPaciente(id);
            return View(mismedicamentos);
        }

        //Controller para sacar un medicamento siendo un Paciente
        public IActionResult SacarMedicamento(int id)
        {
            this.repo.UpdateMedicamentoYPaciente(id);
            return RedirectToAction("MedicamentosYPacientes");
        }
    }
}
