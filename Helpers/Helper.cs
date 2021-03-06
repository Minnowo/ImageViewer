using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Security.Principal;
using System.Windows.Forms;
using ImageViewer.Settings;
using System.Text.RegularExpressions;

namespace ImageViewer.Helpers
{
    public static class Helper
    {
        public const byte PixelPerInch = 96;
        public const float PixelPerCm = 37.8f;

        public static readonly Version OSVersion = Environment.OSVersion.Version;
        public static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public static bool IsElevated
        {
            get
            {
                return WindowsIdentity.GetCurrent().Owner.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid);
            }
        }
      
        public static int StringCompareNatural(string a, string b, StringComparison comparison = StringComparison.CurrentCulture)
        {
            Regex regex = new Regex(@"\d+", RegexOptions.Compiled);

            int max = new[] { a, b }.SelectMany(x => regex.Matches(x).Cast<Match>().Select(y => (int?)y.Value.Length)).Max() ?? 0;

            return string.Compare(
                regex.Replace(a, m => m.Value.PadLeft(max, '0')), 
                regex.Replace(b, m => m.Value.PadLeft(max, '0')), comparison);
        }

        /// <summary>
        /// Returns a file name that does not exist.
        /// </summary>
        /// <param name="dir">The directory of the file.</param>
        /// <returns></returns>
        public static string GetNewFileName(string dir = "")
        {
            string fileFormat = InternalSettings.Default_Image_Format.ToString().ToLower();

            if (string.IsNullOrEmpty(dir)) 
                dir = Directory.GetCurrentDirectory();

            // try 10 times 
            for (int x = 0; x < 10; x++)
            {
                string fileName = string.Format("{0}\\{1}.{2}", 
                    dir, DateTime.Now.Ticks.GetHashCode().ToString("x").ToUpper(), fileFormat);

                if (!File.Exists(fileName))
                    return fileName;
            }

            // start using guid after 10 tries at the other method
            // would be really surprised if this code ever runs tbh
            while (true)
            {
                string fileName = string.Format(@"{0}.{1}", Guid.NewGuid(), fileFormat);

                if (!File.Exists(fileName))
                    return fileName;
            }
        }

        /// <summary>
        /// Asks the user to pick a file using the all files dialog filter.
        /// </summary>
        /// <param name="initialDir">The directory to start in.</param>
        /// <param name="form">The form to parent the dialog.</param>
        /// <param name="DialogFilter">Custom dialog filter.</param>
        /// <param name="multiSelect">Allow multi select.</param>
        /// <returns></returns>
        public static string[] AskOpenFile(string initialDir="", Form form =null, string DialogFilter = InternalSettings.All_Files_File_Dialog, bool multiSelect = false)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = DialogFilter;

                ofd.Multiselect = multiSelect;

                if (!string.IsNullOrEmpty(initialDir))
                {
                    ofd.InitialDirectory = initialDir;
                }

                if (ofd.ShowDialog(form) == DialogResult.OK)
                {
                    return ofd.FileNames;
                }
            }

            return null;
        }


        /// <summary>
        /// Returns the point the child window should spawn to be centered on the parent window.
        /// </summary>
        /// <param name="parent">The parent form.</param>
        /// <param name="child">The child form.</param>
        /// <returns></returns>
        public static Point GetCenteredPoint(Form parent, Form child)
        {
            Point p = parent.Location;

            if (parent.Width < child.Width)
            {
                p.X -= Math.Abs(child.Width - parent.Width) >> 1;
            }
            else
            {
                p.X += Math.Abs(child.Width - parent.Width) >> 1;
            }

            if (parent.Height < child.Height)
            {
                p.Y -= Math.Abs(child.Height - parent.Height) >> 1;
            }
            else
            {
                p.Y += Math.Abs(child.Height - parent.Height) >> 1;
            }

            return p;
        }


        /// <summary>
        /// Opens a save file dialog that doesn't save anything, but instead moves the file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string MoveFileDialog(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return string.Empty;

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = InternalSettings.Move_File_Dialog_Title;
                sfd.FileName = Path.GetFileName(filePath);

                if (sfd.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(sfd.FileName))
                {
                    PathHelper.DeleteFileOrPath(sfd.FileName);  // delete any existing file
                    File.Move(filePath, sfd.FileName);          // move the file.
                    return sfd.FileName;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Read a string from the given stream.
        /// </summary>
        /// <param name="stream">The stream to read the data from.</param>
        /// <param name="length">The length of the string</param>
        /// <returns></returns>
        public static string ReadString(Stream stream, int length)
        {
            // https://www.cyotek.com/blog/reading-photoshop-color-swatch-aco-files-using-csharp
            byte[] buffer;

            buffer = new byte[length * 2];

            stream.Read(buffer, 0, buffer.Length);

            return Encoding.BigEndianUnicode.GetString(buffer);
        }



        /// <summary>
        /// Checks if the given size is valid.
        /// </summary>
        /// <param name="size">The size to check.</param>
        /// <returns></returns>
        public static bool ValidSize(Size size)
        {
            return (size.Width > 0 && size.Height > 0);
        }

        /// <summary>
        /// Convert the given bytes to the proper size suffix. (MB, KB, GB)
        /// </summary>
        /// <param name="value">The bytes.</param>
        /// <param name="decimalPlaces">Number of decimal places.</param>
        /// <returns></returns>
        public static string SizeSuffix(Int64 value, int decimalPlaces = 1)
        {
            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
            if (value < 0) { return "-" + SizeSuffix(-value, decimalPlaces); }
            if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format(
                "{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }

        /// <summary>
        /// Checks if the current OS version is windows vista or greator.
        /// </summary>
        /// <returns></returns>
        public static bool IsWindowsVistaOrGreater()
        {
            return OSVersion.Major >= 6;
        }

        /// <summary>
        /// Gets the file extension from the given string.
        /// </summary>
        /// <param name="filePath">The string.</param>
        /// <param name="includeDot">To include the dot with the file name.</param>
        /// <returns></returns>
        public static string GetFilenameExtension(string filePath, bool includeDot = false)
        {
            if (string.IsNullOrEmpty(filePath))
                return string.Empty;

            int pos = filePath.LastIndexOf('.');

            if (pos < 0)
                return string.Empty;

            if (includeDot)
                return "." + filePath.Substring(pos + 1).ToLowerInvariant().Trim();

            return filePath.Substring(pos + 1).ToLowerInvariant().Trim();
        }

        public static bool IsOnlyModifiers(Keys hotkey)
        {
            return hotkey == Keys.ControlKey || hotkey == Keys.ShiftKey || hotkey == Keys.Menu || hotkey == Keys.LWin || hotkey == Keys.RWin;
        }

        public static bool IsValidHotkey(Keys hotkey)
        {
            return hotkey != Keys.None && !IsOnlyModifiers(hotkey);
        }

        public static string GetRandomString(string initial = "", int length = 8)
        {
            string text = string.Format("{0}{1}", initial, DateTime.Now.Ticks.GetHashCode().ToString("x").ToUpper());

            if (text.Length == length)
                return text;
            if (text.Length > length)
                return text.Substring(0, length);
            return GetRandomString(text, length);
        }

        public static bool IsValidFilePath(string path)
        {
            try
            {
                new FileInfo(path);
                return true;
            }
            catch 
            {
                return false;
            }
        }

        /// <summary>
        /// Opens explorer at the given file or directory.
        /// </summary>
        /// <param name="path">The path to open.</param>
        /// <returns></returns>
        public static bool OpenExplorerAtLocation(string path)
        {
            if (File.Exists(path))
            {
                Process fileopener = new Process();
                fileopener.StartInfo.FileName = "explorer";
                fileopener.StartInfo.Arguments = string.Format("/select,\"{0}\"", path);
                fileopener.Start();
                return true;
            }
            else if (Directory.Exists(path))
            {
                Process fileopener = new Process();
                fileopener.StartInfo.FileName = "explorer";
                fileopener.StartInfo.Arguments = path;
                fileopener.Start();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes file attribute from the given file attributes.
        /// </summary>
        /// <param name="attributes">The attribute to remove from.</param>
        /// <param name="attributesToRemove">The attribute to remove.</param>
        /// <returns></returns>
        public static FileAttributes RemoveAttribute(FileAttributes attributes, FileAttributes attributesToRemove)
        {
            return attributes & ~attributesToRemove;
        }

        /// <summary>
        /// Convert the given byte[] to a string[] of hex.
        /// </summary>
        /// <param name="bytes">The given byte[].</param>
        /// <returns></returns>
        public static string[] BytesToHexadecimal(byte[] bytes)
        {
            string[] result = new string[bytes.Length];

            for (int i = 0; i < bytes.Length; i++)
            {
                result[i] = bytes[i].ToString("x2");
            }

            return result;
        }

        public static void Move<T>(List<T> list, int oldIndex, int newIndex)
        {
            var item = list[oldIndex];

            list.RemoveAt(oldIndex);

            if (newIndex > oldIndex)
                newIndex--;

            list.Insert(newIndex, item);
        }

        public static void Move<T>(List<T> list, T item, int newIndex)
        {
            if (item != null)
            {
                var oldIndex = list.IndexOf(item);
                if (oldIndex > -1)
                {
                    list.RemoveAt(oldIndex);

                    if (newIndex > oldIndex)
                        newIndex--;

                    list.Insert(newIndex, item);
                }
            }

        }
    }
}
