using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueGo
{
    class InstallCMake : BuildProcess
    {
        public void DownloadAndInstall()
        {
            try
            {
                string CMakeEXEDestination = PreferencesManager.Instance.ApplicationDownloadFolder + "/" + "cmake-3.3.1-win32-x86.exe";
 
                message("Downloading " + @"http://www.cmake.org/files/v3.3/cmake-3.3.1-win32-x86.exe" +  "...");

                DownloadHelper.DownloadFileFromURL(@"http://www.cmake.org/files/v3.3/cmake-3.3.1-win32-x86.exe",
                    CMakeEXEDestination);

                message("Starting installation process...");

                var info = new ProcessStartInfo();
                info.UseShellExecute = true;
                info.WorkingDirectory = @"C:\Windows\System32";
                info.FileName = @"C:\Windows\System32\cmd.exe";
                info.Verb = "runas";
                info.RedirectStandardInput = true;
                info.UseShellExecute = false;
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;

                Process p = new Process();
                p.StartInfo = info;

                p.Start();

                using (StreamWriter sw = p.StandardInput)
                {
                    if (sw.BaseStream.CanWrite)
                    {
                        sw.WriteLine("cd /D " + PreferencesManager.Instance.ApplicationDownloadFolder);
                        sw.WriteLine("cmake-3.3.1-win32-x86.exe /S");
                    }
                }

                readStandardOutput(p);
                readStandardError(p);

                p.WaitForExit();
                p.Close();
            }
            catch(Exception ex)
            {
                message("CMake Installation failed: " + ex.ToString());
            }
        }
    }
}
