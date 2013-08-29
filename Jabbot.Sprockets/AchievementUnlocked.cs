// Ported from achievement_unlocked.coffee from https://github.com/github/hubot-scripts/blob/master/src/scripts/achievement_unlocked.coffee

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jabbot.CommandSprockets;

namespace Jabbot.Sprockets
{
    public class AchievementUnlockedSprocket : CommandSprocket
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

        private List<string> _supportedCommands = new List<string> { "achievement", "xbox" };
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
                string helpText = "achievement <achievement> [achiever's gravatar email] - Life's goals are in reach";
                return helpText;
            }
        }

        public override async Task<bool> ExecuteCommand()
        {
            string achievementText;
            string achieverEmail = string.Empty;

            int argCount = this.Arguments.Length;

            if(this.Arguments[argCount - 1].Contains("@"))
            {
                achieverEmail = this.Arguments[argCount - 1];
            }

            achievementText = string.Join(" ", this.Arguments, 0, String.IsNullOrEmpty(achieverEmail) ? argCount : argCount - 1);

            string achievementUrl = String.Format("http://achievement-unlocked.heroku.com/xbox/{0}?&email={1}.png", Uri.EscapeDataString(achievementText), Uri.EscapeDataString(achieverEmail));
            this.Bot.Send(achievementUrl, this.Message.Room);

            return true;
        }
    }
}