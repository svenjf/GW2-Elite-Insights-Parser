﻿using System.Collections.Generic;
using LuckParser.Parser.ParsedData.CombatEvents;

namespace LuckParser.EIData
{
    public class BoonMapDictionary : Dictionary<long, List<AbstractBuffEvent>>
    {
        // Constructors
        public BoonMapDictionary()
        {
        }
        public BoonMapDictionary(Boon boon)
        {
            this[boon.ID] = new List<AbstractBuffEvent>();
        }

        public BoonMapDictionary(IEnumerable<Boon> boons)
        {
            foreach (Boon boon in boons)
            {
                this[boon.ID] = new List<AbstractBuffEvent>();
            }
        }


        public void Add(IEnumerable<Boon> boons)
        {
            foreach (Boon boon in boons)
            {
                if (ContainsKey(boon.ID))
                {
                    continue;
                }
                this[boon.ID] = new List<AbstractBuffEvent>();
            }
        }

        public void Add(Boon boon)
        {
            if (ContainsKey(boon.ID))
            {
                return;
            }
            this[boon.ID] = new List<AbstractBuffEvent>();
        }

        private int CompareApplicationType(AbstractBuffEvent x, AbstractBuffEvent y)
        {
            if (x.Time < y.Time)
            {
                return -1;
            }
            else if (x.Time > y.Time)
            {
                return 1;
            }
            else
            {
                return x.CompareTo(y);
            }
        }


        public void Sort()
        {
            foreach (KeyValuePair<long, List<AbstractBuffEvent>> pair in this)
            {
                pair.Value.Sort(CompareApplicationType);
            }
        }

    }

}