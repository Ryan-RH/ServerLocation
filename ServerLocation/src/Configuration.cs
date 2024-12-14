using ECommons.ChatMethods;
using ECommons.Configuration;
using ECommons.ImGuiMethods;

namespace ServerLocation;

public class Configuration : IEzConfig
{
    public enum Type
    {
        Real,
        Simulated
    }

    public Type type = Type.Real;

    public Vector3 DotColour = new Vector3(255f, 255f, 255f);
    public float DotTransparency = 1.0f;
    public float DotRadius = 3.0f;


    public Vector3 PathColour = new Vector3(255f, 255f, 255f);
    public float PathTransparency = 1.0f;

    public bool Enabled = false;
    public bool PathDraw = false;
    public bool HalfPing = false;
    public bool DisplayDelay = false;
    public int AddedDelay = 0;
}
