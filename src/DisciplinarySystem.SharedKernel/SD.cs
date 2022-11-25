namespace DisciplinarySystem.SharedKernel
{
    public static class SD
    {
        // notification types
        public const string Success = "Success";
        public const string Error = "Error";
        public const string Warning = "Warning";
        public const string Info = "Info";

        // docs path
        public const string ViolationDocumentPath = $"/files/violation/";
        public const string ComplaintDocumentPath = $"/files/complaint/";
        public const string EpistleDocumentPath = $"/files/epistle/";
        public const string InformedDocumentPath = $"/files/informed/";
        public const string InvitationDocumentPath = $"/files/invitation/";
        public const string DefenceDocumentPath = $"/files/defence/";
        public const string PrimaryVoteDocumentPath = $"/files/primary vote/";
        public const string ObjectionDocumentPath = $"/files/objection/";
        public const string FinalVoteDocumentPath = $"/files/final vote/";
        public const string RelatedInfoDocumentPath = $"/files/related info/";
        public const string NotficationDocumentPath = $"/files/notfication/";
        public const string CentralCommitteeVoteDocumentPath = $"/files/central committee vote/";


        // user types
        public const String Badavi = "بدوی";
        public const String Tajdid = "تجدید نظر";

        // case persons
        public const string UserGroup = "اعضای کمیته";
        public const String BadaviUserGroup = "اعضای کمیته بدوی";
        public const String TajdidUserGroup = "اعضای کمیته تجدید نظر";
        public const string ComplainingGroup = "متشاکی";
        public const string PlaitiffGroup = "شاکی";



        public const string ForgetPassword = "ForgetPassword";


        // Roles
        public const String Managment = "Managment";
        public const String Admin = "Admin";
        public const String User = "User";

        // API Details
        public const String KhedmatRaziUserName = "GXBsBt9n";
        public const String KhedmatRaziPassword = "f3EDwlbmrjiFH24234Whr";

        // Defaults
        public const String DefaultPhoneNumber = "00000000000";
        public const String DefaultNationalCode = "3360408330";
        //public const String DefaultNationalCode = "3251345125";
    }
}
