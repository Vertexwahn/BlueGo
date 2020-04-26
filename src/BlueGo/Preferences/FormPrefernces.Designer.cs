namespace BlueGo.GUI
{
    partial class FormPrefernces
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPrefernces));
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkBoxDownloadThirdPartyLibEveryTime = new System.Windows.Forms.CheckBox();
            this.buttonSelectApplicationFolder = new System.Windows.Forms.Button();
            this.buttonSelectThirdPartyLibraryFolder = new System.Windows.Forms.Button();
            this.textBoxThirdPartyLibraryDownloadFolder = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxApplicationDownloadFolder = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxSmtpServer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxUser = new System.Windows.Forms.TextBox();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.checkBoxSendReportEmail = new System.Windows.Forms.CheckBox();
            this.textBoxSubscriber = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControlGUI = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.buttonSelectVS2013Folder = new System.Windows.Forms.Button();
            this.buttonSelectVS2012Folder = new System.Windows.Forms.Button();
            this.buttonSelectVS2010Folder = new System.Windows.Forms.Button();
            this.textBoxVS2013Location = new System.Windows.Forms.TextBox();
            this.textBoxVS2012Location = new System.Windows.Forms.TextBox();
            this.textBoxVS2010Location = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.buttonSelectMSBuildExeLocation = new System.Windows.Forms.Button();
            this.textBoxMSBuildExeLocation = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.buttonSelectCMakeExeLocation = new System.Windows.Forms.Button();
            this.textBoxCMakeExeLocation = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage1.SuspendLayout();
            this.tabPageSettings.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControlGUI.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Location = new System.Drawing.Point(284, 257);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(72, 27);
            this.buttonOk.TabIndex = 248;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(362, 257);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(72, 27);
            this.buttonCancel.TabIndex = 247;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkBoxDownloadThirdPartyLibEveryTime);
            this.tabPage1.Controls.Add(this.buttonSelectApplicationFolder);
            this.tabPage1.Controls.Add(this.buttonSelectThirdPartyLibraryFolder);
            this.tabPage1.Controls.Add(this.textBoxThirdPartyLibraryDownloadFolder);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.textBoxApplicationDownloadFolder);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(427, 226);
            this.tabPage1.TabIndex = 1;
            this.tabPage1.Text = "Download Folder";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkBoxDownloadThirdPartyLibEveryTime
            // 
            this.checkBoxDownloadThirdPartyLibEveryTime.AutoSize = true;
            this.checkBoxDownloadThirdPartyLibEveryTime.Checked = true;
            this.checkBoxDownloadThirdPartyLibEveryTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDownloadThirdPartyLibEveryTime.Location = new System.Drawing.Point(12, 93);
            this.checkBoxDownloadThirdPartyLibEveryTime.Name = "checkBoxDownloadThirdPartyLibEveryTime";
            this.checkBoxDownloadThirdPartyLibEveryTime.Size = new System.Drawing.Size(215, 17);
            this.checkBoxDownloadThirdPartyLibEveryTime.TabIndex = 6;
            this.checkBoxDownloadThirdPartyLibEveryTime.Text = "Download third party libraries every time ";
            this.checkBoxDownloadThirdPartyLibEveryTime.UseVisualStyleBackColor = true;
            this.checkBoxDownloadThirdPartyLibEveryTime.CheckedChanged += new System.EventHandler(this.checkBoxDownloadThirdPartyLibEveryTime_CheckedChanged);
            // 
            // buttonSelectApplicationFolder
            // 
            this.buttonSelectApplicationFolder.Location = new System.Drawing.Point(380, 17);
            this.buttonSelectApplicationFolder.Name = "buttonSelectApplicationFolder";
            this.buttonSelectApplicationFolder.Size = new System.Drawing.Size(37, 20);
            this.buttonSelectApplicationFolder.TabIndex = 5;
            this.buttonSelectApplicationFolder.Text = "...";
            this.buttonSelectApplicationFolder.UseVisualStyleBackColor = true;
            this.buttonSelectApplicationFolder.Click += new System.EventHandler(this.buttonSelectApplicationFolder_Click);
            // 
            // buttonSelectThirdPartyLibraryFolder
            // 
            this.buttonSelectThirdPartyLibraryFolder.Location = new System.Drawing.Point(380, 49);
            this.buttonSelectThirdPartyLibraryFolder.Name = "buttonSelectThirdPartyLibraryFolder";
            this.buttonSelectThirdPartyLibraryFolder.Size = new System.Drawing.Size(37, 20);
            this.buttonSelectThirdPartyLibraryFolder.TabIndex = 4;
            this.buttonSelectThirdPartyLibraryFolder.Text = "...";
            this.buttonSelectThirdPartyLibraryFolder.UseVisualStyleBackColor = true;
            this.buttonSelectThirdPartyLibraryFolder.Click += new System.EventHandler(this.buttonSelectThirdPartyLibraryFolder_Click);
            // 
            // textBoxThirdPartyLibraryDownloadFolder
            // 
            this.textBoxThirdPartyLibraryDownloadFolder.Location = new System.Drawing.Point(115, 50);
            this.textBoxThirdPartyLibraryDownloadFolder.Name = "textBoxThirdPartyLibraryDownloadFolder";
            this.textBoxThirdPartyLibraryDownloadFolder.Size = new System.Drawing.Size(259, 20);
            this.textBoxThirdPartyLibraryDownloadFolder.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 53);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Third Party Libraries";
            // 
            // textBoxApplicationDownloadFolder
            // 
            this.textBoxApplicationDownloadFolder.Location = new System.Drawing.Point(115, 17);
            this.textBoxApplicationDownloadFolder.Name = "textBoxApplicationDownloadFolder";
            this.textBoxApplicationDownloadFolder.Size = new System.Drawing.Size(259, 20);
            this.textBoxApplicationDownloadFolder.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Applications";
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.Controls.Add(this.groupBox1);
            this.tabPageSettings.Controls.Add(this.checkBoxSendReportEmail);
            this.tabPageSettings.Controls.Add(this.textBoxSubscriber);
            this.tabPageSettings.Controls.Add(this.label5);
            this.tabPageSettings.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettings.Size = new System.Drawing.Size(427, 226);
            this.tabPageSettings.TabIndex = 0;
            this.tabPageSettings.Text = "E-Mail-Report";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxSmtpServer);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxPassword);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBoxUser);
            this.groupBox1.Controls.Add(this.textBoxPort);
            this.groupBox1.Location = new System.Drawing.Point(9, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(288, 135);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "E-Mail account";
            // 
            // textBoxSmtpServer
            // 
            this.textBoxSmtpServer.Location = new System.Drawing.Point(84, 19);
            this.textBoxSmtpServer.Name = "textBoxSmtpServer";
            this.textBoxSmtpServer.Size = new System.Drawing.Size(198, 20);
            this.textBoxSmtpServer.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Smtp-Server";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "User";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(84, 97);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(198, 20);
            this.textBoxPassword.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Passwort";
            // 
            // textBoxUser
            // 
            this.textBoxUser.Location = new System.Drawing.Point(84, 71);
            this.textBoxUser.Name = "textBoxUser";
            this.textBoxUser.Size = new System.Drawing.Size(198, 20);
            this.textBoxUser.TabIndex = 6;
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(84, 45);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(198, 20);
            this.textBoxPort.TabIndex = 1;
            // 
            // checkBoxSendReportEmail
            // 
            this.checkBoxSendReportEmail.AutoSize = true;
            this.checkBoxSendReportEmail.Location = new System.Drawing.Point(9, 16);
            this.checkBoxSendReportEmail.Name = "checkBoxSendReportEmail";
            this.checkBoxSendReportEmail.Size = new System.Drawing.Size(113, 17);
            this.checkBoxSendReportEmail.TabIndex = 10;
            this.checkBoxSendReportEmail.Text = "Send report E-Mail";
            this.checkBoxSendReportEmail.UseVisualStyleBackColor = true;
            this.checkBoxSendReportEmail.CheckedChanged += new System.EventHandler(this.checkBoxSendReportEmail_CheckedChanged);
            // 
            // textBoxSubscriber
            // 
            this.textBoxSubscriber.Location = new System.Drawing.Point(93, 184);
            this.textBoxSubscriber.Name = "textBoxSubscriber";
            this.textBoxSubscriber.Size = new System.Drawing.Size(198, 20);
            this.textBoxSubscriber.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 187);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Subscriber";
            // 
            // tabControlGUI
            // 
            this.tabControlGUI.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlGUI.Controls.Add(this.tabPage1);
            this.tabControlGUI.Controls.Add(this.tabPageSettings);
            this.tabControlGUI.Controls.Add(this.tabPage2);
            this.tabControlGUI.Controls.Add(this.tabPage3);
            this.tabControlGUI.Controls.Add(this.tabPage4);
            this.tabControlGUI.Location = new System.Drawing.Point(3, 3);
            this.tabControlGUI.Name = "tabControlGUI";
            this.tabControlGUI.SelectedIndex = 0;
            this.tabControlGUI.Size = new System.Drawing.Size(435, 252);
            this.tabControlGUI.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.buttonSelectVS2013Folder);
            this.tabPage2.Controls.Add(this.buttonSelectVS2012Folder);
            this.tabPage2.Controls.Add(this.buttonSelectVS2010Folder);
            this.tabPage2.Controls.Add(this.textBoxVS2013Location);
            this.tabPage2.Controls.Add(this.textBoxVS2012Location);
            this.tabPage2.Controls.Add(this.textBoxVS2010Location);
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(427, 226);
            this.tabPage2.TabIndex = 2;
            this.tabPage2.Text = "Compiler";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // buttonSelectVS2013Folder
            // 
            this.buttonSelectVS2013Folder.Location = new System.Drawing.Point(383, 155);
            this.buttonSelectVS2013Folder.Name = "buttonSelectVS2013Folder";
            this.buttonSelectVS2013Folder.Size = new System.Drawing.Size(36, 23);
            this.buttonSelectVS2013Folder.TabIndex = 8;
            this.buttonSelectVS2013Folder.Text = "...";
            this.buttonSelectVS2013Folder.UseVisualStyleBackColor = true;
            this.buttonSelectVS2013Folder.Click += new System.EventHandler(this.buttonSelectVS2013Folder_Click);
            // 
            // buttonSelectVS2012Folder
            // 
            this.buttonSelectVS2012Folder.Location = new System.Drawing.Point(383, 99);
            this.buttonSelectVS2012Folder.Name = "buttonSelectVS2012Folder";
            this.buttonSelectVS2012Folder.Size = new System.Drawing.Size(36, 23);
            this.buttonSelectVS2012Folder.TabIndex = 5;
            this.buttonSelectVS2012Folder.Text = "...";
            this.buttonSelectVS2012Folder.UseVisualStyleBackColor = true;
            this.buttonSelectVS2012Folder.Click += new System.EventHandler(this.buttonSelectVS2012Folder_Click);
            // 
            // buttonSelectVS2010Folder
            // 
            this.buttonSelectVS2010Folder.Location = new System.Drawing.Point(383, 43);
            this.buttonSelectVS2010Folder.Name = "buttonSelectVS2010Folder";
            this.buttonSelectVS2010Folder.Size = new System.Drawing.Size(36, 23);
            this.buttonSelectVS2010Folder.TabIndex = 2;
            this.buttonSelectVS2010Folder.Text = "...";
            this.buttonSelectVS2010Folder.UseVisualStyleBackColor = true;
            this.buttonSelectVS2010Folder.Click += new System.EventHandler(this.buttonSelectVS2010Folder_Click);
            // 
            // textBoxVS2013Location
            // 
            this.textBoxVS2013Location.Location = new System.Drawing.Point(16, 157);
            this.textBoxVS2013Location.Name = "textBoxVS2013Location";
            this.textBoxVS2013Location.Size = new System.Drawing.Size(361, 20);
            this.textBoxVS2013Location.TabIndex = 7;
            // 
            // textBoxVS2012Location
            // 
            this.textBoxVS2012Location.Location = new System.Drawing.Point(16, 101);
            this.textBoxVS2012Location.Name = "textBoxVS2012Location";
            this.textBoxVS2012Location.Size = new System.Drawing.Size(361, 20);
            this.textBoxVS2012Location.TabIndex = 4;
            // 
            // textBoxVS2010Location
            // 
            this.textBoxVS2010Location.Location = new System.Drawing.Point(16, 45);
            this.textBoxVS2010Location.Name = "textBoxVS2010Location";
            this.textBoxVS2010Location.Size = new System.Drawing.Size(361, 20);
            this.textBoxVS2010Location.TabIndex = 1;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(13, 141);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(139, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "Visual Studio 2013 Location";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 85);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(139, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "Visual Studio 2012 Location";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 29);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(139, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Visual Studio 2010 Location";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.buttonSelectMSBuildExeLocation);
            this.tabPage3.Controls.Add(this.textBoxMSBuildExeLocation);
            this.tabPage3.Controls.Add(this.label11);
            this.tabPage3.Controls.Add(this.buttonSelectCMakeExeLocation);
            this.tabPage3.Controls.Add(this.textBoxCMakeExeLocation);
            this.tabPage3.Controls.Add(this.label10);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(427, 226);
            this.tabPage3.TabIndex = 3;
            this.tabPage3.Text = "BuildDependencies";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // buttonSelectMSBuildExeLocation
            // 
            this.buttonSelectMSBuildExeLocation.Location = new System.Drawing.Point(339, 127);
            this.buttonSelectMSBuildExeLocation.Name = "buttonSelectMSBuildExeLocation";
            this.buttonSelectMSBuildExeLocation.Size = new System.Drawing.Size(43, 23);
            this.buttonSelectMSBuildExeLocation.TabIndex = 5;
            this.buttonSelectMSBuildExeLocation.Text = "...";
            this.buttonSelectMSBuildExeLocation.UseVisualStyleBackColor = true;
            this.buttonSelectMSBuildExeLocation.Click += new System.EventHandler(this.buttonSelectMSBuildExeLocation_Click);
            // 
            // textBoxMSBuildExeLocation
            // 
            this.textBoxMSBuildExeLocation.Location = new System.Drawing.Point(18, 130);
            this.textBoxMSBuildExeLocation.Name = "textBoxMSBuildExeLocation";
            this.textBoxMSBuildExeLocation.Size = new System.Drawing.Size(313, 20);
            this.textBoxMSBuildExeLocation.TabIndex = 4;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 114);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(111, 13);
            this.label11.TabIndex = 3;
            this.label11.Text = "MSBuild Exe Location";
            // 
            // buttonSelectCMakeExeLocation
            // 
            this.buttonSelectCMakeExeLocation.Location = new System.Drawing.Point(339, 51);
            this.buttonSelectCMakeExeLocation.Name = "buttonSelectCMakeExeLocation";
            this.buttonSelectCMakeExeLocation.Size = new System.Drawing.Size(43, 23);
            this.buttonSelectCMakeExeLocation.TabIndex = 2;
            this.buttonSelectCMakeExeLocation.Text = "...";
            this.buttonSelectCMakeExeLocation.UseVisualStyleBackColor = true;
            this.buttonSelectCMakeExeLocation.Click += new System.EventHandler(this.buttonSelectCMakeExeLocation_Click);
            // 
            // textBoxCMakeExeLocation
            // 
            this.textBoxCMakeExeLocation.Location = new System.Drawing.Point(18, 53);
            this.textBoxCMakeExeLocation.Name = "textBoxCMakeExeLocation";
            this.textBoxCMakeExeLocation.Size = new System.Drawing.Size(315, 20);
            this.textBoxCMakeExeLocation.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 37);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(106, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "CMake Exe Location";
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(427, 226);
            this.tabPage4.TabIndex = 4;
            this.tabPage4.Text = "Libraries";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // FormPrefernces
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 288);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.tabControlGUI);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormPrefernces";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Preferences";
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPageSettings.ResumeLayout(false);
            this.tabPageSettings.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControlGUI.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPageSettings;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxUser;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.TextBox textBoxSmtpServer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControlGUI;
        private System.Windows.Forms.TextBox textBoxSubscriber;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBoxSendReportEmail;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxThirdPartyLibraryDownloadFolder;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxApplicationDownloadFolder;
        private System.Windows.Forms.Button buttonSelectThirdPartyLibraryFolder;
        private System.Windows.Forms.Button buttonSelectApplicationFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox textBoxVS2010Location;
        private System.Windows.Forms.TextBox textBoxVS2012Location;
        private System.Windows.Forms.TextBox textBoxVS2013Location;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button buttonSelectVS2010Folder;
        private System.Windows.Forms.Button buttonSelectVS2012Folder;
        private System.Windows.Forms.Button buttonSelectVS2013Folder;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox checkBoxDownloadThirdPartyLibEveryTime;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button buttonSelectCMakeExeLocation;
        private System.Windows.Forms.TextBox textBoxCMakeExeLocation;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button buttonSelectMSBuildExeLocation;
        private System.Windows.Forms.TextBox textBoxMSBuildExeLocation;
        private System.Windows.Forms.TabPage tabPage4;
    }
}