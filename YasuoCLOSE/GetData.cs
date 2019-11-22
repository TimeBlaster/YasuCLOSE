using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RiotNet;
using RiotNet.Models;
using YasuoCLOSE;

namespace LoLInfo
{
    public class GetData
    {        
        private static CurrentGameInfo GetCurrentGame(RiotClient client, string summonerName, CancellationToken ct)
        {
            var summoner = client.GetSummonerBySummonerNameAsync(summonerName, default, ct).Result;
            return client.GetCurrentGameBySummonerIdAsync(summoner.Id, default, ct).Result;
        }

        public static bool YasuoInGame(RiotClient client,string summonerName, CancellationToken ct)
        {
            var currentGame = GetCurrentGame(client, summonerName, ct);

            foreach(var participant in currentGame.Participants)
                if(participant.ChampionId == 157)
                    return true;

            return false;
        }
    }
}
