﻿using System.Collections.Generic;
using System.Linq;
using GW2EIEvtcParser.ParsedData;
using static GW2EIEvtcParser.ArcDPSEnums;
using static GW2EIEvtcParser.EIData.Buff;
using static GW2EIEvtcParser.EIData.DamageModifier;
using static GW2EIEvtcParser.ParserHelper;

namespace GW2EIEvtcParser.EIData
{
    internal static class WarriorHelper
    {

        internal static readonly List<InstantCastFinder> InstantCastFinder = new List<InstantCastFinder>()
        {
            new DamageCastFinder(14268, 14268, EIData.InstantCastFinder.DefaultICD, 84794, GW2Builds.EndOfLife), // Reckless Impact
            new BuffGainCastFinder(14406, 14453, EIData.InstantCastFinder.DefaultICD), // Berserker Stance
            new BuffGainCastFinder(14412, 34778, EIData.InstantCastFinder.DefaultICD), // Balanced Stance
            new BuffGainCastFinder(14392, 787, EIData.InstantCastFinder.DefaultICD), // Endure Pain
        };

        private static HashSet<AgentItem> GetBannerAgents(CombatData combatData, long id, HashSet<AgentItem> playerAgents)
        {
            return new HashSet<AgentItem>(combatData.GetBuffData(id).Where(x => x is BuffApplyEvent && x.CreditedBy.Type == AgentItem.AgentType.Gadget && x.CreditedBy.Master == null && playerAgents.Contains(x.To.GetFinalMaster())).Select(x => x.CreditedBy));
        }


        internal static readonly List<DamageModifier> DamageMods = new List<DamageModifier>
        {
            // Strength
            new BuffDamageModifier(46853, "Peak Performance (absent)", "5%", DamageSource.NoPets, 5.0, DamageType.Strike, DamageType.All, Source.Warrior, ByAbsence, "https://wiki.guildwars2.com/images/9/98/Peak_Performance.png", GW2Builds.July2018Balance, GW2Builds.EndOfLife, DamageModifierMode.PvE),
            new BuffDamageModifier(46853, "Peak Performance (present)", "20%", DamageSource.NoPets, 20.0, DamageType.Strike, DamageType.All, Source.Warrior, ByPresence, "https://wiki.guildwars2.com/images/9/98/Peak_Performance.png", GW2Builds.July2018Balance, GW2Builds.May2021Balance, DamageModifierMode.PvE),
            new BuffDamageModifier(46853, "Peak Performance (present)", "15%", DamageSource.NoPets, 15.0, DamageType.Strike, DamageType.All, Source.Warrior, ByPresence, "https://wiki.guildwars2.com/images/9/98/Peak_Performance.png", GW2Builds.May2021Balance, GW2Builds.EndOfLife, DamageModifierMode.PvE),
            new BuffDamageModifier(46853, "Peak Performance (absent)", "3%", DamageSource.NoPets, 3.0, DamageType.Strike, DamageType.All, Source.Warrior, ByAbsence, "https://wiki.guildwars2.com/images/9/98/Peak_Performance.png", GW2Builds.July2018Balance, GW2Builds.EndOfLife, DamageModifierMode.sPvPWvW),
            new BuffDamageModifier(46853, "Peak Performance (present)", "10%", DamageSource.NoPets, 10.0, DamageType.Strike, DamageType.All, Source.Warrior, ByPresence, "https://wiki.guildwars2.com/images/9/98/Peak_Performance.png", GW2Builds.July2018Balance, GW2Builds.EndOfLife, DamageModifierMode.sPvPWvW),
            new BuffDamageModifier(46853, "Peak Performance", "33%", DamageSource.NoPets, 33.0, DamageType.Strike, DamageType.All, Source.Warrior, ByPresence, "https://wiki.guildwars2.com/images/9/98/Peak_Performance.png", 0, GW2Builds.July2018Balance, DamageModifierMode.PvE),
            new BuffDamageModifier(42539, "Berserker's Power", "7% per stack", DamageSource.NoPets, 7.0, DamageType.Strike, DamageType.All, Source.Warrior, ByStack, "https://wiki.guildwars2.com/images/6/6f/Berserker%27s_Power.png", DamageModifierMode.All),
            // Can merciless hammer conditions be tracked reliably?
            // Defense
            new BuffDamageModifierTarget(742, "Cull the Weak", "7% on weakened target", DamageSource.NoPets, 7.0, DamageType.Strike, DamageType.All, Source.Warrior, ByPresence, "https://wiki.guildwars2.com/images/7/72/Cull_the_Weak.png", 0, GW2Builds.May2021Balance, DamageModifierMode.All),
            new BuffDamageModifierTarget(742, "Cull the Weak", "7% on weakened target", DamageSource.NoPets, 7.0, DamageType.Strike, DamageType.All, Source.Warrior, ByPresence, "https://wiki.guildwars2.com/images/7/72/Cull_the_Weak.png", GW2Builds.May2021Balance, GW2Builds.EndOfLife, DamageModifierMode.sPvPWvW),
            new BuffDamageModifierTarget(742, "Cull the Weak", "10% on weakened target", DamageSource.NoPets, 10.0, DamageType.Strike, DamageType.All, Source.Warrior, ByPresence, "https://wiki.guildwars2.com/images/7/72/Cull_the_Weak.png", GW2Builds.May2021Balance, GW2Builds.EndOfLife, DamageModifierMode.PvE),
            // Tactics
            new BuffDamageModifierTarget(new long[] {721, 727, 722}, "Leg Specialist", "7% to movement-impaired foes", DamageSource.NoPets, 7.0, DamageType.Strike, DamageType.All, Source.Warrior, ByPresence, "https://wiki.guildwars2.com/images/9/9e/Leg_Specialist.png", GW2Builds.October2019Balance, GW2Builds.May2021Balance, DamageModifierMode.All),
            new BuffDamageModifierTarget(new long[] {721, 727, 722}, "Leg Specialist", "7% to movement-impaired foes", DamageSource.NoPets, 7.0, DamageType.Strike, DamageType.All, Source.Warrior, ByPresence, "https://wiki.guildwars2.com/images/9/9e/Leg_Specialist.png", GW2Builds.May2021Balance, GW2Builds.EndOfLife, DamageModifierMode.sPvPWvW),
            new BuffDamageModifierTarget(new long[] {721, 727, 722}, "Leg Specialist", "10% to movement-impaired foes", DamageSource.NoPets, 10.0, DamageType.Strike, DamageType.All, Source.Warrior, ByPresence, "https://wiki.guildwars2.com/images/9/9e/Leg_Specialist.png", GW2Builds.May2021Balance, GW2Builds.EndOfLife, DamageModifierMode.PvE),
            new BuffDamageModifier(NumberOfBoonsID, "Empowered", "1% per boon", DamageSource.NoPets, 1.0, DamageType.Strike, DamageType.All, Source.Warrior, ByStack, "https://wiki.guildwars2.com/images/c/c2/Empowered.png", DamageModifierMode.All),
            new DamageLogApproximateDamageModifier("Warrior's Cunning (Barrier)", "50% against barrier", DamageSource.NoPets, 50.0, DamageType.Strike, DamageType.All, Source.Warrior,"https://wiki.guildwars2.com/images/9/96/Warrior%27s_Cunning.png", (x, log) => x.To.GetCurrentBarrierPercent(log, x.Time) > 0.0 , ByPresence, 100690, GW2Builds.EndOfLife, DamageModifierMode.PvE),
            new DamageLogApproximateDamageModifier("Warrior's Cunning (High HP, no Barrier)", "25% if foe >90%", DamageSource.NoPets, 25.0, DamageType.Strike, DamageType.All, Source.Warrior,"https://wiki.guildwars2.com/images/9/96/Warrior%27s_Cunning.png", (x, log) => x.To.GetCurrentBarrierPercent(log, x.Time) == 0.0 && x.To.GetCurrentHealthPercent(log, x.Time) >= 90.0 , ByPresence, 100690, GW2Builds.May2021Balance, DamageModifierMode.PvE),
            new DamageLogApproximateDamageModifier("Warrior's Cunning (High HP, no Barrier)", "25% if foe >80%", DamageSource.NoPets, 25.0, DamageType.Strike, DamageType.All, Source.Warrior,"https://wiki.guildwars2.com/images/9/96/Warrior%27s_Cunning.png", (x, log) => x.To.GetCurrentBarrierPercent(log, x.Time) == 0.0 && x.To.GetCurrentHealthPercent(log, x.Time) >= 80.0 , ByPresence, GW2Builds.May2021Balance, GW2Builds.EndOfLife, DamageModifierMode.PvE),
            new DamageLogApproximateDamageModifier("Warrior's Cunning (Barrier)", "10% against barrier", DamageSource.NoPets, 10.0, DamageType.Strike, DamageType.All, Source.Warrior,"https://wiki.guildwars2.com/images/9/96/Warrior%27s_Cunning.png", (x, log) => x.To.GetCurrentBarrierPercent(log, x.Time) > 0.0 , ByPresence, 100690, GW2Builds.EndOfLife, DamageModifierMode.sPvPWvW),
            new DamageLogApproximateDamageModifier("Warrior's Cunning (High HP, no Barrier)", "7% if foe >90%", DamageSource.NoPets, 7.0, DamageType.Strike, DamageType.All, Source.Warrior,"https://wiki.guildwars2.com/images/9/96/Warrior%27s_Cunning.png", (x, log) =>x.To.GetCurrentBarrierPercent(log, x.Time) == 0.0 && x.To.GetCurrentHealthPercent(log, x.Time) >= 90.0 , ByPresence, 100690, GW2Builds.May2021Balance, DamageModifierMode.sPvPWvW),
            new DamageLogApproximateDamageModifier("Warrior's Cunning (High HP, no Barrier)", "7% if foe >80%", DamageSource.NoPets, 7.0, DamageType.Strike, DamageType.All, Source.Warrior,"https://wiki.guildwars2.com/images/9/96/Warrior%27s_Cunning.png", (x, log) =>x.To.GetCurrentBarrierPercent(log, x.Time) == 0.0 && x.To.GetCurrentHealthPercent(log, x.Time) >= 80.0 , ByPresence, GW2Builds.May2021Balance, GW2Builds.EndOfLife, DamageModifierMode.sPvPWvW),
            // Discipline
            new BuffDamageModifier(719, "Warrior's Sprint", "7% under swiftness", DamageSource.NoPets, 7.0, DamageType.Strike, DamageType.All, Source.Warrior, ByPresence, "https://wiki.guildwars2.com/images/e/e3/Warrior%27s_Sprint.png", 86181 , GW2Builds.May2021Balance, DamageModifierMode.PvE),
            new BuffDamageModifier(719, "Warrior's Sprint", "3% under swiftness", DamageSource.NoPets, 3.0, DamageType.Strike, DamageType.All, Source.Warrior, ByPresence, "https://wiki.guildwars2.com/images/e/e3/Warrior%27s_Sprint.png", 86181 , GW2Builds.EndOfLife, DamageModifierMode.sPvPWvW),
            new BuffDamageModifier(719, "Warrior's Sprint", "10% under swiftness", DamageSource.NoPets, 10.0, DamageType.Strike, DamageType.All, Source.Warrior, ByPresence, "https://wiki.guildwars2.com/images/e/e3/Warrior%27s_Sprint.png", GW2Builds.May2021Balance, GW2Builds.EndOfLife, DamageModifierMode.PvE),
            new BuffDamageModifierTarget(NumberOfBoonsID, "Destruction of the Empowered", "3% per target boon", DamageSource.NoPets, 3.0, DamageType.Strike, DamageType.All, Source.Warrior, ByMultipliyingStack, "https://wiki.guildwars2.com/images/5/5c/Destruction_of_the_Empowered.png", DamageModifierMode.All),

        };

        internal static readonly List<Buff> Buffs = new List<Buff>
        {
            //skills
                new Buff("Riposte",14434, Source.Warrior, BuffClassification.Other,"https://wiki.guildwars2.com/images/d/de/Riposte.png"),
                //signets
                new Buff("Healing Signet",786, Source.Warrior, BuffClassification.Other,"https://wiki.guildwars2.com/images/8/85/Healing_Signet.png"),
                new Buff("Dolyak Signet",14458, Source.Warrior, BuffClassification.Other, "https://wiki.guildwars2.com/images/6/60/Dolyak_Signet.png"),
                new Buff("Signet of Fury",14459, Source.Warrior, BuffClassification.Other, "https://wiki.guildwars2.com/images/c/c1/Signet_of_Fury.png"),
                new Buff("Signet of Might",14444, Source.Warrior, BuffClassification.Other, "https://wiki.guildwars2.com/images/4/40/Signet_of_Might.png"),
                new Buff("Signet of Stamina",14478, Source.Warrior, BuffClassification.Other, "https://wiki.guildwars2.com/images/6/6b/Signet_of_Stamina.png"),
                new Buff("Signet of Rage",14496, Source.Warrior, BuffClassification.Other, "https://wiki.guildwars2.com/images/b/bc/Signet_of_Rage.png"),
                //banners
                new Buff("Banner of Strength", 14417, Source.Warrior, BuffClassification.Offensive, "https://wiki.guildwars2.com/images/thumb/e/e1/Banner_of_Strength.png/33px-Banner_of_Strength.png"),
                new Buff("Banner of Discipline", 14449, Source.Warrior, BuffClassification.Offensive, "https://wiki.guildwars2.com/images/thumb/5/5f/Banner_of_Discipline.png/33px-Banner_of_Discipline.png"),
                new Buff("Banner of Tactics",14450, Source.Warrior, BuffClassification.Support, "https://wiki.guildwars2.com/images/thumb/3/3f/Banner_of_Tactics.png/33px-Banner_of_Tactics.png"),
                new Buff("Banner of Defense",14543, Source.Warrior, BuffClassification.Defensive, "https://wiki.guildwars2.com/images/thumb/f/f1/Banner_of_Defense.png/33px-Banner_of_Defense.png"),
                //stances
                new Buff("Shield Stance",756, Source.Warrior, BuffClassification.Other,"https://wiki.guildwars2.com/images/d/de/Shield_Stance.png"),
                new Buff("Berserker's Stance",14453, Source.Warrior, BuffClassification.Other,"https://wiki.guildwars2.com/images/8/8a/Berserker_Stance.png"),
                new Buff("Enduring Pain",787, Source.Warrior, BuffStackType.Queue, 25, BuffClassification.Other, "https://wiki.guildwars2.com/images/2/24/Endure_Pain.png"),
                new Buff("Balanced Stance",34778, Source.Warrior, BuffClassification.Other, "https://wiki.guildwars2.com/images/2/27/Balanced_Stance.png"),
                new Buff("Defiant Stance",21816, Source.Warrior, BuffClassification.Other, "https://wiki.guildwars2.com/images/d/db/Defiant_Stance.png"),
                new Buff("Rampage",14484, Source.Warrior, BuffClassification.Other, "https://wiki.guildwars2.com/images/e/e4/Rampage.png"),
                //traits
                new Buff("Soldier's Focus", 58102, Source.Warrior, BuffClassification.Other, "https://wiki.guildwars2.com/images/2/29/Soldier%27s_Focus.png", GW2Builds.October2019Balance, GW2Builds.EndOfLife),
                new Buff("Brave Stride", 43063, Source.Warrior, BuffClassification.Other, "https://wiki.guildwars2.com/images/b/b8/Death_from_Above.png"),
                new Buff("Empower Allies", 14222, Source.Warrior, BuffClassification.Offensive, "https://wiki.guildwars2.com/images/thumb/4/4c/Empower_Allies.png/20px-Empower_Allies.png"),
                new Buff("Peak Performance",46853, Source.Warrior, BuffClassification.Other, "https://wiki.guildwars2.com/images/9/98/Peak_Performance.png"),
                new Buff("Furious Surge", 30204, Source.Warrior, BuffStackType.Stacking, 25, BuffClassification.Other, "https://wiki.guildwars2.com/images/6/65/Furious.png"),
                //new Boon("Health Gain per Adrenaline bar Spent",-1, BoonSource.Warrior, BoonType.Intensity, 3, BoonEnum.GraphOnlyBuff,RemoveType.Normal),
                new Buff("Rousing Resilience",24383, Source.Warrior, BuffClassification.Other, "https://wiki.guildwars2.com/images/c/ca/Rousing_Resilience.png"),
                new Buff("Berserker's Power",42539, Source.Warrior, BuffStackType.Stacking, 3, BuffClassification.Other, "https://wiki.guildwars2.com/images/6/6f/Berserker%27s_Power.png"),
                new Buff("Signet of Ferocity",41922, Source.Warrior, BuffStackType.Stacking, 5, BuffClassification.Other, "https://wiki.guildwars2.com/images/e/ef/Signet_Mastery.png"),
                new Buff("Adrenal Health",34766, Source.Warrior, BuffStackType.Stacking, 3, BuffClassification.Other, "https://wiki.guildwars2.com/images/2/24/Adrenal_Health.png"),
        };


        /*private static HashSet<AgentItem> FindBattleStandards(Dictionary<long, List<AbstractBuffEvent>> buffData, HashSet<AgentItem> playerAgents)
        {
            if (buffData.TryGetValue(725, out List<AbstractBuffEvent> list))
            {
                var battleBannerCandidates = new HashSet<AgentItem>(list.Where(x => x is BuffApplyEvent && x.By.Type == AgentItem.AgentType.Gadget && (playerAgents.Contains(x.To) || playerAgents.Contains(x.To.Master))).Select(x => x.By));
                if (battleBannerCandidates.Count > 0)
                {
                    if (buffData.TryGetValue(740, out list))
                    {
                        battleBannerCandidates.IntersectWith(new HashSet<AgentItem>(list.Where(x => x is BuffApplyEvent && x.By.Type == AgentItem.AgentType.Gadget && (playerAgents.Contains(x.To) || playerAgents.Contains(x.To.Master))).Select(x => x.By)));
                        if (battleBannerCandidates.Count > 0)
                        {
                            if (buffData.TryGetValue(719, out list))
                            {
                                battleBannerCandidates.IntersectWith(new HashSet<AgentItem>(list.Where(x => x is BuffApplyEvent && x.By.Type == AgentItem.AgentType.Gadget && (playerAgents.Contains(x.To) || playerAgents.Contains(x.To.Master))).Select(x => x.By)));
                                return battleBannerCandidates;
                            }
                        }
                    }
                }
            }
            return new HashSet<AgentItem>();
        }*/

        public static void AttachMasterToWarriorBanners(IReadOnlyList<Player> players, CombatData combatData)
        {
            var playerAgents = new HashSet<AgentItem>(players.Select(x => x.AgentItem));
            HashSet<AgentItem> strBanners = GetBannerAgents(combatData, 14417, playerAgents),
                defBanners = GetBannerAgents(combatData, 14543, playerAgents),
                disBanners = GetBannerAgents(combatData, 14449, playerAgents),
                tacBanners = GetBannerAgents(combatData, 14450, playerAgents);
            //battleBanner = FindBattleStandards(buffData, playerAgents);
            var warriors = players.Where(x => x.BaseSpec == Spec.Warrior).ToList();
            // if only one warrior, could only be that one
            if (warriors.Count == 1)
            {
                Player warrior = warriors[0];
                ProfHelper.SetGadgetMaster(strBanners, warrior.AgentItem);
                ProfHelper.SetGadgetMaster(disBanners, warrior.AgentItem);
                ProfHelper.SetGadgetMaster(tacBanners, warrior.AgentItem);
                ProfHelper.SetGadgetMaster(defBanners, warrior.AgentItem);
                //SetBannerMaster(battleBanner, warrior.AgentItem);
            }
            else if (warriors.Count > 1)
            {
                // land and under water cast ids
                ProfHelper.AttachMasterToGadgetByCastData(combatData, strBanners, new List<long> { 14405, 14572 }, 1000);
                ProfHelper.AttachMasterToGadgetByCastData(combatData, defBanners, new List<long> { 14528, 14570 }, 1000);
                ProfHelper.AttachMasterToGadgetByCastData(combatData, disBanners, new List<long> { 14407, 14571 }, 1000);
                ProfHelper.AttachMasterToGadgetByCastData(combatData, tacBanners, new List<long> { 14408, 14573 }, 1000);
                //AttachMasterToBanner(castData, battleBanner, 14419, 14569);
            }
        }

    }
}
