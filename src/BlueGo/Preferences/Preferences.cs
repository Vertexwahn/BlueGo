using System;
using System.IO;
using System.Xml;

namespace BlueGo
{
    class Preferences
    {
        #region E-Mail-Report
        /// <summary>
        /// Soll ein E-Mail Report gesendet werden?
        /// </summary>
        bool m_SendReportEmail = false;

        /// <summary>
        /// Benutzerkennung
        /// </summary>
        string m_User = "User@domain.de";

        /// <summary>
        /// Passwort
        /// </summary>
        string m_Password = "password";

        /// <summary>
        /// An wenn soll eine email gesendet werden?
        /// </summary>
        string m_Subscriber = "user@mail.com";

        bool m_DownloadThirdPartyLibEveryTime = true;

        private static string EncryptionPassword = "fsdfez65856-5634)df";

        /// <summary>
        /// Ausgangsmail Server
        /// </summary>
        string m_SmtpServer = "mail.domain.de";

        /// <summary>
        /// Port des Ausgangs-Mail-Servers
        /// </summary>
        string m_Port = "25";

        public bool DownloadThirdPartyLibEveryTime
        {
            get
            {
                return m_DownloadThirdPartyLibEveryTime;
            }
            set
            {
                m_DownloadThirdPartyLibEveryTime = value;
                m_bDirty = true;
            }
        }

        /// <summary>
        /// Ausgangsmail Server
        /// </summary>
        public string SmtpServer
        {
            get 
            {
                return m_SmtpServer; 
            }
            set 
            { 
                m_SmtpServer = value; 
                m_bDirty = true; 
            }
        }

        /// <summary>
        /// Benutzerkennung
        /// </summary>
        public string User
        {
            get 
            { 
                return m_User; 
            }
            set 
            { 
                m_User = value;
                m_bDirty = true;  
            }
        }

        /// <summary>
        /// Passwort
        /// </summary>
        public string Password
        {
            get 
            { 
                return m_Password; 
            }
            set
            { 
                m_Password = value; 
                m_bDirty = true;
            }
        }

         /// <summary>
        /// An wenn geht die E-Mail?
        /// </summary>
        public string Subscriber
        {
            get
            {
                return m_Subscriber;
            }
            set
            {
                m_Subscriber = value;
                m_bDirty = true;
            }
        }

        public bool SendReportEmail
        {
            get 
            { 
                return m_SendReportEmail; 
            }
            set
            { 
                m_SendReportEmail = value; 
                m_bDirty = true;
            }
        }

        /// <summary>
        /// Port des Ausgangs-Mail-Servers
        /// </summary>
        public string Port
        {
            get 
            {
                return m_Port; 
            }
            set
            {
                m_Port = value;  
                m_bDirty = true; 
            }
        }
        #endregion

        #region DownloadFolder
        /// <summary>
        /// Where to store downloaded applications?
        /// </summary>
        string m_ApplicationDownloadFolder = @"C:\thirdparty";

        /// <summary>
        /// Where to store thirdparty libraries?
        /// </summary>
        string m_ThirdPartyDownloadFolder = @"C:\thirdparty";

        public string ApplicationDownloadFolder
        {
            get 
            { 
                return m_ApplicationDownloadFolder; 
            }
            set 
            {
                m_ApplicationDownloadFolder = value;
                m_bDirty = true; 
            }
        }

        public string ThirdPartyDownloadFolder
        {
            get 
            { 
                return m_ThirdPartyDownloadFolder; 
            }
            set 
            { 
                m_ThirdPartyDownloadFolder = value;
                m_bDirty = true;
            }
        }
        #endregion

        #region Compiler
        string m_VS2010Location = @"C:\Program Files (x86)\Microsoft Visual Studio 10.0";
 
        string m_VS2012Location = @"C:\Program Files (x86)\Microsoft Visual Studio 11.0";

        string m_VS2013Location = @"C:\Program Files (x86)\Microsoft Visual Studio 12.0";

        string m_VS2015Location = @"C:\Program Files (x86)\Microsoft Visual Studio 14.0";

        string m_VS2019Location = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community";

        public string VS2010Location
        {
            get
            { 
                return m_VS2010Location;
            }
            set
            { 
                m_VS2010Location = value;
                m_bDirty = true;
            }
        }

        public string VS2012Location
        {
            get { return m_VS2012Location; }
            set 
            {
                m_VS2012Location = value;
                m_bDirty = true;
            }
        }

        public string VS2013Location
        {
            get { return m_VS2013Location; }
            set 
            {
                m_VS2013Location = value;
                m_bDirty = true;
            }
        }

        public string VS2015Location
        {
            get { return m_VS2015Location; }
            set
            {
                m_VS2015Location = value;
                m_bDirty = true;
            }
        }

        public string VS2019Location
        {
            get { return m_VS2019Location; }
            set
            {
                m_VS2019Location = value;
                m_bDirty = true;
            }
        }
        #endregion

        #region BuildDependencies

        string cmakeExeLocation = @"C:\Program Files (x86)\CMake 2.8\bin\cmake.exe";

        string msbuildExeLocation = @"C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe";

        public string CMakeExeLocation
        {
            get
            {
                return cmakeExeLocation;
            }
            set
            {
                cmakeExeLocation = value;
                m_bDirty = true;
            }
        }

        public string MSBuildExeLocation
        {
            get
            {
                return msbuildExeLocation;
            }
            set
            {
                msbuildExeLocation = value;
                m_bDirty = true;
            }
        }

        #endregion

        /// <summary>
        /// Did the user change the preferences?
        /// </summary>
        private bool m_bDirty = false;

        public Preferences()
        {
            // try to load preferences from file
            string BaseDir = System.Threading.Thread.GetDomain().BaseDirectory;

            if (!File.Exists(BaseDir + "preferences.xml"))
            {
      
            }
            else
            {
                Load(BaseDir + "preferences.xml");
            }
        }

        ~Preferences() 
        {
            if (m_bDirty == true)
            {
                // save changes
                string BaseDir = System.Threading.Thread.GetDomain().BaseDirectory;

                Save(BaseDir + "preferences.xml");
            }
        }

        private void Load(string filename)
        {
            XmlTextReader xmlReader = new XmlTextReader(filename);
           
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlReader);

            // E-Mail-Report
            XmlNode xmlEMailReport = xmlDoc.SelectSingleNode("Preferences/EMailReport");

            m_User = xmlEMailReport.Attributes["User"].InnerText;

            // encrypt password
            try
            {
                string password = BlueGo.Util.Cryptomat.DecryptMessage(
                    xmlEMailReport.Attributes["Password"].InnerText,
                    EncryptionPassword);


                m_Password = password;
            }
            catch
            {
                m_Password = "";
            }

            m_SmtpServer = xmlEMailReport.Attributes["SmtpServer"].InnerText;
            m_Port = xmlEMailReport.Attributes["Port"].InnerText;

            try
            {
                m_SendReportEmail = bool.Parse(xmlEMailReport.Attributes["SendReportEmail"].InnerText);
            }
            catch
            {
                m_SendReportEmail = false;
            }

            try
            {
                m_Subscriber = xmlEMailReport.Attributes["Subscriber"].InnerText;
            }
            catch
            {
                m_Subscriber = "subscriber@mail.com";
            }

            // Download Folder
            XmlNode xmlDownloadFolder = xmlDoc.SelectSingleNode("Preferences/DownloadFolder");

            try
            {
                m_ApplicationDownloadFolder = xmlDownloadFolder.Attributes["ApplicationDownloadFolder"].InnerText;
            }
            catch
            {
                m_ApplicationDownloadFolder = @"C:\thirdparty";
            }
            
            try
            {
                m_ThirdPartyDownloadFolder = xmlDownloadFolder.Attributes["ThirdPartyDownloadFolder"].InnerText;
            }
            catch
            {
                m_ThirdPartyDownloadFolder = @"C:\thirdparty";
            }

            // Compiler
            XmlNode xmlCompiler = xmlDoc.SelectSingleNode("Preferences/Compiler");

            try
            {
                m_VS2010Location = xmlCompiler.Attributes["VS2010Location"].InnerText;
            }
            catch
            {
                m_VS2010Location = @"C:\Program Files (x86)\Microsoft Visual Studio 10.0";
            }

            try
            {
                m_VS2012Location = xmlCompiler.Attributes["VS2012Location"].InnerText;
            }
            catch
            {
                m_VS2012Location = @"C:\Program Files (x86)\Microsoft Visual Studio 11.0";
            }

            try
            {
                m_VS2013Location = xmlCompiler.Attributes["VS2013Location"].InnerText;
            }
            catch
            {
                m_VS2013Location = @"C:\Program Files (x86)\Microsoft Visual Studio 12.0";
            }

            try
            {
                m_VS2013Location = xmlCompiler.Attributes["VS2015Location"].InnerText;
            }
            catch
            {
                m_VS2013Location = @"C:\Program Files (x86)\Microsoft Visual Studio 13.0";
            }

            // Build Dependencies
            XmlNode xmlBuildDependencies = xmlDoc.SelectSingleNode("Preferences/BuildDependencies");

            try
            {
                cmakeExeLocation = xmlCompiler.Attributes["CMakeExeLocation"].InnerText;
            }
            catch
            {
                cmakeExeLocation = @"C:\Program Files (x86)\CMake 2.8\bin\cmake.exe";
            }

            try
            {
                msbuildExeLocation = xmlCompiler.Attributes["MSBuildExeLocation"].InnerText;
            }
            catch
            {
                msbuildExeLocation = @"C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe";
            }

        }

        private void Save(string filename)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration declaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", "yes");

            xmlDoc.AppendChild(declaration);

            XmlNode root = xmlDoc.CreateNode(XmlNodeType.Element, "Preferences", "");

            // E-Mail-Report
            XmlElement xmlEmailReport = xmlDoc.CreateElement("EMailReport");    
            root.AppendChild(xmlEmailReport);

            xmlEmailReport.SetAttribute("SmtpServer", PreferencesManager.Instance.SmtpServer);
            xmlEmailReport.SetAttribute("Port", PreferencesManager.Instance.Port);
            xmlEmailReport.SetAttribute("User", PreferencesManager.Instance.User);

            // Passwort verschlüsselt speichern
            string password = BlueGo.Util.Cryptomat.EncryptMessage(
                PreferencesManager.Instance.Password, 
                EncryptionPassword);

            xmlEmailReport.SetAttribute("Password", password);

            xmlEmailReport.SetAttribute("SendReportEmail", m_SendReportEmail.ToString());

            xmlEmailReport.SetAttribute("Subscriber", m_Subscriber);

            // Download folder
            XmlElement xmlDownloadFolder = xmlDoc.CreateElement("DownloadFolder");
            xmlDownloadFolder.SetAttribute("ApplicationDownloadFolder", m_ApplicationDownloadFolder);
            xmlDownloadFolder.SetAttribute("ThirdPartyDownloadFolder", m_ThirdPartyDownloadFolder);
            root.AppendChild(xmlDownloadFolder);

            // Compiler
            XmlElement xmlCompiler = xmlDoc.CreateElement("Compiler");
            xmlCompiler.SetAttribute("VS2010Location", m_VS2010Location);
            xmlCompiler.SetAttribute("VS2012Location", m_VS2012Location);
            xmlCompiler.SetAttribute("VS2013Location", m_VS2013Location);
            xmlCompiler.SetAttribute("VS2015Location", m_VS2015Location);
            root.AppendChild(xmlCompiler);

            // Build Dependencies
            XmlElement xmlBuildDependencies = xmlDoc.CreateElement("BuildDependencies");
            xmlBuildDependencies.SetAttribute("CMakeExeLocation", cmakeExeLocation);
            xmlBuildDependencies.SetAttribute("MSBuildExeLocation", msbuildExeLocation);
            root.AppendChild(xmlBuildDependencies);
            
            // Einstellungen speichern
            xmlDoc.AppendChild(root);
            xmlDoc.Save(filename);
        }
    }
}