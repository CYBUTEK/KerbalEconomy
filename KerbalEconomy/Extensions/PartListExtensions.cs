﻿// PROJECT: Kerbal Economy
// AUTHOR:  CYBUTEK
// LICENSE: Attribution-NonCommercial-ShareAlike 3.0 Unported

using System.Collections.Generic;
using UnityEngine;

namespace KerbalEconomy.Extensions
{
    public static class PartListExtensions
    {
        /// <summary>
        /// Gets the total cost of all the parts.
        /// </summary>
        public static int Cost(this List<Part> value)
        {
            int cost = 0;

            foreach (Part part in value)
                cost += part.partInfo.cost;

            return cost;
        }
    }
}
