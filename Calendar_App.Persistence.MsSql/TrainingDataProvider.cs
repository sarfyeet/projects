using Calendar_App.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar_App.Persistence.MsSql
{
    public interface ITrainingDataProvider
    {
        public void AddTraining(TrainingEntity training);
        public IEnumerable<TrainingEntity> ReadAllTrainings();

    }

    public class TrainingDataProvider : ITrainingDataProvider
    {
        private readonly AppDbContext context;

        public TrainingDataProvider(AppDbContext context)
        {
            this.context = context;
        }


        public void AddTraining(TrainingEntity training)
        {

            context.Trainings.Add(training);
            context.SaveChanges();
        }

        public IEnumerable<TrainingEntity> ReadAllTrainings()
        {
            return context.Trainings;
        }

    }

    public class TrainingCollisionException : Exception
    {
        public TrainingCollisionException(string message) : base(message) { }
    }

    public class RoomCapacityExceededException : Exception
    {
        public RoomCapacityExceededException(string message) : base(message) { }
    }

    public class WrongDatesGivenException : Exception
    {
        public WrongDatesGivenException(string message) : base(message) { }
    }
}
