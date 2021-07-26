using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using ImageViewer.Helpers;
using ImageViewer.structs;
using System.Xml.Serialization;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;

namespace ImageViewer.Settings
{
    public static class InternalSettings
    {
        public const string User_Settings_Path = "usrConfig.xml";
        public static string Temp_Image_Folder = Path.Combine(AppContext.BaseDirectory, "tmp");


        public static List<string> Readable_Image_Formats_Dialog_Options = new List<string>
        { "*.png", "*.jpg", "*.jpeg", "*.jpe", "*.jfif", "*.gif", "*.bmp", "*.tif", "*.tiff" };

        public static List<string> Readable_Image_Formats = new List<string>()
        { "png", "jpg", "jpeg", "jpe", "jfif", "gif", "bmp", "tif", "tiff" };

        public static List<string> Readable_Color_Palette_Dialog_Options = new List<string>
        { "*.aco", "*.lbm", "*.bmm", "*.txt" };


        public const string Folder_Select_Dialog_Title = "Select a folder";
        public const string Image_Select_Dialog_Title = "Select an image";
        public const string Save_File_Dialog_Title = "Save Item";
        public const string Move_File_Dialog_Title = "Move Item";

        public const string All_Files_File_Dialog = "All Files (*.*)|*.";

        public static string All_Image_Files_File_Dialog = string.Format(
            "Graphic Types ({0})|{1}",
            string.Join(", ", Readable_Image_Formats_Dialog_Options),
            string.Join(";", Readable_Image_Formats_Dialog_Options));

        public static string All_Palette_Files_File_Dialog = string.Format(
           "Color Palettes ({0})|{1}",
           string.Join(", ", Readable_Color_Palette_Dialog_Options),
           string.Join(";", Readable_Color_Palette_Dialog_Options));

        // image formats
        public const string PNG_File_Dialog = "PNG (*.png)|*.png";
        public const string BMP_File_Dialog = "BMP (*.bmp)|*.bmp";
        public const string GIF_File_Dialog = "GIF (*.gif)|*.gif";
        public const string WEBP_File_Dialog = "WEBP (*.webp)|*.webp";
        public const string TIFF_File_Dialog = "TIFF (*.tif, *.tiff)|*.tif;*.tiff";
        public const string JPEG_File_Dialog = "JPEG (*.jpg, *.jpeg, *.jpe, *.jfif)|*.jpg;*.jpeg;*.jpe;*.jfif";

        // color palettes
        public const string ACO_File_Dialog = "ACO (*.aco)|*.aco";
        public const string LBM_File_Dialog = "LBM (*.lbm)|*.lbm";
        public const string BBM_File_Dialog = "BBM (*.bmm)|*.bbm";
        public const string TXT_File_Dialog = "TXT (*.txt)|*.txt";


        public static string Image_Dialog_Filters = string.Join("|",
            new string[]
            {
                PNG_File_Dialog,
                JPEG_File_Dialog,
                BMP_File_Dialog,
                TIFF_File_Dialog,
                GIF_File_Dialog
            });

        public static string Color_Palette_Dialog_Filters = string.Join("|",
            new string[]
            {
                ACO_File_Dialog,
                LBM_File_Dialog,
                BBM_File_Dialog,
                TXT_File_Dialog,
            });

        #region message box 

        public const string Delete_Item_Messagebox_Title = "Delete Item?";
        public const string Delete_Item_Messagebox_Message = "Are you sure you would like to delete this item: ";

        public const string Item_Does_Not_Exist_Title = "Item not found";
        public const string Item_Does_Not_Exist_Message = "This item does not exist";

        public const string Invalid_Size_Messagebox_Title = "Invalid image size";
        public const string Invalid_Size_Messagebox_Message = "The image size is not valid";

        public const string Unable_To_Invert_Image_Title = "Unable to invert colors.";
        public const string Unable_To_Invert_Image_Message = "There was an error when trying to invert the colors of this image, most likely the pixel format is not 32bppArgb.";

        public const string Unable_To_Convert_To_Grey_Image_Title = "Unable to convert to grey.";
        public const string Unable_To_Convert_To_Grey_Image_Message = "There was an error when trying to convert to grey, most likely the pixel format is not 32bppArgb.";

        public const string Unable_To_Convert_To_Fill_Transparent_Title = "Unable to fill transparent pixels.";
        public const string Unable_To_Convert_To_Fill_Transparent_Message = "There was an error when trying to fill transparent pixels, most likely the pixel format is not 32bppArgb.";

        public const string No_Animation_Frames_Found_Title = "No animation frames found.";
        public const string No_Animation_Frames_Found_Message = "Unable to detect any animation frames to export.";

        public const string Error_Loading_Settings_Title = "Unable to load settings";
        public const string Error_Loading_Settings_Message = "There was an error trying to load the settings, the file will be deleted / overwritten when the program exits";

        #endregion

        #region plugin dll paths

        public const string libwebP_x64 = "plugins\\libwebp_x64.dll";
        public const string libwebP_x86 = "plugins\\libwebp_x86.dll";

        #endregion

        #region rate limits

        public static short Fit_To_Screen_On_Resize_Limit = 250;

        public static short Dither_Threshold_Update_Limit = 1000;

        public static short Folder_Watcher_Resort_Timer_Limit = 5000;

        #endregion


        public static Font CloseButtonFont = new Font(new Font("Consolas", 10), FontStyle.Bold);

        public static double[] Grey_Scale_Multipliers = new double[] { 0.11, 0.59, 0.3};

        public static Size TSMI_Generated_Icon_Size = new Size(16, 16);

        public static int Fill_Alpha_Less_Than
        {
            get { return fill_Alpha_Less_Than; }
            set { fill_Alpha_Less_Than = value.Clamp(0, 255); }
        }
        private static int fill_Alpha_Less_Than = 128;

        public static Color Image_Box_Back_Color
        {
            get { return CurrentUserSettings.Image_Box_Back_Color; }
            set { CurrentUserSettings.Image_Box_Back_Color = value; }
        }

        public static ImgFormat Default_Image_Format
        {
            get { return CurrentUserSettings.Default_Image_Format; }
            set { CurrentUserSettings.Default_Image_Format = value; }
        }

        public static InterpolationMode Default_Interpolation_Mode
        {
            get { return CurrentUserSettings.Default_Interpolation_Mode; }
            set { CurrentUserSettings.Default_Interpolation_Mode = value; }
        }

        public static WebPQuality WebpQuality_Default
        {
            get { return CurrentUserSettings.WebpQuality_Default; }
            set { CurrentUserSettings.WebpQuality_Default = value; }
        }

        public static Color Default_Transparent_Grid_Color
        {
            get { return CurrentUserSettings.Default_Transparent_Grid_Color; }
            set { CurrentUserSettings.Default_Transparent_Grid_Color = value; }
        }
        public static Color Default_Transparent_Grid_Color_Alternate
        {
            get { return CurrentUserSettings.Default_Transparent_Grid_Color_Alternate; }
            set { CurrentUserSettings.Default_Transparent_Grid_Color_Alternate = value; }
        }

        public static Color Current_Transparent_Grid_Color
        {
            get { return CurrentUserSettings.Current_Transparent_Grid_Color; }
            set { CurrentUserSettings.Current_Transparent_Grid_Color = value; }
        }
        public static Color Current_Transparent_Grid_Color_Alternate
        {
            get { return CurrentUserSettings.Current_Transparent_Grid_Color_Alternate; }
            set { CurrentUserSettings.Current_Transparent_Grid_Color_Alternate = value; }
        }

        public static Color Fill_Transparency_On_Copy_Color
        {
            get { return CurrentUserSettings.Fill_Transparency_On_Copy_Color; }
            set { CurrentUserSettings.Fill_Transparency_On_Copy_Color = value; }
        }

        public static bool Show_Default_Transparent_Colors
        {
            get { return CurrentUserSettings.Show_Default_Transparent_Colors; }
            set { CurrentUserSettings.Show_Default_Transparent_Colors = value; }
        }
        public static bool Only_Show_Transparent_Color_1
        {
            get { return CurrentUserSettings.Only_Show_Transparent_Color_1; }
            set { CurrentUserSettings.Only_Show_Transparent_Color_1 = value; }
        }
        public static bool Show_Pixel_Grid
        {
            get { return CurrentUserSettings.Show_Pixel_Grid; }
            set { CurrentUserSettings.Show_Pixel_Grid = value; }
        }
        public static bool Remove_Selected_Area_On_Pan
        {
            get { return CurrentUserSettings.Remove_Selected_Area_On_Pan; }
            set { CurrentUserSettings.Remove_Selected_Area_On_Pan = value; }
        }

        public static bool Fit_Image_When_Unmaximized
        {
            get { return CurrentUserSettings.Fit_Image_When_Unmaximized; }
            set { CurrentUserSettings.Fit_Image_When_Unmaximized = value; }
        }
        public static bool Fit_Image_When_Maximized
        {
            get { return CurrentUserSettings.Fit_Image_When_Maximized; }
            set { CurrentUserSettings.Fit_Image_When_Maximized = value; }
        }
        public static bool Fit_Image_On_Resize
        {
            get { return CurrentUserSettings.Fit_Image_On_Resize; }
            set { CurrentUserSettings.Fit_Image_On_Resize = value; }
        }

        public static bool Lock_Selection_To_Image
        {
            get { return CurrentUserSettings.Lock_Selection_To_Image; }
            set { CurrentUserSettings.Lock_Selection_To_Image = value; }
        }

        public static bool CenterChild_When_Parent_Following_Child
        {
            get { return CurrentUserSettings.CenterChild_When_Parent_Following_Child; }
            set { CurrentUserSettings.CenterChild_When_Parent_Following_Child = value; }
        }
        public static bool Parent_Follow_Child
        {
            get { return CurrentUserSettings.Parent_Follow_Child; }
            set { CurrentUserSettings.Parent_Follow_Child = value; }
        }

        public static bool Watch_Directory
        {
            get { return CurrentUserSettings.Watch_Directory; }
            set { CurrentUserSettings.Watch_Directory = value; }
        }
        public static bool Open_Explorer_After_Export
        {
            get { return CurrentUserSettings.Open_Explorer_After_Export; }
            set { CurrentUserSettings.Open_Explorer_After_Export = value; }
        }
        public static bool Open_Explorer_After_SaveAs
        {
            get { return CurrentUserSettings.Open_Explorer_After_SaveAs; }
            set { CurrentUserSettings.Open_Explorer_After_SaveAs = value; }
        }

        public static bool Use_Async_Dither
        {
            get { return CurrentUserSettings.Use_Async_Dither; }
            set { CurrentUserSettings.Use_Async_Dither = value; }
        }

        // because of how we load the image there is extra memory that doesn't get disposed
        // and calling GC.Collect removes that, but since garbage collection can cause problems
        // gonna allow the user to disable it as the please 
        // i also just hate when stuff doesn't get cleared from meory so i like to GC a lot
        public static bool Garbage_Collect_On_Image_Unload 
        { 
            get { return CurrentUserSettings.Garbage_Collect_On_Image_Unload; }
            set { CurrentUserSettings.Garbage_Collect_On_Image_Unload = value; }
        }
        public static bool Garbage_Collect_On_Dither_Form_Cancel
        {
            get { return CurrentUserSettings.Garbage_Collect_On_Dither_Form_Cancel; }
            set { CurrentUserSettings.Garbage_Collect_On_Dither_Form_Cancel = value; }
        }
        public static bool Garbage_Collect_After_Unmanaged_Image_Manipulation
        {
            get { return CurrentUserSettings.Garbage_Collect_After_Unmanaged_Image_Manipulation; }
            set { CurrentUserSettings.Garbage_Collect_After_Unmanaged_Image_Manipulation = value; }
        }
        public static bool Garbage_Collect_After_Gif_Export
        {
            get { return CurrentUserSettings.Garbage_Collect_After_Gif_Export; }
            set { CurrentUserSettings.Garbage_Collect_After_Gif_Export = value; }
        }

        public static bool Delete_Temp_Directory
        {
            get { return CurrentUserSettings.Delete_Temp_Directory_On_Close; }
            set { CurrentUserSettings.Delete_Temp_Directory_On_Close = value; }
        }

        public static bool Use_Alternate_Copy_Method
        {
            get { return CurrentUserSettings.Use_Alternate_Copy_Method; }
            set { CurrentUserSettings.Use_Alternate_Copy_Method = value; }
        }

        public static bool Replace_Transparency_On_Copy
        {
            get { return CurrentUserSettings.Replace_Transparency_On_Copy; }
            set { CurrentUserSettings.Replace_Transparency_On_Copy = value; }
        }
        
        public static bool Always_On_Top
        {
            get { return CurrentUserSettings.Always_On_Top; }
            set { CurrentUserSettings.Always_On_Top = value; }
        }

        public static bool WebP_Plugin_Exists = false;

        public static bool CPU_Type_x64 = IntPtr.Size == 8;

        public static UserControlledSettings CurrentUserSettings = new UserControlledSettings();
        public static SettingsProfiles SettingProfiles = new SettingsProfiles();


        public static void UpdateDialogFilters()
        {
           All_Image_Files_File_Dialog = string.Format(
            "Graphic types ({0})|{1}",
            string.Join(", ", Readable_Image_Formats_Dialog_Options),
            string.Join(";", Readable_Image_Formats_Dialog_Options));

            Image_Dialog_Filters = string.Join("|",
                new string[]
                {
                    PNG_File_Dialog,
                    JPEG_File_Dialog,
                    BMP_File_Dialog,
                    TIFF_File_Dialog,
                    GIF_File_Dialog,
                });

            if (WebP_Plugin_Exists)
                Image_Dialog_Filters += "|" + WEBP_File_Dialog;
        }

        public static void EnableWebPIfPossible()
        {
            if (CPU_Type_x64)
            {
                if (File.Exists(libwebP_x64))
                {
                    WebP_Plugin_Exists = true;
                    Readable_Image_Formats_Dialog_Options.Add("*.webp");
                    Readable_Image_Formats.Add("webp");
                    UpdateDialogFilters();
                }
            }
            else
            {
                if (File.Exists(libwebP_x86))
                {
                    WebP_Plugin_Exists = true;
                    Readable_Image_Formats_Dialog_Options.Add("*.webp");
                    Readable_Image_Formats.Add("webp");
                    UpdateDialogFilters();
                }
            }
            
        }
    }

    [Serializable()]
    [XmlRoot(ElementName = "SettingsProfiles", Namespace = "", IsNullable = false)]
    public class SettingsProfiles : List<UserControlledSettings>
    {
        public SettingsProfiles()
        {

        }

        public SettingsProfiles(List<UserControlledSettings> items)
        {
            this.AddRange(items);
        }
    }
    

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class UserControlledSettings
    {
        [Browsable(false)]
        [XmlIgnore]
        public Guid ID = Guid.NewGuid();

        [Description("The name of the profile."), DisplayName("Name")]
        public string ProfileName { get; set; } = "Nil";


        [Description("Should the garbage collecter be called after every image unloads."), DisplayName("Garbage Collect On Image Unload")]
        public bool Garbage_Collect_On_Image_Unload { get; set; } = false;


        [Description("Should the garbage collecter be called after the dither form."), DisplayName("Garbage Collect After Dither")]
        public bool Garbage_Collect_On_Dither_Form_Cancel { get; set; } = true;


        [Description("Should the garbage collecter be called after unmanaged image manipulation."), DisplayName("Garbage Collect After Unmanaged Image Manipulation")]
        public bool Garbage_Collect_After_Unmanaged_Image_Manipulation { get; set; } = true;


        [Description("Should the garbage collecter be called after exporting a gif."), DisplayName("Garbage Collect After Gif Export")]
        public bool Garbage_Collect_After_Gif_Export { get; set; } = true;



        [Description("Should the tmp directory be deleted when the application closes."), DisplayName("Delete Temp Dir On Exit")]
        public bool Delete_Temp_Directory_On_Close { get; set; } = true;


        [Description("Should the mainform be a topmost window."), DisplayName("Always On Top")]
        public bool Always_On_Top { get; set; } = false;


        [Description("Should the image backing only show the default colors."), DisplayName("Show Default Image Backing")]
        public bool Show_Default_Transparent_Colors { get; set; } = false;


        [Description("Should the image backing be only color 1."), DisplayName("Show Color 1 As Image Backing")]
        public bool Only_Show_Transparent_Color_1 { get; set; } = false;


        [Description("Should the pixel grid be shown."), DisplayName("Display Pixel Grid")]
        public bool Show_Pixel_Grid { get; set; } = true;


        [Description("Should the selected area be removed when panning."), DisplayName("Deselect On Pan")]
        public bool Remove_Selected_Area_On_Pan { get; set; } = false;



        [Description("Should the current image be fit to the screen when maximized."), DisplayName("Fit Image To Screen When Maximized")]
        public bool Fit_Image_When_Maximized { get; set; } = true;


        [Description("Should the current image be fit to the screen when unmaximized."), DisplayName("Fit Image To Screen When Un-Maximized")]
        public bool Fit_Image_When_Unmaximized { get; set; } = true;


        [Description("Should the current image be fit to the screen when the window is resized."), DisplayName("Fit Image To Screen On Window Resize")]
        public bool Fit_Image_On_Resize { get; set; } = true;


        [Description("Should the selection region be able to leave the image."), DisplayName("Lock Selection To Image")]
        public bool Lock_Selection_To_Image { get; set; } = true;



        [Description("Should the parent window follow children that take control."), DisplayName("Parent Follow Children")]
        public bool Parent_Follow_Child { get; set; } = true;


        [Description("Should the child window be centered on the parent when parent following child."), DisplayName("Center Child When Parent Following")]
        public bool CenterChild_When_Parent_Following_Child { get; set; } = true;



        [Description("Should the current image directory be watched for changed."), DisplayName("Watch Current Image Directory")]
        public bool Watch_Directory { get; set; } = true;


        [Description("Should explorer be opened to the file after export."), DisplayName("Open Explorer After Exporting")]
        public bool Open_Explorer_After_Export { get; set; } = true;


        [Description("Should explorer be opened to the file after save as."), DisplayName("Open Explorer After Saving")]
        public bool Open_Explorer_After_SaveAs { get; set; } = true;



        [Description("Should async be used when dithering."), DisplayName("Use Async When Dithering")]
        public bool Use_Async_Dither { get; set; } = true;


        [Description("Copy image in a way that keeps transparency."), DisplayName("Alternate Image Copy Method (keeps transparency)")]
        public bool Use_Alternate_Copy_Method 
        {
            get { return use_Alternate_Copy_Method; }
            set 
            { 
                use_Alternate_Copy_Method = value;
                if (value)
                    Replace_Transparency_On_Copy = false;
            }
        }
        [Browsable(false)]
        [XmlIgnore]
        private bool use_Alternate_Copy_Method  = true;

        [Description("Fill transparency when copying image."), DisplayName("Fill Transparency On Copy")]
        public bool Replace_Transparency_On_Copy
        {
            get { return replace_Transparency_On_Copy; }
            set
            {
                replace_Transparency_On_Copy = value;
                if (value)
                    use_Alternate_Copy_Method = false;
            }
        }
        [Browsable(false)]
        [XmlIgnore]
        private bool replace_Transparency_On_Copy  = false;


        [XmlIgnore]
        [Description("The color that fills the transparent pixels when copying an image."), DisplayName("Fill Transparency On Copy Color")]
        public Color Fill_Transparency_On_Copy_Color { get; set; } = Color.White;


        [XmlIgnore]
        [Description("The default image format."), DisplayName("Default Image Format")]
        public ImgFormat Default_Image_Format
        {
            get { return default_Image_Format; }
            set
            {
                if (value == ImgFormat.webp && !InternalSettings.WebP_Plugin_Exists)
                {
                    MessageBox.Show("The webp plugin dll needed to support webp images do not exist visit https://www.dll-files.com/libwebp.dll.html to download the dll, it must be places in \\plugins\\ as libwebp_x64.dll or libwebp_x86.dll depending on your application version");
                    return;
                }

                default_Image_Format = value;
            }
        }
        [XmlIgnore]
        [Browsable(false)]
        private ImgFormat default_Image_Format = ImgFormat.png;


        [XmlIgnore]
        [Description("The default interpolation mode."), DisplayName("Interpolation Mode")]
        public InterpolationMode Default_Interpolation_Mode { get; set; } = InterpolationMode.NearestNeighbor;


        [Browsable(false)]
        [XmlElement("Default_Image_Format")]
        public int WebpQuality_DefaultAsInt
        {
            get { return (int)Default_Image_Format; }
            set { Default_Image_Format = (ImgFormat)value.Clamp(0,5); }
        }


        [Browsable(false)]
        [XmlElement("Default_Interpolation_Mode")]
        public int Default_Interpolation_ModeAsInt
        {
            get { return (int)Default_Interpolation_Mode; }
            set { Default_Interpolation_Mode = (InterpolationMode)value; }
        }


        [XmlIgnore]
        [Description("The default webp quality"), DisplayName("Default Webp Quality")]
        public WebPQuality WebpQuality_Default { get; set; } = new WebPQuality(WebpEncodingFormat.EncodeLossy, 74, 6);



        [Browsable(false)]
        [XmlElement("WebpQuality_Default")]
        public int WebpQuality_DefaultAsDecimal
        {
            get { return WebpQuality_Default.ToDecimal(); }
            set { WebpQuality_Default = WebPQuality.FromDecimal(value); }
        }

        [XmlIgnore]
        [Description("The back color of the image display."), DisplayName("Image Display Back Color")]
        public Color Image_Box_Back_Color { get; set; } = Color.Black;


        [XmlIgnore]
        [Description("The default transparent grid color 1."), DisplayName("Default Image Backing 1")]
        public Color Default_Transparent_Grid_Color { get; set; } = Color.Gainsboro;


        [XmlIgnore]
        [Description("The default transparent grid color 2."), DisplayName("Default Image Backing 2")]
        public Color Default_Transparent_Grid_Color_Alternate { get; set; } = Color.White;


        [XmlIgnore]
        [Description("The current transparent grid color 1."), DisplayName("Current Image Backing 1")]
        public Color Current_Transparent_Grid_Color { get; set; } = Color.Gainsboro;


        [XmlIgnore]
        [Description("The current transparent grid color 2."), DisplayName("Current Image Backing 2")]
        public Color Current_Transparent_Grid_Color_Alternate { get; set; } = Color.White;



        [Browsable(false)]
        [XmlElement("Image_Box_Back_Color")]
        public int BackColorAsDecimal
        {
            get { return ColorHelper.ColorToDecimal(Image_Box_Back_Color, ColorFormat.ARGB); }
            set { Image_Box_Back_Color = ColorHelper.DecimalToColor(value, ColorFormat.ARGB); }
        }

        [Browsable(false)]
        [XmlElement("Default_Transparent_Grid_Color")]
        public int Default_Transparent_Grid_ColorAsDecimal
        {
            get { return ColorHelper.ColorToDecimal(Default_Transparent_Grid_Color, ColorFormat.ARGB); }
            set { Default_Transparent_Grid_Color = ColorHelper.DecimalToColor(value, ColorFormat.ARGB); }
        }

        [Browsable(false)]
        [XmlElement("Default_Transparent_Grid_Color_Alternate")]
        public int Default_Transparent_Grid_Color_AlternateAsDecimal
        {
            get { return ColorHelper.ColorToDecimal(Default_Transparent_Grid_Color_Alternate, ColorFormat.ARGB); }
            set { Default_Transparent_Grid_Color_Alternate = ColorHelper.DecimalToColor(value, ColorFormat.ARGB); }
        }

        [Browsable(false)]
        [XmlElement("Current_Transparent_Grid_Color")]
        public int Current_Transparent_Grid_ColorAsDecimal
        {
            get { return ColorHelper.ColorToDecimal(Current_Transparent_Grid_Color, ColorFormat.ARGB); }
            set { Current_Transparent_Grid_Color = ColorHelper.DecimalToColor(value, ColorFormat.ARGB); }
        }

        [Browsable(false)]
        [XmlElement("Current_Transparent_Grid_Color_Alternate")]
        public int Current_Transparent_Grid_Color_AlternateAsDecimal
        {
            get { return ColorHelper.ColorToDecimal(Current_Transparent_Grid_Color_Alternate, ColorFormat.ARGB); }
            set { Current_Transparent_Grid_Color_Alternate = ColorHelper.DecimalToColor(value, ColorFormat.ARGB); }
        }

        [Browsable(false)]
        [XmlElement("Fill_Transparency_On_Copy_Color")]
        public int Fill_Transparency_On_Copy_ColorAsDecimal
        {
            get { return ColorHelper.ColorToDecimal(Fill_Transparency_On_Copy_Color, ColorFormat.ARGB); }
            set { Fill_Transparency_On_Copy_Color = ColorHelper.DecimalToColor(value, ColorFormat.ARGB); }
        }

        public override string ToString()
        {
            return this.ProfileName;
        }
    }
}
