using System;
using System.IO;

namespace ShikigamiEngine
{
    static class Paths
    {
        public static string Data = "data/";
        public static string Scripts
        {
            get
            {
                return $"{Data}scripts/";
            }
        }
        public static string Textures
        {
            get
            {
                return $"{Data}textures/";
            }
        }
    }
}
