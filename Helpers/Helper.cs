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

namespace ImageViewer.Helpers
{
    public static class Helper
    {
        public const byte PixelPerInch = 96;
        public const float PixelPerCm = 37.8f;

        public static readonly Version OSVersion = Environment.OSVersion.Version;
        public static readonly string[] SizeSuffixes =
                   { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public static bool IsElevated
        {
            get
            {
                return WindowsIdentity.GetCurrent().Owner.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid);
            }
        }

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

        public static string ReadString(Stream stream, int length)
        {
            // https://www.cyotek.com/blog/reading-photoshop-color-swatch-aco-files-using-csharp
            byte[] buffer;

            buffer = new byte[length * 2];

            stream.Read(buffer, 0, buffer.Length);

            return Encoding.BigEndianUnicode.GetString(buffer);
        }

        /// <summary>
        /// Reads a 32bit unsigned integer in big-endian format.
        /// </summary>
        /// <param name="stream">The stream to read the data from.</param>
        /// <returns>The unsigned 32bit integer cast to an <c>Int32</c>.</returns>
        public static int ReadInt32(Stream stream)
        {
            // https://www.cyotek.com/blog/reading-photoshop-color-swatch-aco-files-using-csharp
            return ((byte)stream.ReadByte() << 24) | ((byte)stream.ReadByte() << 16) | ((byte)stream.ReadByte() << 8) | ((byte)stream.ReadByte() << 0);
        }

        /// <summary>
        /// Reads a 16bit unsigned integer in big-endian format.
        /// </summary>
        /// <param name="stream">The stream to read the data from.</param>
        /// <returns>The unsigned 16bit integer cast to an <c>Int32</c>.</returns>
        public static int ReadInt16(Stream stream)
        {
            // https://www.cyotek.com/blog/reading-photoshop-color-swatch-aco-files-using-csharp
            return (stream.ReadByte() << 8) | (stream.ReadByte() << 0);
        }

        public static int ReadInt(Stream stream)
        {
            byte[] buffer;

            // big endian conversion: http://stackoverflow.com/a/14401341/148962

            buffer = new byte[4];
            stream.Read(buffer, 0, buffer.Length);

            return (buffer[0] << 24) | (buffer[1] << 16) | (buffer[2] << 8) | buffer[3];
        }

        public static bool ValidSize(Size s)
        {
            return (s.Width > 0 && s.Height > 0);
        }


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

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }

        public static bool IsWindowsVistaOrGreater()
        {
            return OSVersion.Major >= 6;
        }

        public static string GetFilenameExtension(string filePath, bool includeDot = false)
        {
            string extension = "";

            if (!string.IsNullOrEmpty(filePath))
            {
                int pos = filePath.LastIndexOf('.');

                if (pos >= 0)
                {
                    extension = filePath.Substring(pos + 1);

                    if (includeDot)
                    {
                        extension = "." + extension;
                    }
                }
            }

            return extension.ToLower();
        }

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

        public static FileAttributes RemoveAttribute(FileAttributes attributes, FileAttributes attributesToRemove)
        {
            return attributes & ~attributesToRemove;
        }

        public static string[] BytesToHexadecimal(byte[] bytes)
        {
            string[] result = new string[bytes.Length];

            for (int i = 0; i < bytes.Length; i++)
            {
                result[i] = bytes[i].ToString("x2");
            }

            return result;
        }

        public static string ColorArrayToString(Color[] input)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Color c in input)
            {
                sb.Append(c.ToString() + " | ");
            }
            return sb.ToString();
        }
        public class HashCheck
        {
            public bool isRunning { get; private set; } = false;

            private CancellationTokenSource cts;

            public async Task<string> Start(string filePath, HashType hashType)
            {
                string result = null;

                if (!isRunning && !string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    isRunning = true;

                    cts = new CancellationTokenSource();
                    result = await Task.Run(() =>
                    {
                        try
                        {
                            return HashCheckThread(filePath, hashType, cts.Token);
                        }
                        catch (OperationCanceledException)
                        {
                        }

                        return null;
                    }, cts.Token);

                    isRunning = false;
                }

                return result;
            }

            public void Stop()
            {
                if (cts != null)
                {
                    cts.Cancel();
                }
            }

            private string HashCheckThread(string filePath, HashType hashType, CancellationToken ct)
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (HashAlgorithm hash = GetHashType(hashType))
                using (CryptoStream cs = new CryptoStream(stream, hash, CryptoStreamMode.Read))
                {
                    long bytesRead, totalRead = 0;
                    byte[] buffer = new byte[8192];

                    while ((bytesRead = cs.Read(buffer, 0, buffer.Length)) > 0 && !ct.IsCancellationRequested)
                    {
                        totalRead += bytesRead;
                    }

                    if (ct.IsCancellationRequested)
                    {
                        ct.ThrowIfCancellationRequested();
                    }
                    else
                    {
                        string[] hex = BytesToHexadecimal(hash.Hash);
                        return string.Concat(hex);
                    }
                }

                return null;
            }

            public HashAlgorithm GetHashType(HashType type)
            {
                switch (type)
                {
                    case HashType.MD5:
                        return new MD5CryptoServiceProvider();
                    case HashType.SHA1:
                        return new SHA1CryptoServiceProvider();
                    case HashType.SHA256:
                        return new SHA256CryptoServiceProvider();
                    case HashType.SHA384:
                        return new SHA384CryptoServiceProvider();
                    case HashType.SHA512:
                        return new SHA512CryptoServiceProvider();
                }
                return null;
            }
        }
    }
}
