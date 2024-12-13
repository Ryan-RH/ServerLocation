using Dalamud.Interface.Utility;
using ServerLocation.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommons.ImGuiMethods;
using ServerLocation.Framework;

namespace ServerLocation.UI;

internal unsafe class Canvas : Window
{
    public Canvas() : base("ServerLocation overlay",
      ImGuiWindowFlags.NoInputs
      | ImGuiWindowFlags.NoTitleBar
      | ImGuiWindowFlags.NoScrollbar
      | ImGuiWindowFlags.NoBackground
      | ImGuiWindowFlags.AlwaysUseWindowPadding
      | ImGuiWindowFlags.NoSavedSettings
      | ImGuiWindowFlags.NoFocusOnAppearing
      , true)
    {
        IsOpen = true;
        RespectCloseHotkey = false;
    }

    public override void PreDraw()
    {
        base.PreDraw();
        ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(0, 0));
        ImGuiHelpers.SetNextWindowPosRelativeMainViewport(Vector2.Zero);
        ImGui.SetNextWindowSize(ImGuiHelpers.MainViewport.Size);
    }

    public override bool DrawConditions()
    {
        return Svc.ClientState.LocalPlayer != null;
    }

    public override void Draw()
    {
        var drawList = ImGui.GetBackgroundDrawList();

        if (PingTracker.Enabled && PPosition.Positions.Count > 0 && Svc.ClientState.LocalPlayer != null)
        {
            var averageDelay = (int)PingTracker.delay.Average();
            if (P.Config.HalfPing)
                averageDelay /= 2;
            if (P.Config.Enabled)
            {
                if (P.Config.DisplayDelay)
                    ImGui.Text(averageDelay.ToString());

                if (P.Config.PathDraw)
                {
                    foreach (var position in PPosition.Positions)
                    {
                        Svc.GameGui.WorldToScreen(position.Position, out Vector2 pathPoint);
                        drawList.PathLineTo(pathPoint);
                    }
                    Svc.GameGui.WorldToScreen(Svc.ClientState.LocalPlayer.Position, out Vector2 realPos);
                    drawList.PathLineTo(realPos);
                    Vector4 PathColour = new Vector4(P.Config.PathColour, P.Config.PathTransparency);
                    drawList.PathStroke(PathColour.ToUint());
                }
                Svc.GameGui.WorldToScreen(PPosition.Positions.Peek().Position, out Vector2 pos);
                Vector4 DotColour = new Vector4(P.Config.DotColour, P.Config.DotTransparency);
                drawList.AddCircleFilled(pos, P.Config.DotRadius, DotColour.ToUint(), 100);
            }
            if (PPosition.Positions.Count > 1 && PPosition.Positions.Peek().ms > averageDelay)
                PPosition.Positions.Dequeue();
        }
    }

    public override void PostDraw()
    {
        base.PostDraw();
        ImGui.PopStyleVar();
    }
}
