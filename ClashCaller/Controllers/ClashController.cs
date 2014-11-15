using System.Web.Http;
using ClashCaller.Helpers;
using ClashCaller.Models;

namespace ClashCaller.Controllers
{
    public class ClashController : ApiController
    {
        // GET api/clash/12345
        public War Get(string id)
        {
            return ClashHelper.GetWar(id);
        }

        // POST api/clash
        public void Post([FromBody]string value)
        {
        }

        // PUT api/clash/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/clash/5
        public void Delete(int id)
        {
        }
    }
}