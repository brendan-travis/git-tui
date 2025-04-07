namespace GitTui.Abstractions.State;

public interface IInputStateManager
{
    void StoreKeyPress(ConsoleKeyInfo key);
    ConsoleKeyInfo? GetKeyPress();
}