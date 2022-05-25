using MysteryOpertion.Model.MysteryItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion
{
    public static class MysteryItemPool
    {
        public static List<ItemType> pool = new List<ItemType>();

        public static void Init()
        {
            pool.Add(ItemType.SheriffBadge);
            pool.Add(ItemType.WorkClothes);
            pool.Add(ItemType.CoronerScalpel);
        }

        public static void Clear()
        {
            pool = new List<ItemType>();
        }
    }
}
