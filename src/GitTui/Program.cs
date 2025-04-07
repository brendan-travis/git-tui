using GitTui.Abstractions.Services;
using GitTui.Abstractions.State;
using GitTui.Abstractions.UserInterface.Scenes;
using GitTui.Services;
using GitTui.State;
using GitTui.UserInterface.Scenes;
using GitTui.UserInterface.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<ICommandState, CommandState>();

builder.Services.AddSingleton<ISceneManager, SceneManager>();
builder.Services.AddSingleton<IMainScene, MainScene>();

builder.Services.AddSingleton<IGitService, GitService>();

builder.Services.AddHostedService<InputHandler>();
builder.Services.AddHostedService<UiRefresher>();

using var host = builder.Build();

await host.RunAsync();