using CentroMedico.Models;
using CentroMedico.Repositories;
using Microsoft.AspNetCore.Mvc;

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
                ViewData["mensaje"] = "Datos Erroneos, vuelve a intentarlo";
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
                return View(medico); //Buscar la manera de redirigir automaticamente a la pagina anterior
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
                return View(paciente); //Buscar la manera de redirigir automaticamente a la pagina anterior
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
                return View(usuario);
            }
            else
            {
                int id = (int)HttpContext.Session.GetInt32("IDUSUARIOEDIT");
                Usuario usuario = this.repo.FindUsuario(id);
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
        public IActionResult ZonaAdminCitas()
        {
            List<CitaDetallado> citas = this.repo.GetAllCitas();
            return View(citas);
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

        //Controller para eliminar una CITA
        public IActionResult DeleteCita(int idCita)
        {
            this.repo.DeleteCita(idCita);
            return RedirectToAction("ZonaAdminCitas");
        }

        //Controller para editar una CITA
        public IActionResult EditCita(int idCita)
        {
            Cita cita = this.repo.FindCita(idCita);
            return View(cita);
        }
        [HttpPost]
        public IActionResult EditCita(Cita cita)
        {
            this.repo.EditCita(cita.Id,cita.Fecha,cita.Hora,cita.SeguimientoCita,cita.Medico,cita.Comentario);
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
        
        public IActionResult OkPeticiones(int idPeticion, int idUsuario, int idEstadoNuevo)
        {
            this.repo.OkPetcion(idPeticion,idUsuario,idEstadoNuevo);
            return RedirectToAction("ZonaAdminPeticiones");
        }
        public IActionResult OkNoPeticiones(int idPeticion)
        {
            this.repo.OkNoPeticion(idPeticion);
            return RedirectToAction("ZonaAdminPeticiones");
        }
        
        
        /// Fin PETICIONES///


        // Zona de RECEPCIONISTA ***.
        public IActionResult ZonaRecepcionista()
        {
            int idUsuario = (int)HttpContext.Session.GetInt32("IDUSUARILOGUEADO");
            Usuario usuario = this.repo.FindUsuario(idUsuario);
            return View(usuario);
        }



        // Zona de MEDICO ***.
        
        //Los dos controllers de abajo devuelven lo mismo , solo que el primero guarda un model en la vista y usamos algunas propiedades
        //El otro envia a la vista zonamedicaperfil y carga ahi los datos del medico
        public IActionResult ZonaMedico()
        {
            int idUsuario = (int)HttpContext.Session.GetInt32("IDUSUARILOGUEADO");
            Usuario usuario = this.repo.FindUsuario(idUsuario);
            return View(usuario);
        }

        public IActionResult MisPacientes(int idMedico)
        {
            List<MedicosPacientes> mispaciente = this.repo.MisPacientes(idMedico);
            return View(mispaciente);
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
            return View(medicoAsignado);
        }
    }
}
