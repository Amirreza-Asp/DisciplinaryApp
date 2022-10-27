﻿using DisciplinarySystem.Domain.Commonications;

namespace DisciplinarySystem.Domain.Authentication
{
    public class AuthUser : BaseEntity<long>
    {
        public AuthUser ( PhoneNumber phoneNumber , NationalCode nationalCode , string name , string family , string userName , string password , long roleId )
        {
            PhoneNumber = phoneNumber;
            NationalCode = nationalCode;
            UserName = Guard.Against.NullOrEmpty(userName);
            Password = Guard.Against.NullOrEmpty(password);
            Name = Guard.Against.NullOrEmpty(name);
            Family = Guard.Against.NullOrEmpty(family);
            IsDeleted = false;
            RoleId = Guard.Against.NegativeOrZero(roleId);
        }

        private AuthUser ()
        {
        }

        public PhoneNumber PhoneNumber { get; private set; }
        public NationalCode NationalCode { get; private set; }
        public String Name { get; private set; }
        public String Family { get; private set; }
        public String UserName { get; private set; }
        public String Password { get; private set; }
        public bool IsDeleted { get; private set; }
        public long RoleId { get; private set; }


        public AuthRole Role { get; private set; }
        public ICollection<SMS> SMSCollection { get; set; }


        public AuthUser WithPassword ( String password )
        {
            Password = Guard.Against.NullOrEmpty(password);
            return this;
        }
    }
}
