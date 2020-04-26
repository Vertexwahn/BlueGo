using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace BlueGo
{
    enum eOpenCASCADEVersion
    {
        OpenCASCADE_2_4_3
    }

    class OpenCASCADEInfo
    {
        OpenCASCADEInfo(string filename, string downloadURL, eOpenCASCADEVersion version)
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

        public BlueGo.eOpenCASCADEVersion Version
        {
            get { return version; }
            set { version = value; }
        }

        string zipFilename;
        string downloadURL;
        eOpenCASCADEVersion version;

        public static OpenCASCADEInfo GetInfo(eOpenCASCADEVersion version)
        {
            foreach (OpenCASCADEInfo info in CreateInfoList())
            {
                if (info.version == version)
                {
                    return new OpenCASCADEInfo(info.ZIPFilename, info.downloadURL, info.version); // hand back a copy
                }
            }

            throw new Exception("Unknown OpenCASCADE version.");
        }

        public static List<OpenCASCADEInfo> CreateInfoList()
        {
            List<OpenCASCADEInfo> list = new List<OpenCASCADEInfo>();

            list.Add(new OpenCASCADEInfo(
                "OpenCASCADE-2.4.3.zip",
                @"https://nodeload.github.com/Itseez/OpenCASCADE/zip/2.4.3",
                eOpenCASCADEVersion.OpenCASCADE_2_4_3
            ));

            return list;
        }

        public static string VersionToString(eOpenCASCADEVersion version)
        {
            switch (version)
            {
                case eOpenCASCADEVersion.OpenCASCADE_2_4_3:
                    return "2.4.3";
            }

            throw new Exception("Unknown OpenCASCADE version");
        }

        public static string GetDownloadURL(eOpenCASCADEVersion version)
        {
            foreach (OpenCASCADEInfo info in CreateInfoList())
            {
                if (info.version == version)
                {
                    return info.DownloadURL;
                }
            }

            throw new Exception("Unknown OpenCASCADE version.");
        }

        public static string GetZipFileName(eOpenCASCADEVersion version)
        {
            foreach (OpenCASCADEInfo bi in CreateInfoList())
            {
                if (bi.version == version)
                {
                    return bi.ZIPFilename;
                }
            }

            throw new Exception("Unknown OpenCASCADE version");
        }
    }

    class openCASCADEBuildProcessDescripton
    {
        public string destinationFolder;// Where to download and build boost?
        public eCompiler compilerType;
        public eOpenCASCADEVersion version;
        public int coreCount;
        public ePlatform platform;
        public string withLibraries; // empty or for instance " --with-filesystem --with-signals"
    }

    class OpenCASCADEBuildProcess : BuildProcess
    {
        public OpenCASCADEBuildProcess(openCASCADEBuildProcessDescripton bbpd)
        {
            destinationFolder = bbpd.destinationFolder;
            compilerType = bbpd.compilerType;
            boostVersion = bbpd.version;
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

                message("Downloading OpenCASCADE...");

                // get OpenCASCADE from
                // git clone ssh://gitolite@git.dev.opencascade.org/occt occt

                bool bGitAvailable = Executable.ExistsOnPath("git.exe");

                if (!bGitAvailable)
                    throw new Exception("Git is not available on path.");

                CloneCodeFromGitRepository();


                /*
                string boostDownloadURL = OpenCASCADEInfo.GetDownloadURL(boostVersion);
                string boostZIPFilename = OpenCASCADEInfo.GetZipFileName(boostVersion);

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
                */

                OnFinished();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CloneCodeFromGitRepository()
        {
            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo("cmd.exe");
            info.RedirectStandardInput = true;
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo = info;
            p.Start();

            using (StreamWriter sw = p.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("git.exe clone ssh://gitolite@git.dev.opencascade.org/occt occt");
                }
            }

            readStandardOutput(p);
            readStandardError(p);

            p.WaitForExit();
            p.Close();
        }
        string destinationFolder;// Where to download and build boost?
        eCompiler compilerType;
        eOpenCASCADEVersion boostVersion;
        int coreCount;
        ePlatform platform;
        string withLibraries;
    }
}
