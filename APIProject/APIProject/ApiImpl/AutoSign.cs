using APIProject.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.ApiImpl
{
    public class AutoSign : ApiHelper
    {
        private string accessToken;
        private string contractNum;
        public AutoSign(string domain, string accessToken, string key, string contractNum) : base(domain, key)
        {
            this.accessToken = accessToken;
            this.contractNum = contractNum;
        }
        protected override string onExecute()
        {

            var data = @"
                {{
                  'signerInfo':{{
                    'name':'signername',
                    'signkeyword':'[SIGN#721126035429]',
                    'email':'peishan.chang@securemetric.com'
                  }},
                 'callUrl': 'https://jsonplaceholder.typicode.com/todos/1',
                 'contractnum': '{0}'
                }}
            ";

            data = string.Format(data, contractNum);

            var encryptedData = Crypt.toHex(Crypt.encrypt(key, data));
            var mac = Crypt.getMac(key, encryptedData);
            return new HttpHelper(base.domain, "/signserver/v1/contract/signature/automatic")
                .post(
                    new Dictionary<string, string>()
                    {
                        ["accesstoken"] = accessToken,
                        ["mac"] = mac,
                        ["data"] = encryptedData
                    });
        }
    }
}
