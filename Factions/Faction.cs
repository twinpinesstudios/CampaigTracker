using System;
using System.Collections;
using System.Collections.Generic;

namespace Factions
{
    public class Faction
    {
        private Guid id;
        private string name;
        private ICollection<string> keywords;

        public Faction() 
        {
            keywords = new List<string>();
        }

        public Guid Id
        {
            get => id;
            set => id = value;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

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
