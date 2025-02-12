﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using GW2EIEvtcParser.ParsedData;

namespace GW2EIEvtcParser.EIData.BuffSimulators
{
    internal class QueueLogic : StackingLogic
    {
        protected override void Sort(ParsedEvtcLog log, List<BuffStackItem> stacks)
        {
            // nothign to sort
        }

        public override bool FindLowestValue(ParsedEvtcLog log, BuffStackItem stackItem, List<BuffStackItem> stacks, List<BuffSimulationItemWasted> wastes)
        {
            if (stacks.Count <= 1)
            {
                throw new InvalidDataException("Queue logic based must have a >1 capacity");
            }
            BuffStackItem first = stacks[0];
            BuffStackItem minItem = stacks.Where(x => x != first).MinBy(x => x.TotalDuration);
            wastes.Add(new BuffSimulationItemWasted(minItem.Src, minItem.Duration, minItem.Start));
            if (minItem.Extensions.Any())
            {
                foreach ((AgentItem src, long value) in minItem.Extensions)
                {
                    wastes.Add(new BuffSimulationItemWasted(src, value, minItem.Start));
                }
            }
            stacks[stacks.IndexOf(minItem)] = stackItem;
            Sort(log, stacks);
            return true;
        }
    }
}
