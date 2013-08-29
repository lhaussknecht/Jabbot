// Adapted from wits.coffee from https://github.com/github/hubot-scripts/blob/master/src/scripts/wits.coffee

using System;
using System.Text.RegularExpressions;
using System.Web;

namespace Jabbot.Sprockets
{
    class WTF : RegexSprocket
    {
        public override System.Text.RegularExpressions.Regex Pattern
        {
            get
            {
                return new Regex("(.*)(?i)(wtf|fuq|what the fuck|wat[^a-z]|what the shit)(.*)");
            }
        }

        protected override void ProcessMatch(System.Text.RegularExpressions.Match match, Models.ChatMessage message, Jabbot.Core.IBot bot)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            string[] images = {
                                "http://i.imgur.com/g5GET.jpg",
                                "http://i.imgur.com/HSSmy.jpg",
                                "http://i.imgur.com/wVIkb.jpg",
                                "http://i.imgur.com/a6uNS.jpg",
                                "http://i.imgur.com/QDEtx.jpg",
                                "http://i.imgur.com/gED5u.jpg",
                                "http://i.imgur.com/u6dvm.jpg",
                                "http://i.imgur.com/TEtBW.jpg",
                                "http://i.imgur.com/MMqJW.jpg",
                                "http://i.imgur.com/4aa9h.jpg",
                                "http://i.imgur.com/b3nmR.jpg"
                              };

            bot.Send(HttpUtility.HtmlEncode(images[random.Next(images.Length)]), message.Room);
        }
    }
}
