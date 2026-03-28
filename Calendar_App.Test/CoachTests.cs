using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calendar_App.Application;
using Calendar_App.Model;
using Calendar_App.Persistence.MsSql;
using Moq;
using NUnit.Framework;

namespace 
    
    Test
{
    [TestFixture]
    public class CoachTests
    {
        private Mock<ICoachDataProvider> _coachDataProviderMock;
        private CoachService _coachService;


        [SetUp]
        public void SetUp()
        {
            _coachDataProviderMock = new Mock<ICoachDataProvider>();
            _coachService = new CoachService(_coachDataProviderMock.Object);
        }

        [Test]
        public void GetCoachEntities_ReturnsCoaches()
        {
            var coaches = new List<CoachEntity>
            {new CoachEntity{ Name = "Izmos Izolda", PhoneNumber = 36205033145 },
             new CoachEntity{Name = "Súlyos Sándor", PhoneNumber = 36305033147 }
            };
            
            _coachDataProviderMock.Setup(x => x.GetCoachEntities()).Returns(coaches);

            var result = _coachService.GetCoachEntities();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
            
        }

        [Test]
        public void GetCoachById_ReturnsCorrectCoach_WhenIdExists()
        {

            int coachId = 1;
            var coachList = new List<CoachEntity>
            {
            new CoachEntity { Id = 1, Name = "Izmos István", PhoneNumber = 123 },
            new CoachEntity { Id = 2, Name = "Gyors Gyula", PhoneNumber = 456 }
            };


            _coachDataProviderMock.Setup(x => x.GetCoachEntities()).Returns(coachList);


            CoachEntity result = _coachService.GetCoachById(coachId);


            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(coachId));
            Assert.That(result.Name, Is.EqualTo("Izmos István"));
            Assert.That(result.PhoneNumber, Is.EqualTo(123));
        }

        [Test]
        public void GetCoachById_ReturnsNull_WhenIdDoesNotExist()
        {

            int nonExistentCoachId = 999;
            var coachList = new List<CoachEntity>
            {
            new CoachEntity { Id = 1, Name = "Izmos István", PhoneNumber = 123 },
            new CoachEntity { Id = 2, Name = "Gyors Gyula", PhoneNumber = 456 }
            };


            _coachDataProviderMock.Setup(x => x.GetCoachEntities()).Returns(coachList);


            CoachEntity result = _coachService.GetCoachById(nonExistentCoachId);


            Assert.That(result, Is.Null);
        }

        [Test]
        public void AddCoach_Successful_DataProviderCalled()
        {

            var newCoach = new CoachEntity { Name = "Bicepsz Bendegúz", PhoneNumber = 36201234567 };

            _coachService.AddCoach(newCoach);

            _coachDataProviderMock.Verify(dataProvider => dataProvider.AddCoach(newCoach), Times.Once);
        }


        [Test]
        public void AddCoach_Successful_EventTest()
        {

            var newCoach = new CoachEntity { Name = "Fekvenyomó Ferenc", PhoneNumber = 36201234567 };
            bool eventIsWorking = false;

            _coachService.CoachAdded += () => eventIsWorking = true;

            _coachService.AddCoach(newCoach);

            Assert.That(eventIsWorking, Is.True);
        }

    }
}
