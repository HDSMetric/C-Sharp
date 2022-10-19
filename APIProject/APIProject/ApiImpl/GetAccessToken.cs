using APIProject.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.ApiImpl
{
    public class GetAccessToken : ApiHelper
    {
        string apikey;
        public GetAccessToken(string domain, string apikey, string apisecret) : base(domain, apisecret)
        {
            this.apikey = apikey;
        }

        protected override string onExecute()
        {
            return new HttpHelper(base.domain, "/signserver/v1/accesstoken").get(new Dictionary<string, string>() { ["client_id"] = apikey });
        }

        protected override string onPostExecute(string result)
        {
            var res = base.onPostExecute(result);
            var jsonRes = JsonConvert.DeserializeObject<Dictionary<string, string>>(res);
            return jsonRes["at"];
        }
    }
}
