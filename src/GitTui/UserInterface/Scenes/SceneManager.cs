using GitTui.Abstractions.UserInterface.Scenes;
using Spectre.Console;

namespace GitTui.UserInterface.Scenes;

public class SceneManager(IMainScene mainScene) : ISceneManager
{
    private Scene _currentScene = Scene.Main;

    public void DrawUi()
    {
        var layout = _currentScene switch
        {
            Scene.Main => mainScene.GetLayout(),
            _ => throw new ArgumentOutOfRangeException()
        };

        Console.Clear();
        AnsiConsole.Write(layout);
    }

    public void SetScene(Scene scene) => _currentScene = scene;
}