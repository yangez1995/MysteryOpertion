using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion.Model.MysteryItems.Implement
{
    public class SheriffBadge : ItemBase
    {
        public SheriffBadge(Player player) : base(player)
        {

        }

        public override void OnGet()
        {
            Owner.MaxSanityPoint += 25;
            Owner.CalcSanityPoint(25);
        }
    }
}
