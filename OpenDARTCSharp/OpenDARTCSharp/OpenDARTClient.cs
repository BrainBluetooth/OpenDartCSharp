#pragma warning disable IDE0003
using System;
using System.Collections.ObjectModel;

namespace OpenDARTCSharp
{
    public sealed class OpenDARTClient
    {
        private readonly string apiKey;

        public OpenDARTClient(string apiKey)
        {
            ValidateApiKey(apiKey);
            this.apiKey = apiKey;
        }
        public OpenDARTClient(byte[] apiKey)
        {
            ValidateApiKey(apiKey);
            char[] arr = new char[40];
            for (int i = 0; i < 20; i++)
            {
                uint item = apiKey[i];
                uint iLow = item & 15;
                uint iBig = item >> 4;
                char cLow = default(char);
                char cBig = default(char);
                if (iLow < 10)
                    cLow = (char)(iLow + '0');
                else
                    cLow = (char)(iLow - 10 + 'a');
                if (iBig < 10)
                    cBig = (char)(iBig + '0');
                else
                    cBig = (char)(iBig - 10 + 'a');
                arr[i * 2] = cBig;
                arr[i * 2 + 1] = cLow;
            }
            this.apiKey = new string(arr);
        }

        private void ValidateApiKey(byte[] apiKey)
        {
            if (apiKey.Length != 20)
                throw new ArgumentException("API Key's length must be 20");
        }
        private void ValidateApiKey(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey) || apiKey.Length != 40)
                throw new ArgumentException("Invalid API Key");

            if (apiKey == "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")
            {
#if !DEBUG
                throw new ArgumentException("Don't use Test API Key in Release");
#endif
            }
            else
            {
                foreach (var c in apiKey)
                {
                    if (!(c >= '0' && c <= '9') && !(c >= 'a' && c <= 'f'))
                        throw new ArgumentException("Invalid API Key");
                }
            }
        }

        public ReadOnlyCollection<CorpCodeData> LoadCorpCodes(bool forciblyUpdate = false)
        {
            return OpenDartApi.LoadCorpCodes(this.apiKey, forciblyUpdate);
        }

        public CompanyData LoadCompany(CorpCodeData corpCode)
        {
            return OpenDartApi.LoadCompany(this.apiKey, corpCode);
        }

        public void LoadList()
        {
            OpenDartApi.LoadList(this.apiKey, new DateTime(2020, 1, 17), new DateTime(2020, 1, 17), CompanyData.CORP_CLASS.Y);
        }
    }
}