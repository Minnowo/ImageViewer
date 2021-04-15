using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Helpers
{
    public static class Helpers
    {
        public static readonly Version OSVersion = Environment.OSVersion.Version;

        public static bool IsWindowsVistaOrGreater()
        {
            return OSVersion.Major >= 6;
        }
    }
}
