using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar_App.Model
{
    public class TrainingEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string? Type { get; set; }

        [Required]
        public int SignUps { get; set; }

        [Required]
        public int CoachEntityId { get; set; }

        [Required]
        public string? Location { get; set; }

        [Required]
        public string? Level { get; set; }

        [Required]
        public DateTime Beginning { get; set; }

        [Required]
        public DateTime End { get; set; }

    }
}
