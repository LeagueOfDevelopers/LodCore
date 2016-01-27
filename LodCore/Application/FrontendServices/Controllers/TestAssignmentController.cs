using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace FrontendServices.Controllers
{
    public class TestAssignmentController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var playerNames = new[]
            {
                "LeBron James",
                "Kobe Bryant",
                "Chris Paul",
                "Pau Gasol",
                "Dirk Nowitzki",
                "Dwyane Wade",
                "Dwight Howard",
                "Tony Parker",
                "Andrey Kirillenko",
                "Vince Carter"
            };

            var teams = new[] {"CLE", "LAL", "CLE", "CLE", "LAL", "CLE", "CLE", "LAL", "LAL", "LAL" };

            var rand = new Random();
            var scores = Enumerable.Range(0, 10).Select(num => rand.Next(0, 30)).ToArray();

            return Request.CreateResponse(
                HttpStatusCode.OK,
                playerNames.Select((name, index) => new
                {
                    PlayerName = name,
                    Team = teams[index],
                    Score = scores[index]
                }), 
                new JsonMediaTypeFormatter());
        }
    }
}
