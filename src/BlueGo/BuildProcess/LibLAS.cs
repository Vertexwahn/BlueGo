using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BlueGo.GUI;
using BlueGo.Util;

namespace BlueGo
{
    /// <summary>
    ///     Enum for libLAS version supported by BlueGo
    /// </summary>
    enum eLibLASVersion
    {
        LibLAS1_7_0,
        LibLAS1_8_0
    }

    /// <summary>
    ///     Enum for supported Boost version for libLAS
    /// </summary>
    enum eLibLASSupportedBoostVersion
    {
        LibLASBoost1_55_0,
        LibLASBoost1_60_0,
        LibLASBoostFromSourceSVN
    }

    /// <summary>
    ///     LibLASInfo class details about libLAS zip file, version, download url, supported Boost version
    /// </summary>
    class LibLASInfo
    {
        #region Fields -------------------------------------------------------------------------------------------------------------------------

        #region Private ------------------------------------------------------------------------------------------------------------------------

        private string zipFilename;
        private string downloadURL;
        private eLibLASVersion version;
        private eLibLASSupportedBoostVersion boostVersion;

        #endregion//Private

        #endregion //Fields

        #region Constructor --------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     LibLASInfo constructor
        /// </summary>
        /// <param name="filename">
        ///     libLAS zip file to be downloaded
        /// </param>
        /// <param name="downloadURL">
        ///     libLAS download URL string
        /// </param>
        /// <param name="version">
        ///     libLAS version to be downloaded
        /// </param>
        /// <param name="boostVersion">
        ///     libLAS supported Boost version
        /// </param>
        LibLASInfo(string filename, string downloadURL, eLibLASVersion version, eLibLASSupportedBoostVersion boostVersion)
        {
            this.zipFilename = filename;
            this.downloadURL = downloadURL;
            this.version = version;
            this.boostVersion = boostVersion;
        }

        #endregion //Constructor

        #region Properties ---------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     Gets & Sets libLAS download URL
        /// </summary>
        public string DownloadURL
        {
            get { return downloadURL; }
            set { downloadURL = value; }
        }

        /// <summary>
        ///     Gets & Sets libLAS zip file name to download
        /// </summary>
        public string ZIPFilename
        {
            get { return zipFilename; }
            set { zipFilename = value; }
        }
        
        /// <summary>
        ///     Gets folder name from downloaded libLAS zip file, where libLAS to be extracted
        /// </summary>
        public string ExtractFolderName
        {
            get
            {
                return zipFilename.Substring(0, zipFilename.Length - 4);
            }
        }

        /// <summary>
        ///     Gets & Sets libLAS version
        /// </summary>
        public BlueGo.eLibLASVersion Version
        {
            get { return version; }
            set { version = value; }
        }

        /// <summary>
        ///  Gets & Sets libLAS supported Boost version
        /// </summary>
        public BlueGo.eLibLASSupportedBoostVersion BoostVersion
        {
            get { return boostVersion; }
            set { boostVersion = value; }
        }

        #endregion //Properties

        #region Methods ------------------------------------------------------------------------------------------------------------------------

        #region Public -------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     GetLibLASInfo method returns a new LibLASInfo instance if the libLAS & supported Boost version exists.
        /// </summary>
        /// <param name="version">
        ///     libLAS version
        /// </param>
        /// <param name="boostVersion">
        ///     Supported Boost version
        /// </param>
        /// <returns>
        ///     A new LibLASInfo instance with matching libLAS version and supported Boost version
        /// </returns>
        public static LibLASInfo GetLibLASInfo(eLibLASVersion version, eLibLASSupportedBoostVersion boostVersion)
        {
            foreach (LibLASInfo info in CreateInfoList())
            {
                if (info.version == version && info.boostVersion == boostVersion)
                {
                    return new LibLASInfo(info.ZIPFilename, info.downloadURL, info.version, info.boostVersion); // hand back a copy
                }
            }

            throw new Exception("Unknown libLAS version.");
        }

        /// <summary>
        ///     CreateInfoList method creates supported libLASInfo list containing zip file name, download URL, version,
        ///     & supported Boost version
        /// </summary>
        /// <returns>
        ///     A List of supported libLAS information supported by BlueGo
        /// </returns>
        public static List<LibLASInfo> CreateInfoList()
        {
            List<LibLASInfo> list = new List<LibLASInfo>();
            
            list.Add(new LibLASInfo(
                "libLAS-1.7.0.zip",
                @"http://download.osgeo.org/liblas/libLAS-1.7.0.zip",
                eLibLASVersion.LibLAS1_7_0,
                eLibLASSupportedBoostVersion.LibLASBoost1_55_0
            ));


            list.Add(new LibLASInfo(
                "libLAS-1.8.0.tar.bz2",
                @"http://download.osgeo.org/liblas/libLAS-1.8.0.tar.bz2",
                eLibLASVersion.LibLAS1_8_0,
                eLibLASSupportedBoostVersion.LibLASBoost1_60_0
            ));

            return list;
        }

        /// <summary>
        ///     TransformLibLASVersionToString method transforms enum value of libLAS version to libLAS version string
        /// </summary>
        /// <param name="version">
        ///     Enum value of libLAS version 
        /// </param>
        /// <returns>
        ///     String value of enum libLAS version
        /// </returns>
        public static string TransformLibLASVersionToString(eLibLASVersion version)
        {
            switch (version)
            {
                case eLibLASVersion.LibLAS1_7_0:
                    return "1.7.0";
                case eLibLASVersion.LibLAS1_8_0:
                    return "1.8.0";
            }

            throw new Exception("Unknown libLAS version");
        }

        /// <summary>
        ///     TransformLibLASSupportedBoostVersionToString method transforms enum value of supported Boost version to
        ///     corresponding string value
        /// </summary>
        /// <param name="boostVersion">
        ///     Enum value of Boost version
        /// </param>
        /// <returns>
        ///     String value of enum Boost version
        /// </returns>
        public static string TransformLibLASSupportedBoostVersionToString(eLibLASSupportedBoostVersion boostVersion)
        {
            switch (boostVersion)
            {
                case eLibLASSupportedBoostVersion.LibLASBoost1_55_0:
                    return "1.55.0";
                case eLibLASSupportedBoostVersion.LibLASBoost1_60_0:
                    return "1.60.0";
            }

            throw new Exception("Unknown libLAS supported boost version");
        }

        /// <summary>
        ///     GetDownloadURL method gets the download URL for requested libLAS version from LibLASInfo list
        /// </summary>
        /// <param name="version">
        ///     libLAS version for which download URL is required
        /// </param>
        /// <returns>
        ///     Download libLAS URL for requested libLAS version
        /// </returns>
        public static string GetDownloadURL(eLibLASVersion version)
        {
            foreach (LibLASInfo info in CreateInfoList())
            {
                if(info.version == version)
                {
                    return info.DownloadURL;
                }
            }

            throw new Exception("Unknown libLAS version.");
        }

        /// <summary>
        ///     GetLibLASZipFileName method gets the libLAS zip file name for requested libLAS version
        /// </summary>
        /// <param name="version">
        ///     libLAS version for which zip file name is requested
        /// </param>
        /// <returns>
        ///     libLAS zip file name for matching libLAS version from LibLAS information list
        /// </returns>
        public static string GetLibLASZipFileName(eLibLASVersion version)
        {
            foreach (LibLASInfo lib in CreateInfoList())
            {
                if (lib.version == version)
                {
                    return lib.ZIPFilename;
                }
            }

            throw new Exception("Unknown libLAS version");
        }

        #endregion //Public

        #endregion //Methods
    }

    /// <summary>
    ///     LibLASBuildProcessDescripton class contains properties that describe libLAS build process
    /// </summary>
    class LibLASBuildProcessDescripton
    {
        #region Fields -------------------------------------------------------------------------------------------------------------------------

        #region Public -------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     Destination folder where libLAS is downloaded and built
        /// </summary>
        public string destinationFolder;

        /// <summary>
        ///     Compiler type as selected for libLAS build
        /// </summary>
        public eCompiler compilerType;

        /// <summary>
        ///     libLAS version  as selected for build
        /// </summary>
        public eLibLASVersion libLASVersion;

        /// <summary>
        ///     Supported Boost version for selected libLAS version
        /// </summary>
        public eLibLASSupportedBoostVersion libLASSupportedBoostVersion;

        /// <summary>
        ///     Platform(x86, x64) type as selected for libLAS build
        /// </summary>
        public ePlatform platform;

        #endregion //Public

        #endregion //Fields
    }

    /// <summary>
    ///     LibLASBuildProcess class builds libLAS using CMake and MSBuild
    /// </summary>
    class LibLASBuildProcess : BuildProcess
    {
        #region Fields -------------------------------------------------------------------------------------------------------------------------

        #region Private ------------------------------------------------------------------------------------------------------------------------

        private string destinationFolder;
        private eCompiler compilerType;
        private eLibLASVersion libLASVersion;
        private eLibLASSupportedBoostVersion libLASSupportedBoostVersion;
        private ePlatform platform;

        #endregion //Private

        #endregion //Fields

        #region Constructor --------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     LibLASBuildProcess constructor
        /// </summary>
        /// <param name="llbpd">
        ///     LibLASBuildProcessDescripton describes libLAS build parameters
        /// </param>
        public LibLASBuildProcess(LibLASBuildProcessDescripton llbpd)
        {
            destinationFolder = llbpd.destinationFolder;
            compilerType = llbpd.compilerType;
            libLASVersion = llbpd.libLASVersion;
            libLASSupportedBoostVersion = llbpd.libLASSupportedBoostVersion;
            platform = llbpd.platform;
        }

        #endregion //Constructor

        #region Methods ------------------------------------------------------------------------------------------------------------------------

        #region Public -------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     DownloadAndBuild method downloads selected libLAS version and builds it for selected parameters (i.e. x64
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

                message("Downloading libLAS...");

                string libLASDownloadURL = LibLASInfo.GetDownloadURL(libLASVersion);
                string libLASZIPFilename = LibLASInfo.GetLibLASZipFileName(libLASVersion);
                
                DownloadHelper.DownloadFileFromURL(libLASDownloadURL, destinationFolder + libLASZIPFilename);

                message("Start to unzip libLAS...");

                // Unzip libLAS
                SevenZip.Decompress(destinationFolder + "/" + libLASZIPFilename, destinationFolder);

                if (libLASVersion == eLibLASVersion.LibLAS1_8_0)
                {
                    SevenZip.Decompress(destinationFolder + "/" + "libLAS-1.8.0.tar", destinationFolder);
                }

                message("libLAS has been unzipped!");
                
                message("Start to build libLAS...");

                runCMake(destinationFolder);
                runMSBuild(destinationFolder, " /property:Configuration=Release");

                message("libLAS successfully built!");
                
                // remove downloaded file
                if (File.Exists(Path.Combine(destinationFolder, libLASZIPFilename)))
                {
                    System.IO.File.Delete(Path.Combine(destinationFolder, libLASZIPFilename));
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
                string extractFolderName = LibLASInfo.GetLibLASInfo(libLASVersion, libLASSupportedBoostVersion).ExtractFolderName;

                if (this.libLASVersion == eLibLASVersion.LibLAS1_8_0)
                {
                    extractFolderName = extractFolderName.Substring(0, extractFolderName.Length - 4);
                }

                const string BinFolderName = "bin";

                if (sw.BaseStream.CanWrite)
                {
                    string boostFullPath = string.Empty;
                    string boostFolder = string.Empty;
                    string cmakeCommand = string.Empty;
                    string cmakeGeneratorName = string.Empty;

                    if (compilerType == eCompiler.VS2015)
                    {
                        boostFolder = getSupportedBoostFolder(libLASSupportedBoostVersion);
                        boostFullPath = Path.GetFullPath(Path.Combine(PreferencesManager.Instance.ThirdPartyDownloadFolder, eCompiler.VS2015.ToString(), platform.ToString(), boostFolder));

                        if (platform == ePlatform.x64)
                        {
                            cmakeGeneratorName = string.Concat(Helper.GetVisualStudioProductName(eVisualStudioProduct.Visual_Studio_14_2015), " ", "Win64");
                        }
                        else if (platform == ePlatform.x86)
                        {
                            cmakeGeneratorName = Helper.GetVisualStudioProductName(eVisualStudioProduct.Visual_Studio_14_2015);
                        }

                        if (!Directory.Exists(boostFullPath))
                        {
                            throw new Exception("Supported boost folder:" + boostFolder + " does not exists!\n libLAS requires supported compiled boost version.");
                        }

                        sw.WriteLine("cd /D " + destinationFolder + extractFolderName);

                        cmakeCommand = "\"" + Path.GetFullPath(Executable.cmakeExePath) + "\"" +
                            " -G\"" + cmakeGeneratorName + "\"" +
                            " -H\"" + Path.GetFullPath(Path.Combine(destinationFolder, extractFolderName)) + "\"" +
                            " -B\"" + Path.GetFullPath(Path.Combine(destinationFolder, extractFolderName, BinFolderName)) + "\"" +
                            " -DBOOST_ROOT=" + "\"" + boostFullPath + "\"";

                        sw.WriteLine(cmakeCommand);
                    }
                    else if (compilerType == eCompiler.VS2013)
                    {
                        boostFolder = getSupportedBoostFolder(libLASSupportedBoostVersion);
                        boostFullPath = Path.GetFullPath(Path.Combine(PreferencesManager.Instance.ThirdPartyDownloadFolder, eCompiler.VS2013.ToString(), platform.ToString(), boostFolder));
                        
                        if(platform == ePlatform.x64)
                        {
                            cmakeGeneratorName = string.Concat(Helper.GetVisualStudioProductName(eVisualStudioProduct.Visual_Studio_12_2013), " ", "Win64");
                        }
                        else if(platform == ePlatform.x86)
                        {
                            cmakeGeneratorName = Helper.GetVisualStudioProductName(eVisualStudioProduct.Visual_Studio_12_2013);
                        }

                        if (!Directory.Exists(boostFullPath))
                        {
                            throw new Exception("Supported boost folder:" + boostFolder + " does not exists!\n libLAS requires supported compiled boost version.");
                        }

                        sw.WriteLine("cd /D " + destinationFolder + extractFolderName);

                        cmakeCommand = "\"" + Path.GetFullPath(Executable.cmakeExePath) + "\"" + 
                            " -G\"" + cmakeGeneratorName + "\"" +
                            " -H\"" + Path.GetFullPath(Path.Combine(destinationFolder, extractFolderName)) + "\"" +
                            " -B\"" + Path.GetFullPath(Path.Combine(destinationFolder, extractFolderName, BinFolderName)) + "\"" + 
                            " -DBOOST_ROOT=" + "\"" + boostFullPath + "\"";

                        sw.WriteLine(cmakeCommand);
                    }
                    else if (compilerType == eCompiler.VS2012)
                    {
                        boostFolder = getSupportedBoostFolder(libLASSupportedBoostVersion);
                        boostFullPath = Path.GetFullPath(Path.Combine(PreferencesManager.Instance.ThirdPartyDownloadFolder, eCompiler.VS2012.ToString(), platform.ToString(), boostFolder));

                        if (platform == ePlatform.x64)
                        {
                            cmakeGeneratorName = string.Concat(Helper.GetVisualStudioProductName(eVisualStudioProduct.Visual_Studio_11_2012), " ", "Win64");
                        }
                        else if (platform == ePlatform.x86)
                        {
                            cmakeGeneratorName = Helper.GetVisualStudioProductName(eVisualStudioProduct.Visual_Studio_11_2012);
                        }

                        if (!Directory.Exists(boostFullPath))
                        {
                            throw new Exception("Supported boost folder:" + boostFolder + " does not exists!\n libLAS requires supported compiled boost version.");
                        }

                        sw.WriteLine("cd /D " + destinationFolder + extractFolderName);

                        cmakeCommand = "\"" + Path.GetFullPath(Executable.cmakeExePath) + "\"" +
                            " -G\"" + cmakeGeneratorName + "\"" +
                            " -H\"" + Path.GetFullPath(Path.Combine(destinationFolder, extractFolderName)) + "\"" +
                            " -B\"" + Path.GetFullPath(Path.Combine(destinationFolder, extractFolderName, BinFolderName)) + "\"" +
                            " -DBOOST_ROOT=" + "\"" + boostFullPath + "\"";

                        sw.WriteLine(cmakeCommand);
                    }
                    else if (compilerType == eCompiler.VS2010)
                    {
                        boostFolder = getSupportedBoostFolder(libLASSupportedBoostVersion);
                        boostFullPath = Path.GetFullPath(Path.Combine(PreferencesManager.Instance.ThirdPartyDownloadFolder, eCompiler.VS2010.ToString(), platform.ToString(), boostFolder));

                        if (platform == ePlatform.x64)
                        {
                            cmakeGeneratorName = string.Concat(Helper.GetVisualStudioProductName(eVisualStudioProduct.Visual_Studio_10_2010), " ", "Win64");
                        }
                        else if (platform == ePlatform.x86)
                        {
                            cmakeGeneratorName = Helper.GetVisualStudioProductName(eVisualStudioProduct.Visual_Studio_10_2010);
                        }

                        if (!Directory.Exists(boostFullPath))
                        {
                            throw new Exception("Supported boost folder:" + boostFolder + " does not exists!\n libLAS requires supported compiled boost version.");
                        }

                        sw.WriteLine("cd /D " + destinationFolder + extractFolderName);

                        cmakeCommand = "\"" + Path.GetFullPath(Executable.cmakeExePath) + "\"" +
                            " -G\"" + cmakeGeneratorName + "\"" +
                            " -H\"" + Path.GetFullPath(Path.Combine(destinationFolder, extractFolderName)) + "\"" +
                            " -B\"" + Path.GetFullPath(Path.Combine(destinationFolder, extractFolderName, BinFolderName)) + "\"" +
                            " -DBOOST_ROOT=" + "\"" + boostFullPath + "\"";

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
                string extractFolderName = LibLASInfo.GetLibLASInfo(libLASVersion, libLASSupportedBoostVersion).ExtractFolderName;

                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("cd /D " + Path.GetFullPath(Path.Combine(destinationFolder, extractFolderName, "bin")));
                    sw.WriteLine("\"" + Executable.msbuildExePath + "\"" + " libLAS.sln" + arguments);
                }
            }

            readStandardOutput(p);
            readStandardError(p);

            p.WaitForExit();
            p.Close();
        }

        private string getSupportedBoostFolder(eLibLASSupportedBoostVersion libLASupportedBoostVersion)
        {
            switch (libLASupportedBoostVersion)
            {
                case eLibLASSupportedBoostVersion.LibLASBoostFromSourceSVN:
                    return BoostHelper.BoostFromSourceSVNFolderName;

                case eLibLASSupportedBoostVersion.LibLASBoost1_60_0:
                    return "boost_1_60_0";

                case eLibLASSupportedBoostVersion.LibLASBoost1_55_0:
                    return "boost_1_55_0";
            }

            throw new Exception("Unknown supported boost version");
        }

        #endregion //Private

        #endregion //Methods
    }
}