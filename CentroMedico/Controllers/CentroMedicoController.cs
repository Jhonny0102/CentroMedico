﻿using CentroMedico.Models;
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
            return View();
        }
        [HttpPost]
        public IActionResult CreatePaciente(Paciente paciente)
        {
            this.repo.CreatePaciente(paciente.Nombre, paciente.Apellido, paciente.Correo,paciente.Contra, paciente.Telefono, paciente.Direccion, paciente.Edad, paciente.Genero);
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
        public IActionResult EditUsuario()
        {
            int id = (int)HttpContext.Session.GetInt32("IDUSUARIOEDIT");
            Usuario usuario = this.repo.FindUsuario(id);
            return View(usuario);
        }
        [HttpPost]
        public IActionResult EditUsuario(Usuario usuario)
        {
            this.repo.EditUsuario(usuario.Id,usuario.Nombre,usuario.Apellido,usuario.Correo,usuario.Contra,usuario.Id_EstadoUsuario,usuario.Id_TipoUsuario);
            return RedirectToAction("ZonaAdminUsuarios");
        }

        //Controller de editar MEDICO
        public IActionResult EditMedico()
        {
            int id = (int)HttpContext.Session.GetInt32("IDUSUARIOEDIT");
            Medico medico = this.repo.FindMedico(id);
            ViewData["ESPECIALIDADES"] = this.repo.GetEspecialidades();
            return View(medico);
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







        /// Fin Zona CRUD CITAS ///

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
