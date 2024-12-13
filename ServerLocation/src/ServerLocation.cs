using Dalamud.Game.Text.SeStringHandling.Payloads;
using ECommons.Configuration;
using ECommons.EzIpcManager;
using ECommons.SimpleGui;
using ECommons.Singletons;
using ServerLocation.Framework;
using ServerLocation.Network;
using ServerLocation.UI;

namespace ServerLocation;

public unsafe class ServerLocation : IDalamudPlugin
{
    public string Name
    {
        get
        {
            return "ServerLocation";
        }
    }

    internal static ServerLocation P;
    internal Configuration Config;
    private WindowSystem windowSystem;
    private Canvas canvas;
    public ServerLocation(IDalamudPluginInterface pi)
    {
        // Plugin Initialisation
        P = this;
        ECommonsMain.Init(pi, this, Module.DalamudReflector);


        // Config Window
        EzConfig.Migrate<Configuration>();
        Config = EzConfig.Init<Configuration>();
        EzConfigGui.Init(new MainWindow());

        // Command + IPC
        EzCmd.Add("/sl", OnChatCommand, "Toggles plugin interface");;

        canvas = new();
        windowSystem = new();
        windowSystem.AddWindow(canvas);
        Svc.PluginInterface.UiBuilder.Draw += windowSystem.Draw;

        Svc.GameNetwork.NetworkMessage += PingTracker.PingResolver;
        Svc.GameNetwork.NetworkMessage += PositionTracker.PositionPacketFetch;
        Svc.Framework.Update += FrameworkManager.Framework_Update;
    }

    public void Dispose()
    {
        Svc.PluginInterface.UiBuilder.Draw -= windowSystem.Draw;
        Svc.GameNetwork.NetworkMessage -= PingTracker.PingResolver;
        Svc.GameNetwork.NetworkMessage -= PositionTracker.PositionPacketFetch;
        Svc.Framework.Update -= FrameworkManager.Framework_Update;
        ECommonsMain.Dispose();
        P = null;
    }

    private void OnChatCommand(string command, string arguments)
    {
        arguments = arguments.Trim();

        if (arguments == string.Empty)
        {
            EzConfigGui.Window.Toggle();
        }
    }
}
