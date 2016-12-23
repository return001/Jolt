﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JoltHttp.Http.Post
{
    public class JoltPostForm
    {
        private string url;

        private Dictionary<string, string> FormFields = new Dictionary<string, string>();

        public JoltPostForm(string url)
        {
            this.url = url;
        }

        public JoltPostForm AddField(string key, string value)
        {
            FormFields.Add(key, value);
            return this;
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
                    var content = new FormUrlEncodedContent(FormFields);

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
