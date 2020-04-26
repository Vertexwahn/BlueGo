using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Forms;

using Microsoft.Win32;

using BlueGo.GUI;
using BlueGo.Util;
using System.Collections.Specialized;

namespace BlueGo
{
    public partial class FormMain : Form
    {    
        int coreCount = 0;

        public FormMain()
        {
            InitializeComponent();

            
            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                coreCount += int.Parse(item["NumberOfCores"].ToString());
            }
        }

        void sendMail()
        {
            if (!BlueGo.PreferencesManager.Instance.SendReportEmail)
                return;

            string strVon = PreferencesManager.Instance.User;
            string strAn = PreferencesManager.Instance.Subscriber;
            string strSmtpServer = PreferencesManager.Instance.SmtpServer;
            string strUser = PreferencesManager.Instance.User;
            string strPasswort = PreferencesManager.Instance.Password;

            string host = strSmtpServer;
            int port = int.Parse(PreferencesManager.Instance.Port);

            SmtpClient client = new SmtpClient(host, port);

            System.Net.NetworkCredential nc = new System.Net.NetworkCredential(strUser, strPasswort);
            client.Credentials = nc;

            client.EnableSsl = true;
            
            MailMessage mail = new MailMessage();
            MailAddress from = new MailAddress(strVon);
            mail.To.Add(strAn);
            mail.From = from;
            mail.Subject = "BlueGo";

            mail.Body = "Hello it´s me BlueGo";

            // collect some info for mail
            string ComputerName = System.Windows.Forms.SystemInformation.ComputerName.ToString();
            IPHostEntry Host = Dns.GetHostEntry(Dns.GetHostName());

            mail.Body += "\n";
            mail.Body += "Computer Name: " + ComputerName;

            foreach (IPAddress ipaddress in Host.AddressList)
            {
                mail.Body += "\n";
                mail.Body += "IP           : " + ipaddress.ToString();   
            }

            mail.Body += "\n";
            mail.Body += "ProcessorCount: " + Environment.ProcessorCount;
            mail.Body += "\n";
            mail.Body += "Is64BitOperatingSystem: " + Environment.Is64BitOperatingSystem;
            mail.Body += "\n";
            mail.Body += "OSVersion: " + Environment.OSVersion;
            mail.Body += "\n";
            mail.Body += "UserName: " + Environment.UserName;
            mail.Body += "Cpu Caption: " + WMI_ProcessorInformation.WMI_Processor_Information.GetCpuCaption();
            mail.Body += "\n";
            mail.Body += "Cpu Cores: " + WMI_ProcessorInformation.WMI_Processor_Information.GetCpuCores();
            mail.Body += "\n";
            mail.Body += "Cpu Id: " + WMI_ProcessorInformation.WMI_Processor_Information.GetCpuId();
            mail.Body += "\n";
            mail.Body += "Cpu Socket Designation" + WMI_ProcessorInformation.WMI_Processor_Information.GetCpuSocketDesignation();
            mail.Body += "\n";
            mail.Body += "Cpu's current operating voltage: " + WMI_ProcessorInformation.WMI_Processor_Information.GetCpuVoltage();
            mail.Body += "\n";
            mail.Body += "Cpu's Manufacturer: " + WMI_ProcessorInformation.WMI_Processor_Information.GetCpuManufacturer();
            mail.Body += "\n";
            mail.Body += "Cpu Number of logical processors: " + WMI_ProcessorInformation.WMI_Processor_Information.GetCpuNumberOfLogicalProcessors();
            mail.Body += "\n";
            mail.Body += "Cpu's Data width (32 or 64bit): " + WMI_ProcessorInformation.WMI_Processor_Information.GetCpuDataWidth();
            mail.Body += "\n";
            mail.Body += "Cpu speed is (in mega-hertz): " + WMI_ProcessorInformation.WMI_Processor_Information.GetCpuClockSpeed();
            mail.Body += "\n";

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DisplayConfiguration");

            string graphicsCard = string.Empty;
            foreach (ManagementObject mo in searcher.Get())
            {
                foreach (PropertyData property in mo.Properties)
                {
                    if (property.Name == "Description")
                    {
                        graphicsCard = property.Value.ToString();

                        mail.Body += "\n";
                        mail.Body += "graphicsCard: " + graphicsCard;
                    }
                }
            }


            try
            {
                client.Send(mail);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void sendMail(string Text)
        {
            string strVon = PreferencesManager.Instance.User;
            string strAn = PreferencesManager.Instance.Subscriber;
            string strSmtpServer = PreferencesManager.Instance.SmtpServer;
            string strUser = PreferencesManager.Instance.User;
            string strPasswort = PreferencesManager.Instance.Password;

            string host = strSmtpServer;
            int port = int.Parse(PreferencesManager.Instance.Port);


            SmtpClient client = new SmtpClient(host, port);

            System.Net.NetworkCredential nc = new System.Net.NetworkCredential(strUser, strPasswort);
            client.Credentials = nc;

            client.EnableSsl = true;

            MailMessage mail = new MailMessage();
            MailAddress from = new MailAddress(strVon);
            mail.To.Add(strAn);
            mail.From = from;
            mail.Subject = "BlueGo";

            mail.Body = Text;

            string ComputerName = System.Windows.Forms.SystemInformation.ComputerName.ToString();
            mail.Body += "\n";
            mail.Body += "Computer Name: " + ComputerName;

            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        
        string DetermineDestinationFolder()
        {
            ePlatform platformType = DetermineSelectedPlatform();
            string platform = "x64";
            switch(platformType)
            {
                case ePlatform.x86:
                    platform = "x86";
                    break;
                case ePlatform.x64:
                    platform = "x64";
                    break;
                default:
                    throw new Exception("Unknown platform.");
            }

            string destinationFolder = PreferencesManager.Instance.ThirdPartyDownloadFolder + "/vs2010/" + platform + "/";

            eCompiler compilerType = DetermineSelectedCompiler();

            if (compilerType == eCompiler.VS2019)
            {
                destinationFolder = PreferencesManager.Instance.ThirdPartyDownloadFolder + "/vs2019/" + platform + "/";
            }
            else if (compilerType == eCompiler.VS2015)
            {
                destinationFolder = PreferencesManager.Instance.ThirdPartyDownloadFolder + "/vs2015/" + platform + "/";
            }
            else if (compilerType == eCompiler.VS2013)
            {
                destinationFolder = PreferencesManager.Instance.ThirdPartyDownloadFolder + "/vs2013/" + platform + "/";
            }
            else if (compilerType == eCompiler.VS2012)
            {
                destinationFolder = PreferencesManager.Instance.ThirdPartyDownloadFolder + "/vs2012/" + platform + "/";
            }
            else if (compilerType == eCompiler.VS2010)
            {
                destinationFolder = PreferencesManager.Instance.ThirdPartyDownloadFolder + "/vs2010/" + platform + "/";
            }
            else
                throw new Exception("Invalid compiler");

            return destinationFolder;
        }
       
        void message(string msg)
        {
            SetWindowTitleStatusInfo(msg);
            
            if(BlueGo.PreferencesManager.Instance.SendReportEmail)
                sendMail(msg);

            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add(msg);
            });
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                this.WindowState = FormWindowState.Minimized;
                this.notifyIconBlueDeployment.Visible = true;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.ShowInTaskbar = true;
            this.notifyIconBlueDeployment.Visible = false;
            this.WindowState = FormWindowState.Normal;
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_FormPreferences.Clean();
            m_FormPreferences.ShowDialog();
        }

        BlueGo.GUI.FormPrefernces m_FormPreferences = new BlueGo.GUI.FormPrefernces();
 
        List<BlueGo.Data.ApplicationInfo> appInfoList = new List<BlueGo.Data.ApplicationInfo>();

        private void CheckForUpdates()
        {
            try
            {
                WebClient client = new WebClient();
                Stream data = client.OpenRead("http://www.vertexwahn.de/bluego.xml");
                StreamReader reader = new StreamReader(data);
                XmlTextReader xmlReader = new XmlTextReader(reader);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlReader);

                XmlNode xmlInfo = xmlDoc.SelectSingleNode("BlueGo/Info");
                string strVersion = xmlInfo.Attributes["Version"].InnerText;
                string strDownloadLink = xmlInfo.Attributes["DownloadLink"].InnerText;

                if (strVersion != "0.2.8")
                {
                    //MessageBox.Show("A new version of BlueGo is available (BlueGo " + strVersion + "). It can be downloaded from " + strDownloadLink, "BlueGo");
                    FormUpdate formUpdate = new FormUpdate();
                    formUpdate.Clean(strVersion, strDownloadLink);
                    formUpdate.ShowDialog();
                }

                reader.Close();
                data.Close();
                reader.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Could not check for updates. Maybe your internet connection is not working."); // ex.ToString());
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            CheckForUpdates();

            // Check if application data is installed correctly
            if (!IsInstalledCorrectly())
            {
                MessageBox.Show("BlueGo seems not to be installed correctly.");
            }

            // Fill gui elements

            // fill boost gui
            List<BoostInfo> boostInfoList = BoostInfo.CreateInfoList();
            boostInfoList.Reverse();
            foreach(BoostInfo bi in boostInfoList)
            {
                comboBoxbBoostVersion.Items.Add(BoostInfo.BoostVersionToString(bi.Version));
            }

            if (comboBoxbBoostVersion.Items.Count > 0)
                comboBoxbBoostVersion.SelectedIndex = 0;

            if (comboBoxPlatform.Items.Count > 0)
                comboBoxPlatform.SelectedIndex = 0;

            if (comboBoxCompiler.Items.Count > 0)
                comboBoxCompiler.SelectedIndex = 0;

            // fill qt gui
            List<QtInfo> qtInfoList = QtInfo.CreateInfoList();
            qtInfoList.Reverse();
            foreach (QtInfo info in qtInfoList)
            {
                comboBoxQtVersion.Items.Add(QtInfo.TransformVersionToString(info.Version));
            }
            if (comboBoxQtVersion.Items.Count > 0)
                comboBoxQtVersion.SelectedIndex = 0;

            // fill OpenSceneGraph gui
            List<OpenSceneGraphInfo> osgInfoList = OpenSceneGraphInfo.CreateInfoList();
            osgInfoList.Reverse();
            foreach (OpenSceneGraphInfo info in osgInfoList)
            {
                comboBoxOpenSceneGraphVersion.Items.Add(OpenSceneGraphInfo.TransformVersionToString(info.Version));
            }
            if (comboBoxOpenSceneGraphVersion.Items.Count > 0)
                comboBoxOpenSceneGraphVersion.SelectedIndex = 0;

            // fill eigen gui
            List<EigenInfo> eigenInfoList = EigenInfo.CreateInfoList();
            eigenInfoList.Reverse();
            foreach (EigenInfo info in eigenInfoList)
            {
                comboBoxEigenVersion.Items.Add(EigenInfo.TransformVersionToString(info.Version));
            }
            if (comboBoxEigenVersion.Items.Count > 0)
                comboBoxEigenVersion.SelectedIndex = 0;

            // fill libLAS gui
            List<LibLASInfo> libLASInfoList = LibLASInfo.CreateInfoList();
            libLASInfoList.Reverse();
            foreach (LibLASInfo info in libLASInfoList)
            {
                comboBoxLibLASVersion.Items.Add(LibLASInfo.TransformLibLASVersionToString(info.Version));
                comboBoxLibLASBoostVersion.Items.Add(LibLASInfo.TransformLibLASSupportedBoostVersionToString(info.BoostVersion));
            }
            if (comboBoxLibLASVersion.Items.Count > 0)
                comboBoxLibLASVersion.SelectedIndex = 0;

            if (comboBoxLibLASBoostVersion.Items.Count > 0)
                comboBoxLibLASBoostVersion.SelectedIndex = 0;

            // fill PCL gui
            List<PCLInfo> pclInfoList = PCLInfo.CreateInfoList();
            pclInfoList.Reverse();
            foreach (PCLInfo info in pclInfoList)
            {
                comboBoxPCLVersion.Items.Add(PCLInfo.TransformPCLVersionToString(info.Version));
            }
            if (comboBoxPCLVersion.Items.Count > 0)
                comboBoxPCLVersion.SelectedIndex = 0;

            foreach (ePCLSupportedBoostVersion boostEnum in Enum.GetValues(typeof(ePCLSupportedBoostVersion)))
            {
                comboBoxPCLSupportedBoostVersion.Items.Add(PCLInfo.TransformPCLSupportedBoostVersionToString(boostEnum));
            }
            if (comboBoxPCLSupportedBoostVersion.Items.Count > 0)
                comboBoxPCLSupportedBoostVersion.SelectedIndex = 0;

            foreach (ePCLSupportedEigenVersion eigenEnum in Enum.GetValues(typeof(ePCLSupportedEigenVersion)))
            {
                comboBoxPCLSupportedEigenVersion.Items.Add(PCLInfo.TransformPCLSupportedEigenVersionToString(eigenEnum));
            }
            if (comboBoxPCLSupportedEigenVersion.Items.Count > 0)
                comboBoxPCLSupportedEigenVersion.SelectedIndex = 0;

            foreach (ePCLSupportedFlannVersion flannEnum in Enum.GetValues(typeof(ePCLSupportedFlannVersion)))
            {
                comboBoxPCLSupportedFlannVersion.Items.Add(PCLInfo.TransformPCLSupportedFlannVersionToString(flannEnum));
            }
            if (comboBoxPCLSupportedFlannVersion.Items.Count > 0)
                comboBoxPCLSupportedFlannVersion.SelectedIndex = 0;

            foreach (ePCLSupportedVTKVersion vtkEnum in Enum.GetValues(typeof(ePCLSupportedVTKVersion)))
            {
                comboBoxPCLSupportedVTKVersion.Items.Add(PCLInfo.TransformPCLSupportedVTKVersionToString(vtkEnum));
            }
            if (comboBoxPCLSupportedVTKVersion.Items.Count > 0)
                comboBoxPCLSupportedVTKVersion.SelectedIndex = 0;

            // fill Flann gui
            List<FlannInfo> flannInfoList = FlannInfo.CreateInfoList();
            flannInfoList.Reverse();
            foreach (FlannInfo info in flannInfoList)
            {
                comboBoxFlannVersion.Items.Add(FlannInfo.TransformFlannVersionToString(info.Version));
            }
            if (comboBoxFlannVersion.Items.Count > 0)
                comboBoxFlannVersion.SelectedIndex = 0;

            // fill VTK gui
            List<VTKInfo> vtkInfoList = VTKInfo.CreateInfoList();
            vtkInfoList.Reverse();
            foreach (VTKInfo info in vtkInfoList)
            {
                comboBoxVTKVersion.Items.Add(VTKInfo.TransformVTKVersionToString(info.Version));
            }
            if (comboBoxVTKVersion.Items.Count > 0)
                comboBoxVTKVersion.SelectedIndex = 0;
            
            // fill application gui
            appInfoList = BlueGo.Data.ApplicationInfo.Load("applications.xml");

            // Fill PrebuiltBinaries
            TreeNode vs2010 = new TreeNode("Microsoft Visual Studio 2010");

            TreeNode vs2010_x86 = new TreeNode("x86"); 
            TreeNode vs2010_x64 = new TreeNode("x64");
            vs2010.Nodes.Add(vs2010_x86);
            vs2010.Nodes.Add(vs2010_x64);

            TreeNode vs2012 = new TreeNode("Microsoft Visual Studio 2012");
            TreeNode vs2012_x86 = new TreeNode("x86");
            TreeNode vs2012_x64 = new TreeNode("x64");
            vs2012.Nodes.Add(vs2012_x86);
            vs2012.Nodes.Add(vs2012_x64);

            TreeNode vs2013 = new TreeNode("Microsoft Visual Studio 2013");
            TreeNode vs2013_x86 = new TreeNode("x86");
            TreeNode vs2013_x64 = new TreeNode("x64");
            vs2013.Nodes.Add(vs2013_x86);
            vs2013.Nodes.Add(vs2013_x64);

            TreeNode vs2015 = new TreeNode("Microsoft Visual Studio 2015");
            TreeNode vs2015_x86 = new TreeNode("x86");
            TreeNode vs2015_x64 = new TreeNode("x64");
            vs2015.Nodes.Add(vs2015_x86);
            vs2015.Nodes.Add(vs2015_x64);

            TreeNode vs2019 = new TreeNode("Microsoft Visual Studio 2019");
            TreeNode vs2019_x86 = new TreeNode("x86");
            TreeNode vs2019_x64 = new TreeNode("x64");
            vs2015.Nodes.Add(vs2019_x86);
            vs2015.Nodes.Add(vs2019_x64);

            TreeNode applications = new TreeNode("Applications");

            foreach (BlueGo.Data.ApplicationInfo ai in appInfoList)
            {
                if (ai.IDE == "vs2010")
                {
                    if (ai.Platfom == "x86")
                    {
                        TreeNode n = new TreeNode(ai.Name);
                        n.Tag = ai;
                        vs2010_x86.Nodes.Add(n);
                    }

                    if (ai.Platfom == "x64")
                    {
                        TreeNode n = new TreeNode(ai.Name);
                        n.Tag = ai;
                        vs2010_x64.Nodes.Add(n);
                    }
                }

                if (ai.IDE == "vs2012")
                {
                    if (ai.Platfom == "x86")
                    {
                        TreeNode n = new TreeNode(ai.Name);
                        n.Tag = ai;
                        vs2012_x86.Nodes.Add(n);
                    }

                    if (ai.Platfom == "x64")
                    {
                        TreeNode n = new TreeNode(ai.Name);
                        n.Tag = ai;
                        vs2012_x64.Nodes.Add(n);
                    }
                }

                if (ai.IDE == "vs2013")
                {
                    if (ai.Platfom == "x86")
                    {
                        TreeNode n = new TreeNode(ai.Name);
                        n.Tag = ai;
                        vs2013_x86.Nodes.Add(n);
                    }

                    if (ai.Platfom == "x64")
                    {
                        TreeNode n = new TreeNode(ai.Name);
                        n.Tag = ai;
                        vs2013_x64.Nodes.Add(n);
                    }
                }

                if (ai.IDE == "vs2015")
                {
                    if (ai.Platfom == "x86")
                    {
                        TreeNode n = new TreeNode(ai.Name);
                        n.Tag = ai;
                        vs2015_x86.Nodes.Add(n);
                    }

                    if (ai.Platfom == "x64")
                    {
                        TreeNode n = new TreeNode(ai.Name);
                        n.Tag = ai;
                        vs2015_x64.Nodes.Add(n);
                    }
                }

                if (ai.IDE == "none")
                {
                    TreeNode n = new TreeNode(ai.Name);
                    n.Tag = ai;
                    applications.Nodes.Add(n);
                }
            }

            treeViewPrebuiltBinaries.Nodes.Add(vs2010);
            treeViewPrebuiltBinaries.Nodes.Add(vs2012);
            treeViewPrebuiltBinaries.Nodes.Add(vs2013);
            treeViewPrebuiltBinaries.Nodes.Add(vs2015);
            treeViewPrebuiltBinaries.Nodes.Add(vs2019);
            treeViewPrebuiltBinaries.Nodes.Add(applications);
        }

        private void Download(TreeNode treeNode)
        {
            if (treeNode.Checked)
            {
                if (treeNode.Tag != null)
                {
                    BlueGo.Data.ApplicationInfo ai = (BlueGo.Data.ApplicationInfo)treeNode.Tag;

                    // Download application
                    try
                    {
                        if (ai.IDE == "vs2013" && ai.Platfom == "x64")
                        {
                            DownloadHelper.DownloadFileFromURL(ai.DownloadUrl, PreferencesManager.Instance.ApplicationDownloadFolder + "\\vs2013\\x64\\" + ai.Filename);

                            // Unzip file
                            SevenZip.Decompress(PreferencesManager.Instance.ApplicationDownloadFolder + "\\vs2013\\x64\\" + ai.Filename, PreferencesManager.Instance.ApplicationDownloadFolder + "\\vs2013\\x64");

                            System.IO.File.Delete(PreferencesManager.Instance.ApplicationDownloadFolder + "\\vs2013\\x64\\" + ai.Filename);
                        }
                        else if(ai.IDE == "vs2015" && ai.Platfom == "x64")
                        {
                            DownloadHelper.DownloadFileFromURL(ai.DownloadUrl, PreferencesManager.Instance.ApplicationDownloadFolder + "\\vs2015\\x64\\" + ai.Filename);

                            // Unzip file
                            SevenZip.Decompress(PreferencesManager.Instance.ApplicationDownloadFolder + "\\vs2015\\x64\\" + ai.Filename, PreferencesManager.Instance.ApplicationDownloadFolder + "\\vs2015\\x64");

                            System.IO.File.Delete(PreferencesManager.Instance.ApplicationDownloadFolder + "\\vs2015\\x64\\" + ai.Filename);
                        }
                        else
                            DownloadHelper.DownloadFileFromURL(ai.DownloadUrl, PreferencesManager.Instance.ApplicationDownloadFolder + "\\" + ai.Filename);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show("Application " + ai.Name + " can not be downloaded (Invalid Url). Please send a bug report do us to exclude or fix the download path.", "BlueGo");
                    }         
                }
            }
            
            // Print each node recursively.
            foreach (TreeNode tn in treeNode.Nodes)
            {
                Download(tn);
            }
        }

        // Call the procedure using the TreeView.
        private void CallRecursive(TreeView treeView)
        {
            // Print each node recursively.
            TreeNodeCollection nodes = treeView.Nodes;
            foreach (TreeNode n in nodes)
            {
                Download(n);
            }
        }

        private void buttonDonloadApplications_Click(object sender, EventArgs e)
        {
            buttonDonloadApplications.Enabled = false;

            if (!Directory.Exists(PreferencesManager.Instance.ApplicationDownloadFolder))
            {
                Directory.CreateDirectory(PreferencesManager.Instance.ApplicationDownloadFolder);
            }

            CallRecursive(treeViewPrebuiltBinaries);
                        
            buttonDonloadApplications.Enabled = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout fa = new FormAbout() ;
            fa.Show();
        }

        static void GetInstalled()
        {
            string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(uninstallKey))
            {
                foreach (string skName in rk.GetSubKeyNames())
                {
                    using (RegistryKey sk = rk.OpenSubKey(skName))
                    {
                        Console.WriteLine(sk.GetValue("DisplayName"));
                    }
                }
            }
        }

        private void buttonQT_Click(object sender, EventArgs e)
        {
            QtBuildStartTime = DateTime.Now;

            eCompiler compiler = DetermineSelectedCompiler();

            if (!Compiler.IsPresent(compiler))
            {
                ReportCompilerNotPresent(compiler);
                return; // Compiler not present we can not do anything useful
            }
            
            // prevent that user double clicks the qt build button
            buttonBuildQT.Enabled = false;

            // check if perl is available
            bool perlAvailable = false;
            
            if( System.IO.Directory.Exists(@"C:\strawberry\perl") || // Strawberry Perl
                System.IO.Directory.Exists(@"C:\Perl64\bin") || // Active State Perl
                Executable.ExistsOnPath("perl.exe") )      
            {
                perlAvailable = true;
            }
               
            if(!perlAvailable)
            {
                if (MessageBox.Show("Warning: Perl must be installed in order to build qt. Do you want to continue? (You can use ActivePerl - http://www.activestate.com/activeperl) ", 
                    "Warning", 
                    MessageBoxButtons.OKCancel) != DialogResult.OK)
                {
                    buttonBuildQT.Enabled = true;
                    return;
                }
            }
            
            // Setup build process
            qtBuildProcessDescripton qbpd = new qtBuildProcessDescripton();
            qbpd.qtVersion = DetermineSelectedQtVersion();
            qbpd.compilerType = compiler;
            qbpd.platform = DetermineSelectedPlatform();
            qbpd.destinationFolder = DetermineDestinationFolder();
            qbpd.withLibraries = GetSelectedQtLibraries();
            qbpd.qtCommandLineArguments = DetermineQtCommandLineArguments();
            qbpd.downloadThirdPartyLibraryEveryTime = PreferencesManager.Instance.DownloadThirdPartyLibEveryTime;
            QtBuildProcess qtBuildProcess = new QtBuildProcess(qbpd);

            // check if python is available on path variable if using Qt5
            if (qbpd.qtVersion != eQtVersion.Qt4_8_3 &&
                qbpd.qtVersion != eQtVersion.Qt4_8_4)
            {
                if (!Executable.ExistsOnPath("python.exe"))
                {
                    if (MessageBox.Show("Warning: Python could not be found on the Windows path. Do you want to continue? (Add python.exe to the Windows path to build Qt5 successfully) ",
                        "Warning",
                         MessageBoxButtons.OKCancel) != DialogResult.OK)
                    {
                        buttonBuildQT.Enabled = true;
                        return;
                    }
                }
            }

            qtBuildProcess.Message += new MessageEventHandler(qtBuildProcess_Message);
            qtBuildProcess.StandardOutputMessage += new StandardOutputMessageEventHandler(qtBuildProcess_StandardOutputMessage);
            qtBuildProcess.StandardErrorMessage += new StandardErrorMessageEventHandler(qtBuildProcess_StandardErrorMessage);
            qtBuildProcess.Finished += new FinishedEventHandler(qtBuildProcess_Finished);
            qtBuildProcess.Failure += new FailureEventHandler(qtBuildProcess_Failure);

            // Run build process
            Thread workerThread = new Thread(qtBuildProcess.DownloadAndBuild);
            workerThread.Start();
        }

        void qtBuildProcess_Failure(object sender)
        {
            UnlockQtBuildButton();
        }

        void qtBuildProcess_Finished(object sender)
        {
            UnlockQtBuildButton();

            QtBuildEndTime = DateTime.Now;
            TimeSpan time = QtBuildEndTime - QtBuildStartTime;

            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add("Qt build time: " + time.ToString());
            });
        }

        void qtBuildProcess_StandardErrorMessage(object sender, string message)
        {
            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add(message);
            });
        }

        void qtBuildProcess_StandardOutputMessage(object sender, string message)
        {
            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add(message);
            });
        }

        void qtBuildProcess_Message(object sender, string message)
        {
            this.message(message);
        }

        private void radioButtonQtMinimal_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonQtMinimal.Checked == true)
            {
                MessageBox.Show("Selecting Qt Minimim build will not consider Variants option. Qt minimum will build with following options:\n" +
                    " -release -opensource -nomake demos -nomake examples -nomake tests" +
                    " -no-webkit -no-phonon -no-phonon-backend -no-script -no-scripttools -no-multimedia -nomake docs -nomake demos");
            }
        }          

        string DetermineQtCommandLineArguments()
        {
            string arguments = "";

            if (!checkBoxQtShowAdvancedSettings.Checked || radioButtonQtMinimal.Checked)
            {
                return arguments;
            }

            if (checkBoxQtVariantDebug.Checked && checkBoxQtVariantRelease.Checked)
            {
                arguments += " -debug-and-release";
            }
            else if (checkBoxQtVariantDebug.Checked)
            {
                arguments += " -debug";
            }
            else if (checkBoxQtVariantRelease.Checked)
            {
                arguments += " -release";
            }

            if (checkBoxQtLinkShared.Checked)
            {
                arguments += " -shared";
            }

            if (checkBoxQtLinkStatic.Checked)
            {
                arguments += " -static";
            }

            return arguments;
        }
        
        string DetermineB2CommandLineArguments()
        {
            if (!checkBoxBoostShowAdvancedSettings.Checked)
                return "";

            string arguments = "";

            if (checkBoxBoostVariantDebug.Checked)
            {
                arguments += " variant=debug";
            }

            if (checkBoxBoostVariantRelease.Checked)
            {
                arguments += " variant=Release";
            }

            if (checkBoxBoostLinkShared.Checked)
            {
                arguments += " link=shared";
            }

            if (checkBoxBoostLinkStatic.Checked)
            {
                arguments += " link=static";
            }

            if (checkBoxBoostThreadingSingle.Checked)
            {
                arguments += " threading=single";
            }

            if (checkBoxBoostThreadingMulti.Checked)
            {
                arguments += " threading=multi";
            }

            if (checkBoxBoostRuntimeLinkShared.Checked)
            {
                arguments += " runtime-link=shared";
            }

            if (checkBoxBoostRuntimeLinkStatic.Checked)
            {
                arguments += " runtime-link=static";
            }

            return arguments;
        }

        private void buttonBoostBuild_Click(object sender, EventArgs e)
        {
            BoostBuildStartTime = DateTime.Now;

            eCompiler compiler = DetermineSelectedCompiler();

            if (!Compiler.IsPresent(compiler))
            {
                ReportCompilerNotPresent(compiler);
                return; // Compiler not present we can not do anything useful
            }

            // Lock GUI
            buttonBoostBuild.Enabled = false;
            
            boostBuildProcessDescripton bbpd = new boostBuildProcessDescripton();
            bbpd.boostVersion = DetermineSelectedBoostVersion();
            bbpd.compilerType = compiler;
            bbpd.coreCount = coreCount;
            bbpd.destinationFolder = DetermineDestinationFolder();
            bbpd.platform = DetermineSelectedPlatform();
            bbpd.withLibraries = GetSelectedBoostLibraries();
            bbpd.B2CommandLineArguments = DetermineB2CommandLineArguments();

            BoostBuildProcess boostBuildProcess = new BoostBuildProcess(bbpd);

            if ((bbpd.compilerType == eCompiler.VS2013) && ((bbpd.boostVersion == eBoostVersion.Boost1_55_0) || 
                (bbpd.boostVersion == eBoostVersion.Boost1_54_0) || 
                (bbpd.boostVersion == eBoostVersion.Boost1_53_0) ||
                (bbpd.boostVersion == eBoostVersion.Boost1_52_0) || 
                (bbpd.boostVersion == eBoostVersion.Boost1_51_0) || 
                (bbpd.boostVersion == eBoostVersion.Boost1_44_0)))
            {
                if (MessageBox.Show("Warning: Please use one of the following options to build boost successfully:\n" +
                    "Boost Version: 1.55.0 or earlier & Compiler: VS2012 \\ VS2010\n",
                   "Warning",
                    MessageBoxButtons.OK) == DialogResult.OK)
                {
                    buttonBoostBuild.Enabled = true;
                    return;
                }
            }

            if (bbpd.withLibraries.Contains("python") && 
                !Executable.ExistsOnPath("python.exe"))
            {
                if (MessageBox.Show("Warning: Python could not be found on the Windows path. (Add python.exe to the Windows path to build boost successfully) ",
                    "Warning",
                     MessageBoxButtons.OK) == DialogResult.OK)
                {
                    buttonBoostBuild.Enabled = true;
                    return;
                }
            }

            boostBuildProcess.Message += new MessageEventHandler(boostBuildProcess_Message);
            boostBuildProcess.StandardOutputMessage += new StandardOutputMessageEventHandler(boostBuildProcess_StandardOutputMessage);
            boostBuildProcess.StandardErrorMessage += new StandardErrorMessageEventHandler(boostBuildProcess_StandardErrorMessage);
            boostBuildProcess.Finished += new FinishedEventHandler(boostBuildProcess_Finished);
            boostBuildProcess.Failure += new FailureEventHandler(boostBuildProcess_Failure);

            Thread workerThread = new Thread(boostBuildProcess.DownloadAndBuild);
            workerThread.Start();
        }

        // some statistics
        private DateTime BoostBuildStartTime = DateTime.Now;
        private DateTime BoostBuildEndTime = DateTime.Now;
        private DateTime QtBuildStartTime = DateTime.Now;
        private DateTime QtBuildEndTime = DateTime.Now;

        eCompiler DetermineSelectedCompiler()
        {
            string text = "";
            this.Invoke((MethodInvoker)delegate()
            {
                text = comboBoxCompiler.Text;
            });

            if (text == "Microsoft Visual Studio 10 2010")
            {
                return eCompiler.VS2010;
            }

            if (text == "Microsoft Visual Studio 11 2012")
            {
                return eCompiler.VS2012;
            }

            if (text == "Microsoft Visual Studio 12 2013")
            {
                return eCompiler.VS2013;
            }

            if (text == "Microsoft Visual Studio 14 2015")
            {
                return eCompiler.VS2015;
            }

            if (text == "Microsoft Visual Studio 16 2019")
            {
                return eCompiler.VS2019;
            }

            throw new Exception("Invalid compiler selected");
            //return eCompiler.Unknown;
        }

        void ReportCompilerNotPresent(eCompiler compiler)
        {
            MessageBox.Show("Error: Selected Compiler is not present.", "BlueGo",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void boostBuildProcess_StandardErrorMessage(object sender, string message)
        {
            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add(message);
            });
        }

        void boostBuildProcess_StandardOutputMessage(object sender, string message)
        {
            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add(message);
            });
        }

        void boostBuildProcess_Failure(object sender)
        {
            UnlockBoostBuildButton();
        }

        void boostBuildProcess_Finished(object sender)
        {
            UnlockBoostBuildButton();

            BoostBuildEndTime = DateTime.Now;
            TimeSpan time = BoostBuildEndTime - BoostBuildStartTime;

            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add("Boost build time: " + time.ToString());
            });
        }

        void boostBuildProcess_Message(object sender, string message)
        {
            this.message(message);
        }

        private static void CheckDestinationFolder(string destinationFolder)
        {
            if (!System.IO.File.Exists(destinationFolder))
            {
                throw new Exception("Destination folder does not exist " + destinationFolder);
            }
        }

        private void SetWindowTitleStatusInfo(String statusInfo)
        {
            string windowTitle = "BlueGo (0.2.8) [" + statusInfo + "]";
        
            this.Invoke((MethodInvoker)delegate()
            {
                this.Text = windowTitle;
            });
        }

        private void UnlockBoostBuildButton()
        {
            this.Invoke((MethodInvoker)delegate()
            {
                this.buttonBoostBuild.Enabled = true;
            });
        }

        private void UnlockQtBuildButton()
        {
            this.Invoke((MethodInvoker)delegate()
            {
                buttonBuildQT.Enabled = true;
            });
        }

        private void buttonCMakeBrowseSource_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog objDialog = new FolderBrowserDialog();
            objDialog.Description = "Select the source folder";
            objDialog.SelectedPath = @"C:\";       
            DialogResult objResult = objDialog.ShowDialog(this);
            if (objResult == DialogResult.OK)
            {
                textBoxCMakeSource.Text = objDialog.SelectedPath;
            }               
        }

        private void buttonCMakeBrowseBuild_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog objDialog = new FolderBrowserDialog();
            objDialog.Description = "Select the build folder";
            objDialog.SelectedPath = @"C:\";      
            DialogResult objResult = objDialog.ShowDialog(this);
            if (objResult == DialogResult.OK)
            {
                textBoxCMakeBinariesFolder.Text = objDialog.SelectedPath;
            }         
        }

        private void buttonCMakeGo_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This will create a batch (BlueGo.bat) in your source folder which can be used to run CMake.");

            // Open CMakeLists.txt
            string filename = textBoxCMakeSource.Text + @"\CMakeLists.txt";
            StreamReader sr;
            sr = File.OpenText(filename);


            List<Package> packages = new List<Package>();

            // find out which packages are needed
            string s = sr.ReadLine();
            
            while (s != null)
            {
                if (s.Contains("find_package"))
                {
                    listBoxLogOutput.Items.Add(s);

                    if (s.Contains("DirectX"))
                    {
                        listBoxLogOutput.Items.Add("DirectX needed!");

                        packages.Add(new Package("DirectX"));
                    }
                    if (s.Contains("VLD"))
                    {
                        listBoxLogOutput.Items.Add("VLD needed!");

                        packages.Add(new Package("VLD"));
                    }
                    if (s.Contains("OptiX"))
                    {
                        listBoxLogOutput.Items.Add("OptiX needed!");

                        packages.Add(new Package("OptiX"));
                    }
                    if (s.Contains("CUDA"))
                    {
                        listBoxLogOutput.Items.Add("CUDA needed!");
                        packages.Add(new Package("CUDA"));
                    }
                    if (s.Contains("Qt4"))
                    {
                        listBoxLogOutput.Items.Add("Qt4 needed!");
                        packages.Add(new Package("Qt4"));
                    }
                    if (s.Contains("Boost"))
                    {
                        listBoxLogOutput.Items.Add("Boost needed!");
                        packages.Add(new Package("Boost"));
                    }
                }
                
                s = sr.ReadLine();
            }

            string batchFile = "";

            batchFile += "cd " + textBoxCMakeBinariesFolder.Text + Environment.NewLine;
            batchFile += "\"C:\\Program Files (x86)\\CMake 2.8\\bin\\cmake\"" + " " +
                         "-G\"Visual Studio 10 Win64\"" + " " +
                         "-H\"" + textBoxCMakeSource.Text + "\" " +
                         "-B\"" + textBoxCMakeBinariesFolder.Text + "\" " + 
                         "-DQT_QMAKE_EXECUTABLE=\"C:\\thirdparty\\qt-everywhere-opensource-src-4.8.3\\bin\\qmake.exe" + " " +
                         "-DBOOST_ROOT=\"C:\\thirdparty\\vs2010\\x64\\boost_1_52_0" + " " +
                         "-DVLD_ROOT_DIR=\"C:\\Program Files (x86)\\Visual Leak Detector\"" + " " +
                         Environment.NewLine;
            batchFile += "cd " + textBoxCMakeSource.Text + Environment.NewLine;

            using (StreamWriter outfile = new StreamWriter(textBoxCMakeSource.Text + @"\BlueGo.bat"))
            {
                outfile.Write(batchFile);
            }
        }

        private ePlatform DetermineSelectedPlatform()
        {
            string text = "";
            this.Invoke((MethodInvoker)delegate()
            {
                text = comboBoxPlatform.Text;
            });

            if (text == "x64")
            {
                return ePlatform.x64;
            }

            if (text == "x86")
            {
                return ePlatform.x86;
            }

            throw new Exception("Unknown Boost Version.");
        }

        private eBoostVersion DetermineSelectedBoostVersion()
        {
            string text = "";
            this.Invoke((MethodInvoker)delegate()
            {
                text = comboBoxbBoostVersion.Text;
            });


            foreach (BoostInfo bi in BoostInfo.CreateInfoList())
            {
                if (BoostInfo.BoostVersionToString(bi.Version) == text)
                    return bi.Version;
            }

            throw new Exception("Unknown Boost Version.");
        }

        private eQtVersion DetermineSelectedQtVersion()
        {
            string text = "";
            this.Invoke((MethodInvoker)delegate()
            {
                text = comboBoxQtVersion.Text;
            });

            foreach (QtInfo info in QtInfo.CreateInfoList())
            {
                if (QtInfo.TransformVersionToString(info.Version) == text)
                    return info.Version;
            }

            throw new Exception("Unknown Qt Version.");
        }

        private eOpenSceneGraphVersion DetermineSelectedOpenSceneGraphVersion()
        {
            string text = "";
            this.Invoke((MethodInvoker)delegate()
            {
                text = comboBoxOpenSceneGraphVersion.Text;
            });

            foreach (OpenSceneGraphInfo info in OpenSceneGraphInfo.CreateInfoList())
            {
                if (OpenSceneGraphInfo.TransformVersionToString(info.Version) == text)
                    return info.Version;
            }

            throw new Exception("Unknown OpenSceneGraph Version.");
        }

        private eEigenVersion DetermineSelectedEigenVersion()
        {
            string text = "";
            this.Invoke((MethodInvoker)delegate()
            {
                text = comboBoxEigenVersion.Text;
            });

            foreach (EigenInfo info in EigenInfo.CreateInfoList())
            {
                if (EigenInfo.TransformVersionToString(info.Version) == text)
                    return info.Version;
            }

            throw new Exception("Unknown OpenSceneGraph Version.");
        }

        private string GetSelectedBoostLibraries()
        {
            if (radioButtonBoostSpecific.Checked)
            {
                return GetSelectedBoostLibraries2();
            }
            else
                return "";
        }

        private string GetSelectedBoostLibraries2()
        {
            string libs = "";

            foreach (object o in checkedListBoxLibraries.CheckedItems)
            {
                libs += " --with-" + o.ToString();
            }

            return libs;
        }

        private string GetSelectedQtLibraries()
        {
            if (radioButtonQtSpecific.Checked)
            {
                string libs = "";

                foreach (object o in checkedListBoxQtLibraries.CheckedItems)
                {
                    libs += " -" + o.ToString();
                }

                return libs;
            }
            else if(radioButtonQtMinimal.Checked)
            {
                string minimumQtOptions = " -release -no-webkit -no-phonon -no-phonon-backend -no-script -no-scripttools"
                    + " -no-multimedia -nomake docs -nomake demos";
                
                return minimumQtOptions;              
            }
            else if(radioButtonQtAll.Checked)
            {
                return " -opengl desktop";
            }

            return string.Empty;
        }

        private void buttonOpenDownloadFolder_Click(object sender, EventArgs e)
        {

            string myDocspath = PreferencesManager.Instance.ApplicationDownloadFolder; //Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string windir = Environment.GetEnvironmentVariable("WINDIR");
            System.Diagnostics.Process prc = new System.Diagnostics.Process();
            prc.StartInfo.FileName = windir + @"\explorer.exe";
            prc.StartInfo.Arguments = myDocspath;
            prc.Start();
        }

        private void buttonBuildOpenSceneGraph_Click(object sender, EventArgs e)
        {
            // prevent that user clicks the dOpenSceneGraph build button again
            buttonBuildOpenSceneGraph.Enabled = false;

            eCompiler compiler = DetermineSelectedCompiler();

            if (!Compiler.IsPresent(compiler))
            {
                ReportCompilerNotPresent(compiler);
                return; // Compiler not present we can not do anything useful
            }

            // prevent that user double clicks the OpenSceneGraph build button
            buttonBuildOpenSceneGraph.Enabled = false;

            // Setup build process
            OpenSceneGraphBuildProcessDescripton qbpd = new OpenSceneGraphBuildProcessDescripton();
            qbpd.version = DetermineSelectedOpenSceneGraphVersion();
            qbpd.compilerType = compiler;
            qbpd.platform = DetermineSelectedPlatform();
            qbpd.destinationFolder = DetermineDestinationFolder();

            OpenSceneGraphBuildProcess OpenSceneGraphBuildProcess = new OpenSceneGraphBuildProcess(qbpd);
            OpenSceneGraphBuildProcess.Message += new MessageEventHandler(OpenSceneGraphBuildProcess_Message);
            OpenSceneGraphBuildProcess.StandardOutputMessage += new StandardOutputMessageEventHandler(OpenSceneGraphBuildProcess_StandardOutputMessage);
            OpenSceneGraphBuildProcess.StandardErrorMessage += new StandardErrorMessageEventHandler(OpenSceneGraphBuildProcess_StandardErrorMessage);
            OpenSceneGraphBuildProcess.Finished += new FinishedEventHandler(OpenSceneGraphBuildProcess_Finished);
            OpenSceneGraphBuildProcess.Failure += new FailureEventHandler(OpenSceneGraphBuildProcess_Failure);

            // Run build process
            Thread workerThread = new Thread(OpenSceneGraphBuildProcess.DownloadAndBuild);
            workerThread.Start();
        }

        private void UnlockOpenSceneGraphBuildButton()
        {
            this.Invoke((MethodInvoker)delegate()
            {
                this.buttonBuildOpenSceneGraph.Enabled = true;
            });
        }

        void OpenSceneGraphBuildProcess_Failure(object sender)
        {
            UnlockOpenSceneGraphBuildButton();
        }

        void OpenSceneGraphBuildProcess_Finished(object sender)
        {
            UnlockOpenSceneGraphBuildButton();
        }

        void OpenSceneGraphBuildProcess_StandardErrorMessage(object sender, string message)
        {
            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add(message);
            });
        }

        void OpenSceneGraphBuildProcess_StandardOutputMessage(object sender, string message)
        {
            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add(message);
            });
        }

        void OpenSceneGraphBuildProcess_Message(object sender, string message)
        {
            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add(message);
            });
        }

        void StandardProcess_Finished(object sender)
        {
            
        }

        void StandardProcess_StandardErrorMessage(object sender, string message)
        {
            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add(message);
            });
        }

        void DetectSuperfluousIncludesProcess_StandardOutputMessage(object sender, string message)
        {
            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add(message);
            });
        }

        void StandardProcess_Message(object sender, string message)
        {
            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add(message);
            });
        }

        private bool IsInstalledCorrectly()
        {
            string path = System.Windows.Forms.Application.StartupPath;

            if (!File.Exists(path + "\\b2.exe") ||
                !File.Exists(path + "\\7z.dll") ||
                !File.Exists(path + "\\7z.exe") ||
                //!File.Exists(path + "\\build_qt4.8.2_x64_msvc10_2ndstep.txt") ||
                !Directory.Exists(path + "\\qt4.8.2vs2012patch") ||
                !Directory.Exists(path + "\\qt4.8.3vs2012patch")
                )
            {
                return false;
            }

            return true;
        }

        private void checkBoxBoostShowAdvancedSettings_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxBoostLibs.Visible = !groupBoxBoostLibs.Visible;
            groupBoxBoostVariants.Visible = !groupBoxBoostVariants.Visible;
        }

        private void checkBoxQtShowAdvancedSettings_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxQtLibs.Visible = !groupBoxQtLibs.Visible;
            groupBoxQtVariants.Visible = !groupBoxQtVariants.Visible;
        }

        private void buttonDetectSuperfluousIncludes_Click(object sender, EventArgs e)
        {
            DetectSuperfluousIncludesDescription desc = new DetectSuperfluousIncludesDescription();
            desc.compilerType = DetermineSelectedCompiler();

            //desc.destinationFolder = @"C:\build\BlueFramework";
            //desc.projectName = "BlueFramework.sln";

            desc.destinationFolder = textBoxDSIDestinationFolder.Text;
            desc.projectName = textBoxDSIProjectName.Text;
            desc.sourceFilePath = textBoxDSISourceFilePath.Text;

            DetectSuperfluousIncludes DetectSuperfluousIncludesProcess = new DetectSuperfluousIncludes(desc);

            DetectSuperfluousIncludesProcess.Message += new MessageEventHandler(StandardProcess_Message);
            DetectSuperfluousIncludesProcess.StandardOutputMessage += new StandardOutputMessageEventHandler(DetectSuperfluousIncludesProcess_StandardOutputMessage);
            DetectSuperfluousIncludesProcess.StandardErrorMessage += new StandardErrorMessageEventHandler(StandardProcess_StandardErrorMessage);
            DetectSuperfluousIncludesProcess.Finished += new FinishedEventHandler(StandardProcess_Finished);

            // Run build process
            Thread workerThread = new Thread(DetectSuperfluousIncludesProcess.FindSuperfluousIncludes);
            workerThread.Start();
        }

        private void checkedListBoxLibraries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!radioButtonBoostSpecific.Checked)
            {
                if (GetSelectedBoostLibraries2() != "")
                {
                    radioButtonBoostSpecific.Checked = true;
                }
            }
        }

        private void checkedListBoxQtLibraries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!radioButtonQtSpecific.Checked)
            {
                if (GetSelectedQtLibraries() != "")
                {
                    radioButtonQtSpecific.Checked = true;
                }
            }           
        }
        
        #region Eigen
        private void buttonBuildEigen_Click(object sender, EventArgs e)
        {
            // prevent that user clicks the build button two times
            buttonBuildEigen.Enabled = false;

            eCompiler compiler = DetermineSelectedCompiler();

            if (!Compiler.IsPresent(compiler))
            {
                ReportCompilerNotPresent(compiler);
                return; // Compiler not present we can not do anything useful
            }
            
            // Setup build process
            EigenBuildProcessDescripton ebpd = new EigenBuildProcessDescripton();
            ebpd.version = DetermineSelectedEigenVersion();
            ebpd.compilerType = compiler;
            ebpd.platform = DetermineSelectedPlatform();
            ebpd.destinationFolder = DetermineDestinationFolder();

            EigenBuildProcess EigenBuildProcess = new EigenBuildProcess(ebpd);
            EigenBuildProcess.Message += EigenBuildProcess_Message;
            EigenBuildProcess.StandardOutputMessage += EigenBuildProcess_StandardOutputMessage;
            EigenBuildProcess.StandardErrorMessage += EigenBuildProcess_StandardErrorMessage;
            EigenBuildProcess.Finished += EigenBuildProcess_Finished;

            // Run build process
            Thread workerThread = new Thread(EigenBuildProcess.DownloadAndBuild);
            workerThread.Start();
        }

        void EigenBuildProcess_Finished(object sender)
        {
            this.Invoke((MethodInvoker)delegate
            {
                buttonBuildEigen.Enabled = true;
            });
        }

        void EigenBuildProcess_StandardErrorMessage(object sender, string message)
        {
            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add(message);
            });
        }

        void EigenBuildProcess_StandardOutputMessage(object sender, string message)
        {
            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add(message);
            });
        }


        void EigenBuildProcess_Message(object sender, string message)
        {
            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add(message);
            });
        }
        #endregion

        #region libLAS
        
        private DateTime LibLASBuildStartTime = DateTime.Now;
        private DateTime LibLASBuildEndTime = DateTime.Now;
       
        private void buttonLibLASBuild_Click(object sender, EventArgs e)
        {
            LibLASBuildStartTime = DateTime.Now;

            eCompiler compiler = DetermineSelectedCompiler();

            if (!Compiler.IsPresent(compiler))
            {
                ReportCompilerNotPresent(compiler);
                return; // Compiler not present we can not do anything useful
            }

            // Lock GUI
            buttonLibLASBuild.Enabled = false;

            LibLASBuildProcessDescripton llbpd = new LibLASBuildProcessDescripton();
            llbpd.libLASVersion = DetermineSelectedLibLASVersion();
            llbpd.libLASSupportedBoostVersion = DetermineSelectedSupportedBoostVersion();
            llbpd.compilerType = compiler;
            llbpd.destinationFolder = DetermineDestinationFolder();
            llbpd.platform = DetermineSelectedPlatform();
            
            LibLASBuildProcess libLASBuildProcess = new LibLASBuildProcess(llbpd);

            libLASBuildProcess.Message += new MessageEventHandler(libLASBuildProcess_Message);
            libLASBuildProcess.StandardOutputMessage += new StandardOutputMessageEventHandler(libLASBuildProcess_StandardOutputMessage);
            libLASBuildProcess.StandardErrorMessage += new StandardErrorMessageEventHandler(libLASBuildProcess_StandardErrorMessage);
            libLASBuildProcess.Finished += new FinishedEventHandler(libLASBuildProcess_Finished);
            libLASBuildProcess.Failure += new FailureEventHandler(libLASBuildProcess_Failure);

            Thread workerThread = new Thread(libLASBuildProcess.DownloadAndBuild);
            workerThread.Start();

        }

        private eLibLASVersion DetermineSelectedLibLASVersion()
        {
            string text = "";
            this.Invoke((MethodInvoker)delegate()
            {
                text = comboBoxLibLASVersion.Text;
            });
            
            foreach (LibLASInfo libLAS in LibLASInfo.CreateInfoList())
            {
                if (LibLASInfo.TransformLibLASVersionToString(libLAS.Version) == text)
                    return libLAS.Version;
            }

            throw new Exception("Unknown libLAS version.");
        }

        private eLibLASSupportedBoostVersion DetermineSelectedSupportedBoostVersion()
        {
            string text = "";
            this.Invoke((MethodInvoker)delegate()
            {
                text = comboBoxLibLASBoostVersion.Text;
            });
            
            foreach (LibLASInfo libLAS in LibLASInfo.CreateInfoList())
            {
                if (LibLASInfo.TransformLibLASSupportedBoostVersionToString(libLAS.BoostVersion) == text)
                    return libLAS.BoostVersion;
            }

            throw new Exception("Unknown libLAS supported boost version.");
        }

        private void UnlockLibLASBuildButton()
        {
            this.Invoke((MethodInvoker)delegate()
            {
                this.buttonLibLASBuild.Enabled = true;
            });
        }

        void libLASBuildProcess_StandardErrorMessage(object sender, string message)
        {
            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add(message);
            });
        }

        void libLASBuildProcess_StandardOutputMessage(object sender, string message)
        {
            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add(message);
            });
        }

        void libLASBuildProcess_Finished(object sender)
        {
            UnlockLibLASBuildButton();

            LibLASBuildEndTime = DateTime.Now;
            TimeSpan time = LibLASBuildEndTime - LibLASBuildStartTime;

            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add("libLAS build time: " + time.ToString());
            });
        }

        void libLASBuildProcess_Failure(object sender)
        {
            UnlockLibLASBuildButton();
        }

        void libLASBuildProcess_Message(object sender, string message)
        {
            this.message(message);
        }

        #endregion

        #region PCL

        private DateTime PCLBuildStartTime = DateTime.Now;
        private DateTime PCLBuildEndTime = DateTime.Now;

        private void buttonPCLBuild_Click(object sender, EventArgs e)
        {
            PCLBuildStartTime = DateTime.Now;

            eCompiler compiler = DetermineSelectedCompiler();

            if (!Compiler.IsPresent(compiler))
            {
                ReportCompilerNotPresent(compiler);
                return; // Compiler not present we can not do anything useful
            }

            // Lock GUI
            buttonPCLBuild.Enabled = false;

            PCLBuildProcessDescripton pclbpd = new PCLBuildProcessDescripton();
            pclbpd.pclVersion = DetermineSelectedPCLVersion();
            pclbpd.compilerType = compiler;
            pclbpd.destinationFolder = DetermineDestinationFolder();
            pclbpd.platform = DetermineSelectedPlatform();
            pclbpd.pclSupportedBoostVersion = DetermineSelectedPCLSupportedBoostVersion();
            pclbpd.pclSupportedEigenVersion = DetermineSelectedPCLSupportedEigenVersion();
            pclbpd.pclSupportedFlannVersion = DetermineSelectedPCLSupportedFlannVersion();
            pclbpd.pclSupportedVTKVersion = DetermineSelectedPCLSupportedVTKVersion();   

            PCLBuildProcess pclBuildProcess = new PCLBuildProcess(pclbpd);

            pclBuildProcess.Message += new MessageEventHandler(pclBuildProcess_Message);
            pclBuildProcess.StandardOutputMessage += new StandardOutputMessageEventHandler(pclBuildProcess_StandardOutputMessage);
            pclBuildProcess.StandardErrorMessage += new StandardErrorMessageEventHandler(pclBuildProcess_StandardErrorMessage);
            pclBuildProcess.Finished += new FinishedEventHandler(pclBuildProcess_Finished);
            pclBuildProcess.Failure += new FailureEventHandler(pclBuildProcess_Failure);

            Thread workerThread = new Thread(pclBuildProcess.DownloadAndBuild);
            workerThread.Start();

        }

        private ePCLVersion DetermineSelectedPCLVersion()
        {
            string text = "";
            this.Invoke((MethodInvoker)delegate()
            {
                text = comboBoxPCLVersion.Text;
            });

            foreach (PCLInfo pcl in PCLInfo.CreateInfoList())
            {
                if (PCLInfo.TransformPCLVersionToString(pcl.Version) == text)
                    return pcl.Version;
            }

            throw new Exception("Unknown PCL version.");
        }

        private ePCLSupportedBoostVersion DetermineSelectedPCLSupportedBoostVersion()
        {
            string text = "";
            this.Invoke((MethodInvoker)delegate()
            {
                text = comboBoxPCLSupportedBoostVersion.Text;
            });

            foreach (BoostInfo boost in BoostInfo.CreateInfoList())
            {
                if (BoostInfo.BoostVersionToString(boost.Version) == text)
                    return (ePCLSupportedBoostVersion)boost.Version;
            }

            throw new Exception("Unknown PCL supported Boost version.");
        }

        private ePCLSupportedEigenVersion DetermineSelectedPCLSupportedEigenVersion()
        {
            string text = "";
            this.Invoke((MethodInvoker)delegate()
            {
                text = comboBoxPCLSupportedEigenVersion.Text;
            });

            foreach (EigenInfo eigen in EigenInfo.CreateInfoList())
            {
                if (EigenInfo.TransformVersionToString(eigen.Version) == text)
                    return (ePCLSupportedEigenVersion)eigen.Version;
            }

            throw new Exception("Unknown PCL supported Eigen version.");
        }

        private ePCLSupportedFlannVersion DetermineSelectedPCLSupportedFlannVersion()
        {
            string text = "";
            this.Invoke((MethodInvoker)delegate()
            {
                text = comboBoxPCLSupportedFlannVersion.Text;
            });

            foreach (FlannInfo flann in FlannInfo.CreateInfoList())
            {
                if (FlannInfo.TransformFlannVersionToString(flann.Version) == text)
                    return (ePCLSupportedFlannVersion)flann.Version;
            }

            throw new Exception("Unknown PCL supported Flann version.");
        }

        private ePCLSupportedVTKVersion DetermineSelectedPCLSupportedVTKVersion()
        {
            string text = "";
            this.Invoke((MethodInvoker)delegate()
            {
                text = comboBoxPCLSupportedVTKVersion.Text;
            });

            foreach (VTKInfo vtk in VTKInfo.CreateInfoList())
            {
                if (VTKInfo.TransformVTKVersionToString(vtk.Version) == text)
                    return (ePCLSupportedVTKVersion)vtk.Version;
            }

            throw new Exception("Unknown PCL supported VTK version.");
        }

        private void UnlockPCLBuildButton()
        {
            this.Invoke((MethodInvoker)delegate()
            {
                this.buttonPCLBuild.Enabled = true;
            });
        }

        void pclBuildProcess_StandardErrorMessage(object sender, string message)
        {
            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add(message);
            });
        }

        void pclBuildProcess_StandardOutputMessage(object sender, string message)
        {
            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add(message);
            });
        }

        void pclBuildProcess_Finished(object sender)
        {
            UnlockPCLBuildButton();

            PCLBuildEndTime = DateTime.Now;
            TimeSpan time = PCLBuildEndTime - PCLBuildStartTime;

            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add("PCL build time: " + time.ToString());
            });
        }

        void pclBuildProcess_Failure(object sender)
        {
            UnlockPCLBuildButton();
        }

        void pclBuildProcess_Message(object sender, string message)
        {
            this.message(message);
        }
        
        #endregion

        #region Flann

        private DateTime FlannBuildStartTime = DateTime.Now;
        private DateTime FlannBuildEndTime = DateTime.Now;

        private void buttonFlannBuild_Click(object sender, EventArgs e)
        {
            FlannBuildStartTime = DateTime.Now;

            eCompiler compiler = DetermineSelectedCompiler();

            if (!Compiler.IsPresent(compiler))
            {
                ReportCompilerNotPresent(compiler);
                return; // Compiler not present we can not do anything useful
            }

            // Lock GUI
            buttonFlannBuild.Enabled = false;

            FlannBuildProcessDescripton flannbpd = new FlannBuildProcessDescripton();
            flannbpd.flannVersion = DetermineSelectedFlannVersion();
            flannbpd.compilerType = compiler;
            flannbpd.destinationFolder = DetermineDestinationFolder();
            flannbpd.platform = DetermineSelectedPlatform();

            FlannBuildProcess flannBuildProcess = new FlannBuildProcess(flannbpd);

            flannBuildProcess.Message += new MessageEventHandler(flannBuildProcess_Message);
            flannBuildProcess.StandardOutputMessage += new StandardOutputMessageEventHandler(flannBuildProcess_StandardOutputMessage);
            flannBuildProcess.StandardErrorMessage += new StandardErrorMessageEventHandler(flannBuildProcess_StandardErrorMessage);
            flannBuildProcess.Finished += new FinishedEventHandler(flannBuildProcess_Finished);
            flannBuildProcess.Failure += new FailureEventHandler(flannBuildProcess_Failure);

            Thread workerThread = new Thread(flannBuildProcess.DownloadAndBuild);
            workerThread.Start();

        }

        private eFlannVersion DetermineSelectedFlannVersion()
        {
            string text = "";
            this.Invoke((MethodInvoker)delegate()
            {
                text = comboBoxFlannVersion.Text;
            });

            foreach (FlannInfo flann in FlannInfo.CreateInfoList())
            {
                if (FlannInfo.TransformFlannVersionToString(flann.Version) == text)
                    return flann.Version;
            }

            throw new Exception("Unknown Flann version.");
        }

        private void UnlockFlannBuildButton()
        {
            this.Invoke((MethodInvoker)delegate()
            {
                this.buttonFlannBuild.Enabled = true;
            });
        }

        void flannBuildProcess_StandardErrorMessage(object sender, string message)
        {
            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add(message);
            });
        }

        void flannBuildProcess_StandardOutputMessage(object sender, string message)
        {
            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add(message);
            });
        }

        void flannBuildProcess_Finished(object sender)
        {
            UnlockFlannBuildButton();

            FlannBuildEndTime = DateTime.Now;
            TimeSpan time = FlannBuildEndTime - FlannBuildStartTime;

            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add("Flann build time: " + time.ToString());
            });
        }

        void flannBuildProcess_Failure(object sender)
        {
            UnlockFlannBuildButton();
        }

        void flannBuildProcess_Message(object sender, string message)
        {
            this.message(message);
        }

        #endregion

        #region VTK

        private DateTime VTKBuildStartTime = DateTime.Now;
        private DateTime VTKBuildEndTime = DateTime.Now;

        private void buttonVTKBuild_Click(object sender, EventArgs e)
        {
            VTKBuildStartTime = DateTime.Now;

            eCompiler compiler = DetermineSelectedCompiler();

            if (!Compiler.IsPresent(compiler))
            {
                ReportCompilerNotPresent(compiler);
                return; // Compiler not present we can not do anything useful
            }

            // Lock GUI
            buttonVTKBuild.Enabled = false;

            VTKBuildProcessDescripton vtkbpd = new VTKBuildProcessDescripton();
            vtkbpd.vtkVersion = DetermineSelectedVTKVersion();
            vtkbpd.compilerType = compiler;
            vtkbpd.destinationFolder = DetermineDestinationFolder();
            vtkbpd.platform = DetermineSelectedPlatform();

            VTKBuildProcess vtkBuildProcess = new VTKBuildProcess(vtkbpd);

            vtkBuildProcess.Message += new MessageEventHandler(vtkBuildProcess_Message);
            vtkBuildProcess.StandardOutputMessage += new StandardOutputMessageEventHandler(vtkBuildProcess_StandardOutputMessage);
            vtkBuildProcess.StandardErrorMessage += new StandardErrorMessageEventHandler(vtkBuildProcess_StandardErrorMessage);
            vtkBuildProcess.Finished += new FinishedEventHandler(vtkBuildProcess_Finished);
            vtkBuildProcess.Failure += new FailureEventHandler(vtkBuildProcess_Failure);

            Thread workerThread = new Thread(vtkBuildProcess.DownloadAndBuild);
            workerThread.Start();

        }

        private eVTKVersion DetermineSelectedVTKVersion()
        {
            string text = "";
            this.Invoke((MethodInvoker)delegate()
            {
                text = comboBoxVTKVersion.Text;
            });

            foreach (VTKInfo vtk in VTKInfo.CreateInfoList())
            {
                if (VTKInfo.TransformVTKVersionToString(vtk.Version) == text)
                    return vtk.Version;
            }

            throw new Exception("Unknown VTK version.");
        }

        private void UnlockVTKBuildButton()
        {
            this.Invoke((MethodInvoker)delegate()
            {
                this.buttonVTKBuild.Enabled = true;
            });
        }

        void vtkBuildProcess_StandardErrorMessage(object sender, string message)
        {
            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add(message);
            });
        }

        void vtkBuildProcess_StandardOutputMessage(object sender, string message)
        {
            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add(message);
            });
        }

        void vtkBuildProcess_Finished(object sender)
        {
            UnlockVTKBuildButton();

            VTKBuildEndTime = DateTime.Now;
            TimeSpan time = VTKBuildEndTime - VTKBuildStartTime;

            this.Invoke((MethodInvoker)delegate
            {
                listBoxLogOutput.Items.Add("VTK build time: " + time.ToString());
            });
        }

        void vtkBuildProcess_Failure(object sender)
        {
            UnlockVTKBuildButton();
        }

        void vtkBuildProcess_Message(object sender, string message)
        {
            this.message(message);
        }

        #endregion

        private void linkLabelDetectSuberflousIncludes_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkLabelDetectSuberflousIncludes.Text);
        }

        private void buttonInstallCMake_Click(object sender, EventArgs e)
        {
            if (checkBoxCMake.Checked)
            {
                InstallCMake installCMake = new InstallCMake();

                installCMake.Message += new MessageEventHandler(StandardProcess_Message);
                installCMake.StandardOutputMessage += new StandardOutputMessageEventHandler(DetectSuperfluousIncludesProcess_StandardOutputMessage);
                installCMake.StandardErrorMessage += new StandardErrorMessageEventHandler(StandardProcess_StandardErrorMessage);
                installCMake.Finished += new FinishedEventHandler(StandardProcess_Finished);

                Thread workerThread = new Thread(installCMake.DownloadAndInstall);
                workerThread.Start();
            }
        }
    }
}