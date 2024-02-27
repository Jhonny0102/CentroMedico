using CentroMedico.Models;

namespace CentroMedico.Repositories
{
    public interface IRepositoryCentroMedico
    {
        public void AltaPaciente(string nombre, string apellido, string correo, string contra);
        public Usuario Login(string correo, string contra);
        public Usuario FindUsuario(int id);
        public void EliminarUsuario(int id);
        public void EditarUsuario(int id);
    }
}
