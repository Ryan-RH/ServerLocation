using ECommons.ImGuiMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;

namespace ServerLocation.UI;

internal static class TabCustomise
{
    internal static void DotDraw()
    {
        //ImGui.ColorPicker4("##2", ref P.Config.DotColour);
        ImGui.Text("Colour");
        ImGui.ColorEdit3("##2", ref P.Config.DotColour);
        ImGui.Spacing();
        ImGui.Text("Transparency");
        ImGui.SliderFloat("##3", ref P.Config.DotTransparency, 0.0f, 1.0f);
        ImGui.Spacing();
        ImGui.Text("Radius");
        ImGui.SliderFloat("##4", ref P.Config.DotRadius, 0.1f, 10.0f);

        PPSettings.Locate();
        if (PPSettings.PPDoodles.Count > 0)
        {
            ImGui.Spacing();
            ImGui.TextColored(EColor.RedBright, "Pixel Perfect Config Detected");
            ImGui.Text("Click the Dot Doodle you wish to copy.");
            foreach (var doodle in PPSettings.PPDoodles)
            {
                if (ImGui.Button(doodle.Name))
                    PPSettings.Change(doodle);
            }
        }
    }

    internal static void PathDraw()
    {
        ImGui.Text("Colour");
        ImGui.ColorEdit3("##5", ref P.Config.PathColour);
        ImGui.Spacing();
        ImGui.Text("Transparency");
        ImGui.SliderFloat("##6", ref P.Config.PathTransparency, 0.0f, 1.0f);
    }

    internal static void Draw()
    {
        if (P.Config.PathDraw)
        {
            ImGuiEx.EzTabBar("##customisebar",
                ("Dot", DotDraw, null, true),
                ("Path", PathDraw, null, true)
            );
        }
        else
        {
            ImGuiEx.EzTabBar("##customisebar",
                ("Dot", DotDraw, null, true)
            );
        }
    }
}
