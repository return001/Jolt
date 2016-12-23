﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JoltHttp.Http.Put
{
    public class JoltPutFile
    {
        private string url;
        private string filePath;
        byte[] file;

        private Action OnSuccess;
        private Action<string> OnFail;
        private Action<long, long, long> OnProgress;

        public JoltPutFile(string filePath, string url)
        {
            this.filePath = filePath;
            this.url = url;
        }

        public JoltPutFile(byte[] file, string url)
        {
            this.file = file;
            this.url = url;
        }


        public void MakeRequest(Action OnSuccess, Action<string> OnFail = null,
                                Action OnStart = null, Action OnFinish = null,
                                Action<long, long, long> OnProgress = null)
        {

            this.OnSuccess = OnSuccess;
            this.OnProgress = OnProgress;
            this.OnFail = OnFail;

            // If you didn't read the file in your own way, this reads it automatically.
            // Automatic read can cause memory problems.
            byte[] fileToSend;

            if (file == null)
            {
                fileToSend = File.ReadAllBytes(filePath);
            }
            else
            {
                fileToSend = file;
            }

            using (var client = new WebClient())
            {

                // Call OnStart() at the beginning
                if (OnStart != null)
                    OnStart();

                try
                {
                    client.UploadDataAsync(new Uri(url), fileToSend);
                    client.UploadDataCompleted += Completed;
                    client.UploadProgressChanged += UploadProgress;
                }
                catch (Exception e)
                {
                    OnFail(e.ToString());
                }

                // Call OnFinish() at the end
                if (OnFinish != null)
                    OnFinish();

            }             
        }

        private void UploadProgress(object sender, UploadProgressChangedEventArgs e)
        {
            if(OnProgress != null)
                OnProgress(e.BytesReceived, e.TotalBytesToReceive, e.ProgressPercentage);
        }

        private void Completed(object sender, UploadDataCompletedEventArgs e)
        {
            if (e.Error == null)
                OnSuccess();
            else
                OnFail(e.Error.ToString());
        }

    }
}
