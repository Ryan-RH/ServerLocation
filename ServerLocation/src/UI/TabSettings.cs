using Dalamud.Interface.Components;
using ECommons.ImGuiMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLocation.UI;

internal static class TabSettings
{
    internal static void Draw()
    {
        ImGui.Checkbox("Enable Server Location", ref P.Config.Enabled);
        ImGui.Spacing();
        ImGui.Text("Type of Displayed Server Location:");
        ImGuiEx.EnumCombo($"##1", ref P.Config.type);
        ImGui.Checkbox("Draw Path", ref P.Config.PathDraw);
        ImGui.Checkbox("Latency Conversion", ref P.Config.HalfPing);
        ImGuiComponents.HelpMarker("Changes calculated delay from Packet Ping to Client-To-Server Latency. I honestly do not know if the delay is correct yet.");
        ImGui.Checkbox("Display Delay", ref P.Config.DisplayDelay);
        ImGuiComponents.HelpMarker("Calculated by measuring time difference between sent packet and response packet. This time might be different to PingPlugin as it uses multiple methods of retrieving ping, some have different uses than others.");
    }
}
