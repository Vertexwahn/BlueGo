using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace BlueGo.Data
{
    class ApplicationInfo
    {
        public static void Save(string filename, List<ApplicationInfo> ail)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration declaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", "yes");

            xmlDoc.AppendChild(declaration);

            XmlNode root = xmlDoc.CreateNode(XmlNodeType.Element, "Applications", "");

            foreach(ApplicationInfo ai in ail)
            {  
                XmlElement xmlApplicationInfo = xmlDoc.CreateElement("ApplicationInfo");    
                root.AppendChild(xmlApplicationInfo);

                xmlApplicationInfo.SetAttribute("Name",        ai.m_Name);
                xmlApplicationInfo.SetAttribute("DownloadUrl", ai.m_DownloadUrl);
                xmlApplicationInfo.SetAttribute("Description", ai.m_Description);
                xmlApplicationInfo.SetAttribute("Filename",    ai.m_Filename);
            }
         

            // Einstellungen speichern
            xmlDoc.AppendChild(root);
            xmlDoc.Save(filename);
        }

        static public List<ApplicationInfo> Load(string filename)
        {
            List<ApplicationInfo> list = new List<ApplicationInfo>();

            if(!System.IO.File.Exists(filename))
            {
                return list;
            }

            XmlTextReader xmlReader = new XmlTextReader(filename);
           
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlReader);

            // E-Mail-Report
            XmlNodeList xmlApplicationInfoList = xmlDoc.SelectNodes("Applications/ApplicationInfo");
            foreach (XmlNode xmlAIN in xmlApplicationInfoList)
            {
                string name = xmlAIN.Attributes["Name"].InnerText;
                string url = xmlAIN.Attributes["DownloadUrl"].InnerText;
                string description = xmlAIN.Attributes["Description"].InnerText;
                string filenameXML = xmlAIN.Attributes["Filename"].InnerText;

                string platformXML = "unknown";
                if (xmlAIN.Attributes["Platform"] != null)
                {
                    platformXML = xmlAIN.Attributes["Platform"].InnerText;
                }

                string ideXML = "none";
                if (xmlAIN.Attributes["IDE"] != null)
                {
                    ideXML = xmlAIN.Attributes["IDE"].InnerText;
                }

                ApplicationInfo ai = new ApplicationInfo(name, url, filenameXML, description, platformXML, ideXML);

                list.Add(ai);
            }

            return list;
        }

        public ApplicationInfo(string name, string downloadUrl)
        {
            m_Name = name;
            m_DownloadUrl = downloadUrl;
            m_Description = "";
            m_Filename = GetFilenameFromUrl(downloadUrl);
            m_Platfom = "unknown";
            m_IDE = "none";
        }

        public ApplicationInfo(string name, string downloadUrl, string filename, string description)
        {
            m_Name = name;
            m_DownloadUrl = downloadUrl;
            m_Description = description;
            m_Filename = filename;
            m_Platfom = "unknown";
            m_IDE = "none";
        }

        public ApplicationInfo(string name, string downloadUrl, string filename, string description, string platform, string ide)
        {
            m_Name = name;
            m_DownloadUrl = downloadUrl;
            m_Description = description;
            m_Filename = filename;
            m_Platfom = platform;
            m_IDE = ide;
        }

        public ApplicationInfo(string name, string downloadUrl, string filename)
        {
            m_Name = name;
            m_DownloadUrl = downloadUrl;
            m_Description = "";
            m_Filename = filename;
            m_Platfom = "unknown";
            m_IDE = "none";
        }
    
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }

        public string DownloadUrl
        {
            get { return m_DownloadUrl; }
            set { m_DownloadUrl = value; }
        }

         public string Filename
        {
            get { return m_Filename; }
            set { m_Filename = value; }
        }

        private string GetFilenameFromUrl(string url)
        {
             int index = url.LastIndexOf(@"/");
             return url.Substring(index+1);
        }

        public string Platfom
        {
            get { return m_Platfom; }
            set { m_Platfom = value; }
        }

        public string IDE
        {
            get { return m_IDE; }
            set { m_IDE = value; }
        }

        string m_Name;
        string m_DownloadUrl;
        string m_Description;
        string m_Filename;
        string m_Platfom;
        string m_IDE;
    }
}
