using System;
using System.Collections.Generic;

namespace Identity
{
    public class User
    {
        private Guid id;
        private string username;

        private ICollection<SecurityGroup> securityGroups;
        private ICollection<OrderOfBattle> orderOfBattles;

        public User() { }

        public Guid Id
        {
            get => this.id;

            set => this.id = value;
        }

        public string Username
        {
            get => this.username;

            set => this.username = value;
        }

        public ICollection<SecurityGroup> SecurityGroups
        {
            get => this.securityGroups;

            set
            {
                securityGroups.Clear();

                foreach(var group in value ?? Array.Empty<SecurityGroup>())
                {
                    securityGroups.Add(group);
                }
            }
        }

        public ICollection<OrderOfBattle> OrderOfBattles
        {
            get => this.orderOfBattles;

            set
            {
                orderOfBattles.Clear();

                foreach(var order in value ?? Array.Empty<OrderOfBattle>())
                {
                    orderOfBattles.Add(order);
                }
            }
        }
    }
}
