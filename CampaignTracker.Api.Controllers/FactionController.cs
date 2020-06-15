using Abstractions;
using Factions;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CampaignTracker.Api.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class FactionController
        : ODataController
    {
        private readonly IRepositoryProvider repositoryProvider;

        public FactionController(IRepositoryProvider repositoryProvider)
        {
            this.repositoryProvider = repositoryProvider;
        }
        
        [HttpGet]
        [EnableQuery]
        [ODataRoute("Factions")]
        public async Task<IActionResult> Get()
        {
            var results = this.repositoryProvider.RepositoryFor<Faction>().CreateQuery();

            return this.Ok(results);
        }
    }
}
