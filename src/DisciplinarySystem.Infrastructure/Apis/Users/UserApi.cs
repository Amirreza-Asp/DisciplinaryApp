﻿using DisciplinarySystem.SharedKernel;
using DisciplinarySystem.SharedKernel.Common;
using Newtonsoft.Json;

namespace DisciplinarySystem.Infrastructure.Apis.Users
{
    public class UserApi : IUserApi
    {
        public async Task<UserInfo> GetUserAsync ( String nationalCode )
        {
            String url = $"https://khedmat.razi.ac.ir/api/KhedmatAPI/khedmat/users?action=details&username={SD.KhedmatRaziUserName}&password={SD.KhedmatRaziPassword}&nationalCode={nationalCode}";
            var request = new HttpRequestMessage(HttpMethod.Post , url);
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            var client = new HttpClient(handler);

            client.DefaultRequestVersion = new Version("1.1");

            var res = await client.SendAsync(request);

            if ( res.IsSuccessStatusCode )
            {
                var strContent = await res.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserInfo>(strContent);
                return user;
            }

            return null;
        }

    }


}
