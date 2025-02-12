﻿using System;
using System.Collections.Generic;
using GW2EIEvtcParser.ParsedData;

namespace GW2EIEvtcParser.Extensions
{
    public class EXTDirectBarrierEvent : EXTAbstractBarrierEvent
    {

        internal EXTDirectBarrierEvent(CombatItem evtcItem, AgentData agentData, SkillData skillData) : base(evtcItem, agentData, skillData)
        {
            BarrierGiven = -evtcItem.Value;
            AgainstDowned = ((evtcItem.IsOffcycle & ~SrcPeerMask) & ~DstPeerMask) == 1;
        }
    }
}
