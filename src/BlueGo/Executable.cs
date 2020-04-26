using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Win32;

using BlueGo.GUI;
using System.Diagnostics;
using System.Collections;

namespace BlueGo
{
    /// <summary>
    ///     Executable class provides looking up for PATH variable and 
    ///     other installation files
    /// </summary>
    class Executable
    {
        #region Fields -------------------------------------------------------------------------------------------------------------------------

        #region Private ------------------------------------------------------------------------------------------------------------------------

        private const string CMakeName = "CMake";
        private const string CMakeExeName = "cmake.exe";
        private const string CMDPrompt = "cmd.exe";
        private const string PythonExeName = "python.exe";

        private const string CurrentUserUninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
        private const string LocalMachine32UninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
        private const string LocalMachine64UninstallKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

        private const string MSBuildName = "MSBuild";
        private const string MSBuildExeName = "MSBuild.exe";
        private const string LocalMachine32MSBuildKey = @"SOFTWARE\Microsoft\MSBuild\ToolsVersions";
        private const string LocalMachine64MSBuildKey = @"SOFTWARE\Wow6432Node\Microsoft\MSBuild\ToolsVersions";

        private const string PathName = "PATH";
        private const string MSBuildToolsPathName = "MSBuildToolsPath";

        #endregion //Private

        #region Public -------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     CMake executable path
        /// </summary>
        public static string cmakeExePath = string.Empty;

        /// <summary>
        ///     MSBuild executable path
        /// </summary>
        public static string msbuildExePath = string.Empty;

        #endregion //Public

        #endregion //Fields

        #region Methods ------------------------------------------------------------------------------------------------------------------------

        #region Public -------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     ExistsOnPath method searches the file name in the PATH environment variable
        /// </summary>
        /// <param name="fileName">
        ///     fileName to locate in the PATH environment variable
        /// </param>
        /// <returns>
        ///     True if the fileName exists otherwise False
        /// </returns>
        public static bool ExistsOnPath(string fileName)
        {
            if (GetFullPath(fileName) != null)
                return true;
            return false;
        }

        /// <summary>
        ///     GetFullPath method gets the full path for the desired file name
        /// </summary>
        /// <param name="fileName">
        ///     fileName for which full path is required
        /// </param>
        /// <returns>
        ///     Full path of the desired file name
        /// </returns>
        public static string GetFullPath(string fileName)
        {
            if (File.Exists(fileName))
                return Path.GetFullPath(fileName);

            var values = Environment.GetEnvironmentVariable(PathName, EnvironmentVariableTarget.Machine);
           
            foreach (var path in values.Split(';'))
            {
                var fullPath = Path.Combine(path, fileName);
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }
                    
            }
            return null;
        }

        /// <summary>
        ///     Iterate Windows environment path variable
        /// </summary>
        /// <param name="fileName">
        ///     Search for the fileName in the path variable
        /// </param>
        /// <param name="iterateAll">
        ///     For True, all the paths are searched otherwise first found path is returned
        /// </param>
        /// <returns>
        ///     Returns list of full path found
        /// </returns>
        public static IList<string> GetFullPath(string fileName, bool iterateAll)
        {
            List<string> listFullPath = new List<string>();
            var values = Environment.GetEnvironmentVariable(PathName, EnvironmentVariableTarget.Machine);

            foreach (var path in values.Split(';'))
            {
                var fullPath = Path.Combine(path, fileName);
                if (File.Exists(fullPath))
                {
                    listFullPath.Add(fullPath);
                    if (!iterateAll)
                    {
                        break;
                    }

                }
            }

            return listFullPath;
        }

        /// <summary>
        ///     IsCMakeInstalled method searches for CMake executable and returns True if it is found
        ///     otherwise False
        /// </summary>
        /// <returns>
        ///     True if CMake executable is found otherwise False
        /// </returns>
        public static bool IsCMakeInstalled()
        {
            RegistryKey key;

            if (File.Exists(PreferencesManager.Instance.CMakeExeLocation))
            {
                cmakeExePath = PreferencesManager.Instance.CMakeExeLocation;
                return true;
            }

            string cmakeFullPath = GetFullPath(CMakeExeName);
            
            if (cmakeFullPath != null)
            {
                cmakeExePath = cmakeFullPath;
                return true;
            }

            try
            {
                // search in: CurrentUser
                key = Registry.CurrentUser.OpenSubKey(CurrentUserUninstallKey);
                if (IsSubKeyPresent(key, CMakeName))
                {
                    return true;
                }

                // search in: LocalMachine_32
                key = Registry.LocalMachine.OpenSubKey(LocalMachine32UninstallKey);
                if (IsSubKeyPresent(key, CMakeName))
                {
                    return true;
                }

                // search in: LocalMachine_64
                key = Registry.LocalMachine.OpenSubKey(LocalMachine64UninstallKey);
                if (IsSubKeyPresent(key, CMakeName))
                {
                    return true;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
            

            // NOT FOUND
            return false;
        }

        /// <summary>
        ///     IsMSBuildInstalled method searches for MSBuild executable and returns True if it is found
        ///     otherwise False
        /// </summary>
        /// <returns>
        ///     True if MSBuild executable is found otherwise False
        /// </returns>
        public static bool IsMSBuildInstalled()
        {
            if (File.Exists(PreferencesManager.Instance.MSBuildExeLocation))
            {
                msbuildExePath = PreferencesManager.Instance.MSBuildExeLocation;
                return true;
            }
                        
            string msbuildFullPath = GetFullPath(MSBuildExeName);

            // search in: Environment Path
            if (msbuildFullPath != null)
            {
                msbuildExePath = msbuildFullPath;
                return true;
            }

            // search in: LocalMachine_32
            if (IsSubKeyPresent(LocalMachine32MSBuildKey, MSBuildName))
            {
                return true;
            }
            
            // search in: LocalMachine_64
            if (IsSubKeyPresent(LocalMachine64MSBuildKey, MSBuildName))
            {
                return true;
            }

            // NOT FOUND
            return false;
        }

        /// <summary>
        ///     Returns the command prompt output for the arguements supplied to the cmd prompt
        /// </summary>
        /// <param name="arguements">
        ///     Command to execute on command prompt for which the output is required
        /// </param>
        /// <returns>
        ///     Output of the arguement(cmd) executed on cmd prompt
        /// </returns>
        public static string GetCommandPromptOutput(string arguements)
        {
            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo(CMDPrompt);

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
                    sw.WriteLine(arguements);
                }
            }
            
            string cmdOutput = string.Empty;
            using (StreamReader sr = p.StandardError)
            {
                while (sr.BaseStream.CanRead)
                {
                    string line = sr.ReadLine();

                    cmdOutput += line;
                    if (line == null)
                    {
                        break;
                    }          
                }
            }

            p.WaitForExit();
            p.Close();
            return cmdOutput;
        }

        /// <summary>
        ///     Gets the Python2 exe full path
        /// </summary>
        /// <returns>
        ///     Python2 exe full path value as a string
        /// </returns>
        public static string GetPython2ExePath()
        {
            IList<string> listPythonFullPath = GetFullPath(PythonExeName, true);

            if (listPythonFullPath == null || listPythonFullPath.Count() == 0)
            {
                return null;
            }

            string pythonVersionInfoOutput = string.Empty;

            foreach (string pythonPath in listPythonFullPath)
            {
                pythonVersionInfoOutput = GetCommandPromptOutput(Path.GetFullPath(pythonPath) + " --version");

                if (!String.IsNullOrEmpty(pythonVersionInfoOutput) && pythonVersionInfoOutput.ToLower().Contains("python 2."))
                {
                    return pythonPath;
                }
            }

            return null;
        }

        #endregion //Public

        #region Private ------------------------------------------------------------------------------------------------------------------------

        private static bool IsSubKeyPresent(RegistryKey key, string name)
        {
            string uninstallString;
            string displayName;
            

            foreach (String keyName in key.GetSubKeyNames())
            {
                RegistryKey subkey = key.OpenSubKey(keyName);
                if (subkey.Name.ToString().Contains(name))
                {
                    uninstallString = subkey.GetValue("UninstallString") as string;

                    if (uninstallString != null && uninstallString.Length > 0)
                    {
                        string[] folderName = uninstallString.Split('\\');
                        if (folderName.Length > 2)
                        {
                            string cmakeFolderName = folderName[folderName.Length - 2];
                            displayName = cmakeFolderName.Split(' ')[0];

                            if (name.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
                            {
                                string cmakePath = uninstallString.Substring(0, uninstallString.LastIndexOf("\\"));
                                cmakeExePath = Path.Combine(cmakePath, "bin", CMakeExeName);
                                return true;
                            }
                        }
                    }
                }
            }     
            
            return false;
        }

        private static bool IsSubKeyPresent(string subKey, string name)
        {
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(subKey);

                if (key != null && key.SubKeyCount > 0)
                {
                    string[] toolVersions = key.GetSubKeyNames();
                    Single[] msbuildToolVersions = Array.ConvertAll(toolVersions, Single.Parse);
                    string version = msbuildToolVersions.Max().ToString("0.0");
                    string msbuildBinPath = key.OpenSubKey(version).GetValue(MSBuildToolsPathName).ToString();
                    if (!string.IsNullOrEmpty(msbuildBinPath))
                    {
                        msbuildExePath = Path.Combine(msbuildBinPath, MSBuildExeName);
                        return true;
                    }
                }
            }
            catch(Exception ex)
            {
                return false;
            }
            
            return false;
        }

        #endregion //Private

        #endregion //Methods
    }
}