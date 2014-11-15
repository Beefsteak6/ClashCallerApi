using System.Web.Http;
using ClashCaller.Helpers;
using ClashCaller.Models;

namespace ClashCaller.Controllers
{
    public class WarController : ApiController
    {
        // GET api/clash/12345
        public War Get(string id)
        {
            return ClashHelper.GetWar(id);
        }

        // POST api/war
        [HttpPost]
        public void Post([FromBody]Update warUpdate)
        {
            var result = ClashHelper.SetCalledTarget(warUpdate.Id.ToLower(), warUpdate.PlayerName, warUpdate.Rank, warUpdate.PositionInLine);
        }
    }
}