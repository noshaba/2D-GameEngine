using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoot_em_Up
{
    class FactionManager
    {
        public static Faction[] factions;
        private static string json =
        @"[
            {
                ""ID"" : 0,
                ""GainableRep"" : false,
                ""Reputation"" : [100, 50, 100]
            },
            {
                ""ID"" : 1,
                ""GainableRep"" : true,
                ""Reputation"" : [50, 100, 0]
            },
            {
                ""ID"" : 2,
                ""GainableRep"" : true,
                ""Reputation"" : [100, 0, 100]
            }
        ]";

        public static void LoadJSON() {
            factions = JSONManager.deserializeJson<Faction[]>(json);
        }
    }
}
