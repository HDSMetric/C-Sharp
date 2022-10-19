using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace APIProject.Helper
{
    public abstract class ApiHelper
    {
        protected string key;
        protected string domain;

        public ApiHelper(string domain, string key)
        {
            this.key = key;
            this.domain = domain;
        }

        protected virtual void onPreExecute()
        {

        }
        protected abstract string onExecute();

        protected virtual string onPostExecute(string result)
        {
            string returnResult = result;
            var res = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
            if (res.TryGetValue("data", out var da))
            {
                returnResult = Crypt.decrypt(key, da);
            }
            Console.WriteLine(string.Format(@"[{0}] : {1}", this.GetType().Name, returnResult));
            Console.WriteLine();
            return returnResult;
        }

        public string execute()
        {
            onPreExecute();
            return onPostExecute(onExecute());
        }
    }
}
