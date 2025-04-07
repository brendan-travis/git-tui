using Spectre.Console;

namespace GitTui.Abstractions.UserInterface.Scenes;

public interface IScene
{
    public Layout GetLayout();
}