﻿using System.Collections.Generic;
using static GW2EIEvtcParser.ArcDPSEnums;
using static GW2EIEvtcParser.EIData.Buff;
using static GW2EIEvtcParser.ParserHelper;

namespace GW2EIEvtcParser.EIData
{
    internal static class DruidHelper
    {
        internal static readonly List<InstantCastFinder> InstantCastFinder = new List<InstantCastFinder>()
        {
            new BuffGainCastFinder(31869,31508,EIData.InstantCastFinder.DefaultICD), // Celestial Avatar
            new BuffLossCastFinder(31411,31508,EIData.InstantCastFinder.DefaultICD), // Release Celestial Avatar
            new DamageCastFinder(31658, 31658, EIData.InstantCastFinder.DefaultICD), // Glyph of Equality (non-CA)
        };

        private static readonly HashSet<long> _celestialAvatar = new HashSet<long>
        {
            31869, 31411
        };

        public static bool IsCelestialAvatarTransform(long id)
        {
            return _celestialAvatar.Contains(id);
        }

        internal static readonly List<DamageModifier> DamageMods = new List<DamageModifier>
        {
        };

        internal static readonly List<Buff> Buffs = new List<Buff>
        {
                new Buff("Celestial Avatar", 31508, ParserHelper.Source.Druid, BuffClassification.Other, "https://wiki.guildwars2.com/images/5/59/Celestial_Avatar.png"),
                new Buff("Ancestral Grace", 31584, ParserHelper.Source.Druid, BuffClassification.Other, "https://wiki.guildwars2.com/images/4/4b/Ancestral_Grace.png"),
                new Buff("Glyph of Empowerment", 31803, ParserHelper.Source.Druid, BuffClassification.Offensive, "https://wiki.guildwars2.com/images/d/d7/Glyph_of_the_Stars.png", 0 , GW2Builds.April2019Balance),
                new Buff("Glyph of Unity", 31385, ParserHelper.Source.Druid, BuffClassification.Other, "https://wiki.guildwars2.com/images/b/b1/Glyph_of_Unity.png"),
                new Buff("Glyph of Unity (CA)", 31556, ParserHelper.Source.Druid, BuffClassification.Defensive, "https://wiki.guildwars2.com/images/4/4c/Glyph_of_Unity_%28Celestial_Avatar%29.png"),
                new Buff("Glyph of the Stars", 55048, ParserHelper.Source.Druid, BuffClassification.Defensive, "https://wiki.guildwars2.com/images/d/d7/Glyph_of_the_Stars.png", GW2Builds.April2019Balance, GW2Builds.EndOfLife),
                new Buff("Glyph of the Stars (CA)", 55049, ParserHelper.Source.Druid, BuffClassification.Defensive, "https://wiki.guildwars2.com/images/d/d7/Glyph_of_the_Stars.png", GW2Builds.April2019Balance, GW2Builds.EndOfLife),
                new Buff("Natural Mender",30449, ParserHelper.Source.Druid, BuffStackType.Stacking, 10, BuffClassification.Other, "https://wiki.guildwars2.com/images/e/e9/Natural_Mender.png"),
                new Buff("Lingering Light",32248, ParserHelper.Source.Druid, BuffClassification.Other, "https://wiki.guildwars2.com/images/5/5d/Lingering_Light.png"),
        };

    }
}
