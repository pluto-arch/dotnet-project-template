using System;
using Dapper;
using PlutoNetCoreTemplate.Domain.Entities;

namespace PlutoNetCoreTemplate.Domain.Aggregates.System
{
    [Table("Users")]
    public class UserEntity : BaseAggregateRoot<int>
    {
        public UserEntity()
        {
        }

        public string UserName { get; set; }

        /// <summary>
        ///     A random value that must change whenever a users credentials change
        ///     (password changed, login removed)
        /// </summary>
        public string SecurityStamp { get; private set; }

        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }

        public string Phone { get; set; }
        public bool PhoneConfirmed { get; set; }


        public DateTime? LockoutEndDateUtc { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }


        public string PasswordHash { get; internal set; }

        public string GetSecurityStamp()
        {
            return SecurityStamp;
        }

        internal bool HasPassword()
        {
            return !string.IsNullOrEmpty(PasswordHash);
        }

        public void SetPasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash;
        }

        public void SetSecurityStamp(string securityStamp)
        {
            SecurityStamp = securityStamp;
        }
    }
}