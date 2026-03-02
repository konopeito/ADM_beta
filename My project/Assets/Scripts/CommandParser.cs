using System.Collections;
using System.Collections.Generic;
using System;


public class CommandParser
{
    private Dictionary<string, Action> commands = new Dictionary<string, Action>();
    private Dictionary<string, Action> hiddenCommands = new Dictionary<string, Action>();

    public void RegisterCommand(string keyword, Action action)
    {
        commands[keyword.ToLower()] = action;
    }

    public void RegisterHiddenCommand(string keyword, Action action)
    {
        hiddenCommands[keyword.ToLower()] = action;
    }

    public bool TryExecute(string input)
    {
        string cleaned = input.Trim().ToLower();

        if (commands.ContainsKey(cleaned))
        {
            commands[cleaned].Invoke();
            return true;
        }

        if (hiddenCommands.ContainsKey(cleaned))
        {
            hiddenCommands[cleaned].Invoke();
            return true;
        }

        return false;
    }

    public List<string> GetVisibleCommands()
    {
        return new List<string>(commands.Keys);
    }
}
