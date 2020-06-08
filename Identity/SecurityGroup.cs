using System;
using System.Collections.Generic;

namespace Identity
{
    public class SecurityGroup
    {
        private Guid id;
        private string name;

        private ICollection<User> users;

        public SecurityGroup() { }

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

        public ICollection<User> Users
        {
            get => users;
            set
            {
                users.Clear();

                foreach(var user in value ?? Array.Empty<User>())
                {
                    users.Add(user);
                }
            }
        }
    }
}
