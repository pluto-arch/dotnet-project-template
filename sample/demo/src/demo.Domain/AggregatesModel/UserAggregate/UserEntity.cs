using System;
using System.Collections.Generic;
using Demo.Domain.Events.AccountEvents;

namespace Demo.Domain.AggregatesModel.UserAggregate
{
    public class UserEntity : BaseEntity<int>
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

        public IReadOnlyCollection<UserRoleEntity> Roles { get; set; }

        public void ChangeSecurityStamp(string securityStamp)
        {
            SecurityStamp = securityStamp;
            AddDomainEvent(new SecurityStampChangedEvent<int>(this.Id, "安全戳已变更！"));
        }

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
            AddDomainEvent(new SecurityStampChangedEvent<int>(this.Id, "安全戳已变更！"));
        }

        public void SetSecurityStamp(string securityStamp)
        {
            SecurityStamp = securityStamp;
            AddDomainEvent(new SecurityStampChangedEvent<int>(this.Id, "安全戳已变更！"));
        }
    }
}