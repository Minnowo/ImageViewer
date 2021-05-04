using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace ImageViewer.Settings
{
    public static class InternalSettings
    {
        public static string FolderSelectDialog_Title_Select_a_folder = "Select a folder";

        #region Paths

        public static string ImageCacheFolder = Directory.GetCurrentDirectory() + "\\imCache";

        #endregion

        #region message box 

        public static string Delete_Item_Messagebox_Title = "Delete Item?";
        public static string Delete_Item_Messagebox_Message = "Are you sure you would like to delete this item: ";

        public static string Item_Does_Not_Exist_Title = "Item not found";
        public static string Item_Does_Not_Exist_Message = "This item does not exist";

        public static string Invalid_Size_Messagebox_Title = "Invalid image size";
        public static string Invalid_Size_Messagebox_Message = "The image size is not valid";

        #endregion
        public static Font CloseButtonFont = new Font(new Font("Consolas", 10), FontStyle.Bold);

        public static double[] Grey_Scale_Multipliers = new double[] { 0.11, 0.59, 0.3};

        public static Color DrawingBoard_Clear_Background_Color = Color.Black;

        public static bool Remember_Images_On_Close = true;

        public static bool High_Def_Scale_On_Zoom_Out = false;
        public static bool Use_Lite_Load_Image = true;
        public static bool Use_Fast_Invert_Color = true;
        public static bool Use_Fast_Grey_Scale = true;
    }
}
