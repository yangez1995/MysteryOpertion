using MysteryOpertion.Model.MysteryItems.Implement;
using MysteryOpertion.Model.Roles.CrewmateRoles;
using MysteryOpertion.Model.Roles.ImpostorRoles;
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
                case ItemType.SheriffBadge: 
                    if(player.MainRole is not Crewmate || player.MainRole is Sheriff)
                        return null;

                    return new SheriffBadge(player);
                case ItemType.WorkClothes:
                    if (player.MainRole is Impostor || player.MainRole is MechanicExpert)
                        return null;

                    return new WorkClothes(player);
                case ItemType.CoronerScalpel: 
                    if(player.MainRole is not Crewmate || player.MainRole is Coroner)
                        return null;
                    
                    return new CoronerScalpel(player);
                default: return null;
            }
        }
    }
}
