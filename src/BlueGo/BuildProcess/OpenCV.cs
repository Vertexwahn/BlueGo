using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace BlueGo
{
    enum eOpenCVVersion
    {
        OpenCV_2_4_3
    }

    class OpenCVInfo
    {
        OpenCVInfo(string filename, string downloadURL, eOpenCVVersion version)
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

        public BlueGo.eOpenCVVersion Version
        {
            get { return version; }
            set { version = value; }
        }

        string zipFilename;
        string downloadURL;
        eOpenCVVersion version;

        public static OpenCVInfo GetInfo(eOpenCVVersion version)
        {
            foreach (OpenCVInfo info in CreateInfoList())
            {
                if (info.version == version)
                {
                    return new OpenCVInfo(info.ZIPFilename, info.downloadURL, info.version); // hand back a copy
                }
            }

            throw new Exception("Unknown OpenCV version.");
        }

        public static List<OpenCVInfo> CreateInfoList()
        {
            List<OpenCVInfo> list = new List<OpenCVInfo>();

            list.Add(new OpenCVInfo(
                "opencv-2.4.3.zip",
                @"https://nodeload.github.com/Itseez/opencv/zip/2.4.3",
                eOpenCVVersion.OpenCV_2_4_3
            ));

            return list;
        }

        public static string VersionToString(eOpenCVVersion version)
        {
            switch (version)
            {
                case eOpenCVVersion.OpenCV_2_4_3:
                    return "2.4.3";
            }

            throw new Exception("Unknown OpenCV version");
        }

        public static string GetDownloadURL(eOpenCVVersion version)
        {
            foreach (OpenCVInfo info in CreateInfoList())
            {
                if (info.version == version)
                {
                    return info.DownloadURL;
                }
            }

            throw new Exception("Unknown OpenCV version.");
        }

        public static string GetZipFileName(eOpenCVVersion version)
        {
            foreach (OpenCVInfo bi in CreateInfoList())
            {
                if (bi.version == version)
                {
                    return bi.ZIPFilename;
                }
            }

            throw new Exception("Unknown OpenCV version");
        }
    }

    class OpenCVBuildProcessDescripton
    {
        public string destinationFolder;// Where to download and build boost?
        public eCompiler compilerType;
        public eOpenCVVersion boostVersion;
        public int coreCount;
        public ePlatform platform;
        public string withLibraries; // empty or for instance " --with-filesystem --with-signals"
    }

    class OpenCVBuildProcess : BuildProcess
    {
        public OpenCVBuildProcess(OpenCVBuildProcessDescripton bbpd)
        {
            destinationFolder = bbpd.destinationFolder;
            compilerType = bbpd.compilerType;
            boostVersion = bbpd.boostVersion;
            coreCount = bbpd.coreCount;
            platform = bbpd.platform;
            withLibraries = bbpd.withLibraries;
        }

        public void DownloadAndBuild()
        {
            try
            {
                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                message("Downloading OpenCV...");

                string boostDownloadURL = OpenCVInfo.GetDownloadURL(boostVersion);
                string boostZIPFilename = OpenCVInfo.GetZipFileName(boostVersion);

                DownloadHelper.DownloadFileFromURL(boostDownloadURL, destinationFolder + boostZIPFilename);

                message("Start to unzip boost...");

                // Unzip Boost 
                SevenZip.Decompress(destinationFolder + "/" + boostZIPFilename, destinationFolder);

                message("boost has been unzipped!");
                message("start building boost...");

                // Build boost
                

                // remove downloaded file
                System.IO.File.Delete(destinationFolder + boostZIPFilename);

                message("boost successfully built!");


                OnFinished();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        string destinationFolder;// Where to download and build boost?
        eCompiler compilerType;
        eOpenCVVersion boostVersion;
        int coreCount;
        ePlatform platform;
        string withLibraries;
    }
}
