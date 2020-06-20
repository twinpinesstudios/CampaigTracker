using Abstractions;
using Factions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampaignTracker.Api.Controllers.Tests
{
    [TestClass]
    public class FactionControllerTests
    {
        private Mock<IRepositoryProvider> mockRepositoryProvider;
        private Mock<IRepository<Faction>> mockFactionRepository;

        [TestInitialize]
        public void Setup()
        {
            mockRepositoryProvider = new Mock<IRepositoryProvider>(MockBehavior.Strict);
            mockFactionRepository = new Mock<IRepository<Faction>>(MockBehavior.Strict);
        }

        [TestCleanup]
        public void Cleanup()
        {
            mockRepositoryProvider.Verify();
            mockFactionRepository.Verify();
        }

        [TestMethod]
        public async Task GetShouldReturnAQueryableListOfFactions()
        {
            var factions = new List<Faction>
            {
                new Faction{Name = "faction1"},
                new Faction{Name = "faction2"},
                new Faction{Name = "faction3"},
                new Faction{Name = "faction4"},
            };

            mockRepositoryProvider
                .Setup(x => x.RepositoryFor<Faction>())
                .Returns(mockFactionRepository.Object)
                .Verifiable();

            mockFactionRepository
                .Setup(x => x.CreateQuery())
                .Returns(factions.AsQueryable())
                .Verifiable();

            var subject = new FactionController(mockRepositoryProvider.Object);

            var result = await subject.Get();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);
            var results = okObjectResult.Value as IQueryable<Faction>;
            Assert.IsNotNull(results);
            Assert.AreEqual(4, results.Count());
            Assert.IsTrue(results.Any(x => x.Name == "faction1"));
            Assert.IsTrue(results.Any(x => x.Name == "faction2"));
            Assert.IsTrue(results.Any(x => x.Name == "faction3"));
            Assert.IsTrue(results.Any(x => x.Name == "faction4"));
        }
    }
}
