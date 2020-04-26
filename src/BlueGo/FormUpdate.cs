using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BlueGo
{
    public partial class FormUpdate : Form
    {
        public FormUpdate()
        {
            InitializeComponent();
        }

        public void Clean(string version, string downloadLink)
        {
            labelVersion.Text = "A new version of BlueGo is available (BlueGo " + version + ").";
            linkLabelDownloadUrl.Text = downloadLink;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void linkLabelDownloadUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabelDownloadUrl.Text);
        }
    }
}
