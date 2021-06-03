﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using ImageViewer.Helpers;

namespace ImageViewer.Settings
{
    public static class InternalSettings
    {
        public static string FolderSelectDialog_Title_Select_a_folder = "Select a folder";

        public const string All_Files_File_Dialog = "All Files (*.*)|*.";

        public static string Color_Palette_File_Dialog = "Color Palettes (*.aco, *.lbm, *bbm, *.txt)|*.aco;*lbm;*bbm;*.txt| ACO (*.aco)|*.aco| LBM (*.lbm)|*.lbm| BBM (*.bmm)|*.bbm| TXT (*.txt)|*.txt";

        public static string Save_File_Dialog_Default = "PNG (*.png)|*.png|JPEG (*.jpg, *.jpeg, *.jpe, *.jfif)|*.jpg;*.jpeg;*.jpe;*.jfif|GIF (*.gif)|*.gif|BMP (*.bmp)|*.bmp|TIFF (*.tif, *.tiff)|*.tif;*.tiff";

        public static string WebP_File_Dialog_Option = "WebP (*.webp)|*.webp";

        public static List<string> Open_All_Image_Files_File_Dialog_Options = new List<string> { "*.png", "*.jpg", "*.jpeg", "*.jpe", "*.jfif", "*.gif", "*.bmp", "*.tif", "*.tiff" };

        #region message box 

        public static string Delete_Item_Messagebox_Title = "Delete Item?";
        public static string Delete_Item_Messagebox_Message = "Are you sure you would like to delete this item: ";

        public static string Item_Does_Not_Exist_Title = "Item not found";
        public static string Item_Does_Not_Exist_Message = "This item does not exist";

        public static string Invalid_Size_Messagebox_Title = "Invalid image size";
        public static string Invalid_Size_Messagebox_Message = "The image size is not valid";

        #endregion

        #region plugin dll paths

        public const string libwebP_x64 = "plugins\\libwebp_x64.dll";
        public const string libwebP_x86 = "plugins\\libwebp_x86.dll";

        #endregion

        #region rate limits

        public static short Fit_To_Screen_On_Resize_Limit = 250;

        public static short Dither_Threshold_Update_Limit = 1000;
        
        #endregion

        public static Font CloseButtonFont = new Font(new Font("Consolas", 10), FontStyle.Bold);

        public static double[] Grey_Scale_Multipliers = new double[] { 0.11, 0.59, 0.3};

        public static int Fill_Alpha_Less_Than
        {
            get
            {
                return fill_Alpha_Less_Than;
            }
            set
            {
                fill_Alpha_Less_Than = value.Clamp(0, 255);
            }
        }
        private static int fill_Alpha_Less_Than = 128;

        public static Color Image_Box_Back_Color = Color.Black; // going to be removed
        public static Color Fill_Transparent_Color = Color.White;

        public static Color Default_Transparent_Grid_Color = Color.Gainsboro;
        public static Color Default_Transparent_Grid_Color_Alternate = Color.White;


        public static WebPQuality WebpQuality_Default = new WebPQuality(Format.EncodeLossy, 74, 6);

        public static bool Remove_Selected_Area_On_Pan = false;

        public static bool Fit_Image_When_Maximized = true;
        public static bool Fit_Image_On_Resize = true;

        public static bool CenterChild_When_Parent_Following_Child = true;
        public static bool Fill_Transparent = false;

        public static bool Remember_Images_On_Close = true;

        public static bool Use_Lite_Load_Image = true;
        public static bool Use_Fast_Invert_Color = true;
        public static bool Use_Fast_Grey_Scale = true;
        public static bool Use_Async_Dither = true;

        public static bool WebP_Plugin_Exists = false;

        public static bool CPU_Type_x64 = IntPtr.Size == 8;


        public static void EnabledLibwebPExtension()
        {
            WebP_Plugin_Exists = true;
            Open_All_Image_Files_File_Dialog_Options.Add("*.webp");
        }
    }
}
