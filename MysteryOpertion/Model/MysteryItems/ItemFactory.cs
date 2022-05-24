using MysteryOpertion.Model.MysteryItems.Implement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion.Model.MysteryItems
{
    public static class ItemFactory
    {
        public static MysteryItem Produce(ItemType itemType, Player player)
        {
            switch (itemType)
            {
                case ItemType.SheriffBadge: return new SheriffBadge(player);
                case ItemType.WorkClothes: return new WorkClothes(player);
                default: return null;
            }
        }
    }
}
