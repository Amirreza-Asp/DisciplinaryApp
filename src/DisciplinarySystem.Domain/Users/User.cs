﻿using DisciplinarySystem.Domain.Authentication;
using DisciplinarySystem.Domain.Invitations;
using DisciplinarySystem.Domain.Meetings;

namespace DisciplinarySystem.Domain.Users
{
    public class User : BaseEntity<Guid>
    {
        public User ( string fullName , string nationalCode , DateTime startDate , DateTime endDate , Guid roleId , string type )
        {
            Id = Guid.NewGuid();
            FullName = Guard.Against.NullOrEmpty(fullName);
            NationalCode = nationalCode;
            AttendenceTime = new DateTimeRange(startDate , endDate);
            RoleId = Guard.Against.Default(roleId , nameof(RoleId));
            Type = Guard.Against.NullOrEmpty(type);
        }

        private User () { }

        public String FullName { get; private set; }
        public NationalCode NationalCode { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTimeRange AttendenceTime { get; private set; }
        public String Type { get; private set; }
        public Guid RoleId { get; private set; }


        public Role Role { get; }
        public AuthUser AuthUser { get; private set; }
        public ICollection<InvitationUser> InvitationUsers { get; private set; }
        public ICollection<MeetingUsers> MeetingUsers { get; set; }


        public User WithFullName ( String fullName )
        {
            FullName = Guard.Against.NullOrEmpty(fullName);
            return this;
        }
        public User WithNationalCode ( String nationalCode )
        {
            NationalCode = Guard.Against.NullOrEmpty(nationalCode);
            return this;
        }
        public User WithStartDate ( DateTime startDate )
        {
            AttendenceTime = new DateTimeRange(startDate , AttendenceTime.To);
            return this;
        }
        public User WithEndDate ( DateTime endDate )
        {
            AttendenceTime = new DateTimeRange(AttendenceTime.From , endDate);
            return this;
        }
        public User WithRoleId ( Guid id )
        {
            RoleId = Guard.Against.Default(id);
            return this;
        }
        public User WithType ( String type )
        {
            Type = Guard.Against.NullOrEmpty(type);
            return this;
        }

    }
}
