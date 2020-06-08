using Factions;
using System;

namespace Identity
{
    public class OrderOfBattle
    {
        private Guid id;
        private string name;
        private Guid userId;
        private Guid factionId;

        private User user;
        private Faction faction;

        public OrderOfBattle() { }

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

        public Guid UserId
        {
            get => userId;
            set => userId = value;
        }

        public User User
        {
            get => user;
            set => user = value;
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
