using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Factions
{
    [DataContract]
    public class Faction
    {
        private Guid id;
        private string name;
        private ICollection<string> keywords;

        public Faction() 
        {
            keywords = new List<string>();
        }

        [DataMember(Name = "id")]
        public Guid Id
        {
            get => id;
            set => id = value;
        }

        [DataMember(Name = "name")]
        public string Name
        {
            get => name;
            set => name = value;
        }

        [DataMember(Name = "keys")]
        public ICollection<string> Keywords
        {
            get => keywords;
            set
            {
                keywords.Clear();

                foreach(var key in value ?? Array.Empty<string>())
                {
                    keywords.Add(key);
                }
            }
        }
    }
}
