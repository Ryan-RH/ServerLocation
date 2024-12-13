using ServerLocation.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLocation;

public static class PPosition
{
    public class PositionInfo
    {
        public Vector3 Position;
        public int ms = 0;

        public PositionInfo(Vector3 position)
        {
            Position = position;
        }
    }

    public static Queue<PositionInfo> Positions = new Queue<PositionInfo>();
}
