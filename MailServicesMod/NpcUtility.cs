﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using StardewValley.Locations;

namespace MailServicesMod
{
    internal class NpcUtility
    {
        public static NPC getCharacterFromName(string npcName)
        {
            var npc = Game1.getCharacterFromName(npcName);
            if (npc == null)
            {
                var movieTheaterLocation = Game1.locations.FirstOrDefault(l => l is MovieTheater);
                if (movieTheaterLocation != null)
                {
                    npc = movieTheaterLocation.getCharacters().FirstOrDefault(character => !character.eventActor && character.Name.Equals(npcName));
                }
            }
            if (npc == null) throw new Exception($"Character '{npcName}' not found.");
            return npc;
        }
    }
}
