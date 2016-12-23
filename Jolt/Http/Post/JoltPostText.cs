﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JoltHttp.Http.Post
{
    public class JoltPostText
    {

        private string url;
        private string text;

        public JoltPostText(string url, string text)
        {
            this.url = url;
            this.text = text;
        }

        public async void MakeRequest(Action<string> OnSuccess, Action<string> OnFail = null,
                                      Action OnStart = null, Action OnFinish = null)
        {

            using (var client = new HttpClient())
            {

                // Call OnStart() at the beginning
                if (OnStart != null)
                    OnStart();

                try
                {
                    client.BaseAddress = new Uri(url);               
                    var content = new StringContent(text);

                    var response = await client.PostAsync(url, content);
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
