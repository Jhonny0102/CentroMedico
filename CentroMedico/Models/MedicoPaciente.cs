using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CentroMedico.Models
{
    [Table("MEDICOPACIENTE")]
    public class MedicoPaciente
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("ID_MEDICO")]
        public int Id_Medico { get; set; }
        [Column("ID_PACIENTE")]
        public int Id_Paciente { get; set; }
    }
}
