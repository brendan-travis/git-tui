using Spectre.Console.Rendering;

namespace GitTui.Abstractions.UserInterface.Scenes;

public interface ISceneManager
{
    public IRenderable DrawUi();
    public void SetScene(Scene scene);
}