using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MysteryOpertion.Model.Roles
{
    public class RoleInfo
    {
        public RoleType Type {get;set;}
        public string Name { get; set; }
        public string Blurb { get; set; }
        public string Target { get; set; }
        public Color Color { get; set; }
    }
}
