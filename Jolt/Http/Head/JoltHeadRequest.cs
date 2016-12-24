﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JoltHttp.Http.Head
{

    public class JoltHeadRequest
    {

        private string url;
        private CookieContainer cookieContainer = new CookieContainer();

        public JoltHeadRequest(string url)
        {
            this.url = url;
        }

        public JoltHeadRequest SetCookies(string CookieName, string CookieValue)
        {
            cookieContainer.Add(new Cookie(CookieName, CookieValue));
            return this;
        }

        public async void MakeRequest(Action<string> OnSuccess, Action<string> OnFail = null,
                                Action OnStart = null, Action OnFinish = null)
        {

            var handler = new HttpClientHandler();

            if (cookieContainer.Count != 0)
            {
                handler.UseCookies = false;
                handler.CookieContainer = cookieContainer;
            }

            using (var client = new HttpClient(handler))
            {
                // Call OnStart() at the beginning
                if (OnStart != null)
                    OnStart();

                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Head, new Uri(url));
                    var response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        OnSuccess(result.ToString());
                    }
                    else
                    {
                        OnFail(response.StatusCode.ToString());
                    }
                }
                catch (Exception e)
                {
                    OnFail(e.ToString());
                }

                // Call OnFinish() at the beginning
                if (OnFinish != null)
                    OnFinish();

            }

                
        }

        

    }
}
