using Dalamud.Interface.Components;
using ECommons.ImGuiMethods;
using ServerLocation.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLocation.UI;

internal unsafe static class TabSettings
{
    internal static void Main()
    {
        ImGui.Checkbox("Enable Server Location", ref P.Config.Enabled);
        ImGui.Spacing();
        ImGui.Text("Type of Displayed Server Location:");
        ImGuiEx.EnumCombo($"##1", ref P.Config.type);
        ImGui.Checkbox("Draw Path", ref P.Config.PathDraw);


        ImGui.Spacing();
        ImGui.Spacing();
        ImGui.Spacing();
        ImGui.Spacing();
        ImGui.Spacing();
        ImGui.Separator();
        ImGui.TextColored(EColor.RedBright, "DISCLAIMER");
        ImGui.BulletText("It is impossible to get a fully accurate match between dot and\nserver. This is due to unknown behaviour of SE servers.");
        ImGui.BulletText("Teleporting/instancing could affect the pathing of the dot");
        ImGui.BulletText("Crashes can (very rarely) occur - Most should be fixed");
    }

    internal static void Delay()
    { 
        ImGui.Checkbox("Display Delay", ref P.Config.DisplayDelay);
        ImGuiComponents.HelpMarker("Calculated by measuring time difference between sent packet and response packet. This time might be different to PingPlugin as it uses multiple methods of retrieving ping, some have different uses than others.");
        ImGui.Checkbox("Latency Conversion", ref P.Config.HalfPing);
        ImGuiComponents.HelpMarker("Changes calculated delay from Packet Ping to Client-To-Server Latency. I honestly do not know if the delay is correct yet.");
        ImGui.Spacing();
        ImGui.TextColored(EColor.Yellow, "Through a myriad of tests across EU and NA, the average additional\ndelay was found to be around 120ms which is set as config default.\nChanging it is not recommended.");
        ImGui.Text("Additional Delay");
        // Default size is 18f
         
        ImGui.SliderInt("##7", ref P.Config.AddedDelay, 0, 300);
        ImGuiComponents.HelpMarker("SE servers suffer from queueing issues and do not update as frequently as the local player uploads their packets to the server. This causes unknown delays.");
        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Text($"Raw Ping: {P.Config.RawDelay}");
        ImGui.Text($"Average Ping: {P.Config.AverageDelay}");
        //ImGui.Text($"No. Packets: {P.Config.PacketNumber}");
        //ImGui.Text($"No. Frames: {P.Config.FrameNumber}");
    }


    internal static void Draw()
    {
        ImGuiEx.EzTabBar("##settingsbar",
                ("Main", Main, null, true),
                ("Delay", Delay, null, true)
            );
    }
}
