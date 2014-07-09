using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using NullQuestOnline.Extensions;

namespace NullQuestOnline.Models
{
    public class Character
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public int Level { get; set; }

        [XmlAttribute]
        public int Gold { get; set; }

        [XmlAttribute]
        public int Experience { get; set; }

        [XmlAttribute]
        public int CharacterPowerRoll { get; set; }

        [XmlAttribute]
        public bool Created { get; set; }

        [XmlAttribute]
        public int CurrentHitPoints { get; set; }

        public int MaxHitPoints { get { return (int)Math.Round((double)(25 + (Level * (10 + CharacterPowerRoll.GetStatModifier()))), MidpointRounding.AwayFromZero); } }
    }
}