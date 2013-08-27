using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Jabbot.CommandSprockets;
using Newtonsoft.Json;

namespace Jabbot.Sprockets
{
    public class MathSprocket : CommandSprocket
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

        private List<string> _supportedCommands = new List<string> { "calc", "calculate", "convert", "math" };
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
                string helpText = "math me <expression> - Calculate the given expression";
                return helpText;
            }
        }

        public override async Task<bool> ExecuteCommand()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-us"));
            client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));

            string args;

            if(String.Compare(Arguments[0], "me", StringComparison.OrdinalIgnoreCase) == 0)
            {
                args = String.Join(" ", Arguments.Skip(1));
            }
            else
            {
                args = String.Join(" ", Arguments);
            }

            HttpResponseMessage response = await client.GetAsync("http://www.google.com/ig/calculator?hl=en&q=" + Uri.EscapeDataString(args));
            if(response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                dynamic json = JsonConvert.DeserializeObject(result);

                string solution = json.rhs;

                this.Bot.Send(solution ?? "Could not compute.", this.Message.Room);
            }

            return response.IsSuccessStatusCode;
        }
    }
}