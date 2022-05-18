using MysteryOpertion.Model.Buttons;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Roles
{
    public interface Role 
    {
        string GetRoleName();
        Color GetRoleColor();
        string GetRoleBlurb();
        int GetMaxSanityPoint();
        int GetInitialSanityPoint();
        void UpdateButtons();
        void UpdateButtonsOnMeetingEnd();
    }

    public class RoleBase : Role
    {
        protected RoleInfo info;
        protected int maxSanityPoint;
        protected int initialSanityPoint;

        protected Dictionary<ButtonType, Button> buttonDict;

        public RoleBase(Player player) 
        {
            this.info = new RoleInfo();
            this.maxSanityPoint = 50;
            this.initialSanityPoint = 50;

            this.buttonDict = new Dictionary<ButtonType, Button>();
        }

        public string GetRoleName()
        {
            return this.info.Name;
        }

        public Color GetRoleColor()
        {
            return this.info.Color;
        }

        public string GetRoleBlurb()
        {
            return this.info.Blurb;
        }

        public int GetMaxSanityPoint()
        {
            return this.maxSanityPoint;
        }

        public int GetInitialSanityPoint()
        {
            return this.initialSanityPoint;
        }

        public void UpdateButtons()
        {
            foreach(var item in buttonDict)
            {
                item.Value.Update();
            }
        }

        public void UpdateButtonsOnMeetingEnd()
        {
            foreach (var item in buttonDict)
            {
                item.Value.OnMeetingEnd();
            }
        }
    }

    
}
