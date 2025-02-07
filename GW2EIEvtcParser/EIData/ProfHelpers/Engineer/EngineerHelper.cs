﻿using System.Collections.Generic;
using System.Linq;
using GW2EIEvtcParser.ParsedData;
using static GW2EIEvtcParser.ArcDPSEnums;
using static GW2EIEvtcParser.EIData.Buff;
using static GW2EIEvtcParser.EIData.DamageModifier;
using static GW2EIEvtcParser.ParserHelper;

namespace GW2EIEvtcParser.EIData
{
    internal static class EngineerHelper
    {
        private class EngineerKitFinder : WeaponSwapCastFinder
        {
            public EngineerKitFinder(long skillID, long icd, ulong minBuild = GW2Builds.StartOfLife, ulong maxBuild = GW2Builds.EndOfLife) : base(skillID, SkillItem.KitSet, icd, minBuild, maxBuild, (swap, combatData, skillData) =>
            {
                SkillItem skill = skillData.Get(skillID);
                if (skill.ApiSkill == null || skill.ApiSkill.BundleSkills == null)
                {
                    return false;
                }
                WeaponSwapEvent nextSwap = combatData.GetWeaponSwapData(swap.Caster).FirstOrDefault(x => x.Time > swap.Time + ServerDelayConstant);
                long nextSwapTime = nextSwap != null ? nextSwap.Time : long.MaxValue;
                var castIds = new HashSet<long>(combatData.GetAnimatedCastData(swap.Caster).Where(x => x.Time >= swap.Time + WeaponSwapDelayConstant && x.Time <= nextSwapTime).Select(x => x.SkillId));
                return skill.ApiSkill.BundleSkills.Intersect(castIds).Any();
            })
            {
                NotAccurate = true;
            }
        }

        private static readonly HashSet<long> _engineerKit = new HashSet<long>
        {
            5812,
            5933,
            5927,
            6020,
            5802,
            5904,
            30800,
        };

        public static bool IsEngineerKit(long id)
        {
            return _engineerKit.Contains(id);
        }


        internal static readonly List<InstantCastFinder> InstantCastFinder = new List<InstantCastFinder>()
        {
            new BuffLossCastFinder(59562, 59579, EIData.InstantCastFinder.DefaultICD, GW2Builds.February2020Balance, GW2Builds.EndOfLife), // Explosive Entrance
            new BuffGainCastFinder(5861, 5863,EIData.InstantCastFinder.DefaultICD), // Elixir S
            new DamageCastFinder(6154,6154,EIData.InstantCastFinder.DefaultICD), // Overcharged Shot
            // Kits
            new EngineerKitFinder(5812, EIData.InstantCastFinder.DefaultICD), // Bomb Kit
            new EngineerKitFinder(5933, EIData.InstantCastFinder.DefaultICD), // Elixir Gun
            new EngineerKitFinder(5927, EIData.InstantCastFinder.DefaultICD), // Flamethrower
            new EngineerKitFinder(6020, EIData.InstantCastFinder.DefaultICD), // Grenade Kit
            new EngineerKitFinder(5802, EIData.InstantCastFinder.DefaultICD), // Med Kit
            new EngineerKitFinder(5904, EIData.InstantCastFinder.DefaultICD), // Tool Kit
            new EngineerKitFinder(30800, EIData.InstantCastFinder.DefaultICD), // Elite Mortar Kit
        };


        internal static readonly List<DamageModifier> DamageMods = new List<DamageModifier>
        {
            // Explosives
            new DamageLogApproximateDamageModifier("Glass Cannon", "5% if self >75%", DamageSource.NoPets, 5.0, DamageType.Strike, DamageType.All, Source.Engineer,"https://wiki.guildwars2.com/images/6/6e/Glass_Cannon.png", (x, log) => (x.From.GetCurrentHealthPercent(log, x.Time) >= 75.0), ByPresence, 72781, 98248, DamageModifierMode.All),
            new DamageLogApproximateDamageModifier("Glass Cannon", "7% if self >75%", DamageSource.NoPets, 7.0, DamageType.Strike, DamageType.All, Source.Engineer,"https://wiki.guildwars2.com/images/6/6e/Glass_Cannon.png", (x, log) => (x.From.GetCurrentHealthPercent(log, x.Time) >= 75.0), ByPresence, 98248, GW2Builds.May2021Balance, DamageModifierMode.All),
            new DamageLogApproximateDamageModifier("Glass Cannon", "10% if self >75%", DamageSource.NoPets, 10.0, DamageType.Strike, DamageType.All, Source.Engineer,"https://wiki.guildwars2.com/images/6/6e/Glass_Cannon.png", (x, log) => (x.From.GetCurrentHealthPercent(log, x.Time) >= 75.0), ByPresence, GW2Builds.May2021Balance, GW2Builds.EndOfLife, DamageModifierMode.All),
            new BuffDamageModifierTarget(738, "Shaped Charge", "10% on vulnerable enemies", DamageSource.NoPets, 10.0, DamageType.Strike, DamageType.All, Source.Engineer, ByPresence, "https://wiki.guildwars2.com/images/f/f3/Explosive_Powder.png", 0, GW2Builds.October2019Balance, DamageModifierMode.PvE),
            new BuffDamageModifierTarget(738, "Shaped Charge", "0.5% per stack vuln", DamageSource.NoPets, 0.5, DamageType.Strike, DamageType.All, Source.Engineer, ByStack, "https://wiki.guildwars2.com/images/f/f3/Explosive_Powder.png", GW2Builds.October2019Balance, GW2Builds.EndOfLife, DamageModifierMode.All),
            new DamageLogApproximateDamageModifier("Big Boomer", "10% if target hp% lower than self hp%", DamageSource.NoPets, 10.0, DamageType.Strike, DamageType.All, Source.Engineer, "https://wiki.guildwars2.com/images/8/83/Big_Boomer.png", (x,log) =>
            {
                var selfHP = x.From.GetCurrentHealthPercent(log, x.Time);
                var dstHP = x.To.GetCurrentHealthPercent(log, x.Time);
                if (selfHP < 0.0 || dstHP < 0.0)
                {
                    return false;
                }
                return selfHP > dstHP;
            }, ByPresence, DamageModifierMode.All ),
            // Firearms
            new BuffDamageModifier(51389, "Thermal Vision", "5% (4s) after burning foe", DamageSource.NoPets, 5.0, DamageType.Condition, DamageType.All, Source.Engineer, ByPresence, "https://wiki.guildwars2.com/images/8/8a/Skilled_Marksman.png", GW2Builds.August2018Balance, GW2Builds.EndOfLife, DamageModifierMode.All),
            new BuffDamageModifier(51389, "Thermal Vision", "10% (4s) after burning foe", DamageSource.NoPets, 10.0, DamageType.Condition, DamageType.All, Source.Engineer, ByPresence, "https://wiki.guildwars2.com/images/8/8a/Skilled_Marksman.png", 0, GW2Builds.August2018Balance, DamageModifierMode.PvE),
            new BuffDamageModifierTarget(NumberOfConditionsID, "Modified Ammunition", "2% per condition on target", DamageSource.NoPets, 2.0, DamageType.Strike, DamageType.All, Source.Engineer, ByStack, "https://wiki.guildwars2.com/images/9/94/Modified_Ammunition.png", DamageModifierMode.All),
            // Tools
            new BuffDamageModifier(726, "Excessive Energy", "10% under vigor", DamageSource.NoPets, 10.0, DamageType.Strike, DamageType.All, Source.Engineer, ByPresence, "https://wiki.guildwars2.com/images/1/1f/Excessive_Energy.png", DamageModifierMode.All),
        };

        internal static readonly List<Buff> Buffs = new List<Buff>
        {
                new Buff("Static Shield",6055, Source.Engineer, BuffClassification.Other, "https://wiki.guildwars2.com/images/9/90/Static_Shield.png"),
                new Buff("Absorb",6056, Source.Engineer, BuffClassification.Other, "https://wiki.guildwars2.com/images/8/8a/Absorb.png"),
                new Buff("A.E.D.",21660, Source.Engineer, BuffClassification.Other, "https://wiki.guildwars2.com/images/e/e6/A.E.D..png"),
                new Buff("Elixir S",5863, Source.Engineer, BuffClassification.Other, "https://wiki.guildwars2.com/images/d/d8/Elixir_S.png"),
                new Buff("Utility Goggles",5864, Source.Engineer, BuffClassification.Other, "https://wiki.guildwars2.com/images/0/02/Utility_Goggles.png"),
                new Buff("Slick Shoes",5833, Source.Engineer, BuffClassification.Other, "https://wiki.guildwars2.com/images/3/3d/Slick_Shoes.png"),
                new Buff("Gear Shield",5997, Source.Engineer, BuffClassification.Other, "https://wiki.guildwars2.com/images/c/ca/Gear_Shield.png"),
                new Buff("Iron Blooded",49065, Source.Engineer, BuffStackType.Stacking, 25, BuffClassification.Other, "https://wiki.guildwars2.com/images/1/1e/Iron_Blooded.png"),
                new Buff("Streamlined Kits",18687, Source.Engineer, BuffClassification.Other, "https://wiki.guildwars2.com/images/c/cb/Streamlined_Kits.png"),
                new Buff("Kinetic Charge",45781, Source.Engineer, BuffStackType.Stacking, 5, BuffClassification.Other, "https://wiki.guildwars2.com/images/e/e0/Kinetic_Battery.png"),
                new Buff("Pinpoint Distribution", 38333, Source.Engineer, BuffClassification.Offensive, "https://wiki.guildwars2.com/images/b/bf/Pinpoint_Distribution.png"),
                new Buff("Thermal Vision", 51389, Source.Engineer, BuffClassification.Other, "https://wiki.guildwars2.com/images/8/8a/Skilled_Marksman.png"),
                new Buff("Explosive Entrance",59579, Source.Engineer, BuffClassification.Other, "https://wiki.guildwars2.com/images/3/33/Explosive_Entrance.png", GW2Builds.February2020Balance, GW2Builds.EndOfLife),
                new Buff("Explosive Temper",59528, Source.Engineer, BuffStackType.Stacking, 10, BuffClassification.Other, "https://wiki.guildwars2.com/images/c/c1/Explosive_Temper.png", GW2Builds.February2020Balance, GW2Builds.EndOfLife),
                new Buff("Big Boomer",59601, Source.Engineer, BuffStackType.Queue, 3, BuffClassification.Other, "https://wiki.guildwars2.com/images/8/83/Big_Boomer.png"),
                new Buff("Med Kit Bonus",50393, Source.Engineer, BuffClassification.Other, "https://wiki.guildwars2.com/images/1/14/Med_Kit.png"),

        };

        public static void AttachMasterToEngineerTurrets(IReadOnlyList<Player> players, CombatData combatData)
        {
            var playerAgents = new HashSet<AgentItem>(players.Select(x => x.AgentItem));

            HashSet<AgentItem> flameTurrets = ProfHelper.GetOffensiveGadgetAgents(combatData, 5903, playerAgents);

            HashSet<AgentItem> rifleTurrets = ProfHelper.GetOffensiveGadgetAgents(combatData, 5841, playerAgents);
            rifleTurrets.UnionWith(ProfHelper.GetOffensiveGadgetAgents(combatData, 5875, playerAgents));

            HashSet<AgentItem> netTurrets = ProfHelper.GetOffensiveGadgetAgents(combatData, 5896, playerAgents);
            netTurrets.UnionWith(ProfHelper.GetOffensiveGadgetAgents(combatData, 22137, playerAgents));

            HashSet<AgentItem> rocketTurrets = ProfHelper.GetOffensiveGadgetAgents(combatData, 6108, playerAgents);
            rocketTurrets.UnionWith(ProfHelper.GetOffensiveGadgetAgents(combatData, 5914, playerAgents));

            HashSet<AgentItem> thumperTurrets = ProfHelper.GetOffensiveGadgetAgents(combatData, 5856, playerAgents);
            thumperTurrets.UnionWith(ProfHelper.GetOffensiveGadgetAgents(combatData, 5890, playerAgents));
            // TODO: need ID here
            HashSet<AgentItem> harpoonTurrets = ProfHelper.GetOffensiveGadgetAgents(combatData, -1, playerAgents);

            HashSet<AgentItem> healingTurrets = ProfHelper.GetOffensiveGadgetAgents(combatData, 5958, playerAgents);
            healingTurrets.RemoveWhere(x => thumperTurrets.Contains(x) || rocketTurrets.Contains(x) || netTurrets.Contains(x) || rifleTurrets.Contains(x) || flameTurrets.Contains(x) || harpoonTurrets.Contains(x));

            var engineers = players.Where(x => x.BaseSpec == Spec.Engineer).ToList();
            // if only one engineer, could only be that one
            if (engineers.Count == 1)
            {
                Player engineer = engineers[0];
                ProfHelper.SetGadgetMaster(flameTurrets, engineer.AgentItem);
                ProfHelper.SetGadgetMaster(netTurrets, engineer.AgentItem);
                ProfHelper.SetGadgetMaster(rocketTurrets, engineer.AgentItem);
                ProfHelper.SetGadgetMaster(rifleTurrets, engineer.AgentItem);
                ProfHelper.SetGadgetMaster(thumperTurrets, engineer.AgentItem);
                ProfHelper.SetGadgetMaster(harpoonTurrets, engineer.AgentItem);
                ProfHelper.SetGadgetMaster(healingTurrets, engineer.AgentItem);
            }
            else if (engineers.Count > 1)
            {
                ProfHelper.AttachMasterToGadgetByCastData(combatData, flameTurrets, new List<long> { 5836, 5868 }, 1000);
                ProfHelper.AttachMasterToGadgetByCastData(combatData, rifleTurrets, new List<long> { 5818 }, 1000);
                ProfHelper.AttachMasterToGadgetByCastData(combatData, netTurrets, new List<long> { 5837, 5868, 6183 }, 1000);
                ProfHelper.AttachMasterToGadgetByCastData(combatData, rocketTurrets, new List<long> { 5912, 22574, 6183 }, 1000);
                ProfHelper.AttachMasterToGadgetByCastData(combatData, thumperTurrets, new List<long> { 5838 }, 1000);
                //AttachMasterToGadgetByCastData(castData, harpoonTurrets, new List<long> { 6093, 6183 }, 1000);
                //AttachMasterToGadgetByCastData(castData, healingTurrets, new List<long> { 5857, 5868 }, 1000);
            }
        }

    }
}
