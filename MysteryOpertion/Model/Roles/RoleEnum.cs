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
        Jester,
        Spectator,

        Impostor,
        NoneFaceMan,
        SerialKiller,
        CurseWarlock,
        BloodyHunter,
        Lurker
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
