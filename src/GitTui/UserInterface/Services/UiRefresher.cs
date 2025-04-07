using GitTui.Abstractions.UserInterface.Scenes;
using Microsoft.Extensions.Hosting;

namespace GitTui.UserInterface.Services;

public class UiRefresher(ISceneManager sceneManager) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        Console.CursorVisible = false;
        Console.Clear();
        
        while (!cancellationToken.IsCancellationRequested)
        {
            sceneManager.DrawUi();
            await Task.Delay(1000 / 20, cancellationToken);
        }
    }
}