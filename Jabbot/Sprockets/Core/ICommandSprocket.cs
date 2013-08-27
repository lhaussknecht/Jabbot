using System.Collections.Generic;
using System.Threading.Tasks;
using Jabbot.Models;
using Jabbot.Sprockets.Core;

namespace Jabbot.CommandSprockets
{
    public interface ICommandSprocket : ISprocket
    {
        string[] Arguments { get; }
        Jabbot.Core.IBot Bot { get; }
        string Command { get; }
        Task<bool> ExecuteCommand();
        bool HasArguments { get; }
        string Help { get; }
        string Intitiator { get; }
        bool MayHandle(string initiator, string command);
        ChatMessage Message { get; }
        IEnumerable<string> SupportedCommands { get; }
        IEnumerable<string> SupportedInitiators { get; }
    }
}
