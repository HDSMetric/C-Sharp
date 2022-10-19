using APIProject.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.ApiImpl
{
    public class GetAccountInfo : ApiHelper
    {
        private string accessToken;
        public GetAccountInfo(string domain, string accessToken, string key) : base(domain, key)
        {
            this.accessToken = accessToken;
        }

        protected override string onExecute()
        {
            return new HttpHelper(base.domain, "/signserver/v1/account/infomation").get(new Dictionary<string, string>() { ["accesstoken"] = accessToken });
        }
    }
}
