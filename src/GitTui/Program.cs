using GitTui.Abstractions.State;
using GitTui.Abstractions.UserInterface.Scenes;
using GitTui.State;
using GitTui.UserInterface.Scenes;
using GitTui.UserInterface.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IInputStateManager, InputStateManager>();

builder.Services.AddSingleton<ISceneManager, SceneManager>();
builder.Services.AddSingleton<IMainScene, MainScene>();

builder.Services.AddHostedService<InputHandler>();
builder.Services.AddHostedService<UiRefresher>();

using var host = builder.Build();

await host.RunAsync();