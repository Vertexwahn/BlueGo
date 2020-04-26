using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace BlueGo
{
    enum eOpenSceneGraphVersion
    {
        OpenSceneGraph_3_0_1
    }

    class OpenSceneGraphInfo
    {
        OpenSceneGraphInfo(string filename, string downloadURL, eOpenSceneGraphVersion version)
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

        public BlueGo.eOpenSceneGraphVersion Version
        {
            get { return version; }
            set { version = value; }
        }

        string zipFilename;
        string downloadURL;
        eOpenSceneGraphVersion version;

        public static OpenSceneGraphInfo GetInfo(eOpenSceneGraphVersion version)
        {
            foreach (OpenSceneGraphInfo info in CreateInfoList())
            {
                if (info.version == version)
                {
                    return new OpenSceneGraphInfo(info.ZIPFilename, info.downloadURL, info.version); // hand back a copy
                }
            }

            throw new Exception("Unknown OpenSceneGraph version.");
        }

        public static List<OpenSceneGraphInfo> CreateInfoList()
        {
            List<OpenSceneGraphInfo> list = new List<OpenSceneGraphInfo>();

            list.Add(new OpenSceneGraphInfo(
                "OpenSceneGraph-3.0.1.zip",
                @"http://www.openscenegraph.org/downloads/stable_releases/OpenSceneGraph-3.0.1/source/OpenSceneGraph-3.0.1.zip",
                eOpenSceneGraphVersion.OpenSceneGraph_3_0_1
            ));

            return list;
        }

        public static string TransformVersionToString(eOpenSceneGraphVersion version)
        {
            switch (version)
            {
                case eOpenSceneGraphVersion.OpenSceneGraph_3_0_1:
                    return "3.0.1";
            }

            throw new Exception("Unknown OpenSceneGraph version");
        }

        public static string GetDownloadURL(eOpenSceneGraphVersion version)
        {
            foreach (OpenSceneGraphInfo info in CreateInfoList())
            {
                if (info.version == version)
                {
                    return info.DownloadURL;
                }
            }

            throw new Exception("Unknown OpenSceneGraph version.");
        }

        public static string GetZipFileName(eOpenSceneGraphVersion version)
        {
            foreach (OpenSceneGraphInfo bi in CreateInfoList())
            {
                if (bi.version == version)
                {
                    return bi.ZIPFilename;
                }
            }

            throw new Exception("Unknown OpenSceneGraph version");
        }
    }

    class OpenSceneGraphBuildProcessDescripton
    {
        public string destinationFolder;// Where to download and build boost?
        public eCompiler compilerType;
        public eOpenSceneGraphVersion version;
        public int coreCount;
        public ePlatform platform;
        public string withLibraries; // empty or for instance " --with-filesystem --with-signals"
    }

    class OpenSceneGraphBuildProcess : BuildProcess
    {
        public OpenSceneGraphBuildProcess(OpenSceneGraphBuildProcessDescripton bbpd)
        {
            destinationFolder = bbpd.destinationFolder;
            compilerType = bbpd.compilerType;
            version = bbpd.version;
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

                message("Downloading OpenSceneGraph...");

                string downloadURL = OpenSceneGraphInfo.GetDownloadURL(version);
                string ZIPFilename = OpenSceneGraphInfo.GetZipFileName(version);

                DownloadHelper.DownloadFileFromURL(downloadURL, destinationFolder + ZIPFilename);

                message("Start to unzip boost...");

                // Unzip Boost 
                SevenZip.Decompress(destinationFolder + "/" + ZIPFilename, destinationFolder);

                message("OpenSceneGraph has been unzipped!");
                message("start building OpenSceneGraph...");

                // Build OpenSceneGraph
                runCMake(destinationFolder);

                // now build release mode
                runMSBuild(destinationFolder, " /property:Configuration=Release");

                runMSBuild(destinationFolder);

                // remove downloaded file
                System.IO.File.Delete(destinationFolder + ZIPFilename);

                message("OpenSceneGraph successfully built!");

                OnFinished();
            }
            catch (Exception ex)
            {
                message(string.Empty);
                OnFailure();
                MessageBox.Show(ex.ToString());
            }
        }

        private void runCMake(string destinationFolder)
        {
            Process p = new Process();
            ProcessStartInfo info = CreateVisualStudioCommandPromptProcessStartInfo(compilerType, platform);

            info.RedirectStandardInput = true;
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;

            p.StartInfo = info;
            p.Start();

            using (StreamWriter sw = p.StandardInput)
            {
                string extractFolderName = OpenSceneGraphInfo.GetInfo(version).ExtractFolderName;

                if (sw.BaseStream.CanWrite)
                {
                    if (compilerType == eCompiler.VS2010)
                    {
                        sw.WriteLine("cd /D " + destinationFolder + extractFolderName); //"boost_1_51_0-x64");
                        string cmakeCommand = "cmake -G\"Visual Studio 10 Win64\" + -H" + destinationFolder + extractFolderName + " -B" + destinationFolder + extractFolderName;
                        sw.WriteLine(cmakeCommand);
                    }
                    else
                    {


                        sw.WriteLine("cd /D " + destinationFolder + extractFolderName); //"boost_1_51_0-x64");
                        string cmakeCommand = "cmake -G\"Visual Studio 11 Win64\" + -H" + destinationFolder + extractFolderName + " -B" + destinationFolder + extractFolderName;
                        sw.WriteLine(cmakeCommand);
                    }
                }
            }

            readStandardOutput(p);
            readStandardError(p);

            p.WaitForExit();
            p.Close();
        }
       
        private void runMSBuild(string destinationFolder, string arguments = "")
        {
            Process p = new Process();
            ProcessStartInfo info = CreateVisualStudioCommandPromptProcessStartInfo(compilerType, platform);

            info.RedirectStandardInput = true;
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;

            p.StartInfo = info;
            p.Start();

            using (StreamWriter sw = p.StandardInput)
            {    
                string extractFolderName = OpenSceneGraphInfo.GetInfo(version).ExtractFolderName;

                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("cd /D " + destinationFolder + extractFolderName);
                    sw.WriteLine("msbuild OpenSceneGraph.sln" + arguments);
                }
            }

            readStandardOutput(p);
            readStandardError(p);

            p.WaitForExit();
            p.Close();
        }

        string destinationFolder;// Where to download and build boost?
        eCompiler compilerType;
        eOpenSceneGraphVersion version;
        int coreCount;
        ePlatform platform;
        string withLibraries;
    }
}
