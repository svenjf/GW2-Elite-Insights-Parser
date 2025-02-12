﻿using System.Collections.Generic;
using static GW2EIEvtcParser.ArcDPSEnums;
using static GW2EIEvtcParser.EIData.Buff;
using static GW2EIEvtcParser.EIData.DamageModifier;
using static GW2EIEvtcParser.ParserHelper;

namespace GW2EIEvtcParser.EIData
{
    internal static class RenegadeHelper
    {

        internal static readonly List<InstantCastFinder> InstantCastFinder = new List<InstantCastFinder>()
        {
            new BuffGainCastFinder(41858, 44272, EIData.InstantCastFinder.DefaultICD), // Legendary Renegade Stance
            new DamageCastFinder(46849, 46849, EIData.InstantCastFinder.DefaultICD), // Call of the Renegade
        };

        internal static readonly List<DamageModifier> DamageMods = new List<DamageModifier>
        {
            new BuffDamageModifier(42883, "Kalla's Fervor", "2% per stack", DamageSource.NoPets, 2.0, DamageType.Condition, DamageType.All, Source.Renegade, ByStack, "https://wiki.guildwars2.com/images/9/9e/Kalla%27s_Fervor.png", 0, GW2Builds.May2021Balance, DamageModifierMode.All),
            new BuffDamageModifier(45614, "Improved Kalla's Fervor", "3% per stack", DamageSource.NoPets, 3.0, DamageType.Condition, DamageType.All, Source.Renegade, ByStack, "https://wiki.guildwars2.com/images/9/9e/Kalla%27s_Fervor.png", 0, GW2Builds.May2021Balance, DamageModifierMode.All),
            new BuffDamageModifier(42883, "Kalla's Fervor", "2% per stack", DamageSource.NoPets, 2.0, DamageType.StrikeAndConditionAndLifeLeech, DamageType.All, Source.Renegade, ByStack, "https://wiki.guildwars2.com/images/9/9e/Kalla%27s_Fervor.png", GW2Builds.May2021Balance, GW2Builds.EndOfLife, DamageModifierMode.PvE),
            new BuffDamageModifier(45614, "Improved Kalla's Fervor", "3% per stack", DamageSource.NoPets, 3.0, DamageType.StrikeAndConditionAndLifeLeech, DamageType.All, Source.Renegade, ByStack, "https://wiki.guildwars2.com/images/9/9e/Kalla%27s_Fervor.png", GW2Builds.May2021Balance, GW2Builds.EndOfLife, DamageModifierMode.PvE),
        };

        internal static readonly List<Buff> Buffs = new List<Buff>
        {
                new Buff("Legendary Renegade Stance",44272, Source.Renegade, BuffClassification.Other, "https://wiki.guildwars2.com/images/1/19/Legendary_Renegade_Stance.png"),
                new Buff("Breakrazor's Bastion",44682, Source.Renegade, BuffClassification.Defensive, "https://wiki.guildwars2.com/images/a/a7/Breakrazor%27s_Bastion.png"),
                new Buff("Razorclaw's Rage",41016, Source.Renegade, BuffClassification.Offensive, "https://wiki.guildwars2.com/images/7/73/Razorclaw%27s_Rage.png"),
                new Buff("Soulcleave's Summit",45026, Source.Renegade, BuffClassification.Offensive, "https://wiki.guildwars2.com/images/7/78/Soulcleave%27s_Summit.png"),
                new Buff("Kalla's Fervor",42883, Source.Renegade, BuffStackType.Stacking, 5, BuffClassification.Other, "https://wiki.guildwars2.com/images/9/9e/Kalla%27s_Fervor.png"),
                new Buff("Improved Kalla's Fervor",45614, Source.Renegade, BuffStackType.Stacking, 5, BuffClassification.Other, "https://wiki.guildwars2.com/images/9/9e/Kalla%27s_Fervor.png"),
        };
    }
}
