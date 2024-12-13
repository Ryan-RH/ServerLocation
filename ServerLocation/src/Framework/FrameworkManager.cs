using ServerLocation.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLocation.Framework;

internal static class FrameworkManager
{
    public static void Framework_Update(object framework)
    {
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
        }
    }
}
