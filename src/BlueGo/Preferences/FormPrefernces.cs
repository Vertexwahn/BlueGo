using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BlueGo.GUI
{
    public partial class FormPrefernces : Form
    {
        public FormPrefernces()
        {
            InitializeComponent();
        }

        public void Clean()
        {
            // E-Mail-Report Tab
            textBoxSmtpServer.Text = PreferencesManager.Instance.SmtpServer;
            textBoxPort.Text = PreferencesManager.Instance.Port;
            textBoxUser.Text = PreferencesManager.Instance.User;
            textBoxPassword.Text = PreferencesManager.Instance.Password;
            checkBoxSendReportEmail.Checked = PreferencesManager.Instance.SendReportEmail;
            textBoxSubscriber.Text = PreferencesManager.Instance.Subscriber;
            textBoxApplicationDownloadFolder.Text = PreferencesManager.Instance.ApplicationDownloadFolder;
            textBoxThirdPartyLibraryDownloadFolder.Text = PreferencesManager.Instance.ThirdPartyDownloadFolder;
            textBoxVS2010Location.Text = PreferencesManager.Instance.VS2010Location;
            textBoxVS2012Location.Text = PreferencesManager.Instance.VS2012Location;
            textBoxVS2013Location.Text = PreferencesManager.Instance.VS2013Location;
            textBoxCMakeExeLocation.Text = PreferencesManager.Instance.CMakeExeLocation;
            textBoxMSBuildExeLocation.Text = PreferencesManager.Instance.MSBuildExeLocation;

            UpdateGUI();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            // read input data
            PreferencesManager.Instance.SmtpServer = textBoxSmtpServer.Text;
            PreferencesManager.Instance.Port = textBoxPort.Text;
            PreferencesManager.Instance.User = textBoxUser.Text;
            PreferencesManager.Instance.Password = textBoxPassword.Text;
            PreferencesManager.Instance.Subscriber = textBoxSubscriber.Text;
            PreferencesManager.Instance.ApplicationDownloadFolder = textBoxApplicationDownloadFolder.Text;
            PreferencesManager.Instance.ThirdPartyDownloadFolder = textBoxThirdPartyLibraryDownloadFolder.Text;
            PreferencesManager.Instance.VS2010Location = textBoxVS2010Location.Text;
            PreferencesManager.Instance.VS2012Location = textBoxVS2012Location.Text;
            PreferencesManager.Instance.VS2013Location = textBoxVS2013Location.Text;
            PreferencesManager.Instance.CMakeExeLocation = textBoxCMakeExeLocation.Text;
            PreferencesManager.Instance.MSBuildExeLocation = textBoxMSBuildExeLocation.Text;

            Hide();
        }

        private void checkBoxSendReportEmail_CheckedChanged(object sender, EventArgs e)
        {
            PreferencesManager.Instance.SendReportEmail = checkBoxSendReportEmail.Checked;

            UpdateGUI();
        }

        private void UpdateGUI()
        {
            if (checkBoxSendReportEmail.Checked)
            {
                textBoxSmtpServer.Enabled = true;
                textBoxPort.Enabled = true;
                textBoxUser.Enabled = true;
                textBoxPassword.Enabled = true;
            }
            else
            {
                textBoxSmtpServer.Enabled = false;
                textBoxPort.Enabled = false;
                textBoxUser.Enabled = false;
                textBoxPassword.Enabled = false;
            }
        }

        private void buttonSelectApplicationFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxApplicationDownloadFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void buttonSelectThirdPartyLibraryFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxThirdPartyLibraryDownloadFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void buttonSelectVS2010Folder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxVS2010Location.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void buttonSelectVS2012Folder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxVS2012Location.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void buttonSelectVS2013Folder_Click(object sender, System.EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxVS2013Location.Text = folderBrowserDialog1.SelectedPath;
            }
        }


        private void checkBoxDownloadThirdPartyLibEveryTime_CheckedChanged(object sender, EventArgs e)
        {
            PreferencesManager.Instance.DownloadThirdPartyLibEveryTime = checkBoxDownloadThirdPartyLibEveryTime.Checked;
        }

        private void buttonSelectCMakeExeLocation_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxCMakeExeLocation.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void buttonSelectMSBuildExeLocation_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxMSBuildExeLocation.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
