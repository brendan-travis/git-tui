namespace GitTui.Abstractions.UserInterface.Scenes;

public interface ISceneManager
{
    public void DrawUi();
    public void SetScene(Scene scene);
}