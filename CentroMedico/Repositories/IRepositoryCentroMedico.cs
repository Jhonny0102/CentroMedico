using CentroMedico.Models;

namespace CentroMedico.Repositories
{
    public interface IRepositoryCentroMedico
    {
        public Usuario GetLogin(string correo, string contra);
        
        public void CreatePaciente(string nombre, string apellido, string correo, string contra);

        public Usuario FindUsuario(int id);
        public UsuarioDetallado FindUsuarioDetallado(int id);
        
        public void DeleteUsuario(int id, int tipo);
        public void EditUsuario(int id, int tipo);

        public List<UsuariosTipo> GetTipoUsuarios();
        public List<Usuario> GetUsuariosTipo(int tipo);
        public List<Usuario> GetUsuarios();

        public Paciente FindPaciente(int id);
        public PacienteDetallado FindPacienteDetallado(int id);
        public DatosExtrasPacientes FindDatosExtrasPacientes(int id);

        public Medico FindMedico(int id);
        public MedicoDetallado FindMedicoDetallado(int id);
    }

}
