using Calendar_App.Model;
using Calendar_App.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar_App.Application
{

    public interface ITrainingService
    {
        public void AddTraining(TrainingEntity training);

        public Dictionary<int, Dictionary<string, int>> GetTrainingCountsByCoachIdForWeek(DateTime weekStartDate);

        public Dictionary<DateTime, Dictionary<string, double>> GetRoomBookedPercentage(DateTime startDate, DateTime endDate);

        public Dictionary<string, int> GetTrainingsByLevels();

        public IEnumerable<TrainingEntity> ReadAllTrainings();
    }


    public class TrainingService : ITrainingService
    {

        private readonly ITrainingDataProvider trainingDataProvider;
        public TrainingService(ITrainingDataProvider trainingDataProvider)
        {
            this.trainingDataProvider = trainingDataProvider;
        }

        public void AddTraining(TrainingEntity training)
        {

            var overlappingTrainings = trainingDataProvider.ReadAllTrainings()
                    .Where(t => t.Location == training.Location &&
                    t.End > training.Beginning &&
                    t.Beginning < training.End)
                    .ToList();

            if (overlappingTrainings.Any())
            {
                throw new TrainingCollisionException("There is a collision with another training");
            }

            if (training.Beginning > training.End)
            {
                throw new WrongDatesGivenException("The given duration is invalid");
            }
            else if (training.Location == "kis terem" && training.SignUps > 6)
            {
                throw new RoomCapacityExceededException("Too many signups");
            }
            else if (training.Location == "közepes terem" && training.SignUps > 10)
            {
                throw new RoomCapacityExceededException("Too many signups");
            }
            else if (training.Location == "nagy terem" && training.SignUps > 20)
            {
                throw new RoomCapacityExceededException("Too many signups");
            }

            trainingDataProvider.AddTraining(training);
        }

        public Dictionary<DateTime, Dictionary<string, double>> GetRoomBookedPercentage(DateTime startDate, DateTime endDate)
        {
            var occupation = new Dictionary<DateTime, Dictionary<string, double>>();
            var trainings = ReadAllTrainings()
                .Where(t => t.Beginning.Date >= startDate.Date && t.End.Date <= endDate.Date);

            double openHours = 16;
            for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                occupation[date] = new Dictionary<string, double>();
                foreach (var roomType in new[] { "kis terem", "közepes terem", "nagy terem" })
                {
                    double bookedHours = 0;
                    var dailyRoomBookings = trainings
                        .Where(t => t.Beginning.Date == date && t.Location == roomType);

                    foreach (var booking in dailyRoomBookings)
                    {
                        bookedHours += (booking.End - booking.Beginning).TotalHours;
                    }

                    double bookedPercentage = 0;
                    if (openHours > 0)
                    {
                        bookedPercentage = bookedHours / openHours * 100;
                    }
                    occupation[date][roomType] = Math.Round(bookedPercentage, 2);
                }
            }
            return occupation;
        }

        public Dictionary<int, Dictionary<string, int>> GetTrainingCountsByCoachIdForWeek(DateTime weekStartDate)
        {
            DateTime weekEndDate = weekStartDate.AddDays(6);
            var allTrainingsInWeek = ReadAllTrainings().Where(t => t.Beginning >= weekStartDate && t.Beginning <= weekEndDate);

            var result = new Dictionary<int, Dictionary<string, int>>();

            foreach (var training in allTrainingsInWeek)
            {
                int coachId = training.CoachEntityId;
                string? trainingType = training.Type;

                if (!result.ContainsKey(coachId))
                {
                    result[coachId] = new Dictionary<string, int>();
                }

                if (result[coachId].ContainsKey(trainingType!))
                {
                    result[coachId][trainingType!]++;
                }
                else
                {
                    result[coachId][trainingType!] = 1;
                }
            }

            return result;
        }

        public Dictionary<string, int> GetTrainingsByLevels()
        {
            return ReadAllTrainings()
                        .Where(t => t.Level != null)
                        .GroupBy(t => t.Level!)
                        .ToDictionary(g => g.Key, g => g.Count());
        }

        public IEnumerable<TrainingEntity> ReadAllTrainings()
        {
            return trainingDataProvider.ReadAllTrainings();
        }
    }

}
