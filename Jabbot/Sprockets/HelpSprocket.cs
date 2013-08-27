using System;
using System.Collections.Generic;
using System.Text;
using Jabbot.Core;
using Jabbot.Sprockets.Core;

namespace Jabbot.CommandSprockets
{
    public class HelpSprocket : ISprocket
    {
        public bool Handle(Models.ChatMessage message, IBot bot)
        {
            if((message.Content.IndexOf(bot.Name, StringComparison.OrdinalIgnoreCase) >= 0) && (message.Content.IndexOf("help", StringComparison.OrdinalIgnoreCase) > 0))
            {
                IList<ISprocket> sprockets = (bot as Bot).Sprockets;
                StringBuilder helpText = new StringBuilder();
                helpText.AppendFormat("{0} - A Jabbr Bot Based on Hubot", bot.Name).AppendLine();
                helpText.AppendLine("---------------------------------");
                foreach(var s in sprockets)
                {
                    if(s is ICommandSprocket)
                    {
                        ICommandSprocket commandSprocket = s as ICommandSprocket;
                        helpText.AppendLine(commandSprocket.Help);
                    }
                }

                bot.Send(helpText.ToString(), message.Room);

                return true;
            }
            return false;
        }
    }
}
