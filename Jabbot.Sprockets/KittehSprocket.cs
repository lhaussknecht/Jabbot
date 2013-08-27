using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Jabbot.CommandSprockets;

namespace Jabbot.Sprockets
{
    class KittehSprocket : CommandSprocket
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

        private List<string> _supportedCommands = new List<string> { "cat", "kitty", "kitteh" };
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
                StringBuilder helpText = new StringBuilder("cat me - Receive a cat").AppendLine();
                helpText.Append("cat bomb N - Get N cats");

                return helpText.ToString();
            }
        }

        public override async Task<bool> ExecuteCommand()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-us"));
            client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));

            Uri catBaseUri = new Uri("http://thecatapi.com/");
            string catRelativeUri;
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

                catRelativeUri = string.Format("api/images/get?format=xml&results_per_page={0}", Uri.EscapeDataString(count.ToString()));
                isBomb = true;
            }
            else
            {
                catRelativeUri = "api/images/get?format=src";
            }

            Uri catUri = new Uri(catBaseUri, catRelativeUri);

            HttpResponseMessage response = await client.GetAsync(catUri);
            if(response.IsSuccessStatusCode)
            {
                if(!isBomb)
                {
                    this.Bot.Send(response.RequestMessage.RequestUri.AbsoluteUri, this.Message.Room);
                }
                else
                {
                    string catsResponse = await response.Content.ReadAsStringAsync();
                    XElement cats = XElement.Parse(catsResponse);
                    IEnumerable<XElement> kittehs = cats.Element("data").Element("images").Elements("image").Elements("url");
                    foreach(XElement kitteh in kittehs)
                    {
                        this.Bot.Send(kitteh.Value, this.Message.Room);
                    }
                }
            }

            return response.IsSuccessStatusCode;
        }
    }
}
