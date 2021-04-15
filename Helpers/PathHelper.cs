﻿using System;
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

        public static string[] OpenImageFileDialog(bool multiselect, Form form = null, string initialDirectory = null)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image files (*.png, *.jpg, *.jpeg, *.jpe, *.jfif, *.gif, *.bmp, *.tif, *.tiff)|*.png;*.jpg;*.jpeg;*.jpe;*.jfif;*.gif;*.bmp;*.tif;*.tiff|" +
                    "PNG (*.png)|*.png|JPEG (*.jpg, *.jpeg, *.jpe, *.jfif)|*.jpg;*.jpeg;*.jpe;*.jfif|GIF (*.gif)|*.gif|BMP (*.bmp)|*.bmp|TIFF (*.tif, *.tiff)|*.tif;*.tiff";

                ofd.Multiselect = multiselect;

                if (!string.IsNullOrEmpty(initialDirectory))
                {
                    ofd.InitialDirectory = initialDirectory;
                }

                if (ofd.ShowDialog(form) == DialogResult.OK)
                {
                    return ofd.FileNames;
                }
            }

            return null;
        }
    }
}