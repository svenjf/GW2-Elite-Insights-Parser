﻿using System.Collections.Generic;
using System.Linq;
using GW2EIEvtcParser.ParsedData;

namespace GW2EIEvtcParser.EIData.BuffSimulators
{
    internal class ForceOverrideLogic : StackingLogic
    {

        protected override void Sort(ParsedEvtcLog log, List<BuffStackItem> stacks)
        {
            // no sort
        }

        public override bool FindLowestValue(ParsedEvtcLog log, BuffStackItem stackItem, List<BuffStackItem> stacks, List<BuffSimulationItemWasted> wastes)
        {
            if (!stacks.Any())
            {
                return false;
            }
            BuffStackItem stack = stacks[0];
            wastes.Add(new BuffSimulationItemWasted(stack.Src, stack.Duration, stack.Start));
            if (stack.Extensions.Count > 0)
            {
                foreach ((AgentItem src, long value) in stack.Extensions)
                {
                    wastes.Add(new BuffSimulationItemWasted(src, value, stack.Start));
                }
            }
            stacks[0] = stackItem;
            return true;
        }

        public override bool IsFull(List<BuffStackItem> stacks, int capacity)
        {
            return stacks.Count == 1;
        }
    }
}
