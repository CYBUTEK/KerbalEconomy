using System.Collections.Generic;
using UnityEngine;

namespace KerbalEconomy.Extensions
{
    public static class PartListExtensions
    {
        public static int Cost(this List<Part> value)
        {
            int cost = 0;

            foreach (Part part in value)
                cost += part.partInfo.cost;

            return cost;
        }
    }
}
