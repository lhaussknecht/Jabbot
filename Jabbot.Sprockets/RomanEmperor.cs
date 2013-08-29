// Ported from romanemperor.coffee from https://github.com/github/hubot-scripts/blob/master/src/scripts/romanemperor.coffee

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Jabbot.CommandSprockets;

namespace Jabbot.Sprockets
{
    public class RomanEmperorSprocket : CommandSprocket
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

        private List<string> _supportedCommands = new List<string> { "approve", "approves", "disapprove", "disapproves" };
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
                StringBuilder helpText = new StringBuilder("approves - Approves something").AppendLine();
                helpText.Append("disapproves - Disapproves something");
                return helpText.ToString();
            }
        }

        public override async Task<bool> ExecuteCommand()
        {
            string emperorUrl = string.Empty;

            if(this.Command.StartsWith("approve", StringComparison.OrdinalIgnoreCase))
            {
                emperorUrl = "http://i1.kym-cdn.com/photos/images/newsfeed/000/254/655/849.gif ";
            }
            else if(this.Command.StartsWith("disapprove", StringComparison.OrdinalIgnoreCase))
            {
                emperorUrl = "http://i3.kym-cdn.com/photos/images/newsfeed/000/254/517/a70.gif ";
            }

            this.Bot.Send(emperorUrl, this.Message.Room);

            return true;
        }
    }
}