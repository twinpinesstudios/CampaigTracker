using System;

namespace Factions
{
    public class Unit
    {
        private Guid id;
        private string name;
        private Guid factionId;

        private Faction faction;

        public Unit() { }

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

        public Guid FactionId
        {
            get => factionId;
            set => factionId = value;
        }

        public Faction Faction
        {
            get => faction;
            set => faction = value;
        }
    }
}
