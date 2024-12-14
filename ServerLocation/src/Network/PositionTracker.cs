using Dalamud.Game.Network;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ServerLocation.PPosition;

namespace ServerLocation.Network;

public static unsafe class PositionTracker
{
    private static bool TwoPackets = false;

    public static void PositionPacketFetch(nint dataPtr, ushort opCode, uint sourceActorId, uint targetActorId, NetworkMessageDirection direction)
    {
        if (direction == NetworkMessageDirection.ZoneUp && PingTracker.Enabled && P.Config.type == Configuration.Type.Real)
        {
            if (opCode == 680)
            {
                Vector3 coordinates = new Vector3(*(float*)(dataPtr + 2 * sizeof(float)), *(float*)(dataPtr + 3 * sizeof(float)), *(float*)(dataPtr + 4 * sizeof(float)));
                PositionInfo position = new PositionInfo(coordinates);
                Positions.Enqueue(position);
            }
            else if (opCode == 304)
            {
                // Seems like instance position packets send close to every frame which causes queueing/dequeueing issues
                // instead every second packet is collected which is still very smooth
                TwoPackets = !TwoPackets;
                if (TwoPackets)
                {
                    Vector3 coordinates = new Vector3(*(float*)(dataPtr + 3 * sizeof(float)), *(float*)(dataPtr + 4 * sizeof(float)), *(float*)(dataPtr + 5 * sizeof(float)));
                    PositionInfo position = new PositionInfo(coordinates);
                    Positions.Enqueue(position);
                }
            }
        }
    }
}
