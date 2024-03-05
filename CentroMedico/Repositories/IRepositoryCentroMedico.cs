using CentroMedico.Models;

namespace CentroMedico.Repositories
{
    public interface IRepositoryCentroMedico
    {
        public Usuario GetLogin(string correo, string contra);
        
        public void CreatePaciente(string nombre, string apellido, string correo, string contra, int telefono, string direccion, int edad, string genero, int medico);
        public void CreateMedico(string nombre, string apellido, string correo, string contra, int especialidad);
        public void CreateUsuario(string nombre, string apellido, string correo, string contra, int tipo );

        public void DeleteUsuario(int id, int tipo);
        public void EditUsuario(int id, string nombre, string apellido, string correo, string contra, int estado, int tipo);
        public void EditMedico(int id, string nombre, string apellido, string correo, string contra, int especialidad, int estado, int tipo);
        public void EditPaciente(int id, string nombre, string apellido, string correo, string contra, int telefono , string direccion , int edad , string genero , int Estado, int tipo);

        public List<UsuariosTipo> GetTipoUsuarios();
        public List<Usuario> GetUsuariosTipo(int tipo);
        public List<Usuario> GetUsuarios();
        public List<Especialidades> GetEspecialidades();

        public Paciente FindPaciente(int id);
        public PacienteDetallado FindPacienteDetallado(int id);
        public DatosExtrasPacientes FindDatosExtrasPacientes(int id);
        public Medico FindMedico(int id);
        public MedicoDetallado FindMedicoDetallado(int id);
        public Usuario FindUsuario(int id);
        public UsuarioDetallado FindUsuarioDetallado(int id);

        public List<Citas> GetAllCitas();
        public MedicoDetallado GetMiMedico(int idPaciente);
    }

}
