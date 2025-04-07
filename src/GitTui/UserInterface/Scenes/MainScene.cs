using GitTui.Abstractions.Services;
using GitTui.Abstractions.State;
using GitTui.Abstractions.UserInterface.Scenes;
using Spectre.Console;

namespace GitTui.UserInterface.Scenes;

public class MainScene(IGitService gitService, ICommandState commandState) : IMainScene
{
    private const int IndicatorMax = 2;
    private int _indicatorIndex;
    private readonly string[] _indicator = [".", "..", "..."];
    private int _delay;

    public Layout GetLayout()
    {
        TickIndicator();
        
        var repoName = gitService.GetRepoName();
        var status = gitService.GetStatus();
        var graphedLog = gitService.GetGraphLog();
        var lastFetchTime = gitService.GetLastFetchTime();
        var isFetching = gitService.IsFetching();
        var currentBranch = gitService.GetCurrentBranch();

        var layout = new Layout("root").SplitRows(
            new Layout("top-bar").SplitRows(
                    new Layout("top-bar-main").SplitColumns(
                        new Layout("top-left"),
                        new Layout("top-center"),
                        new Layout("top-right")),
                    new Layout("top-bar-command")),
            new Layout("bottom-bar").SplitColumns(
                new Layout("bottom-left"),
                new Layout("bottom-right")));

        layout["top-bar"].Size(commandState.IsCommandWindowActive ? 9 : 6);
        layout["top-bar-main"].Size(6);
        layout["top-left"]
            .Update(new Panel($"Current Repository:\n{repoName}\nCurrent Branch:\n{currentBranch}") { Expand = true })
            .Size(30);
        layout["top-center"]
            .Update(new Panel(GetCommands()) { Expand = true })
            .Ratio(1);
        layout["top-right"]
            .Update(new Panel(isFetching ? $"Fetching{_indicator[_indicatorIndex]}" : $"Last Fetched:\n{lastFetchTime}") { Expand = true })
            .Size(30);

        layout["bottom-left"].Update(new Panel(commandState.IsCommandWindowActive ? commandState.CommandOutput : status) { Expand = true });
        layout["bottom-right"].Update(new Panel(graphedLog) { Expand = true });
        
        layout["top-bar-command"]
            .Update(new Panel($"Run Command: git {commandState.Command}"){ Expand = true })
            .Size(3);
        if (commandState.IsCommandWindowActive)
        {
            layout["top-bar-command"].Visible();
            layout["bottom-right"].Invisible();
        }
        else
        {
            layout["top-bar-command"].Invisible();
            layout["bottom-right"].Visible();
        }

        return layout;
    }

    private void TickIndicator()
    {
        _delay += 50;
        if (_delay < 500) return;
        
        _delay = 0;
        _indicatorIndex++;
        if (_indicatorIndex > IndicatorMax)
            _indicatorIndex = 0;

    }

    private static string GetCommands()
    {
        return "<shift+f> Fetch\n<shift+c> Open Command Window";
    }
}