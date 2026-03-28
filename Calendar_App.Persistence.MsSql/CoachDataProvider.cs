using Calendar_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar_App.Persistence.MsSql
{
    public interface ICoachDataProvider
    {
        public void AddCoach(CoachEntity newCoach);
        public void RemoveCoach(int id);
        public void UpdateCoach(int oldCoachId, CoachEntity newCoach);
        public IEnumerable<CoachEntity> GetCoachEntities();

    }


    public class CoachDataProvider : ICoachDataProvider
    {
        private readonly AppDbContext context;

        public CoachDataProvider(AppDbContext context)
        {
            this.context = context;
        }

        public void AddCoach(CoachEntity newCoach)
        {
            context.Coaches.Add(newCoach);
            context.SaveChanges();
        }



        public IEnumerable<CoachEntity> GetCoachEntities()
        {
            return context.Coaches;
        }

        public void RemoveCoach(int id)
        {
            var coachToDelete = context.Coaches.First(c => c.Id == id);
            context.Coaches.Remove(coachToDelete);
            context.SaveChanges();
        }

        public void UpdateCoach(int oldCoachId, CoachEntity newCoach)
        {
            var oldCoach = context.Coaches.First(c => c.Id == oldCoachId);
            oldCoach.Name = newCoach.Name;
            oldCoach.PhoneNumber = newCoach.PhoneNumber;
            context.SaveChanges();
        }
    }
}
