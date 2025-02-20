﻿using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.TerrainFeatures;
using System.Collections.Generic;

namespace DestroyableBushes
{
    /// <summary>A set of serializable save data for this mod.</summary>
    public class ModData
    {
        /// <summary>A list of destroyed bushes that have not yet been respawned.</summary>
        public List<DestroyedBush> DestroyedBushes { get; set; } = new List<DestroyedBush>();

        /// <summary>The information needed to respawn a destroyed bush.</summary>
        public class DestroyedBush
        {
            /// <summary>The name of the destroyed bush's <see cref="GameLocation"/>.</summary>
            public string LocationName { get; set; }
            /// <summary>The destroyed bush's tile location.</summary>
            /// <remarks>
            /// This value should be based on the <see cref="Bush"/> instance's tile field, not the player or tool.
            /// Note that the field's name varies between Stardew versions.
            /// </remarks>
            public Vector2 Tile { get; set; }
            /// <summary>The destroyed bush's size.</summary>
            public int Size { get; set; }
            /// <summary>The destroyed bush's "town bush" flag.</summary>
            /// <remarks>When true, the bush will use alternate sprites, and medium bushes will not grow berries.</remarks>
            public bool TownBush { get; set; } = false;
            /// <summary>The destroyed bush's tilesheet offset value. If null, this mod should not edit the value.</summary>
            /// <remarks>
            /// This value affects the sprite used by bushes, and also controls whether medium and walnut bushes currently contain those items.
            /// This mod should ignore this value when respawning bushes capable of producing items.
            /// </remarks>
            public int? TilesheetOffset { get; set; } = null;

            [JsonProperty]
            private int day;
            [JsonProperty]
            private string season;
            [JsonProperty]
            private int year;

            [JsonIgnore]
            public SDate DateDestroyed
            {
                get
                {
                    return new SDate(day, season, year);
                }
                set
                {
                    day = value.Day;
                    season = value.Season;
                    year = value.Year;
                }
            }

            public DestroyedBush()
            {

            }

            public DestroyedBush(string locationName, Vector2 tile, int size, bool townBush = false, int? tilesheetOffset = null)
            {
                LocationName = locationName;
                Tile = tile;
                Size = size;
                TownBush = townBush;
                TilesheetOffset = tilesheetOffset;
                DateDestroyed = SDate.Now();
            }

            /// <summary>Returns all tiles that would be obstructed by this bush's collision box.</summary>
            /// <returns>Each tile that would be obstructed by this bush's collision box.</returns>
            public IEnumerable<Vector2> GetCollisionTiles()
            {
                switch (Size)
                {
                    case Bush.mediumBush: //2 tiles wide
                    case Bush.walnutBush:
                        yield return Tile;
                        yield return new Vector2(Tile.X + 1, Tile.Y);
                        break;
                    case Bush.largeBush: //3 tiles wide
                        yield return Tile;
                        yield return new Vector2(Tile.X + 1, Tile.Y);
                        yield return new Vector2(Tile.X + 2, Tile.Y);
                        break;
                    default: //1 tile wide
                        yield return Tile;
                        break;
                }
            }
        }
    }
}
