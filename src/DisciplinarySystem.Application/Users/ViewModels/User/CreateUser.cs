﻿using DisciplinarySystem.SharedKernel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.Users.ViewModels.User
{
    public class CreateUser
    {
        [Required(ErrorMessage = "نام و نام خانوادگی را وارد کنید")]
        public String FullName { get; set; }

        [Required(ErrorMessage = "کد ملی را وارد کنید")]
        [MaxLength(10 , ErrorMessage = "کد ملی 10 رقم است")]
        [MinLength(10 , ErrorMessage = "کد ملی 10 رقم است")]
        public String NationalCode { get; set; }

        [Required(ErrorMessage = "نقش کاربر را وارد کنید")]
        public Guid RoleId { get; set; }

        [Required(ErrorMessage = "تاریخ حضور را وارد کنید")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "تاریخ پایان را وارد کنید")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "نوع فعالیت")]
        public String Type { get; set; }

        [Required(ErrorMessage = "شماره تلفن را وارد کنید")]
        [MaxLength(11 , ErrorMessage = "شماره تلفن 11 رقم است")]
        [MinLength(11 , ErrorMessage = "شماره تلفن 11 رقم است")]
        public String PhoneNumber { get; set; }

        public long Access { get; set; }

        public List<SelectListItem>? Roles { get; set; }
        public IEnumerable<SelectListItem>? AuthRoles { get; set; }
        public IEnumerable<SelectListItem>? Positions { get; set; }


        public bool IsUserInfoEmpty ()
        {
            return String.IsNullOrEmpty(FullName) && String.IsNullOrEmpty(NationalCode);
        }

        public List<SelectListItem> GetUserTypes () =>
            new List<SelectListItem>
            {
                new SelectListItem{Text = SD.Badavi , Value = SD.Badavi},
                new SelectListItem{Text = SD.Tajdid , Value = SD.Tajdid}
            };
    }
}