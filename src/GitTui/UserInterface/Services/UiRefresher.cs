using GitTui.Abstractions.UserInterface.Scenes;
using Microsoft.Extensions.Hosting;
using Spectre.Console;

namespace GitTui.UserInterface.Services;

public class UiRefresher(ISceneManager sceneManager) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        Console.CursorVisible = false;
        AnsiConsole.Clear();
            
        await AnsiConsole.Live(sceneManager.DrawUi())
            .StartAsync(async context =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(1000 / 20, cancellationToken);
                    context.UpdateTarget(sceneManager.DrawUi());
                }
            });
    }
} 