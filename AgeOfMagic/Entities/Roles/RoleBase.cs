using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AgeOfMagic.Entities.Roles
{
    public interface Role 
    {
        string GetRoleName();
        Color GetRoleColor();
        string GetRoleBlurb();
    }

    public class RoleBase : Role
    {
        public RoleBase() { }

        public virtual string GetRoleName()
        {
            return String.Empty;
        }

        public virtual Color GetRoleColor()
        {
            return Color.white;
        }

        public virtual string GetRoleBlurb()
        {
            return String.Empty;
        }
    }

    
}
