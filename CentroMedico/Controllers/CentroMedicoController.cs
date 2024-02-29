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
                HttpContext.Session.SetInt32("IDUSUARIO",usuario.Id);
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
        // Zona de Administrador.
        public IActionResult ZonaAdmin()
        {
            int idUsuario = (int)HttpContext.Session.GetInt32("IDUSUARIO");
            Usuario usuario = this.repo.FindUsuario(idUsuario);
            return View(usuario);
        }

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

        public IActionResult Details(int idUsuario, int idTipo)
        {
            if (idTipo == 1 || idTipo == 2)
            {
                ViewData["tipo"] = "usuario";
                ViewData["usuario"] = this.repo.FindUsuario(idUsuario);
                return View();
            }
            else if (idTipo == 3)
            {
                ViewData["tipo"] = "medico";
                ViewData["usuario"] = this.repo.FindMedico(idUsuario);
                return View();
            }
            else
            {
                ViewData["tipo"] = "paciente";
                ViewData["usuario"] = this.repo.FindPaciente(idUsuario);
                return View();
            }
        }

        // Zona de Recepcionistas.
        public IActionResult ZonaRecepcionista()
        {
            int idUsuario = (int)HttpContext.Session.GetInt32("IDUSUARIO");
            Usuario usuario = this.repo.FindUsuario(idUsuario);
            return View(usuario);
        }

        // Zona de Medico.
        public IActionResult ZonaMedico()
        {
            int idUsuario = (int)HttpContext.Session.GetInt32("IDUSUARIO");
            Usuario usuario = this.repo.FindUsuario(idUsuario);
            return View(usuario);
        }

        // Zona de Pacientes.
        public IActionResult ZonaPaciente()
        {
            int idUsuario = (int)HttpContext.Session.GetInt32("IDUSUARIO");
            Usuario usuario = this.repo.FindUsuario(idUsuario);
            return View(usuario);
        }
    }
}
