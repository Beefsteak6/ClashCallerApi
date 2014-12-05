using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text.RegularExpressions;
using ClashCaller.Models;
using CsQuery;

namespace ClashCaller.Helpers
{
    public static class ClashHelper
    {
        public static War GetWar(string id)
        {
            var result = new War {Attacks = new List<Attack>()};
            var dom = CQ.CreateFromUrl("http://clashcaller.com/war.php?war=" + id);

            var clanSelector = dom["html body center h1 font"];
            result.ClanName = clanSelector.First().Text();
            result.ClanOpponent = clanSelector.Next().Text();
            
            var warSelector = dom[".calltable"].Find("tr td div img");
            foreach (var item in warSelector)
            {
                var attr = item.GetAttribute("onclick").Trim();
                const string matchString = @"^setstars\('(?<name>.*?)',\s'(?<stars>.*?)',\s'(?<rank>\d*)',\s'(?<third>\d*)'\)";
                var regex = new Regex(matchString, RegexOptions.None);
                var match = regex.Match(attr);

                if (match.Success)
                {
                    var attack = new Attack();
                    attack.Name = match.Groups["name"].Value;
                    attack.Stars = match.Groups["stars"].Value;
                    int rank = int.Parse(match.Groups["rank"].Value) + 1; //0 based index so adding one
                    attack.Rank = rank.ToString(CultureInfo.InvariantCulture);

                    result.Attacks.Add(attack);
                }
            }

            result.TotalRanks = GetTotalRanks(dom);

            return result;
        }

        private static int GetTotalRanks(CQ dom)
        {
            var selector = dom[".calltable>tbody>tr:last-child"].Find("img");
            var item = selector.FirstElement().GetAttribute("onclick").Trim();

            const string matchString = @"^requesttext\('(?<rank>\d*)'\)";
            var regex = new Regex(matchString, RegexOptions.None);
            var match = regex.Match(item);

            if (match.Success)
            {
                int result;
                int.TryParse(match.Groups["rank"].Value, out result);
                return result + 1;
            }

            return 0;
        }

        public static bool SetCalledTarget(string id, string playerName, int rank, int positionInLine)
        {
            const string serverAddress = "http://clashcaller.com/";

            using (var client = new HttpClient { BaseAddress = new Uri(serverAddress) })
            {
                var content = new FormUrlEncodedContent(new[] 
                {
                    new KeyValuePair<string, string>("type", "A"),
                    new KeyValuePair<string, string>("playername", playerName), 
                    new KeyValuePair<string, string>("stars", "-1"), 
                    new KeyValuePair<string, string>("posx", positionInLine.ToString(CultureInfo.InvariantCulture)), 
                    new KeyValuePair<string, string>("posy", rank.ToString(CultureInfo.InvariantCulture)), 
                    new KeyValuePair<string, string>("id", id)
                });

                client.DefaultRequestHeaders.Referrer = new Uri(new Uri(serverAddress), "war.php?id=" + id);
                var result = client.PostAsync("update.php", content).Result;

                return result.IsSuccessStatusCode;
            }
        }
    }
}
