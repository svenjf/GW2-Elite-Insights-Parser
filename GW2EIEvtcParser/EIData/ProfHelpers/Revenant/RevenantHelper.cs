﻿using System.Collections.Generic;
using GW2EIEvtcParser.Extensions;
using static GW2EIEvtcParser.ArcDPSEnums;
using static GW2EIEvtcParser.EIData.Buff;
using static GW2EIEvtcParser.EIData.DamageModifier;
using static GW2EIEvtcParser.ParserHelper;

namespace GW2EIEvtcParser.EIData
{
    internal static class RevenantHelper
    {
        internal static readonly List<InstantCastFinder> InstantCastFinder = new List<InstantCastFinder>()
        {
            new BuffGainCastFinder(28134, 27890, EIData.InstantCastFinder.DefaultICD), // Legendary Assassin Stance
            new BuffGainCastFinder(28494, 27928, EIData.InstantCastFinder.DefaultICD), // Legendary Demon Stance
            new BuffGainCastFinder(28419, 27205, EIData.InstantCastFinder.DefaultICD), // Legendary Dwarf Stance
            new BuffGainCastFinder(28195, 27972, EIData.InstantCastFinder.DefaultICD), // Legendary Centaur Stance
            new BuffGainCastFinder(27107, 27581, 500), // Impossible Odds
            new BuffLossCastFinder(28382, 27581, 500), // Relinquish Power
            new BuffGainCastFinder(26557, 27273, EIData.InstantCastFinder.DefaultICD), // Vengeful Hammers
            new BuffLossCastFinder(26956, 27273, EIData.InstantCastFinder.DefaultICD), // Release Hammers
            new DamageCastFinder(59591, 59591, EIData.InstantCastFinder.DefaultICD, GW2Builds.February2020Balance, GW2Builds.EndOfLife), // Invoking Torment
            new DamageCastFinder(46854, 46854, EIData.InstantCastFinder.DefaultICD), // Call of the Assassin
            new DamageCastFinder(46843, 46843, EIData.InstantCastFinder.DefaultICD), // Call of the Dwarf
            new DamageCastFinder(46856, 46856, EIData.InstantCastFinder.DefaultICD), // Call of the Demon
            new EXTHealingCastFinder(46847, 46847, EIData.InstantCastFinder.DefaultICD), // Call of the Centaur
        };


        internal static readonly List<DamageModifier> DamageMods = new List<DamageModifier>
        {
            // Retribution
            new BuffDamageModifierTarget(742, "Dwarven Battle Training", "10% on weakened target", DamageSource.NoPets, 10.0, DamageType.Strike, DamageType.All, Source.Revenant, ByPresence, "https://wiki.guildwars2.com/images/5/50/Dwarven_Battle_Training.png", GW2Builds.December2018Balance, GW2Builds.EndOfLife, DamageModifierMode.All),
            new BuffDamageModifier(873, "Vicious Reprisal", "10% under retaliation", DamageSource.NoPets, 10.0, DamageType.Strike, DamageType.All, Source.Revenant, ByPresence, "https://wiki.guildwars2.com/images/c/cf/Vicious_Reprisal.png", 0, GW2Builds.May2021Balance, DamageModifierMode.All),
            new BuffDamageModifier(873, "Vicious Reprisal", "10% under resolution", DamageSource.NoPets, 10.0, DamageType.StrikeAndCondition, DamageType.All, Source.Revenant, ByPresence, "https://wiki.guildwars2.com/images/c/cf/Vicious_Reprisal.png", GW2Builds.May2021Balance, GW2Builds.EndOfLife, DamageModifierMode.All),
            // Invocation
            new BuffDamageModifier(725, "Ferocious Aggression", "7% under fury", DamageSource.NoPets, 7.0, DamageType.StrikeAndCondition, DamageType.All, Source.Revenant, ByPresence, "https://wiki.guildwars2.com/images/e/ec/Ferocious_Aggression.png", 0, GW2Builds.May2021Balance, DamageModifierMode.All),
            new BuffDamageModifier(725, "Ferocious Aggression", "7% under fury", DamageSource.NoPets, 7.0, DamageType.StrikeAndConditionAndLifeLeech, DamageType.All, Source.Revenant, ByPresence, "https://wiki.guildwars2.com/images/e/ec/Ferocious_Aggression.png", GW2Builds.May2021Balance, GW2Builds.EndOfLife, DamageModifierMode.All),
            new DamageLogDamageModifier("Rising Tide", "7% if hp >90%", DamageSource.NoPets, 7.0, DamageType.Strike, DamageType.All, Source.Revenant,"https://wiki.guildwars2.com/images/0/0c/Rising_Tide.png", (x, log) => x.IsOverNinety, ByPresence, DamageModifierMode.All),
            // Devastation
            new BuffDamageModifier(29395, "Vicious Lacerations", "3% per Stack", DamageSource.NoPets, 3.0, DamageType.Strike, DamageType.All, Source.Revenant, ByStack, "https://wiki.guildwars2.com/images/c/cd/Vicious_Lacerations.png", GW2Builds.October2018Balance, GW2Builds.February2020Balance, DamageModifierMode.PvE),
            new BuffDamageModifier(29395, "Vicious Lacerations", "2% per Stack", DamageSource.NoPets, 2.0, DamageType.Strike, DamageType.All, Source.Revenant, ByStack, "https://wiki.guildwars2.com/images/c/cd/Vicious_Lacerations.png", 0, GW2Builds.October2018Balance, DamageModifierMode.PvE),
            new DamageLogApproximateDamageModifier("Unsuspecting Strikes", "25% if target hp > 80%", DamageSource.NoPets, 25.0, DamageType.Strike, DamageType.All, Source.Revenant, "https://wiki.guildwars2.com/images/c/cd/Vicious_Lacerations.png", (x,log) =>
            {
                var foeHP = x.To.GetCurrentHealthPercent(log, x.Time);
                if (foeHP < 0.0)
                {
                    return false;
                }
                return foeHP > 80.0;
            }, ByPresence, GW2Builds.February2020Balance, GW2Builds.May2021BalanceHotFix, DamageModifierMode.PvE ),
            new DamageLogApproximateDamageModifier("Unsuspecting Strikes", "20% if target hp > 80%", DamageSource.NoPets, 20.0, DamageType.Strike, DamageType.All, Source.Revenant, "https://wiki.guildwars2.com/images/c/cd/Vicious_Lacerations.png", (x,log) =>
            {
                var foeHP = x.To.GetCurrentHealthPercent(log, x.Time);
                if (foeHP < 0.0)
                {
                    return false;
                }
                return foeHP > 80.0;
            }, ByPresence, GW2Builds.May2021BalanceHotFix, GW2Builds.EndOfLife, DamageModifierMode.PvE ),
            new DamageLogApproximateDamageModifier("Unsuspecting Strikes", "10% if target hp > 80%", DamageSource.NoPets, 10.0, DamageType.Strike, DamageType.All, Source.Revenant, "https://wiki.guildwars2.com/images/c/cd/Vicious_Lacerations.png", (x,log) =>
            {
                var foeHP = x.To.GetCurrentHealthPercent(log, x.Time);
                if (foeHP < 0.0)
                {
                    return false;
                }
                return foeHP > 80.0;
            }, ByPresence, GW2Builds.February2020Balance, GW2Builds.EndOfLife, DamageModifierMode.sPvPWvW ),
            new BuffDamageModifierTarget(738, "Targeted Destruction", "0.5% per stack vuln", DamageSource.NoPets, 0.5, DamageType.Strike, DamageType.All, Source.Revenant, ByStack, "https://wiki.guildwars2.com/images/e/ed/Targeted_Destruction.png", GW2Builds.March2019Balance, GW2Builds.EndOfLife, DamageModifierMode.All),
            new BuffDamageModifierTarget(738, "Targeted Destruction", "10.0% if vuln", DamageSource.NoPets, 10.0, DamageType.Strike, DamageType.All, Source.Revenant, ByPresence, "https://wiki.guildwars2.com/images/e/ed/Targeted_Destruction.png", GW2Builds.October2018Balance, GW2Builds.March2019Balance, DamageModifierMode.PvE),
            new BuffDamageModifierTarget(738, "Targeted Destruction", "7.0% if vuln", DamageSource.NoPets, 7.0, DamageType.Strike, DamageType.All, Source.Revenant, ByPresence, "https://wiki.guildwars2.com/images/e/ed/Targeted_Destruction.png", 0, GW2Builds.October2018Balance, DamageModifierMode.PvE),
            new DamageLogDamageModifier("Swift Termination", "20% if target <50%", DamageSource.NoPets, 20.0, DamageType.Strike, DamageType.All, Source.Revenant,"https://wiki.guildwars2.com/images/b/bb/Swift_Termination.png", (x, log) => x.AgainstUnderFifty, ByPresence, DamageModifierMode.All),
        };

        internal static readonly List<Buff> Buffs = new List<Buff>
        {
                new Buff("Vengeful Hammers", 27273, Source.Revenant, BuffClassification.Other,"https://wiki.guildwars2.com/images/c/c8/Vengeful_Hammers.png"),
                new Buff("Rite of the Great Dwarf", 26596, Source.Revenant, BuffClassification.Defensive, "https://wiki.guildwars2.com/images/6/69/Rite_of_the_Great_Dwarf.png"),
                new Buff("Rite of the Great Dwarf (Traited)", 33330, Source.Revenant, BuffClassification.Defensive, "https://wiki.guildwars2.com/images/6/69/Rite_of_the_Great_Dwarf.png"),
                new Buff("Embrace the Darkness", 28001, Source.Revenant, BuffClassification.Other, "https://wiki.guildwars2.com/images/5/51/Embrace_the_Darkness.png"),
                new Buff("Enchanted Daggers", 28557, Source.Revenant, BuffStackType.Stacking, 25, BuffClassification.Other, "https://wiki.guildwars2.com/images/f/fa/Enchanted_Daggers.png"),
                new Buff("Phase Traversal", 28395, Source.Revenant, BuffStackType.Stacking, 25, BuffClassification.Other, "https://wiki.guildwars2.com/images/f/f2/Phase_Traversal.png"),
                new Buff("Impossible Odds", 27581, Source.Revenant, BuffClassification.Other, "https://wiki.guildwars2.com/images/8/87/Impossible_Odds.png"),
                new Buff("Legendary Centaur Stance",27972, Source.Revenant, BuffClassification.Other, "https://wiki.guildwars2.com/images/8/8a/Legendary_Centaur_Stance.png"),
                new Buff("Legendary Dwarf Stance",27205, Source.Revenant, BuffClassification.Other, "https://wiki.guildwars2.com/images/b/b2/Legendary_Dwarf_Stance.png"),
                new Buff("Legendary Demon Stance",27928, Source.Revenant, BuffClassification.Other, "https://wiki.guildwars2.com/images/d/d1/Legendary_Demon_Stance.png"),
                new Buff("Legendary Assassin Stance",27890, Source.Revenant, BuffClassification.Other, "https://wiki.guildwars2.com/images/0/02/Legendary_Assassin_Stance.png"),
                //traits
                new Buff("Vicious Lacerations",29395, Source.Revenant, BuffStackType.Stacking, 3, BuffClassification.Other, "https://wiki.guildwars2.com/images/c/cd/Vicious_Lacerations.png", 0, GW2Builds.February2020Balance),
                new Buff("Assassin's Presence", 26854, Source.Revenant, BuffClassification.Offensive, "https://wiki.guildwars2.com/images/5/54/Assassin%27s_Presence.png"),
                new Buff("Expose Defenses", 48894, Source.Revenant, BuffClassification.Other, "https://wiki.guildwars2.com/images/5/5c/Mutilate_Defenses.png"),
                new Buff("Invoking Harmony",29025, Source.Revenant, BuffClassification.Other, "https://wiki.guildwars2.com/images/e/ec/Invoking_Harmony.png"),
                new Buff("Unyielding Devotion",55044, Source.Revenant, BuffClassification.Other, "https://wiki.guildwars2.com/images/4/4f/Unyielding_Devotion.png", GW2Builds.April2019Balance, GW2Builds.EndOfLife),
                new Buff("Selfless Amplification",30509, Source.Revenant, BuffClassification.Other, "https://wiki.guildwars2.com/images/2/23/Selfless_Amplification.png"),
                new Buff("Battle Scars", 26646, Source.Revenant, BuffStackType.StackingConditionalLoss, 25, BuffClassification.Other, "https://wiki.guildwars2.com/images/3/30/Thrill_of_Combat.png", GW2Builds.February2020Balance, GW2Builds.EndOfLife),
                new Buff("Steadfast Rejuvenation",53500, Source.Revenant, BuffStackType.Stacking, 10, BuffClassification.Other, "https://wiki.guildwars2.com/images/b/bf/Steadfast_Rejuvenation.png"),
        };

        private static readonly HashSet<long> _legendSwaps = new HashSet<long>
        {
            28134, // Assassin
            28494, // Demon
            28419, // Dwarf
            28195, // Centaur
            28085, // Dragon
            41858, // Renegade
            62749, // Alliance
            62891, // Alliance (UW)
        };

        public static bool IsLegendSwap(long id)
        {
            return _legendSwaps.Contains(id);
        }
    }
}
