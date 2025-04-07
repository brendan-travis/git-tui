using GitTui.Abstractions.UserInterface.Scenes;
using LibGit2Sharp;
using Spectre.Console;

namespace GitTui.UserInterface.Scenes;

public class MainScene : IMainScene
{
    public Layout GetLayout()
    {
        using var repo = new Repository(Repository.Discover(Directory.GetCurrentDirectory()));

        var status = repo.RetrieveStatus().Where(item => item.State != FileStatus.Ignored);
        var output = string.Empty;
        foreach (var entry in status)
        {
            output += $"{entry.FilePath} - {entry.State}\n";
        }
        
        return new Layout(new Panel(output));
    }
}