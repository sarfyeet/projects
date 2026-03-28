using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Calendar_App.Model
{
    public class CoachEntity
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        public string? Name { get; set; }

        [Required]
        public long PhoneNumber { get; set; }

        public virtual ICollection<TrainingEntity> Trainings { get; set; }




        public CoachEntity()
        {

            Trainings = new HashSet<TrainingEntity>();

        }
    }
}
