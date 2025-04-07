namespace GitTui.Abstractions.State;

public interface ICommandState
{
    public bool IsCommandWindowActive { get; set; }
    public string Command { get; set; }
    public string CommandOutput { get; set; }
}