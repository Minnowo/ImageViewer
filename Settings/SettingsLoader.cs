using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using ImageViewer.Helpers;

namespace ImageViewer.Settings
{
    public static class SettingsLoader
    {
        private static XmlSerializer serializer = new XmlSerializer(typeof(SettingsProfiles));

        public static void Save()
        {
            int oldIndex = InternalSettings.SettingProfiles.FindIndex(p => p.ID == InternalSettings.CurrentUserSettings.ID);

            if(oldIndex > 0)
                Helper.Move(InternalSettings.SettingProfiles, oldIndex, 0); // put cur profile at 0 for loading 

            using (TextWriter writer = new StreamWriter(InternalSettings.User_Settings_Path))
            {
                serializer.Serialize(writer, InternalSettings.SettingProfiles);
            }
        }

        public static void Load()
        {
            if (!File.Exists(InternalSettings.User_Settings_Path))
            {
                InternalSettings.SettingProfiles.Add(InternalSettings.CurrentUserSettings);
                InternalSettings.CurrentUserSettings._Binds = InternalSettings.Default_Key_Binds.ToList();
                foreach (UserControlledSettings s in InternalSettings.SettingProfiles)
                {
                    s.UpdateBinds();
                }
                return;
            }

            using (TextReader reader = new StreamReader(InternalSettings.User_Settings_Path))
            {
                try
                {
                    InternalSettings.SettingProfiles.AddRange((SettingsProfiles)serializer.Deserialize(reader));
                    InternalSettings.CurrentUserSettings = InternalSettings.SettingProfiles[0];
                }
                catch 
                {
                    InternalSettings.CurrentUserSettings = new UserControlledSettings();
                    InternalSettings.SettingProfiles.Add(InternalSettings.CurrentUserSettings);

                    Task.Run(() => {
                        MessageBox.Show(Program.mainForm,
                           InternalSettings.Error_Loading_Settings_Message,
                           InternalSettings.Error_Loading_Settings_Title,
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error);
                    });
                    
                }
            }

            foreach(UserControlledSettings s in InternalSettings.SettingProfiles)
            {
                s.UpdateBinds();
            }
        }
    }
}
