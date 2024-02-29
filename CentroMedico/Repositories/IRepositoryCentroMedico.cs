using CentroMedico.Models;

namespace CentroMedico.Repositories
{
    public interface IRepositoryCentroMedico
    {
        public Usuario GetLogin(string correo, string contra);
        
        public void CreatePaciente(string nombre, string apellido, string correo, string contra);
        public Usuario FindUsuario(int id);
        public void DeleteUsuario(int id);
        public void EditUsuario(int id);

        public List<UsuariosTipo> GetTipoUsuarios();
        public List<Usuario> GetUsuariosTipo(int tipo);
        public List<Usuario> GetUsuarios();
        public Paciente FindPaciente(int id);
    }
}
