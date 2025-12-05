using LiveStatsManager.Hubs;
using LiveStatsManager.Models.TypedDataStore;
using LiveStatsManager.Services.DataStore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace LiveStatsManager.Controllers
{
    [Route("api/wrestling")]
    [ApiController]
    public class WrestlingAPI(IHubContext<TypedLiveDataHub, ITypedLiveDataHub> hub, TypedDataStore store) : ControllerBase
    {
        private WrestlingScorebugState State => store.WrestlingScorebugState;

        [HttpPost("state/home/wrestler")]
        public async Task<string> SetHomeWrestler(Wrestler wrestler)
        {
            store.WrestlingScorebugState = State with
            {
                HomeWrestler = wrestler
            };

            return "OK";
        }

        [HttpPost("state/away/wrestler")]
        public async Task<string> SetAwayWrestler(Wrestler wrestler)
        {
            store.WrestlingScorebugState = State with
            {
                AwayWrestler = wrestler
            };

            return "OK";
        }

        [HttpPost("state/home/score/{score}")]
        public async Task<string> SetHomeScore(int score)
        {
            store.WrestlingScorebugState = State with { HomeScore = score };
            return "OK";
        }

        [HttpPost("state/away/score/{score}")]
        public async Task<string> SetAwayScore(int score)
        {
            store.WrestlingScorebugState = State with { AwayScore = score };
            return "OK";
        }

        [HttpPost("state/game/clock/{seconds}")]
        public async Task<string> SetClock(int seconds)
        {
            store.WrestlingScorebugState = State with { Clock = seconds };
            return "OK";
        }

        [HttpPost("state/game/period/{period}")]
        public async Task<string> SetPeriod(int period)
        {
            store.WrestlingScorebugState = State with { Period = period };
            return "OK";
        }

        [HttpPost("state/game/weight/{weight}")]
        public async Task<string> SetWeight(string weight)
        {
            store.WrestlingScorebugState = State with { WeightClass = weight };
            return "OK";
        }
    }
}
