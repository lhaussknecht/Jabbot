using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Jabbot.CommandSprockets;
using Newtonsoft.Json;

namespace Jabbot.Sprockets
{
    class PugSprocket : CommandSprocket
    {
        private static Lazy<List<string>> _supportedInitiators = new Lazy<List<string>>(() => new List<string>());
        public override IEnumerable<string> SupportedInitiators
        {
            get
            {
                if(_supportedInitiators.Value.Count == 0)
                {
                    _supportedInitiators.Value.Add(this.Bot.Name);
                    _supportedInitiators.Value.Add("@" + this.Bot.Name);
                }

                return _supportedInitiators.Value;
            }
        }

        private List<string> _supportedCommands = new List<string> { "pug", "dog", "dawg" };
        public override IEnumerable<string> SupportedCommands
        {
            get
            {
                return this._supportedCommands;
            }
        }

        public override string Help
        {
            get
            {
                StringBuilder helpText = new StringBuilder("pug me - Receive a pug").AppendLine();
                helpText.Append("pug bomb N - Get N pugs");

                return helpText.ToString();
            }
        }

        public override async Task<bool> ExecuteCommand()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-us"));
            client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));

            Uri pugBaseUri = new Uri("http://pugme.herokuapp.com");
            string pugRelativeUri;
            bool isBomb = false;

            if(string.Compare(this.Arguments[0], "bomb", true) == 0)
            {
                int count = 10;

                if(this.Arguments.Length > 1)
                {
                    if(!int.TryParse(this.Arguments[1], out count))
                    {
                        count = 10;
                    }
                }

                pugRelativeUri = string.Format("bomb?count={0}", Uri.EscapeDataString(count.ToString()));
                isBomb = true;
            }
            else
            {
                pugRelativeUri = "random";
            }

            Uri pugUri = new Uri(pugBaseUri, pugRelativeUri);


            HttpResponseMessage response = await client.GetAsync(pugUri);
            if(response.IsSuccessStatusCode)
            {
                string pugResponse = await response.Content.ReadAsStringAsync();
                dynamic json = JsonConvert.DeserializeObject(pugResponse);

                List<string> pugs = new List<string>();

                if(isBomb)
                {
                    foreach(dynamic pug in json.pugs)
                    {
                        pugs.Add((string)pug);
                    }
                }
                else
                {
                    pugs.Add((string)json.pug);
                }

                foreach(string pug in pugs)
                {
                    this.Bot.Send(pug ?? "Could not find pug.", this.Message.Room);
                }
            }

            return response.IsSuccessStatusCode;
        }
    }
}
