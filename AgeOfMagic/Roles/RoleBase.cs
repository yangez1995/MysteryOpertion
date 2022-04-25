using System;
using System.Collections.Generic;
using System.Text;

namespace AgeOfMagic.Roles
{
    public interface Role 
    {
        string GetRoleName();
    }

    public abstract class RoleBase : Role
    {
        public RoleBase() { }

        public abstract string GetRoleName();
    }

    public enum RoleType
    {
        Crewmate,
        Sheriff,
        Impostor
    }
}
