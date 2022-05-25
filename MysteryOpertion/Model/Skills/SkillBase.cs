using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion.Model.Skills
{
    public interface Skill
    {
        void OnGet();
        void OnLose();
    }

    public class SkillBase : Skill
    {
        public Player Owner { get; set; }

        public SkillBase(Player player)
        {
            this.Owner = player;
            OnGet();
        }

        public virtual void OnGet()
        {

        }

        public virtual void OnLose()
        {

        }
    }
}
