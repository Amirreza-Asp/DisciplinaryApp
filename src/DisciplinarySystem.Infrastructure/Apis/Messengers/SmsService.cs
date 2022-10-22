﻿using DisciplinarySystem.Application.Contracts.Interfaces;

namespace DisciplinarySystem.Infrastructure.Apis.Messengers
{
    public class SmsService : ISmsService
    {
        public async Task<bool> Send ( String message , String phoneNumber )
        {
            String url = "https://khedmat.razi.ac.ir/api/KhedmatAPI/message?action=sendSMS&username=GXBsBt9n&password=qwe159asd753&text="
                + message + "&FromOutside=true&MobileNumber={\"MobileNumber\":[\"" + phoneNumber + "\"]}";
            var request = new HttpRequestMessage(HttpMethod.Post , url);
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            var client = new HttpClient(handler);

            client.DefaultRequestVersion = new Version("1.1");


            var res = await client.SendAsync(request);

            return res.IsSuccessStatusCode;
        }

    }
}
