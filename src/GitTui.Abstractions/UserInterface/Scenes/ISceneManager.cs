namespace GitTui.Abstractions.UserInterface.Scenes;

public interface ISceneManager
{
    void DrawUi();
    void SetScene(Scene scene);
}