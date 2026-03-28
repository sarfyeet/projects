using Calendar_App.Model;
using Calendar_App.Persistence.MsSql;

namespace Calendar_App.Application
{
    public delegate void CoachAddedEventHandler();
    public interface ICoachService
    {
        event CoachAddedEventHandler CoachAdded;


        CoachEntity GetCoachById(int id);
        public void AddCoach(CoachEntity newCoach);
        public void RemoveCoach(int id);
        public void UpdateCoach(int oldCoachId, CoachEntity newCoach);
        public IEnumerable<CoachEntity> GetCoachEntities();
        public bool IsThereACoach(int id);


    }

    public class CoachService : ICoachService
    {
        private readonly ICoachDataProvider coachDataProvider;
        public CoachService(ICoachDataProvider coachDataProvider)
        {
            this.coachDataProvider = coachDataProvider;
        }

        public event CoachAddedEventHandler? CoachAdded;



        public CoachEntity GetCoachById(int id)
        {
            return GetCoachEntities().FirstOrDefault(c => c.Id == id);
        }



        public bool IsThereACoach(int id)
        {

            return GetCoachEntities().Any(c => c.Id == id);
        }

        public void AddCoach(CoachEntity newCoach)
        {
            coachDataProvider.AddCoach(newCoach);
            CoachAdded?.Invoke();
        }

        public void RemoveCoach(int id)
        {
            coachDataProvider.RemoveCoach(id);
        }

        public void UpdateCoach(int oldCoachId, CoachEntity newCoach)
        {
            coachDataProvider.UpdateCoach(oldCoachId, newCoach);
        }

        public IEnumerable<CoachEntity> GetCoachEntities()
        {
            return coachDataProvider.GetCoachEntities();
        }
    }
}
