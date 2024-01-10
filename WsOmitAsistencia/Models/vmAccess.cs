using System.ComponentModel.DataAnnotations;

namespace WsOmitAsistencia.Models
{
    public class vmAccess
    {
        [Required]
        public string login { get; set; }
        [Required]
        public string pwd { get; set; }

    }
}
