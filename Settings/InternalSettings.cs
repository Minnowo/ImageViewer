using System;
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

        public static Color DrawingBoard_Clear_Background_Color = Color.Black;
        public static Color Fill_Transparent_Color = Color.White;

        public static WebPQuality WebpQuality_Default = new WebPQuality(Format.EncodeLossy, 74, 6);

        public static bool Fit_Image_When_Maximized = true;
        public static bool Fit_Image_On_Resize = true;

        public static bool CenterChild_When_Parent_Following_Child = true;
        public static bool Fill_Transparent = false;

        public static bool Remember_Images_On_Close = true;

        public static bool High_Def_Scale_On_Zoom_Out = false;
        public static bool Use_Lite_Load_Image = true;
        public static bool Use_Fast_Invert_Color = true;
        public static bool Use_Fast_Grey_Scale = true;

        public static bool WebP_Plugin_Exists = false;

        public static bool CPU_Type_x64 = IntPtr.Size == 8;
    }
}
