using GitTui.Abstractions.State;
using Microsoft.Extensions.Hosting;

namespace GitTui.UserInterface.Services;

public class InputHandler(IInputStateManager inputStateManager) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (Console.KeyAvailable)
                inputStateManager.StoreKeyPress(Console.ReadKey(true));
            await Task.Delay(1000 / 200, cancellationToken);
        }
    }
}