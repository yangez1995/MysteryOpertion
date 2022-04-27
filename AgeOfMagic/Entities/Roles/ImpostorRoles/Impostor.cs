using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AgeOfMagic.Entities.Roles.ImpostorRoles
{
    public class Impostor : RoleBase
    {
        public Impostor() : base() { }

        public override string GetRoleName()
        {
            return RoleNameDictionary.Impostor;
        }

        public override Color GetRoleColor()
        {
            return Color.red;
        }

        public override string GetRoleBlurb()
        {
            return RoleBlurbDictionary.ImpostorBlurb;
        }
    }
}
