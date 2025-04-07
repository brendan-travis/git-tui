using GitTui.Abstractions.State;

namespace GitTui.State;

public class CommandState : ICommandState
{
    public bool IsCommandWindowActive { get; set; }
    public string Command { get; set; } = string.Empty;
    public string CommandOutput { get; set; } = string.Empty;
}