using Dalamud.Interface.Utility;
using ECommons.ImGuiMethods;
using ECommons.SimpleGui;
using System.Collections.Generic;

namespace ServerLocation.UI;

public unsafe partial class MainWindow : ConfigWindow
{
    public MainWindow() : base() 
    {
        Size = new(400, 350);
        Flags = ImGuiWindowFlags.NoResize;
    }

    public override void Draw()
    {
        ImGuiEx.EzTabBar("##tabbar",
            ("Settings", TabSettings.Draw, null, true),
            ("Customise", TabCustomise.Draw, null, true)
        );
    }

}
