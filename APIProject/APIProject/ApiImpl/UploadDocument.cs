using APIProject.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.ApiImpl
{
    public class UploadDocument : ApiHelper
    {
        private string accessToken;
        private string filePath;
        private string fileType;
        public UploadDocument(string domain, string accessToken, string key, string filePath) : base(domain, key)
        {
            this.accessToken = accessToken;
            this.filePath = filePath;
            var ext = this.filePath.Split(".");
            this.fileType = ext[ext.Length - 1];
        }
        protected override string onExecute()
        {
            var fileBytes = File.ReadAllBytes(filePath);
            var fileHash = Crypt.toHex(Crypt.sha256_hash(fileBytes));

            var data = @"
                {{
                  'contractInfo': {{
                  'contractname': 'C# API DEMO',
                  'signernum': 1,
                  'contractnum': '',
                  'isWatermark': false,
                  'signerinfo': [{{
                   'name': 'signername',
                   'caprovide': '1',
                   'email': 'peishan.chang@securemetric.com',
                   'authtype': 0
                  }}]
                 }},
                 'uploadFileHash': '{0}',
                 'type': '{1}'
                }}
            ";

            data = string.Format(data, fileHash, fileType);

            var encryptedData = Crypt.toHex(Crypt.encrypt(key, data));
            var mac = Crypt.getMac(key, encryptedData);
            return new HttpHelper(base.domain, "/signserver/v1/contract/file")
                .post(
                    new Dictionary<string, string>()
                    {
                        ["accesstoken"] = accessToken,
                        ["mac"] = mac,
                        ["data"] = encryptedData
                    },
                    fileBytes);
        }

        protected override string onPostExecute(string result)
        {
            var res = base.onPostExecute(result);
            var jsonRes = JsonConvert.DeserializeObject<Dictionary<string, string>>(res);
            return jsonRes["contractnum"];
        }
    }
}
