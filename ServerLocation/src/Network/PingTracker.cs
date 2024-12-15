using Dalamud.Game.Network;
using Lumina.Excel.Sheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerLocation.Network;

public static class PingTracker
{
    public static bool Enabled = false;

    // static form of PingPlugin's packet ping
    // I have absolutely no idea the accuracy of this in terms of game server events, needs rigorous testing
    private static Timer? PacketTimeout = null;
    private static bool ZoneUpReceived = false;
    private static uint ZoneUpTimestamp = 0;

    private static bool ZoneDownReceived = false;

    public static Queue<long> delay = new Queue<long> ();

    public static void PingResolver(nint dataPtr, ushort opCode, uint sourceActorId, uint targetActorId, NetworkMessageDirection direction)
    {
        if (!ZoneUpReceived && direction == NetworkMessageDirection.ZoneUp && opCode == 582)
        {
            TrackPacketUp(dataPtr);
        }
        else
        {
            //P.Config.PacketNumber++;
        }
        
        if (ZoneUpReceived && !ZoneDownReceived && direction == NetworkMessageDirection.ZoneDown)
        {
            CheckPacketDown(dataPtr);
        }

        if (ZoneUpReceived && ZoneDownReceived && direction == NetworkMessageDirection.ZoneDown)
        {
            CalculatePing();
        }
    }

    private static void TrackPacketUp(nint dataPtr)
    {
        ZoneUpTimestamp = (uint)Marshal.ReadInt32(dataPtr);
        if (ZoneUpTimestamp != 0)
            ZoneUpReceived = true;
        PacketTimeout?.Dispose();
        PacketTimeout = new Timer(_ =>
        {
            if (!ZoneDownReceived)
                ZoneUpReceived = false;
        });
        PacketTimeout.Change(TimeSpan.FromSeconds(2), Timeout.InfiniteTimeSpan);
    }

    private static void CheckPacketDown(nint dataPtr)
    {
        var timestap = (uint)Marshal.ReadInt32(dataPtr);
        if (timestap == ZoneUpTimestamp)
        {
            ZoneDownReceived = true;
            PacketTimeout?.Dispose();
            PacketTimeout = null;
            P.Config.PacketNumber = 0;
            P.Config.FrameNumber = 0;
        }
    }

    private static void CalculatePing()
    {
        if (QueryPerformanceCounter(out var nextNs))
        {
            var nextMs = nextNs / 10000;
            var delayMs = nextMs - ZoneUpTimestamp;
            PluginLog.Information($"Packet Ping: {delayMs}ms, Delay: {delayMs/2}");
            P.Config.RawDelay = (int)delayMs;
            delay.Enqueue(delayMs);
            if (delay.Count > 10)
                delay.Dequeue();
            Enabled = true;

            ZoneDownReceived = false;
            ZoneUpReceived = false;
        }
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("kernel32.dll")]
    public static extern bool QueryPerformanceCounter(out long lpPerformanceCount);
}
