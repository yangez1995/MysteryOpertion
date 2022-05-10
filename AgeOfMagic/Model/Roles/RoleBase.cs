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
        public string roleName;
        public Color roleColor;
        public string roleBlurb;
        public int maxSanityPoint;
        public int initialSanityPoint;

        public Dictionary<ButtonType, Button> buttonDict;

        public RoleBase(Player player) 
        {
            this.roleName = string.Empty;
            this.roleColor = Color.white;
            this.roleBlurb = string.Empty;
            this.maxSanityPoint = 50;
            this.initialSanityPoint = 50;

            this.buttonDict = new Dictionary<ButtonType, Button>();
        }

        public string GetRoleName()
        {
            return this.roleName;
        }

        public Color GetRoleColor()
        {
            return this.roleColor;
        }

        public string GetRoleBlurb()
        {
            return this.roleBlurb;
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
