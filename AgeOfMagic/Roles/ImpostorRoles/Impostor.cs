using System;
using System.Collections.Generic;
using System.Text;

namespace AgeOfMagic.Roles.ImpostorRoles
{
    public class Impostor : RoleBase
    {
        public Impostor() : base() { }

        public override string GetRoleName()
        {
            return RoleNameDictionary.Impostor;
        }
    }
}
