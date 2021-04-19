using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageViewer.Helpers
{
    public static class PathHelper
    {
        public static void CreateDirectory(string directoryPath)
        {
            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                try
                {
                    Directory.CreateDirectory(directoryPath);
                }
                catch{}
            }
        }

        public static void CreateDirectoryFromFilePath(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                string directoryPath = Path.GetDirectoryName(filePath);
                CreateDirectory(directoryPath);
            }
        }

        public static bool DeleteFileOrPath(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }
                else if (Directory.Exists(path))
                {
                    Directory.Delete(path);
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
    }
}
