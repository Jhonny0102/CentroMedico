using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentroMedico.Models
{
    [Table("CITAS")]
    public class Cita
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("FECHA")]
        public DateTime Fecha { get; set; }
        [Column("HORA")]
        public TimeSpan Hora { get; set; }
        [Column("ID_ESTADOCITA")]
        public int EstadoCita { get; set; }
        [Column("ID_MEDICO")]
        public int Medico { get; set; }
        [Column("ID_PACIENTE")]
        public int Paciente { get; set; }
        [Column("COMENTARIO")]
        public string? Comentario { get; set; }

    }
}
