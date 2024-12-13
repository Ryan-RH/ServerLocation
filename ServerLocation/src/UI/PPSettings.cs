using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Lumina.Excel.Sheets;

namespace ServerLocation.UI;

public static class PPSettings
{
    public class PPDoodle
    {
        public string Name;
        public Vector3 DotColour;
        public float Alpha;
        public float DotRadius;

        public PPDoodle(string name, Vector3 dotColour, float alpha, float dotRadius)
        {
            Name = name;
            DotColour = dotColour;
            Alpha = alpha;
            DotRadius = dotRadius;
        }
    }

    public static List<PPDoodle> PPDoodles = new List<PPDoodle>();

    public static void Locate()
    {
        PPDoodles.Clear();
        if (Svc.PluginInterface.ConfigDirectory.Parent != null)
        {
            var pp = Svc.PluginInterface.ConfigDirectory.Parent.FullName + "\\PixelPerfect.json";
            if (File.Exists(pp))
            {
                var ppText = File.ReadAllText(pp);
                var doodleBag = JObject.Parse(ppText).GetValue("DoodleBag") as JArray;
                if (doodleBag != null)
                {
                    // Find each doodle and check type
                    foreach (var doodle in doodleBag)
                    {
                        var type = doodle.Value<int>("Type");
                        if (type == 2)
                        {
                            var colourObject = doodle["Colour"] as JObject;
                            if (colourObject != null)
                            {
                                Vector3 colour = new Vector3(
                                    colourObject.Value<float>("X"),
                                    colourObject.Value<float>("Y"),
                                    colourObject.Value<float>("Z")
                                );
                                float alpha = colourObject.Value<float>("W");
                                var name = doodle.Value<string>("Name");
                                var radius = doodle.Value<float>("Radius");
                                PPDoodles.Add(new PPDoodle(name!, colour, alpha, radius));
                            }
                        }
                    }
                }
            }
            else PluginLog.Information("User does not have PixelPerfect");
        }
    }


    public static void Change(PPDoodle doodle)
    {
        P.Config.DotRadius = doodle.DotRadius;
        P.Config.DotColour = doodle.DotColour;
        P.Config.DotTransparency = doodle.Alpha;
    }
}
