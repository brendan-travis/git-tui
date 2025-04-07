using System.Diagnostics;
using GitTui.Abstractions.Services;
using GitTui.Abstractions.State;
using LibGit2Sharp;

namespace GitTui.Services;

public class GitService(ICommandState commandState) : IGitService
{
    private const int CacheLengthInSeconds = 1;
    private const string RepoNameFallback = "Unknown Repository";

    private readonly string _gitPath = Repository.Discover(Directory.GetCurrentDirectory());

    private string _repoName = string.Empty;

    private string _status = string.Empty;
    private DateTimeOffset _statusLastUpdated = DateTimeOffset.MinValue;

    private string _graphedLog = string.Empty;
    private DateTimeOffset _graphedLogLastUpdated = DateTimeOffset.MinValue;

    private string _lastFetched = string.Empty;
    private DateTimeOffset _lastFetchedLastUpdated = DateTimeOffset.MinValue;

    private string _currentBranch = string.Empty;
    private DateTimeOffset _currentBranchLastUpdated = DateTimeOffset.MinValue;
    
    private bool _isFetching;

    public string GetRepoName()
    {
        if (_repoName != string.Empty)
            return _repoName;

        using var repo = new Repository(_gitPath);
        _repoName = repo.Config.FirstOrDefault(config => config.Key == "remote.origin.url")?.Value
            .Split('/')[^1][..^4] ?? RepoNameFallback;

        return _repoName;
    }

    public string GetStatus()
    {
        if (_statusLastUpdated >= DateTimeOffset.UtcNow - TimeSpan.FromSeconds(CacheLengthInSeconds))
            return _status;

        using var repo = new Repository(_gitPath);

        var status = repo.RetrieveStatus().Where(item => item.State != FileStatus.Ignored);
        _status = string.Empty;
        foreach (var entry in status)
        {
            _status += $"{entry.FilePath} - {entry.State}\n";
        }

        _statusLastUpdated = DateTimeOffset.UtcNow;

        return _status;
    }

    public string GetGraphLog()
    {
        if (_graphedLogLastUpdated >= DateTimeOffset.UtcNow - TimeSpan.FromSeconds(CacheLengthInSeconds))
            return _graphedLog;

        _graphedLog = RunCommand("git", "log --oneline --graph --decorate --all -30");
        _graphedLogLastUpdated = DateTimeOffset.UtcNow;

        return _graphedLog;
    }

    public string GetLastFetchTime()
    {
        if (_lastFetchedLastUpdated >= DateTimeOffset.UtcNow - TimeSpan.FromSeconds(CacheLengthInSeconds))
            return _lastFetched;

        var lastWritten = File.GetLastWriteTimeUtc($"{_gitPath}/FETCH_HEAD").ToLocalTime();
        _lastFetched = lastWritten.ToString("dd/MM/yyyy HH:mm:ss");
        _lastFetchedLastUpdated = DateTimeOffset.UtcNow;

        return _lastFetched;
    }

    public string GetCurrentBranch()
    {
        if (_currentBranchLastUpdated >= DateTimeOffset.UtcNow - TimeSpan.FromSeconds(CacheLengthInSeconds))
            return _currentBranch;

        using var repo = new Repository(_gitPath);

        _currentBranch = repo.Head.FriendlyName;
        _currentBranchLastUpdated = DateTimeOffset.UtcNow;

        return _currentBranch;
    }

    public void PerformFetch()
    {
        _isFetching = true;
        
        using var repo = new Repository(_gitPath);
        var remote = repo.Network.Remotes["origin"];

        Commands.Fetch(
            repo,
            remote.Name,
            remote.FetchRefSpecs.Select(spec => spec.Specification),
            new FetchOptions(),
            null);
        _isFetching = false;
    }

    public bool IsFetching() => _isFetching;
    
    public void PerformCommand()
    {
        commandState.CommandOutput = RunCommand("git", commandState.Command).Replace("\t", "  ");
        commandState.Command = string.Empty;
    }

    private static string RunCommand(string command, string arguments)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = command,
                Arguments = arguments,
                WorkingDirectory = Directory.GetCurrentDirectory(),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        process.Start();
        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        return error != string.Empty ? error.Trim() : output.Trim();
    }
}