using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Domain.ValueObjects;

namespace VirtualRoulette.Domain.Entities
{
    //User entity to store user info
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public Money Balance { get; set; }
        public DateTime LastActivity { get; private set; }
        public bool IsActive { get; private set; }

        public User() { }

        public User(string username, string passwordHash) : base()
        {
            Username = username;
            PasswordHash = passwordHash;
            Balance = Money.Zero;
            LastActivity = DateTime.UtcNow;
            IsActive = true;
        }
        
        //Update user acticity time in database and set user active status
        public void UpdateLastActivity()
        {
            LastActivity = DateTime.UtcNow;
            IsActive = true;
        }

        //Set user inactive status
        public void SignOut()
        {
            IsActive = false;
        }
    }
}
