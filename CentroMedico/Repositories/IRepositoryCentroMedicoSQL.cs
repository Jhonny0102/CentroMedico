using CentroMedico.Models;

namespace CentroMedico.Repositories //Interfaz generar automaticamente , lo guardamos por si Ctrl+R+I.
{
    public interface IRepositoryCentroMedicoSQL
    {
        void CreateCitaPaciente(DateTime fecha, TimeSpan hora, int idmedico, int idpaciente);
        void CreateMedico(string nombre, string apellido, string correo, string contra, int especialidad);
        void CreatePaciente(string nombre, string apellido, string correo, string contra, int telefono, string direccion, int edad, string genero, int medico);
        void CreatePeticionMedicamentoConId(int idMedico, int idMedicamento, int estadoMedicamento);
        void CreatePeticionMedicamentoSinId(int idMedico, string nombreMedicamento, string descripcionMedicamento);
        void CreatePeticionUsuarios(int idsolicitante, int idmodificado, int nuevoestado);
        void CreateUsuario(string nombre, string apellido, string correo, string contra, int tipo);
        void DeleteCita(int idCita);
        void DeleteUsuario(int id, int tipo);
        void EditCita(int idCita, DateTime fecha, TimeSpan hora, int idSeguimientoCita, int idMedico, string comentario);
        void EditMedico(int id, string nombre, string apellido, string correo, string contra, int estado, int tipo, int especialidad);
        void EditPaciente(int id, string nombre, string apellido, string correo, string contra, int telefono, string direccion, int edad, string genero, int estado, int tipo);
        void EditUsuario(int id, string nombre, string apellido, string correo, string contra, int estado, int tipo);
        Cita FindCita(int idCita);
        int FindCitaDispo(int idmedico, DateTime fecha, TimeSpan hora);
        List<CitaDetalladaMedicos> FindCitasDetalladasMedicos(int idmedico, DateTime fecha);
        CitaDetalladaMedicos FindCitasDetalladasMedicosSinFiltro(int idcita);
        List<CitaDetalladaMedicos> FindCitasDetalladasPAciente(int idpaciente, DateTime fechadesde, DateTime? fechahasta);
        List<CitaDetalladaMedicos> FindCitasPaciente(int idpaciente);
        DatosExtrasPacientes FindDatosExtrasPacientes(int id);
        Medicamentos FindMedicamento(int idMedicamento);
        MedicamentoYPaciente FindMedicamentoYPaciente(int id);
        Medico FindMedico(int id);
        MedicoDetallado FindMedicoDetallado(int id);
        string FindNombreEstado(int idEstado);
        Paciente FindPaciente(int id);
        PacienteDetallado FindPacienteDetallado(int id);
        Paciente FindPacienteDistintoDetallado(string nombre, string apellido, string correo);
        Usuario FindUsuario(int id);
        UsuarioDetallado FindUsuarioDetallado(int id);
        List<CitaDetallado> GetAllCitas();
        List<Medicamentos> GetAllMedicamentos();
        List<MedicamentoYPaciente> GetAllMedicamentosPaciente(int idpaciente);
        List<Paciente> GetAllPacientes();
        List<Paciente> GetAllPacientesBaja();
        List<SeguimientoCita> GetAllSeguimientoCita();
        List<CitaDetalladaMedicos> GetCitasDetalladasMedico(int idmedico);
        List<Especialidades> GetEspecialidades();
        List<Estados> GetEstados();
        int GetIdMedico(int especialidad);
        Usuario GetLogin(string correo, string contra);
        List<Medicamentos> GetMedicamentos();
        MedicosPacientes GetMedicoPaciente(int idpaciente);
        MedicoDetallado GetMiMedico(int idPaciente);
        List<PeticionesDetallado> GetPeticionesDetallado();
        List<PeticionesMedicamentoDetallado> GetPeticionesMedicametentosDetallado();
        SeguimientoCita GetSeguimientoCita(int idSeguimiento);
        List<UsuariosTipo> GetTipoUsuarios();
        List<Usuario> GetUsuarios();
        List<Usuario> GetUsuariosTipo(int tipo);
        List<MedicosPacientes> MisPacientes(int idMedico);
        void OkNoPeticion(int idPeticion);
        void OkNoPeticionMedicamento(int idPeti);
        void OkPetcion(int idPeticion, int idUsuario, int idEstadoNuevo);
        void OkPeticionMedicamento(int idPeti, int? idMedicamento, string nombre, string? descripcion, int estado);
        void UpdateCitaDetalladaPaciente(int idcita, DateTime fecha, TimeSpan hora);
        void UpdateCitaMedica(int idmedico, int idpaciente, int idcita, string comentario, int seguimiento, List<int> medicamentos);
        void UpdateMedicamentoYPaciente(int id);
    }
}