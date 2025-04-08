using GitTui.Abstractions.UserInterface.Scenes;
using Spectre.Console.Rendering;

namespace GitTui.UserInterface.Scenes;

public class SceneManager(IMainScene mainScene) : ISceneManager
{
    private Scene _currentScene = Scene.Main;

    public IRenderable DrawUi()
    {
        return _currentScene switch
        {
            Scene.Main => mainScene.GetLayout(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public void SetScene(Scene scene) => _currentScene = scene;
}