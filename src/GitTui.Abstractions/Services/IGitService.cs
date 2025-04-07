namespace GitTui.Abstractions.Services;

public interface IGitService
{
    public string GetRepoName();
    public string GetStatus();
    public string GetGraphLog();
    public string GetLastFetchTime();
    public string GetCurrentBranch();

    public void PerformFetch();
    public bool IsFetching();

    public void PerformCommand();
}