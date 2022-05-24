using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion.Model.MysteryItems
{
    public interface MysteryItem
    {
        public void OnGet();
    }

    public abstract class ItemBase : MysteryItem
    {
        public string Name { get; set; }
        public ItemType Type { get; set; }
        public Player Owner { get; set; }
        public int AvailableTimes { get; set; }

        public ItemBase(Player player)
        {
            this.Owner = player;
            this.AvailableTimes = 1;
            OnGet();
        }

        public abstract void OnGet();

        public virtual void OnUse()
        {
            AvailableTimes--;
            if (AvailableTimes == 0)
                Owner.ItemBag.Remove(Type);
        }
    }
}
