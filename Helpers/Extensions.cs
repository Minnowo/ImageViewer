﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using ImageViewer.structs;
namespace ImageViewer.Helpers
{
    public static class Extensions
    {

        #region string Extensions

        public static string Truncate(this string str, int maxLength)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > maxLength)
            {
                return str.Substring(0, maxLength);
            }

            return str;
        }

        public static string[] OnlyValidFiles(this string[] str)
        {
            if (str == null || str.Length < 1)
                return null;
            List<string> newA = str.ToList();

            for(int i = 0; i < newA.Count; i++)
            {
                if (!Helper.IsValidFilePath(newA[i]) || !File.Exists(newA[i]))
                    newA.RemoveAt(i);
                else
                    newA[i] = new FileInfo(newA[i]).FullName; // force absolute paths
            }
            return newA.ToArray();
        }

        #endregion

        #region Bitmap / Image Extensions

        public static Bitmap Copy(this Image image)
        {
            Bitmap copy;

            copy = new Bitmap(image.Size.Width, image.Size.Height, PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(copy))
            {
                g.Clear(Color.Transparent);
                g.PageUnit = GraphicsUnit.Pixel;
                g.DrawImage(image, new Rectangle(Point.Empty, image.Size));
            }

            return copy;
        }

        public static Bitmap Resize(this Bitmap bmp, ResizeImage ri)
        {
            using (Bitmap b = bmp)
            {
                return ImageHelper.ResizeImage(b, ri);
            }
        }

        public static byte[] ToByteArray(this Image x)
        {
            ImageConverter _imageConverter = new ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(x, typeof(byte[]));
            return xByte;
        }

        #endregion

        #region byte[] Extensions

        public static string ReturnStrHash(this byte[] crypto)
        {
            StringBuilder hash = new StringBuilder();
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        #endregion

        public static IEnumerable<T> OrderByNatural<T>(this IEnumerable<T> items, Func<T, string> selector, StringComparer stringComparer = null)
        {
            Regex regex = new Regex(@"\d+", RegexOptions.Compiled);

            int maxDigits = items
                          .SelectMany(i => regex.Matches(selector(i)).Cast<Match>().Select(digitChunk => (int?)digitChunk.Value.Length))
                          .Max() ?? 0;

            return items.OrderBy(i => regex.Replace(selector(i), match => match.Value.PadLeft(maxDigits, '0')), stringComparer ?? StringComparer.CurrentCulture);
        }

        public static byte ToByte(this int input)
        {
            return (byte)input.Clamp(0, 255);
        }

        public static void ShowError(this Exception e)
        {
            MessageBox.Show(Program.mainForm ,e.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }        

        public static void ShowFullScreen(this Control ctl)
        {
            // Setup host form to be full screen
            Form host = new Form();

            host.FormBorderStyle = FormBorderStyle.None;
            host.WindowState = FormWindowState.Maximized;
            host.ShowInTaskbar = false;

            // Save properties of control
            Point loc = ctl.Location;
            DockStyle dock = ctl.Dock;
            Control parent = ctl.Parent;
            Control form = parent;

            while (!(form is Form))
                form = form.Parent;

            // Move control to host
            ctl.Parent = host;
            ctl.Location = Point.Empty;
            ctl.Dock = DockStyle.Fill;

            // Setup event handler to restore control back to form
            host.FormClosing += delegate {
                ctl.Parent = parent;
                ctl.Dock = dock;
                ctl.Location = loc;
                form.Show();
            };

            host.KeyPreview = true;

            // this line feels like cheating lmao bouta intercept some key up events
            host.KeyUp += (KeyEventHandler)((s, e) => {
                Program.mainForm.FullscreenKeyUpCallback(e);

                // Exit full screen with escape key
                if (e.KeyCode == Keys.Escape) host.Close();
            });

            // And go full screen
            host.Show();
            form.Hide();
        }

        public static bool Toggle(this bool input)
        {
            return !input;
        }

        public static T Clamp<T>(this T input, T min, T max) where T : IComparable<T>
        {
            return MathHelper.Clamp(input, min, max);
        }

        public static T ClampMin<T>(this T input, T min) where T : IComparable<T>
        {
            return MathHelper.ClampMin(input, min);
        }

        public static T ClampMax<T>(this T input,T max) where T : IComparable<T>
        {
            return MathHelper.ClampMax(input, max);
        }

        public static T CloneSafe<T>(this T obj) where T : class, ICloneable
        {
            if (obj == null)
                return null;

            try
            {
                    return obj.Clone() as T;
            }
            catch {}

            return null;
        }

        public static void Move<T>(this List<T> list, int oldIndex, int newIndex)
        {
            Helper.Move(list, oldIndex, newIndex);
        }

        public static void Move<T>(this List<T> list, T item, int newIndex)
        {
            Helper.Move(list, item, newIndex);
        }
    }
}
