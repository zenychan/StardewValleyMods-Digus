﻿using Harmony;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.TerrainFeatures;

namespace WaterRetainingFieldMod
{
    /// <summary>The mod entry class loaded by SMAPI.</summary>
    public class WaterRetainingFieldModEntry : Mod
    {
        internal static DataLoader DataLoader;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.GameLoop.DayStarted += OnDayStarted;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the game is launched, right before the first update tick. This happens once per game session (unrelated to loading saves). All mods are loaded and initialised at this point, so this is a good time to set up mod integrations.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            DataLoader = new DataLoader(Helper);

            var harmony = HarmonyInstance.Create("Digus.WaterRetainingFieldMod");

            var hoeDirtDayUpdate = typeof(HoeDirt).GetMethod("dayUpdate");
            var hoeDirtOverridesDayUpdatePrefix = typeof(HoeDirtOverrides).GetMethod("DayUpdatePrefix");
            var hoeDirtOverridesDayUpdatePostfix = typeof(HoeDirtOverrides).GetMethod("DayUpdatePostfix");
            harmony.Patch(hoeDirtDayUpdate, new HarmonyMethod(hoeDirtOverridesDayUpdatePrefix), new HarmonyMethod(hoeDirtOverridesDayUpdatePostfix));
        }

        /// <summary>Raised after the game begins a new day (including when the player loads a save).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            HoeDirtOverrides.TileLocationState.Clear();
        }
    }
}
