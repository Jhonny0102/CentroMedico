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
        public MedicoDetallado FindMedicoDetallado(int id); //Este metodo busca un medico de forma detallada
        public MedicoDetallado GetMiMedico(int idPaciente); //Este metodo es para obtener los datos del medico asignado al paciente.
        public Usuario FindUsuario(int id);
        public UsuarioDetallado FindUsuarioDetallado(int id);

        public List<CitaDetallado> GetAllCitas();
        public Cita FindCita(int idCita);
        public void DeleteCita(int idCita);
        public void EditCita(int idCita, DateTime fecha, TimeSpan hora, int idSeguimientoCita, int idMedico, string comentario);
        public List<MedicosPacientes> MisPacientes(int idMedico);
        public List<PeticionesDetallado> GetPeticionesDetallado();
        public void OkPetcion(int idPeticion, int idUsuario, int idEstadoNuevo);
        public void OkNoPeticion(int idPeticion);
        public List<PeticionesMedicamentoDetallado> GetPeticionesMedicametentosDetallado();

        public void OkPeticionMedicamento(int idPeti, int? idMedicamento ,string nombre , string? descripcion , int estado);
        public void OkNoPeticionMedicamento(int idPeti);
        public void CreatePeticionMedicamentoConId(int idMedico, int idMedicamento, int estadoMedicamento);
        public void CreatePeticionMedicamentoSinId(int idMedico, string nombreMedicamento, string descripcionMedicamento);
        public List<Medicamentos> GetMedicamentos();
        public Medicamentos FindMedicamento(int idMedicamento);
        public List<Estados> GetEstados();
        public string FindNombreEstado(int idEstado);

        public void CreateCitaPaciente(DateTime fecha, TimeSpan hora, int idmedico, int idpaciente);
        public int FindCitaDispo(int idmedico, DateTime fecha, TimeSpan hora);
        public List<CitaDetalladaMedicos> GetCitasDetalladasMedico(int idmedico);
        public List<CitaDetalladaMedicos> FindCitasDetalladasMedicos(int idmedico, DateTime fecha);
        public List<CitaDetalladaMedicos> FindCitasDetalladasPAciente(int idpaciente, DateTime fechadesde, DateTime? fechahasta);
        public List<SeguimientoCita> GetAllSeguimientoCita();
        public void UpdateCitaMedica(int idmedico, int idpaciente, int idcita, string comentario, int seguimiento, List<int> medicamentos);
        public List<Medicamentos> GetAllMedicamentos();
        public void UpdateCitaDetalladaPaciente(int idcita, DateTime fecha, TimeSpan hora);
        public List<MedicamentoYPaciente> GetAllMedicamentosPaciente(int idpaciente);
        public MedicamentoYPaciente FindMedicamentoYPaciente(int id);
        public void UpdateMedicamentoYPaciente(int id);
        public Paciente FindPacienteDistintoDetallado(string nombre, string apellido, string correo);
        public MedicosPacientes GetMedicoPaciente(int idpaciente);
    }

}
