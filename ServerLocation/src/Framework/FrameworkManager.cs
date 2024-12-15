using Dalamud.Plugin.Services;
using ServerLocation.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLocation.Framework;

internal static class FrameworkManager
{
    //private static int TimeMs = 0;
    //private static bool Dead = false;
    //private static bool PreviouslyDead = false;
    //public static List<int> ServerDelay = new List<int>();

    public static void Framework_Update(object framework)
    {
        //P.Config.FrameNumber++;
        if (Svc.ClientState.LocalPlayer != null && PingTracker.Enabled)
        {
            // draw and frame update occur at same time
            // due to this, half ping cannot catch up when enabled mid running
            // adding a delay such that client position is queued every x frames causes jittering
            // defeating the point of simulated server position view

            if (P.Config.type == Configuration.Type.Simulated && (PPosition.Positions.Count == 0 || PPosition.Positions.Peek().Position != Svc.ClientState.LocalPlayer.Position))
                PPosition.Positions.Enqueue(new PPosition.PositionInfo(Svc.ClientState.LocalPlayer.Position));

            foreach (var position in PPosition.Positions)
                position.ms += Svc.Framework.UpdateDelta.Milliseconds;

            /*
            if (Svc.ClientState.LocalPlayer.Position.Z > 120f)
            {
                TimeMs += Svc.Framework.UpdateDelta.Milliseconds;
            }
            if (Svc.ClientState.LocalPlayer.IsDead && !Dead)
            {
                var diff = TimeMs - P.Config.AverageDelay;
                ServerDelay.Add(diff);
                Dead = true;
                PreviouslyDead = true;
            }
            if (!Svc.ClientState.LocalPlayer.IsDead && PreviouslyDead)
            {
                TimeMs = 0;
                Dead = false;
                PreviouslyDead = false;
            }*/
        }

    }
}
