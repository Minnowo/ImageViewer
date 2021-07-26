using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ImageViewer.Settings;
using ImageViewer.Helpers;
namespace ImageViewer
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();

            foreach(UserControlledSettings s in InternalSettings.SettingProfiles)
            {
                cbProfiles.Items.Add(s);
            }
            cbProfiles.SelectedItem = InternalSettings.CurrentUserSettings;

            pgMain.SelectedObject = InternalSettings.CurrentUserSettings;

            FormClosing += SettingsForm_FormClosing;
        }

        private void cbProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            pgMain.SelectedObject = (UserControlledSettings)cbProfiles.SelectedItem;
            InternalSettings.CurrentUserSettings = (UserControlledSettings)cbProfiles.SelectedItem;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            UserControlledSettings newProfile = new UserControlledSettings();

            newProfile.ProfileName = "new profile " + (InternalSettings.SettingProfiles.Count + 1).ToString();

            InternalSettings.SettingProfiles.Add(newProfile);
            cbProfiles.Items.Add(newProfile);

            cbProfiles.SelectedItem = newProfile;
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (cbProfiles.Items.Count < 2)
                return;

            int indexToRemove = cbProfiles.SelectedIndex;

            InternalSettings.SettingProfiles.RemoveAt(indexToRemove);
            cbProfiles.Items.RemoveAt(indexToRemove);

            cbProfiles.SelectedIndex = 0;
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SettingsLoader.Save();
        }
    }
}
