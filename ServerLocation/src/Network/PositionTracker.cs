using Dalamud.Game.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ServerLocation.PPosition;

namespace ServerLocation.Network;

public static unsafe class PositionTracker
{
    public static void PositionPacketFetch(nint dataPtr, ushort opCode, uint sourceActorId, uint targetActorId, NetworkMessageDirection direction)
    {
        if (direction == NetworkMessageDirection.ZoneUp && opCode == 680 && PingTracker.Enabled && P.Config.type == Configuration.Type.Real)
        {
            Vector3 coordinates = new Vector3(*(float*)(dataPtr + 2 * sizeof(float)), *(float*)(dataPtr + 3 * sizeof(float)), *(float*)(dataPtr + 4 * sizeof(float)));
            PositionInfo position = new PositionInfo(coordinates);
            Positions.Enqueue(position);
        }
    }
}
