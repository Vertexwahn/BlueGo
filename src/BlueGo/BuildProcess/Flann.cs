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
    ///     Enum for Flann version supported by BlueGo
    /// </summary>
    enum eFlannVersion
    {
        Flann1_8_4
    }

    /// <summary>
    ///     FlannInfo class details about Flann zip file, version, download url, mandatory dependencies
    /// </summary>
    class FlannInfo
    {
        #region Fields -------------------------------------------------------------------------------------------------------------------------

        #region Private ------------------------------------------------------------------------------------------------------------------------

        private string zipFilename;
        private string downloadURL;
        private eFlannVersion version;

        #endregion//Private

        #endregion //Fields

        #region Constructor --------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     FlannInfo constructor
        /// </summary>
        /// <param name="filename">
        ///     Flann zip file to be downloaded
        /// </param>
        /// <param name="downloadURL">
        ///     Flann download URL string
        /// </param>
        /// <param name="version">
        ///     Flann version to be downloaded
        /// </param>
        FlannInfo(string filename, string downloadURL, eFlannVersion version)
        {
            this.zipFilename = filename;
            this.downloadURL = downloadURL;
            this.version = version;
        }

        #endregion //Constructor

        #region Properties ---------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     Gets & Sets Flann download URL
        /// </summary>
        public string DownloadURL
        {
            get { return downloadURL; }
            set { downloadURL = value; }
        }

        /// <summary>
        ///     Gets & Sets Flann zip file name to download
        /// </summary>
        public string ZIPFilename
        {
            get { return zipFilename; }
            set { zipFilename = value; }
        }

        /// <summary>
        ///     Gets folder name from downloaded Flann zip file, where Flann to be extracted
        /// </summary>
        public string ExtractFolderName
        {
            get
            {
                return zipFilename.Substring(0, zipFilename.Length - 4);
            }
        }

        /// <summary>
        ///     Gets & Sets Flann version
        /// </summary>
        public BlueGo.eFlannVersion Version
        {
            get { return version; }
            set { version = value; }
        }

        #endregion //Properties

        #region Methods ------------------------------------------------------------------------------------------------------------------------

        #region Public -------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     GetFlannInfo method returns a new FlannInfo instance.
        /// </summary>
        /// <param name="version">
        ///     Flann version
        /// </param>
        /// <returns>
        ///     A new FlannInfo instance with matching Flann version
        /// </returns>
        public static FlannInfo GetFlannInfo(eFlannVersion version)
        {
            foreach (FlannInfo info in CreateInfoList())
            {
                if (info.version == version)
                {
                    return new FlannInfo(info.ZIPFilename, info.downloadURL, info.version); // hand back a copy
                }
            }

            throw new Exception("Unknown Flann version.");
        }

        /// <summary>
        ///     CreateInfoList method creates supported FlannInfo list containing zip file name, download URL, version
        /// </summary>
        /// <returns>
        ///     A List of supported Flann information supported by BlueGo
        /// </returns>
        public static List<FlannInfo> CreateInfoList()
        {
            List<FlannInfo> list = new List<FlannInfo>();

            list.Add(new FlannInfo(
                "flann-1.8.4-src.zip",
                @"http://people.cs.ubc.ca/~mariusm/uploads/FLANN/flann-1.8.4-src.zip",
                eFlannVersion.Flann1_8_4
            ));

            return list;
        }

        /// <summary>
        ///     TransformFlannVersionToString method transforms enum value of Flann version to Flann version string
        /// </summary>
        /// <param name="version">
        ///     Enum value of Flann version 
        /// </param>
        /// <returns>
        ///     String value of enum Flann version
        /// </returns>
        public static string TransformFlannVersionToString(eFlannVersion version)
        {
            switch (version)
            {
                case eFlannVersion.Flann1_8_4:
                    return "1.8.4";
            }

            throw new Exception("Unknown Flann version");
        }

        /// <summary>
        ///     GetDownloadURL method gets the download URL for requested Flann version from FlannInfo list
        /// </summary>
        /// <param name="version">
        ///     Flann version for which download URL is required
        /// </param>
        /// <returns>
        ///     Download Flann URL for requested Flann version
        /// </returns>
        public static string GetDownloadURL(eFlannVersion version)
        {
            foreach (FlannInfo info in CreateInfoList())
            {
                if (info.version == version)
                {
                    return info.DownloadURL;
                }
            }

            throw new Exception("Unknown Flann version.");
        }

        /// <summary>
        ///     GetFlannZipFileName method gets the Flann zip file name for requested Flann version
        /// </summary>
        /// <param name="version">
        ///     Flann version for which zip file name is requested
        /// </param>
        /// <returns>
        ///     Flann zip file name for matching Flann version from Flann information list
        /// </returns>
        public static string GetFlannZipFileName(eFlannVersion version)
        {
            foreach (FlannInfo lib in CreateInfoList())
            {
                if (lib.version == version)
                {
                    return lib.ZIPFilename;
                }
            }

            throw new Exception("Unknown Flann version");
        }

        #endregion //Public

        #endregion //Methods
    }

    /// <summary>
    ///     FlannBuildProcessDescripton class contains properties that describe Flann build process
    /// </summary>
    class FlannBuildProcessDescripton
    {
        #region Fields -------------------------------------------------------------------------------------------------------------------------

        #region Public -------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     Destination folder where Flann is downloaded and built
        /// </summary>
        public string destinationFolder;

        /// <summary>
        ///     Compiler type as selected for Flann build
        /// </summary>
        public eCompiler compilerType;

        /// <summary>
        ///     Flann version as selected for build
        /// </summary>
        public eFlannVersion flannVersion;

        /// <summary>
        ///     Platform(x86, x64) type as selected for Flann build
        /// </summary>
        public ePlatform platform;

        #endregion //Public

        #endregion //Fields
    }

    /// <summary>
    ///     FlannBuildProcess class builds Flann using CMake and MSBuild
    /// </summary>
    class FlannBuildProcess : BuildProcess
    {
        #region Fields -------------------------------------------------------------------------------------------------------------------------

        #region Private ------------------------------------------------------------------------------------------------------------------------

        private const string PythonAndNumpyNotInstalledMessage = "Python could not be found in the Windows environment path.\n\n" +
                    "Install Python & Numpy and add python exe path to the Windows environment path to build Flann successfully";
        
        private string destinationFolder;
        private eCompiler compilerType;
        private eFlannVersion flannVersion;
        private ePlatform platform;

        #endregion //Private

        #endregion //Fields

        #region Constructor --------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     FlannBuildProcess constructor
        /// </summary>
        /// <param name="llbpd">
        ///     FlannBuildProcessDescripton describes Flann build parameters
        /// </param>
        public FlannBuildProcess(FlannBuildProcessDescripton flannbpd)
        {
            destinationFolder = flannbpd.destinationFolder;
            compilerType = flannbpd.compilerType;
            flannVersion = flannbpd.flannVersion;
            platform = flannbpd.platform;
        }

        #endregion //Constructor

        #region Methods ------------------------------------------------------------------------------------------------------------------------

        #region Public -------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     DownloadAndBuild method downloads selected Flann version and builds it for selected parameters (i.e. x64
        ///     and Visual Studio 13)
        /// </summary>
        public void DownloadAndBuild()
        {
            try
            {
                // TODO: Ankit - Customize Message box with BlueGo UX
                string fVersion = FlannInfo.TransformFlannVersionToString(flannVersion);
                //prompt for dependencies 
                MessageBox.Show("Information:Flann " + fVersion + " requires Python 2.x & Numpy as dependencies to build successfully!");
                
                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                message("Checking Dependencies...");

                DependencyChecker.CheckCommanDependency();

                string python2FullPath = Executable.GetPython2ExePath();

                if (String.IsNullOrEmpty(python2FullPath))
                {
                    throw new Exception(PythonAndNumpyNotInstalledMessage);
                }
                
                message("Checking Dependencies done!");

                message("Downloading Flann...");

                string FlannDownloadURL = FlannInfo.GetDownloadURL(flannVersion);
                string FlannZIPFilename = FlannInfo.GetFlannZipFileName(flannVersion);

                DownloadHelper.DownloadFileFromURL(FlannDownloadURL, destinationFolder + FlannZIPFilename);

                message("Start to unzip Flann...");

                // Unzip Flann
                SevenZip.Decompress(destinationFolder + "/" + FlannZIPFilename, destinationFolder);
                
                message("Flann has been unzipped!");

                //patch the serialization.h for building successfully with x64
                if (platform == ePlatform.x64)
                {
                    patchForX64Platform();
                }
                
                message("Building Flann...");

                runCMake(destinationFolder, python2FullPath);
                runMSBuild(destinationFolder, " /property:Configuration=" + Folder.ReleaseFolderName);

                message("Flann successfully built!");
                
                // remove downloaded file
                if (File.Exists(Path.Combine(destinationFolder, FlannZIPFilename)))
                {
                    System.IO.File.Delete(Path.Combine(destinationFolder, FlannZIPFilename));
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

        private void runCMake(string destinationFolder, string python2ExePath)
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
                string extractFolderName = FlannInfo.GetFlannInfo(flannVersion).ExtractFolderName;
                
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
                            " -B\"" + Path.GetFullPath(Path.Combine(destinationFolder, extractFolderName, Folder.BinFolderName)) + "\"" +
                            " -DPYTHON_EXECUTABLE=" + "\"" + python2ExePath + "\"" +
                            " -DBUILD_MATLAB_BINDINGS=OFF" + " -DBUILD_PYTHON_BINDINGS=OFF";
                        
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
                            " -B\"" + Path.GetFullPath(Path.Combine(destinationFolder, extractFolderName, Folder.BinFolderName)) + "\"" +
                            " -DPYTHON_EXECUTABLE=" + "\"" + python2ExePath + "\"" +
                            " -DBUILD_MATLAB_BINDINGS=OFF" + " -DBUILD_PYTHON_BINDINGS=OFF";

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
                            " -B\"" + Path.GetFullPath(Path.Combine(destinationFolder, extractFolderName, Folder.BinFolderName)) + "\"" +
                            " -DPYTHON_EXECUTABLE=" + "\"" + python2ExePath + "\"" +
                            " -DBUILD_MATLAB_BINDINGS=OFF" + " -DBUILD_PYTHON_BINDINGS=OFF";

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
                string extractFolderName = FlannInfo.GetFlannInfo(flannVersion).ExtractFolderName;

                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("cd /D " + Path.GetFullPath(Path.Combine(destinationFolder, extractFolderName, Folder.BinFolderName)));
                    sw.WriteLine("\"" + Executable.msbuildExePath + "\"" + " flann.sln" + arguments);
                }
            }

            readStandardOutput(p);
            readStandardError(p);

            p.WaitForExit();
            p.Close();
        }

        private void patchForX64Platform()
        {
            const string toSearch = "BASIC_TYPE_SERIALIZER(char);\n";
            const string toMatch = "BASIC_TYPE_SERIALIZER(unsigned __int64);";
            const string toPatch = "#ifdef _MSC_VER\nBASIC_TYPE_SERIALIZER(unsigned __int64);\n#endif\nBASIC_TYPE_SERIALIZER(char);\n";

            string extractFolderName = FlannInfo.GetFlannInfo(flannVersion).ExtractFolderName;
            string filePath = Path.Combine(destinationFolder, extractFolderName, @"src\cpp\flann\util\serialization.h");

            bool isPatch = false;

            if (File.Exists(filePath))
            {
                string fileContent = string.Empty;
                using (StreamReader streamReader = new StreamReader(filePath))
                {
                    fileContent = streamReader.ReadToEnd();
                    if (!fileContent.Contains(toMatch))
                    {
                        isPatch = true;
                    }
                }
                if (isPatch)
                {

                    fileContent = fileContent.Replace(toSearch, toPatch);
                    File.WriteAllText(filePath, fileContent);
                }
            }
            else
            {
                throw new Exception("For x64 machines, serialization.h requires patching. Patching error::" + filePath + " does not exists!");
            }
        }
                
        #endregion //Private

        #endregion //Methods
    }
}