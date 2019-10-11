using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceUI
{
    public class VoiceAssistantConfig
    {
        public string AssistantName { get; set; }
        public double MinConfidence { get; set; }
        public string SignalPath { get; set; }
        public string WitAiToken { get; set; }
        public int WitAiTimeoutSeconds { get; set; }
        public string PluginsPath { get; set; }
        public string WordNetPath { get; set; }

        public static VoiceAssistantConfig Default()
        {
            return new VoiceAssistantConfig
            {
                AssistantName = "simba",
                MinConfidence = 0.94,
                SignalPath = "Resources/signal.mp3",
                WitAiToken = "CV5BYP4W3CSLZ5IQCEXEX6BBNR5TKJVA",
                WitAiTimeoutSeconds = 10,
                PluginsPath = $"{Directory.GetCurrentDirectory()}/Plugins",
                WordNetPath = $"{Directory.GetCurrentDirectory()}/WordNet"
            };
        }
    }
}
