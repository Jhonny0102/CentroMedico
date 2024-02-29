using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentroMedico.Models
{
    [Table("V_ALL_MEDICOS")]
    public class Medico
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("APELLIDO")]
        public string Apellido { get; set; }
        [Column("CORREO")]
        public string Correo { get; set; }
        [Column("CONTRA")]
        public string Contra { get; set; }
        [Column("ESPECIALIDAD")]
        public string Especialidad{ get; set; }
        public string Genero { get; set; }
        [Column("ID_ESTADOUSUARIO")]
        public int EstadoUsuario { get; set; }
        [Column("ID_TIPOUSUARIO")]
        public int TipoUsuario { get; set; }
    }
}
