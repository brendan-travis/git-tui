using GitTui.Abstractions.Services;
using GitTui.Abstractions.State;
using Microsoft.Extensions.Hosting;

namespace GitTui.UserInterface.Services;

public class InputHandler(IGitService gitService, ICommandState commandState) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (Console.KeyAvailable)
                HandleInput(Console.ReadKey(true));
            await Task.Delay(1000 / 200, cancellationToken);
        }
    }

    private void HandleInput(ConsoleKeyInfo keyInfo)
    {
        if (commandState.IsCommandWindowActive)
        {
            switch (keyInfo)
            {
                case { Modifiers: ConsoleModifiers.Shift, Key: ConsoleKey.C }:
                    commandState.IsCommandWindowActive = !commandState.IsCommandWindowActive;
                    commandState.Command = string.Empty;
                    break;
                case { Key: ConsoleKey.Enter }:
                    gitService.PerformCommand();
                    break;
                case { Key: ConsoleKey.Backspace }:
                    commandState.Command = commandState.Command[..^1];
                    break;
                default:
                    commandState.Command += keyInfo.KeyChar;
                    break;
            }
            return;
        }

        switch (keyInfo)
        {
            case { Modifiers: ConsoleModifiers.Shift, Key: ConsoleKey.F }:
                gitService.PerformFetch();
                break;
            case { Modifiers: ConsoleModifiers.Shift, Key: ConsoleKey.C }:
                commandState.IsCommandWindowActive = !commandState.IsCommandWindowActive;
                commandState.Command = string.Empty;
                break;
        }
    }
}