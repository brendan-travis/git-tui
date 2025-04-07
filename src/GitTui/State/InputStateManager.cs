using GitTui.Abstractions.State;

namespace GitTui.State;

public class InputStateManager : IInputStateManager
{
    private Queue<ConsoleKeyInfo> KeyPresses { get; } = new();
    
    public void StoreKeyPress(ConsoleKeyInfo key) => KeyPresses.Enqueue(key);

    public ConsoleKeyInfo? GetKeyPress() => KeyPresses.Count != 0 ? KeyPresses.Dequeue() : null;
}