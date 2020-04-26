using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using BlueGo.GUI;
using BlueGo.Util;

namespace BlueGo
{
    /// <summary>
    ///     Enum for PCL version supported by BlueGo
    /// </summary>
    enum ePCLVersion
    {
        PCL_1_7_1,
        FromSourceGIT
    }

    /// <summary>
    ///     Enum for supported Boost version for PCL
    /// </summary>
    enum ePCLSupportedBoostVersion
    {
        PCLBoost_1_55_0,
        PCLBoostFromSourceSVN
    }

    /// <summary>
    ///     Enum for supported Eigen version for PCL
    /// </summary>
    enum ePCLSupportedEigenVersion
    {
        PCLEigen_3_2_0,
        PCLEigen_3_2_1
    }

    /// <summary>
    ///     Enum for supported Flann version for PCL
    /// </summary>
    enum ePCLSupportedFlannVersion
    {
        PCLFlann_1_8_4
    }

    /// <summary>
    ///     Enum for supported Flann version for PCL
    /// </summary>
    enum ePCLSupportedVTKVersion
    {
        PCLVTK_6_1_0
    }
    
    /// <summary>
    ///     PCLInfo class details about PCL zip file, version, download url, mandatory dependencies
    /// </summary>
    class PCLInfo
    {
        #region Fields -------------------------------------------------------------------------------------------------------------------------

        #region Private ------------------------------------------------------------------------------------------------------------------------

        private string zipFilename;
        private string downloadURL;
        private ePCLVersion pclVersion;
        
        #endregion//Private

        #endregion //Fields

        #region Constructor --------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     PCLInfo constructor
        /// </summary>
        /// <param name="filename">
        ///     PCL zip file to be downloaded
        /// </param>
        /// <param name="downloadURL">
        ///     PCL download URL string
        /// </param>
        /// <param name="pclVersion">
        ///     PCL version to be downloaded
        /// </param>
        PCLInfo(string filename, string downloadURL, ePCLVersion pclVersion)
        {
            this.zipFilename = filename;
            this.downloadURL = downloadURL;
            this.pclVersion = pclVersion;
        }

        #endregion //Constructor

        #region Properties ---------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     Gets & Sets PCL download URL
        /// </summary>
        public string DownloadURL
        {
            get { return downloadURL; }
            set { downloadURL = value; }
        }

        /// <summary>
        ///     Gets & Sets PCL zip file name to download
        /// </summary>
        public string ZIPFilename
        {
            get { return zipFilename; }
            set { zipFilename = value; }
        }

        /// <summary>
        ///     Gets folder name from downloaded PCL zip file, where PCL to be extracted
        /// </summary>
        public string ExtractFolderName
        {
            get
            {
                return zipFilename.Substring(0, zipFilename.Length - 4);
            }
        }

        /// <summary>
        ///     Gets & Sets PCL version
        /// </summary>
        public BlueGo.ePCLVersion Version
        {
            get { return pclVersion; }
            set { pclVersion = value; }
        }
                
        #endregion //Properties

        #region Methods ------------------------------------------------------------------------------------------------------------------------

        #region Public -------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     GetPCLInfo method returns a new PCLInfo instance 
        /// </summary>
        /// <param name="version">
        ///     PCL version
        /// </param>
        /// <returns>
        ///     A new PCLInfo instance with matching PCL version
        /// </returns>
        public static PCLInfo GetPCLInfo(ePCLVersion version)
        {
            foreach (PCLInfo info in CreateInfoList())
            {
                if (info.pclVersion == version)
                {
                    return new PCLInfo(info.ZIPFilename, info.downloadURL, info.pclVersion); // hand back a copy
                }
            }

            throw new Exception("Unknown PCL version.");
        }

        /// <summary>
        ///     CreateInfoList method creates supported PCLInfo list containing zip file name, download URL, version.
        /// </summary>
        /// <returns>
        ///     A List of supported PCL information supported by BlueGo
        /// </returns>
        public static List<PCLInfo> CreateInfoList()
        {
            List<PCLInfo> list = new List<PCLInfo>();

            list.Add(new PCLInfo(
                "pcl-1.7.1.zip",
                @"https://github.com/PointCloudLibrary/pcl/archive/pcl-1.7.1.zip",
                ePCLVersion.PCL_1_7_1
            ));

            list.Add(new PCLInfo(
                "pcl-master.zip",
                @"https://github.com/PointCloudLibrary/pcl/archive/master.zip",
                ePCLVersion.FromSourceGIT
            ));

            return list;
        }

        /// <summary>
        ///     TransformPCLVersionToString method transforms enum value of PCL version to PCL version string
        /// </summary>
        /// <param name="version">
        ///     Enum value of PCL version 
        /// </param>
        /// <returns>
        ///     String value of enum PCL version
        /// </returns>
        public static string TransformPCLVersionToString(ePCLVersion version)
        {
            switch (version)
            {
                case ePCLVersion.PCL_1_7_1:
                    return "1.7.1";

                case ePCLVersion.FromSourceGIT:
                    return "From Source (GIT)";
            }

            throw new Exception("Unknown PCL version");
        }

        /// <summary>
        ///     TransformPCLSupportedBoostVersionToString method transforms enum value of PCL supported
        ///     Boost version to string
        /// </summary>
        /// <param name="version">
        ///     Enum value of PCL supported Boost version
        /// </param>
        /// <returns>
        ///     String value of PCL supported Boost version
        /// </returns>
        public static string TransformPCLSupportedBoostVersionToString(ePCLSupportedBoostVersion boostVersion)
        {
            switch (boostVersion)
            {
                case ePCLSupportedBoostVersion.PCLBoost_1_55_0:
                    return "1.55.0";

                case ePCLSupportedBoostVersion.PCLBoostFromSourceSVN:
                    return "From Source (SVN)";
            }

            throw new Exception("Unknown PCL supported Boost version");
        }

        /// <summary>
        ///     TransformPCLSupportedEigenVersionToString method transforms enum value of PCL supported
        ///     Eigen version to string
        /// </summary>
        /// <param name="version">
        ///     Enum value of PCL supported Eigen version
        /// </param>
        /// <returns>
        ///     String value of PCL supported Eigen version
        /// </returns>
        public static string TransformPCLSupportedEigenVersionToString(ePCLSupportedEigenVersion eigenVersion)
        {
            switch (eigenVersion)
            {
                case ePCLSupportedEigenVersion.PCLEigen_3_2_0:
                    return "3.2.0";

                case ePCLSupportedEigenVersion.PCLEigen_3_2_1:
                    return "3.2.1";
            }

            throw new Exception("Unknown PCL supported Eigen version");
        }

        /// <summary>
        ///     TransformPCLSupportedFlannVersionToString method transforms enum value of PCL supported
        ///     Flann version to string
        /// </summary>
        /// <param name="version">
        ///     Enum value of PCL supported Flann version
        /// </param>
        /// <returns>
        ///     String value of PCL supported Flann version
        /// </returns>
        public static string TransformPCLSupportedFlannVersionToString(ePCLSupportedFlannVersion flannVersion)
        {
            switch (flannVersion)
            {
                case ePCLSupportedFlannVersion.PCLFlann_1_8_4:
                    return "1.8.4";
            }

            throw new Exception("Unknown PCL supported Flann version");
        }

        /// <summary>
        ///     TransformPCLSupportedVTKVersionToString method transforms enum value of PCL supported
        ///     VTK version to string
        /// </summary>
        /// <param name="version">
        ///     Enum value of PCL supported VTK version
        /// </param>
        /// <returns>
        ///     String value of PCL supported VTK version
        /// </returns>
        public static string TransformPCLSupportedVTKVersionToString(ePCLSupportedVTKVersion vtkVersion)
        {
            switch (vtkVersion)
            {
                case ePCLSupportedVTKVersion.PCLVTK_6_1_0:
                    return "6.1.0";
            }

            throw new Exception("Unknown PCL supported VTK version");
        }

        /// <summary>
        ///     GetDownloadURL method gets the download URL for requested PCL version from PCLInfo list
        /// </summary>
        /// <param name="version">
        ///     PCL version for which download URL is required
        /// </param>
        /// <returns>
        ///     Download PCL URL for requested PCL version
        /// </returns>
        public static string GetDownloadURL(ePCLVersion version)
        {
            foreach (PCLInfo info in CreateInfoList())
            {
                if (info.pclVersion == version)
                {
                    return info.DownloadURL;
                }
            }

            throw new Exception("Unknown PCL version.");
        }

        /// <summary>
        ///     GetPCLZipFileName method gets the PCL zip file name for requested PCL version
        /// </summary>
        /// <param name="version">
        ///     PCL version for which zip file name is requested
        /// </param>
        /// <returns>
        ///     PCL zip file name for matching PCL version from PCL information list
        /// </returns>
        public static string GetPCLZipFileName(ePCLVersion version)
        {
            foreach (PCLInfo lib in CreateInfoList())
            {
                if (lib.pclVersion == version)
                {
                    return lib.ZIPFilename;
                }
            }

            throw new Exception("Unknown PCL version");
        }

        #endregion //Public

        #endregion //Methods
    }

    /// <summary>
    ///     PCLBuildProcessDescripton class contains properties that describe PCL build process
    /// </summary>
    class PCLBuildProcessDescripton
    {
        #region Fields -------------------------------------------------------------------------------------------------------------------------

        #region Public -------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     Destination folder where PCL is downloaded and built
        /// </summary>
        public string destinationFolder;

        /// <summary>
        ///     Compiler type as selected for PCL build
        /// </summary>
        public eCompiler compilerType;

        /// <summary>
        ///     PCL version as selected for build
        /// </summary>
        public ePCLVersion pclVersion;
       
        /// <summary>
        ///     Platform(x86, x64) type as selected for PCL build
        /// </summary>
        public ePlatform platform;

        /// <summary>
        ///     PCL supported Boost version as selected for build
        /// </summary>
        public ePCLSupportedBoostVersion pclSupportedBoostVersion;

        /// <summary>
        ///     PCL supported Eigen version as selected for build
        /// </summary>
        public ePCLSupportedEigenVersion pclSupportedEigenVersion;

        /// <summary>
        ///     PCL supported Flann version as selected for build
        /// </summary>
        public ePCLSupportedFlannVersion pclSupportedFlannVersion;

        /// <summary>
        ///     PCL supported VTK version as selected for build
        /// </summary>
        public ePCLSupportedVTKVersion pclSupportedVTKVersion;

        #endregion //Public

        #endregion //Fields
    }

    /// <summary>
    ///     PCLBuildProcess class builds PCL using CMake and MSBuild
    /// </summary>
    class PCLBuildProcess : BuildProcess
    {
        #region Fields -------------------------------------------------------------------------------------------------------------------------

        #region Private ------------------------------------------------------------------------------------------------------------------------

        private const string PythonAndNumpyNotInstalledMessage = "Python could not be found in the Windows environment path.\n\n" +
                    "Install Python & Numpy and add python exe path to the Windows environment path to build Flann successfully";
        private const string PCLWaring = "It is assumed that all the prerequisites i.e. Boost, Eigen, Flann, VTK are build with BlueGo" +
                    " to build PCL successfully with this release of BlueGo.";
        private const string PCLFromSourceGITFolderName = "pcl_FromSourceGIT";
        
        string pclFolderName = string.Empty;
        string python2ExeFullPath = string.Empty;

        private string destinationFolder;
        private string boostFullPath = string.Empty;
        private string boostFolder = string.Empty;
        private string eigenFullPath = string.Empty;
        private string eigenFolder = string.Empty;
        private string flannFullPath = string.Empty;
        private string flannFolder = string.Empty;
        private string vtkFullPath = string.Empty;
        private string vtkFolder = string.Empty;
        private string cmakeCommand = string.Empty;
        private string cmakeGeneratorName = string.Empty;

        private eCompiler compilerType;
        private ePCLVersion pclVersion;
        private ePlatform platform;
        private ePCLSupportedBoostVersion pclSupportedBoostVersion;
        private ePCLSupportedEigenVersion pclSupportedEigenVersion;
        private ePCLSupportedFlannVersion pclSupportedFlannVersion;
        private ePCLSupportedVTKVersion pclSupportedVTKVersion;

        #endregion //Private

        #endregion //Fields

        #region Constructor --------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     PCLBuildProcess constructor
        /// </summary>
        /// <param name="llbpd">
        ///     PCLBuildProcessDescripton describes PCL build parameters
        /// </param>
        public PCLBuildProcess(PCLBuildProcessDescripton pclbpd)
        {
            destinationFolder = pclbpd.destinationFolder;
            compilerType = pclbpd.compilerType;
            pclVersion = pclbpd.pclVersion;
            platform = pclbpd.platform;            
        }

        #endregion //Constructor

        #region Methods ------------------------------------------------------------------------------------------------------------------------

        #region Public -------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     DownloadAndBuild method downloads selected PCL version and builds it for selected parameters (i.e. x64
        ///     and Visual Studio 13)
        /// </summary>
        public void DownloadAndBuild()
        {
            try
            {
                // TODO: Ankit - Customize Message box with BlueGo UX
                // prompt for dependencies 
                if (MessageBox.Show(PCLWaring, "Warning", MessageBoxButtons.OKCancel) != DialogResult.OK)
                {
                    OnFinished();
                    return;
                }
                
                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                message("Checking Dependencies...");

                DependencyChecker.CheckCommanDependency();

                python2ExeFullPath = Executable.GetPython2ExePath();

                if (String.IsNullOrEmpty(python2ExeFullPath))
                {
                    throw new Exception(PythonAndNumpyNotInstalledMessage);
                }

                message("Checking Dependencies done!");
                
                message("Downloading PCL...");

                string pclDownloadURL = PCLInfo.GetDownloadURL(pclVersion);
                string pclZIPFilename = PCLInfo.GetPCLZipFileName(pclVersion);
                pclFolderName = PCLInfo.GetPCLInfo(pclVersion).ExtractFolderName;
                
                DownloadHelper.DownloadFileFromURL(pclDownloadURL, destinationFolder + pclZIPFilename);

                message("Start to unzip PCL...");

                // unzip pcl
                SevenZip.Decompress(destinationFolder + "/" + pclZIPFilename, destinationFolder);

                message("PCL has been unzipped!");

                if (Directory.Exists(destinationFolder + "/pcl-" + pclFolderName))
                {
                    Directory.Move(destinationFolder + "/pcl-" + pclFolderName, destinationFolder + pclFolderName);
                }
                else if (Directory.Exists(destinationFolder + pclFolderName) && (ePCLVersion.FromSourceGIT == pclVersion))
                {
                    Directory.Move(destinationFolder + pclFolderName, destinationFolder + "/" + PCLFromSourceGITFolderName);
                    pclFolderName = PCLFromSourceGITFolderName;
                }

                message("start to build pcl...");
                
                runCMake(destinationFolder);
                runMSBuild(destinationFolder, " /property:Configuration=" + Folder.ReleaseFolderName);

                message("PCL successfully built!");
                
                // remove downloaded file
                if (File.Exists(Path.Combine(destinationFolder, pclZIPFilename)))
                {
                    System.IO.File.Delete(Path.Combine(destinationFolder, pclZIPFilename));
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
            using (Process p = new Process())
            {
                ProcessStartInfo info = CreateVisualStudioCommandPromptProcessStartInfo(compilerType, platform);

                info.RedirectStandardInput = true;
                info.UseShellExecute = false;
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;
                info.WindowStyle = ProcessWindowStyle.Hidden;

                p.StartInfo = info;

                StringBuilder output = new StringBuilder();
                StringBuilder error = new StringBuilder();

                using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
                using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
                {
                    p.OutputDataReceived += (sender, e) =>
                    {
                        if (e.Data == null)
                        {
                            outputWaitHandle.Set();
                        }
                        else
                        {
                            output.AppendLine(e.Data);
                            writeStandardOutputMessage(e.Data);
                        }
                    };

                    p.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data == null)
                        {
                            errorWaitHandle.Set();
                        }
                        else
                        {
                            error.AppendLine(e.Data);
                            writeStandardErrorMessage(e.Data);
                        }
                    };

                    p.Start();

                    p.BeginOutputReadLine();
                    p.BeginErrorReadLine();

                    using (StreamWriter sw = p.StandardInput)
                    {
                        if (sw.BaseStream.CanWrite)
                        {
                            if (compilerType == eCompiler.VS2013)
                            {
                                boostFolder = getSupportedBoostFolder(pclSupportedBoostVersion);
                                boostFullPath = Path.GetFullPath(Path.Combine(PreferencesManager.Instance.ThirdPartyDownloadFolder, eCompiler.VS2013.ToString(), platform.ToString(), boostFolder));

                                eigenFolder = getSupportedEigenFolder();
                                eigenFullPath = Path.GetFullPath(Path.Combine(PreferencesManager.Instance.ThirdPartyDownloadFolder, eCompiler.VS2013.ToString(), platform.ToString(), eigenFolder));

                                flannFolder = getSupportedFlannFolder();
                                flannFullPath = Path.GetFullPath(Path.Combine(PreferencesManager.Instance.ThirdPartyDownloadFolder, eCompiler.VS2013.ToString(), platform.ToString(), flannFolder));

                                vtkFolder = getSupportedVTKFolder();
                                vtkFullPath = Path.GetFullPath(Path.Combine(PreferencesManager.Instance.ThirdPartyDownloadFolder, eCompiler.VS2013.ToString(), platform.ToString(), vtkFolder));

                                if (platform == ePlatform.x64)
                                {
                                    cmakeGeneratorName = string.Concat(Helper.GetVisualStudioProductName(eVisualStudioProduct.Visual_Studio_12_2013), " ", "Win64");
                                }
                                else if (platform == ePlatform.x86)
                                {
                                    cmakeGeneratorName = Helper.GetVisualStudioProductName(eVisualStudioProduct.Visual_Studio_12_2013);
                                }

                                checkForDependencyFolder();

                                sw.WriteLine("cd /D " + destinationFolder + pclFolderName);

                                sw.WriteLine(getCMakeCommand());
                            }
                            else if (compilerType == eCompiler.VS2012)
                            {
                                boostFolder = getSupportedBoostFolder(pclSupportedBoostVersion);
                                boostFullPath = Path.GetFullPath(Path.Combine(PreferencesManager.Instance.ThirdPartyDownloadFolder, eCompiler.VS2012.ToString(), platform.ToString(), boostFolder));

                                eigenFolder = getSupportedEigenFolder();
                                eigenFullPath = Path.GetFullPath(Path.Combine(PreferencesManager.Instance.ThirdPartyDownloadFolder, eCompiler.VS2012.ToString(), platform.ToString(), eigenFolder));

                                flannFolder = getSupportedFlannFolder();
                                flannFullPath = Path.GetFullPath(Path.Combine(PreferencesManager.Instance.ThirdPartyDownloadFolder, eCompiler.VS2012.ToString(), platform.ToString(), flannFolder));

                                vtkFolder = getSupportedVTKFolder();
                                vtkFullPath = Path.GetFullPath(Path.Combine(PreferencesManager.Instance.ThirdPartyDownloadFolder, eCompiler.VS2012.ToString(), platform.ToString(), vtkFolder));

                                if (platform == ePlatform.x64)
                                {
                                    cmakeGeneratorName = string.Concat(Helper.GetVisualStudioProductName(eVisualStudioProduct.Visual_Studio_11_2012), " ", "Win64");
                                }
                                else if (platform == ePlatform.x86)
                                {
                                    cmakeGeneratorName = Helper.GetVisualStudioProductName(eVisualStudioProduct.Visual_Studio_11_2012);
                                }

                                checkForDependencyFolder();

                                sw.WriteLine("cd /D " + destinationFolder + pclFolderName);

                                sw.WriteLine(getCMakeCommand());
                            }
                            else if (compilerType == eCompiler.VS2010)
                            {
                                boostFolder = getSupportedBoostFolder(pclSupportedBoostVersion);
                                boostFullPath = Path.GetFullPath(Path.Combine(PreferencesManager.Instance.ThirdPartyDownloadFolder, eCompiler.VS2010.ToString(), platform.ToString(), boostFolder));

                                eigenFolder = getSupportedEigenFolder();
                                eigenFullPath = Path.GetFullPath(Path.Combine(PreferencesManager.Instance.ThirdPartyDownloadFolder, eCompiler.VS2010.ToString(), platform.ToString(), eigenFolder));

                                flannFolder = getSupportedFlannFolder();
                                flannFullPath = Path.GetFullPath(Path.Combine(PreferencesManager.Instance.ThirdPartyDownloadFolder, eCompiler.VS2010.ToString(), platform.ToString(), flannFolder));

                                vtkFolder = getSupportedVTKFolder();
                                vtkFullPath = Path.GetFullPath(Path.Combine(PreferencesManager.Instance.ThirdPartyDownloadFolder, eCompiler.VS2010.ToString(), platform.ToString(), vtkFolder));

                                if (platform == ePlatform.x64)
                                {
                                    cmakeGeneratorName = string.Concat(Helper.GetVisualStudioProductName(eVisualStudioProduct.Visual_Studio_10_2010), " ", "Win64");
                                }
                                else if (platform == ePlatform.x86)
                                {
                                    cmakeGeneratorName = Helper.GetVisualStudioProductName(eVisualStudioProduct.Visual_Studio_10_2010);
                                }

                                checkForDependencyFolder();

                                sw.WriteLine("cd /D " + destinationFolder + pclFolderName);

                                sw.WriteLine(cmakeCommand);
                            }
                        }
                    }

                    int timeoutIn30Seconds = 1000 * 60 * 30; // 30 minutes

                    if (p.WaitForExit(timeoutIn30Seconds) &&
                        outputWaitHandle.WaitOne(timeoutIn30Seconds) &&
                        errorWaitHandle.WaitOne(timeoutIn30Seconds))
                    {
                    }
                    else
                    {
                        throw new Exception("Configuring PCL timed out!");
                    }
                }
            }
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
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("cd /D " + Path.GetFullPath(Path.Combine(destinationFolder, pclFolderName, Folder.BinFolderName)));
                    sw.WriteLine("\"" + Executable.msbuildExePath + "\"" + " PCL.sln" + arguments);
                }
            }

            readStandardOutput(p);
            readStandardError(p);

            p.WaitForExit();
            p.Close();
        }

        private string getSupportedBoostFolder(ePCLSupportedBoostVersion pclSupportedBoostVersion)
        {
            switch(pclSupportedBoostVersion)
            {
                case ePCLSupportedBoostVersion.PCLBoostFromSourceSVN:
                    return BoostHelper.BoostFromSourceSVNFolderName;

                case ePCLSupportedBoostVersion.PCLBoost_1_55_0:
                    return "boost_1_55_0";
            }          

            throw new Exception("Unknown PCL supported Boost version");
        }

        private string getSupportedEigenFolder()
        {
            switch (pclSupportedEigenVersion)
            {
                case ePCLSupportedEigenVersion.PCLEigen_3_2_0:
                    return "Eigen 3.2.0";

                case ePCLSupportedEigenVersion.PCLEigen_3_2_1:
                    return "Eigen 3.2.1";
            }

            throw new Exception("Unknown PCL supported Eigen version");
        }

        private string getSupportedFlannFolder()
        {
            switch (pclSupportedFlannVersion)
            {
                case ePCLSupportedFlannVersion.PCLFlann_1_8_4:
                    return "flann-1.8.4-src";
            }

            throw new Exception("Unknown PCL supported Flann version");
        }

        private string getSupportedVTKFolder()
        {
            switch (pclSupportedVTKVersion)
            {
                case ePCLSupportedVTKVersion.PCLVTK_6_1_0:
                    return "VTK-6.1.0";
            }

            throw new Exception("Unknown PCL supported VTK version");
        }       

        private void checkForDependencyFolder()
        {
            if (!Directory.Exists(boostFullPath))
            {
                throw new Exception("Supported boost folder:" + boostFolder + " does not exists!\n PCL requires supported compiled boost version.");
            }

            if (!Directory.Exists(eigenFullPath))
            {
                throw new Exception("Supported eigen folder:" + eigenFolder + " does not exists!\n PCL requires supported compiled eigen version.");
            }

            if (!Directory.Exists(flannFullPath))
            {
                throw new Exception("Supported flann folder:" + flannFolder + " does not exists!\n PCL requires supported compiled flann version.");
            }

            if (!Directory.Exists(vtkFullPath))
            {
                throw new Exception("Supported vtk folder:" + vtkFolder + " does not exists!\n PCL requires supported compiled vtk version.");
            }
        }

        private string getCMakeCommand()
        {
            string cmakeCommand = 
                "\"" + Path.GetFullPath(Executable.cmakeExePath) + "\"" +
                " -G\"" + cmakeGeneratorName + "\"" +
                " -H\"" + Path.GetFullPath(Path.Combine(destinationFolder, pclFolderName)) + "\"" +
                " -B\"" + Path.GetFullPath(Path.Combine(destinationFolder, pclFolderName, Folder.BinFolderName)) + "\"" +
                " -DBOOST_ROOT=" + "\"" + Path.GetFullPath(boostFullPath) + "\"" +
                " -DEIGEN_INCLUDE_DIR=" + "\"" + Path.GetFullPath(eigenFullPath) + "\"" +
                " -DFLANN_INCLUDE_DIR=" + "\"" + Path.GetFullPath(Path.Combine(flannFullPath, Folder.SourceFolderName, Folder.CppFolderName)) + "\"" +
                " -DFLANN_LIBRARY=" + "\"" + Path.GetFullPath(Path.Combine(flannFullPath, Folder.BinFolderName, Folder.LibFolderName, Folder.ReleaseFolderName)) + "\"" +
                " -DVTK_DIR=" + "\"" + Path.GetFullPath(Path.Combine(vtkFullPath, Folder.BinFolderName)) + "\"" +
                " -DPYTHON_EXECUTABLE=" + "\"" + Path.GetFullPath(python2ExeFullPath) + "\"";

            return cmakeCommand;
        }

        #endregion //Private

        #endregion //Methods
    }
}