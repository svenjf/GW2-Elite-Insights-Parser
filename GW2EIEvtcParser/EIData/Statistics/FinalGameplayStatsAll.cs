﻿using System;
using System.Collections.Generic;
using System.Linq;
using GW2EIEvtcParser.ParsedData;
using static GW2EIEvtcParser.EIData.Buff;

namespace GW2EIEvtcParser.EIData
{
    public class FinalGameplayStatsAll : FinalGameplayStats
    {
        // Rates
        public int Wasted { get; }
        public double TimeWasted { get; }
        public int Saved { get; }
        public double TimeSaved { get; }
        public double StackDist { get; }
        public double DistToCom { get; }

        // boons
        public double AvgBoons { get; }
        public double AvgActiveBoons { get; }
        public double AvgConditions { get; }
        public double AvgActiveConditions { get; }

        // Counts
        public int SwapCount { get; }

        private static double GetDistanceToTarget(AbstractSingleActor actor, ParsedEvtcLog log, long start, long end, IReadOnlyList<Point3D> reference)
        {
            var positions = actor.GetCombatReplayPolledPositions(log).Where(x => x.Time >= start && x.Time <= end).ToList();
            int offset = actor.GetCombatReplayPolledPositions(log).Count(x => x.Time < start);
            if (positions.Count > 1 && reference.Count > 0)
            {
                var distances = new List<float>();
                for (int time = 0; time < positions.Count; time++)
                {

                    float deltaX = positions[time].X - reference[time + offset].X;
                    float deltaY = positions[time].Y - reference[time + offset].Y;
                    //float deltaZ = positions[time].Z - StackCenterPositions[time].Z;


                    distances.Add((float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY));
                }
                return distances.Sum() / distances.Count;
            }
            else
            {
                return -1;
            }
        }

        internal FinalGameplayStatsAll(ParsedEvtcLog log, long start, long end, AbstractSingleActor actor) : base(log, start, end, actor, null)
        {
            // If dummy actor, stop
            if (actor.IsFakeActor)
            {
                return;
            }
            long duration = end - start;
            foreach (AbstractCastEvent cl in actor.GetCastEvents(log, start, end))
            {
                switch (cl.Status)
                {
                    case AbstractCastEvent.AnimationStatus.Interrupted:
                        Wasted++;
                        TimeWasted += cl.SavedDuration;
                        break;
                    case AbstractCastEvent.AnimationStatus.Reduced:
                        Saved++;
                        TimeSaved += cl.SavedDuration;
                        break;
                }
                if (cl.Skill.IsSwap)
                {
                    SwapCount++;
                }
            }
            TimeSaved = Math.Round(TimeSaved / 1000.0, ParserHelper.TimeDigit);
            TimeWasted = -Math.Round(TimeWasted / 1000.0, ParserHelper.TimeDigit);

            double avgBoons = 0;
            foreach (long boonDuration in actor.GetBuffPresence(log, start, end).Where(x => log.Buffs.BuffsByIds[x.Key].Classification == BuffClassification.Boon).Select(x => x.Value))
            {
                avgBoons += boonDuration;
            }
            AvgBoons = Math.Round(avgBoons / duration, ParserHelper.BuffDigit);
            long activeDuration = actor.GetActiveDuration(log, start, end);
            AvgActiveBoons = activeDuration > 0 ? Math.Round(avgBoons / activeDuration, ParserHelper.BuffDigit) : 0.0;

            double avgCondis = 0;
            foreach (long conditionDuration in actor.GetBuffPresence(log, start, end).Where(x => log.Buffs.BuffsByIds[x.Key].Classification == BuffClassification.Condition).Select(x => x.Value))
            {
                avgCondis += conditionDuration;
            }
            AvgConditions = Math.Round(avgCondis / duration, ParserHelper.BuffDigit);
            AvgActiveConditions = activeDuration > 0 ? Math.Round(avgCondis / activeDuration, ParserHelper.BuffDigit) : 0.0;

            if (log.CombatData.HasMovementData && log.FriendlyAgents.Contains(actor.AgentItem) && actor.GetCombatReplayPolledPositions(log).Any(x => x.X > int.MinValue + 1))
            {
                StackDist = GetDistanceToTarget(actor, log, start, end, log.StatisticsHelper.GetStackCenterPositions(log));
                DistToCom = GetDistanceToTarget(actor, log, start, end, log.StatisticsHelper.GetStackCommanderPositions(log));
            }
        }
    }
}
