using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace BlueGo
{
    enum eEigenVersion
    {
        Eigen_3_2_0,
        Eigen_3_2_1,
        Eigen_3_2_2,
        Eigen_3_2_8,
        Eigen_3_2_9,
        Eigen_3_2_10,
        Eigen_3_3_0,
        Eigen_3_3_1
    }

    class EigenInfo
    {
        EigenInfo(string filename, string downloadURL, eEigenVersion version)
        {
            this.zipFilename = filename;
            this.downloadURL = downloadURL;
            this.version = version;
        }

        public string DownloadURL
        {
            get { return downloadURL; }
            set { downloadURL = value; }
        }

        public string ZIPFilename
        {
            get { return zipFilename; }
            set { zipFilename = value; }
        }

        public string ExtractFolderName
        {
            get
            {
                return zipFilename.Substring(0, zipFilename.Length - 4);
            }
        }

        public BlueGo.eEigenVersion Version
        {
            get { return version; }
            set { version = value; }
        }

        string zipFilename;
        string downloadURL;
        eEigenVersion version;

        public static EigenInfo GetInfo(eEigenVersion version)
        {
            foreach (EigenInfo info in CreateInfoList())
            {
                if (info.version == version)
                {
                    return new EigenInfo(info.ZIPFilename, info.downloadURL, info.version); // hand back a copy
                }
            }

            throw new Exception("Unknown Eigen version.");
        }

        public static List<EigenInfo> CreateInfoList()
        {
            List<EigenInfo> list = new List<EigenInfo>();

            list.Add(new EigenInfo(
                "3.2.0.zip",
                @"http://bitbucket.org/eigen/eigen/get/3.2.0.zip",
                eEigenVersion.Eigen_3_2_0
            ));

            list.Add(new EigenInfo(
                "3.2.1.zip",
                @"http://bitbucket.org/eigen/eigen/get/3.2.1.zip",
                eEigenVersion.Eigen_3_2_1
            ));

            list.Add(new EigenInfo(
                "3.2.2.zip",
                @"http://bitbucket.org/eigen/eigen/get/3.2.2.zip",
                eEigenVersion.Eigen_3_2_2
            ));
            
            list.Add(new EigenInfo(
                "3.2.8.zip",
                @"http://bitbucket.org/eigen/eigen/get/3.2.8.zip",
                eEigenVersion.Eigen_3_2_8
            ));

            list.Add(new EigenInfo(
                "3.2.9.zip",
                @"http://bitbucket.org/eigen/eigen/get/3.2.9.zip",
                eEigenVersion.Eigen_3_2_9
            ));

            list.Add(new EigenInfo(
               "3.2.10.zip",
               @"http://bitbucket.org/eigen/eigen/get/3.2.10.zip",
               eEigenVersion.Eigen_3_2_10
           ));

           list.Add(new EigenInfo(
               "3.3.0.zip",
               @"http://bitbucket.org/eigen/eigen/get/3.3.0.zip",
               eEigenVersion.Eigen_3_3_0
           ));


            list.Add(new EigenInfo(
             "3.3.1.zip",
             @"http://bitbucket.org/eigen/eigen/get/3.3.1.zip",
             eEigenVersion.Eigen_3_3_1
         ));

            return list;
        }

        public static string TransformVersionToString(eEigenVersion version)
        {
            switch (version)
            {
                case eEigenVersion.Eigen_3_2_0:
                    return "3.2.0";

                case eEigenVersion.Eigen_3_2_1:
                    return "3.2.1";

                case eEigenVersion.Eigen_3_2_2:
                    return "3.2.2";

                case eEigenVersion.Eigen_3_2_8:
                    return "3.2.8";

                case eEigenVersion.Eigen_3_2_9:
                    return "3.2.9";

                case eEigenVersion.Eigen_3_2_10:
                    return "3.2.10";

                case eEigenVersion.Eigen_3_3_0:
                    return "3.3.0";

                case eEigenVersion.Eigen_3_3_1:
                    return "3.3.1";
            }

            throw new Exception("Unknown Eigen version");
        }

        public static string GetDownloadURL(eEigenVersion version)
        {
            foreach (EigenInfo info in CreateInfoList())
            {
                if (info.version == version)
                {
                    return info.DownloadURL;
                }
            }

            throw new Exception("Unknown Eigen version.");
        }

        public static string GetZipFileName(eEigenVersion version)
        {
            foreach (EigenInfo bi in CreateInfoList())
            {
                if (bi.version == version)
                {
                    return bi.ZIPFilename;
                }
            }

            throw new Exception("Unknown Eigen version");
        }
    }

    class EigenBuildProcessDescripton
    {
        public string destinationFolder;// Where to download and build boost?
        public eCompiler compilerType;
        public eEigenVersion version;
        public ePlatform platform;
    }

    class EigenBuildProcess : BuildProcess
    {
        public EigenBuildProcess(EigenBuildProcessDescripton bbpd)
        {
            destinationFolder = bbpd.destinationFolder;
            compilerType = bbpd.compilerType;
            version = bbpd.version;
            platform = bbpd.platform;
        }

        string FindEigenDirectory(string destinationFolder)
        {
            // find eigen folder
            foreach (var dir in Directory.EnumerateDirectories(destinationFolder))
            {
                string path = dir.ToString();
                if (path.Contains("eigen-eigen-"))
                {
                    return path;
                }
            }

            return "/eigen-eigen-ffa86ffb5570";
        }

        public void DownloadAndBuild()
        {
            try
            {
                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                message("Downloading Eigen...");

              
                string downloadURL = EigenInfo.GetDownloadURL(version);
                string eigenZIPFilename = EigenInfo.GetZipFileName(version);

                DownloadHelper.DownloadFileFromURL(downloadURL, destinationFolder + eigenZIPFilename);

                message("Start to unzip...");

                // Unzip Boost 
                SevenZip.Decompress(destinationFolder + eigenZIPFilename, destinationFolder);

                message("file has been unzipped!");

                string strVersion = EigenInfo.TransformVersionToString(version);
                
                Directory.Move(FindEigenDirectory(destinationFolder), destinationFolder +  "/Eigen_" + strVersion);

                message("start building...");
  
                // remove downloaded file
                if (File.Exists(Path.Combine(destinationFolder, eigenZIPFilename)))
                {
                    System.IO.File.Delete(Path.Combine(destinationFolder, eigenZIPFilename));
                }

                message("Eigen successfully built!");
    

                OnFinished();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        string destinationFolder;// Where to download and build boost?
        eCompiler compilerType;
        eEigenVersion version;
        ePlatform platform;
    }
}
