using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoot_em_Up
{
    class FactionManager
    {
        public static Faction[] factions;

        public static void LoadJSON() {
            using (StreamReader sr = new StreamReader("../Content/Factions.json"))
            {
                String json = sr.ReadToEnd();
                factions = JSONManager.deserializeJson<Faction[]>(json);
            }
        }
    }
}
