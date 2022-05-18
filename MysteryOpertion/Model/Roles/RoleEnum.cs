using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOpertion.Model.Roles
{
    public enum RoleType
    {
        Crewmate,
        Sheriff,
        Traveller,
        MechanicExpert,
        LightPrayer,
        Judge,
        DoomBait,
        Coroner,
        Diviner,
        Eavesdropper,
        CurseMage,

        ArsonExpert,

        Impostor,
        NoneFaceMan,
        SerialKiller,
        CurseWarlock
    }

    public enum RoleOrientation
    {
        Offensive,
        Auxiliary,
        Intelligence
    }

    public enum RoleWeight
    {
        None,
        SmallProbability,
        MediumProbability,
        HighProbability,
        Priority
    }
}
