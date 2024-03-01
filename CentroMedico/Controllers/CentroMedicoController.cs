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
        public IActionResult AltaPaciente()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AltaPaciente(Usuario usuario)
        {
            this.repo.CreatePaciente(usuario.Nombre,usuario.Apellido,usuario.Correo,usuario.Contra);
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

        //Controller que recoge un ID de session y muestra su info
        public IActionResult ZonaAdmin()
        {
            int idUsuario = (int)HttpContext.Session.GetInt32("IDUSUARILOGUEADO");
            Usuario usuario = this.repo.FindUsuario(idUsuario);
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
        public IActionResult DetailsMedico()
        {
            int id = (int)HttpContext.Session.GetInt32("IDUSUARIOVER");
            MedicoDetallado medico = this.repo.FindMedicoDetallado(id);
            return View(medico);
        }

        //Controller que muestra los detalles de USUARIO
        public IActionResult DetailsUsuario()
        {
            int id = (int)HttpContext.Session.GetInt32("IDUSUARIOVER");
            UsuarioDetallado usuario = this.repo.FindUsuarioDetallado(id);
            return View(usuario);
        }

        //Controller que muestra los detalles de PACIENTE (Si añadimos una ? seguido del tipo en el model puede aceptar nulos)
        public IActionResult DetailsPaciente()
        {
            int id = (int)HttpContext.Session.GetInt32("IDUSUARIOVER");
            PacienteDetallado paciente = this.repo.FindPacienteDetallado(id);
            return View(paciente);
        }

        //Controller que elimina un USUARIO
        public IActionResult DeleteUsuario(int id, int tipo)
        {
            //**** Mostrar un Alert o algo de confirmacion, ya que esto borra todos los registros relacionados con ese usuario ****//
            this.repo.DeleteUsuario(id, tipo);
            return RedirectToAction("ZonaAdminUsuarios");
        }







        // Zona de RECEPCIONISTA ***.
        public IActionResult ZonaRecepcionista()
        {
            int idUsuario = (int)HttpContext.Session.GetInt32("IDUSUARILOGUEADO");
            Usuario usuario = this.repo.FindUsuario(idUsuario);
            return View(usuario);
        }



        // Zona de MEDICO ***.
        public IActionResult ZonaMedico()
        {
            int idUsuario = (int)HttpContext.Session.GetInt32("IDUSUARILOGUEADO");
            Usuario usuario = this.repo.FindUsuario(idUsuario);
            return View(usuario);
        }



        // Zona de PACIENTES ***.
        public IActionResult ZonaPaciente()
        {
            int idUsuario = (int)HttpContext.Session.GetInt32("IDUSUARILOGUEADO");
            Usuario usuario = this.repo.FindUsuario(idUsuario);
            return View(usuario);
        }
    }
}
