using NUnit.Framework;
using Moq;
using Calendar_App.Application;
using Calendar_App.Model;
using Calendar_App.Persistence.MsSql;


namespace Calendar_App.Test
{
    [TestFixture]
    public class TrainingTests
    {
        private Mock<ITrainingDataProvider> _trainingDataProviderMock;
        private TrainingService _trainingService;

        [SetUp]
        public void SetUp()
        {
            _trainingDataProviderMock = new Mock<ITrainingDataProvider>();
            _trainingService = new TrainingService(_trainingDataProviderMock.Object);

        }

        [Test]
        public void ReadAllTrainings_ReturnsTrainings()
        {
            var trainings = new List<TrainingEntity>
            {
                new TrainingEntity { Type = "Pilátesz", Level = "haladó", Location = "nagy terem", Beginning = DateTime.Now.AddHours(1), End = DateTime.Now.AddHours(2), SignUps = 5 },
                new TrainingEntity { Type = "Súlyzós", Level = "haladó", Location = "kis terem", Beginning = DateTime.Now.AddHours(1), End = DateTime.Now.AddHours(2), SignUps = 3 }
            };
            _trainingDataProviderMock.Setup(p => p.ReadAllTrainings()).Returns(trainings);

            var result = _trainingService.ReadAllTrainings();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));                       
        }

        [Test]
        public void AddTraining_Successful_DataProviderCalled()
        {

            var validTraining = new TrainingEntity
            {
                Id = 1,
                Type = "súlyzós edzés",
                Beginning = DateTime.Now.AddHours(1),
                End = DateTime.Now.AddHours(2),
                Location = "nagy terem" ,
                SignUps = 10,
                CoachEntityId = 1,
                Level = "középhaladó"
            };

            _trainingService.AddTraining(validTraining);

            _trainingDataProviderMock.Verify(dataProvider => dataProvider.AddTraining(validTraining), Times.Once);
        }

        [Test]
        public void AddTraining_InCaseOfCollision_TrainingCollisionExceptionThrow()
        {

            var existingTraining = new TrainingEntity
            {
                Id = 1,
                Type = "súlyzós edzés",
                Beginning = DateTime.Now.AddHours(1),
                End = DateTime.Now.AddHours(2),
                Location = "nagy terem",
                SignUps = 10,
                CoachEntityId = 1,
                Level = "középhaladó"
            };

            var collidingTraining = new TrainingEntity
            {
                Id = 2, 
                Type = "súlyzós edzés",
                Beginning = DateTime.Now.AddHours(1).AddMinutes(15), // Ütközés az existingTraining-gel
                End = DateTime.Now.AddHours(2).AddMinutes(30),
                Location = "nagy terem",
                SignUps = 10,
                CoachEntityId = 1,
                Level = "középhaladó"
            };


            _trainingDataProviderMock.Setup(dataProvider => dataProvider.AddTraining(existingTraining));            
            _trainingDataProviderMock.Setup(dataProvider => dataProvider.AddTraining(collidingTraining))
                .Throws(new TrainingCollisionException("Collision detected"));


            _trainingService.AddTraining(existingTraining);


            Assert.Throws<TrainingCollisionException>(() => _trainingService.AddTraining(collidingTraining));
        }

        [Test]
        public void AddTraining_TooManySignups_RoomCapacityExceededExceptionThrow()
        {

            var overCapacityTraining = new TrainingEntity
            {
                Id = 1,
                Type = "súlyzós edzés",
                Beginning = DateTime.Now.AddHours(1),
                End = DateTime.Now.AddHours(2),
                Location = "kis terem",
                SignUps = 7,
                CoachEntityId = 1,
                Level = "középhaladó"
            };
            Assert.Throws<RoomCapacityExceededException>(() => _trainingService.AddTraining(overCapacityTraining));
        }

        [Test]
        public void AddTraining_WrongDates_WrongDatesGivenExceptionThrow()
        {
            var wrongDateTraining = new TrainingEntity
            {
                Id = 1,
                Type = "súlyzós edzés",
                Beginning = DateTime.Now.AddHours(2),
                End = DateTime.Now,
                Location = "kis terem",
                SignUps = 7,
                CoachEntityId = 1,
                Level = "középhaladó"
            };
            Assert.Throws<WrongDatesGivenException>(() => _trainingService.AddTraining(wrongDateTraining));
        }
    }
}
