using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BlueGo.Util;

namespace BlueGo
{
    /// <summary>
    ///     Enum for VTK version supported by BlueGo
    /// </summary>
    enum eVTKVersion
    {
        VTK6_1_0
    }

    /// <summary>
    ///     VTKInfo class details about VTK zip file, version, download url
    /// </summary>
    class VTKInfo
    {
        #region Fields -------------------------------------------------------------------------------------------------------------------------

        #region Private ------------------------------------------------------------------------------------------------------------------------

        private string zipFilename;
        private string downloadURL;
        private eVTKVersion version;

        #endregion//Private

        #endregion //Fields

        #region Constructor --------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     VTKInfo constructor
        /// </summary>
        /// <param name="filename">
        ///     VTK zip file to be downloaded
        /// </param>
        /// <param name="downloadURL">
        ///     VTK download URL string
        /// </param>
        /// <param name="version">
        ///     VTK version to be downloaded
        /// </param>
        VTKInfo(string filename, string downloadURL, eVTKVersion version)
        {
            this.zipFilename = filename;
            this.downloadURL = downloadURL;
            this.version = version;
        }

        #endregion //Constructor

        #region Properties ---------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     Gets & Sets VTK download URL
        /// </summary>
        public string DownloadURL
        {
            get { return downloadURL; }
            set { downloadURL = value; }
        }

        /// <summary>
        ///     Gets & Sets VTK zip file name to download
        /// </summary>
        public string ZIPFilename
        {
            get { return zipFilename; }
            set { zipFilename = value; }
        }

        /// <summary>
        ///     Gets folder name from downloaded VTK zip file, where VTK to be extracted
        /// </summary>
        public string ExtractFolderName
        {
            get
            {
                return zipFilename.Substring(0, zipFilename.Length - 4);
            }
        }

        /// <summary>
        ///     Gets & Sets VTK version
        /// </summary>
        public BlueGo.eVTKVersion Version
        {
            get { return version; }
            set { version = value; }
        }

        #endregion //Properties

        #region Methods ------------------------------------------------------------------------------------------------------------------------

        #region Public -------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     GetVTKInfo method returns a new VTKInfo instance.
        /// </summary>
        /// <param name="version">
        ///     VTK version
        /// </param>
        /// <returns>
        ///     A new VTKInfo instance with matching VTK version
        /// </returns>
        public static VTKInfo GetVTKInfo(eVTKVersion version)
        {
            foreach (VTKInfo info in CreateInfoList())
            {
                if (info.version == version)
                {
                    return new VTKInfo(info.ZIPFilename, info.downloadURL, info.version); // hand back a copy
                }
            }

            throw new Exception("Unknown VTK version.");
        }

        /// <summary>
        ///     CreateInfoList method creates supported VTKInfo list containing zip file name, download URL, version
        /// </summary>
        /// <returns>
        ///     A List of supported VTK information supported by BlueGo
        /// </returns>
        public static List<VTKInfo> CreateInfoList()
        {
            List<VTKInfo> list = new List<VTKInfo>();

            list.Add(new VTKInfo(
                "VTK-6.1.0.zip",
                @"http://www.vtk.org/files/release/6.1/VTK-6.1.0.zip",
                eVTKVersion.VTK6_1_0
            ));

            return list;
        }

        /// <summary>
        ///     TransformVTKVersionToString method transforms enum value of VTK version to VTK version string
        /// </summary>
        /// <param name="version">
        ///     Enum value of VTK version 
        /// </param>
        /// <returns>
        ///     String value of enum VTK version
        /// </returns>
        public static string TransformVTKVersionToString(eVTKVersion version)
        {
            switch (version)
            {
                case eVTKVersion.VTK6_1_0:
                    return "6.1.0";
            }

            throw new Exception("Unknown VTK version");
        }

        /// <summary>
        ///     GetDownloadURL method gets the download URL for requested VTK version from VTKInfo list
        /// </summary>
        /// <param name="version">
        ///     VTK version for which download URL is required
        /// </param>
        /// <returns>
        ///     Download VTK URL for requested VTK version
        /// </returns>
        public static string GetDownloadURL(eVTKVersion version)
        {
            foreach (VTKInfo info in CreateInfoList())
            {
                if (info.version == version)
                {
                    return info.DownloadURL;
                }
            }

            throw new Exception("Unknown VTK version.");
        }

        /// <summary>
        ///     GetVTKZipFileName method gets the VTK zip file name for requested VTK version
        /// </summary>
        /// <param name="version">
        ///     VTK version for which zip file name is requested
        /// </param>
        /// <returns>
        ///     VTK zip file name for matching VTK version from VTK information list
        /// </returns>
        public static string GetVTKZipFileName(eVTKVersion version)
        {
            foreach (VTKInfo lib in CreateInfoList())
            {
                if (lib.version == version)
                {
                    return lib.ZIPFilename;
                }
            }

            throw new Exception("Unknown VTK version");
        }

        #endregion //Public

        #endregion //Methods
    }

    /// <summary>
    ///     VTKBuildProcessDescripton class contains properties that describe VTK build process
    /// </summary>
    class VTKBuildProcessDescripton
    {
        #region Fields -------------------------------------------------------------------------------------------------------------------------

        #region Public -------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     Destination folder where VTK is downloaded and built
        /// </summary>
        public string destinationFolder;

        /// <summary>
        ///     Compiler type as selected for VTK build
        /// </summary>
        public eCompiler compilerType;

        /// <summary>
        ///     VTK version as selected for build
        /// </summary>
        public eVTKVersion vtkVersion;

        /// <summary>
        ///     Platform(x86, x64) type as selected for VTK build
        /// </summary>
        public ePlatform platform;

        #endregion //Public

        #endregion //Fields
    }

    /// <summary>
    ///     VTKBuildProcess class builds VTK using CMake and MSBuild
    /// </summary>
    class VTKBuildProcess : BuildProcess
    {
        #region Fields -------------------------------------------------------------------------------------------------------------------------

        #region Private ------------------------------------------------------------------------------------------------------------------------

        private string destinationFolder;
        private eCompiler compilerType;
        private eVTKVersion vtkVersion;
        private ePlatform platform;

        #endregion //Private

        #endregion //Fields

        #region Constructor --------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     VTKBuildProcess constructor
        /// </summary>
        /// <param name="llbpd">
        ///     VTKBuildProcessDescripton describes VTK build parameters
        /// </param>
        public VTKBuildProcess(VTKBuildProcessDescripton vtkbpd)
        {
            destinationFolder = vtkbpd.destinationFolder;
            compilerType = vtkbpd.compilerType;
            vtkVersion = vtkbpd.vtkVersion;
            platform = vtkbpd.platform;
        }

        #endregion //Constructor

        #region Methods ------------------------------------------------------------------------------------------------------------------------

        #region Public -------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     DownloadAndBuild method downloads selected VTK version and builds it for selected parameters (i.e. x64
        ///     and Visual Studio 13)
        /// </summary>
        public void DownloadAndBuild()
        {
            try
            {
                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                message("Checking Dependencies...");

                DependencyChecker.CheckCommanDependency();

                message("Checking Dependencies done!");

                message("Downloading VTK...");

                string vtkDownloadURL = VTKInfo.GetDownloadURL(vtkVersion);
                string vtkZIPFilename = VTKInfo.GetVTKZipFileName(vtkVersion);

                DownloadHelper.DownloadFileFromURL(vtkDownloadURL, destinationFolder + vtkZIPFilename);

                message("Start to unzip VTK...");

                // Unzip VTK
                SevenZip.Decompress(destinationFolder + "/" + vtkZIPFilename, destinationFolder);

                message("VTK has been unzipped!");

                message("Building VTK...");

                runCMake(destinationFolder);
                runMSBuild(destinationFolder, " /property:Configuration=" + Folder.ReleaseFolderName);

                message("VTK successfully built!");

                // remove downloaded file
                if (File.Exists(Path.Combine(destinationFolder, vtkZIPFilename)))
                {
                    System.IO.File.Delete(Path.Combine(destinationFolder, vtkZIPFilename));
                }

                OnFinished();
            }
            catch (Exception ex)
            {
                message(string.Empty);
                OnFailure();
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion //Public

        #region Private ------------------------------------------------------------------------------------------------------------------------

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
                string extractFolderName = VTKInfo.GetVTKInfo(vtkVersion).ExtractFolderName;
                
                if (sw.BaseStream.CanWrite)
                {
                    string cmakeCommand = string.Empty;
                    string cmakeGeneratorName = string.Empty;

                    if (compilerType == eCompiler.VS2013)
                    {
                        if (platform == ePlatform.x64)
                        {
                            cmakeGeneratorName = string.Concat(Helper.GetVisualStudioProductName(eVisualStudioProduct.Visual_Studio_12_2013), " ", "Win64");
                        }
                        else if (platform == ePlatform.x86)
                        {
                            cmakeGeneratorName = Helper.GetVisualStudioProductName(eVisualStudioProduct.Visual_Studio_12_2013);
                        }

                        sw.WriteLine("cd /D " + destinationFolder + extractFolderName);

                        cmakeCommand = "\"" + Path.GetFullPath(Executable.cmakeExePath) + "\"" +
                            " -G\"" + cmakeGeneratorName + "\"" +
                            " -H\"" + Path.GetFullPath(Path.Combine(destinationFolder, extractFolderName)) + "\"" +
                            " -B\"" + Path.GetFullPath(Path.Combine(destinationFolder, extractFolderName, Folder.BinFolderName)) + "\"";

                        sw.WriteLine(cmakeCommand);
                    }
                    else if (compilerType == eCompiler.VS2012)
                    {
                        if (platform == ePlatform.x64)
                        {
                            cmakeGeneratorName = string.Concat(Helper.GetVisualStudioProductName(eVisualStudioProduct.Visual_Studio_11_2012), " ", "Win64");
                        }
                        else if (platform == ePlatform.x86)
                        {
                            cmakeGeneratorName = Helper.GetVisualStudioProductName(eVisualStudioProduct.Visual_Studio_11_2012);
                        }

                        sw.WriteLine("cd /D " + destinationFolder + extractFolderName);

                        cmakeCommand = "\"" + Path.GetFullPath(Executable.cmakeExePath) + "\"" +
                            " -G\"" + cmakeGeneratorName + "\"" +
                            " -H\"" + Path.GetFullPath(Path.Combine(destinationFolder, extractFolderName)) + "\"" +
                            " -B\"" + Path.GetFullPath(Path.Combine(destinationFolder, extractFolderName, Folder.BinFolderName)) + "\"";

                        sw.WriteLine(cmakeCommand);
                    }
                    else if (compilerType == eCompiler.VS2010)
                    {
                        if (platform == ePlatform.x64)
                        {
                            cmakeGeneratorName = string.Concat(Helper.GetVisualStudioProductName(eVisualStudioProduct.Visual_Studio_10_2010), " ", "Win64");
                        }
                        else if (platform == ePlatform.x86)
                        {
                            cmakeGeneratorName = Helper.GetVisualStudioProductName(eVisualStudioProduct.Visual_Studio_10_2010);
                        }

                        sw.WriteLine("cd /D " + destinationFolder + extractFolderName);

                        cmakeCommand = "\"" + Path.GetFullPath(Executable.cmakeExePath) + "\"" +
                            " -G\"" + cmakeGeneratorName + "\"" +
                            " -H\"" + Path.GetFullPath(Path.Combine(destinationFolder, extractFolderName)) + "\"" +
                            " -B\"" + Path.GetFullPath(Path.Combine(destinationFolder, extractFolderName, Folder.BinFolderName)) + "\"";

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
                string extractFolderName = VTKInfo.GetVTKInfo(vtkVersion).ExtractFolderName;

                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("cd /D " + Path.GetFullPath(Path.Combine(destinationFolder, extractFolderName, Folder.BinFolderName)));
                    sw.WriteLine("\"" + Executable.msbuildExePath + "\"" + " VTK.sln" + arguments);
                }
            }

            readStandardOutput(p);
            readStandardError(p);

            p.WaitForExit();
            p.Close();
        }       

        #endregion //Private

        #endregion //Methods
    }
}