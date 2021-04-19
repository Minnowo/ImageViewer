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

namespace ImageViewer.Helpers
{
    public static class Helper
    {
        public static readonly Version OSVersion = Environment.OSVersion.Version;
        public static readonly string[] SizeSuffixes =
                   { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

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

            return extension;
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
