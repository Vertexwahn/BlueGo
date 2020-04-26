using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BlueGo
{
    /// <summary>
    /// Can be used do unzip data.
    /// </summary>
    class SevenZip
    {
        /// <summary>
        /// Decompresses a zip file.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="destinationFolder"></param>
        public static void Decompress(string filename, string destinationFolder)
        {
            string sevenZipExePath = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + "7z.exe";

            if (!System.IO.File.Exists(sevenZipExePath))
            {
                throw new Exception("Could not locate 7zip - maybe installation failed. Tried to use 7z.exe from " + sevenZipExePath);
            }

            if (!is64BitOperatingSystem)
            {
                throw new Exception("Expecting a 64 bit operating system. You are running not a x64 operating system. 7z ");
            }

            // path needs not
            //CheckDestinationFolder(destinationFolder);

            try
            {
                Process p = new Process();
                ProcessStartInfo info = new ProcessStartInfo(sevenZipExePath, "x -o" + "\"" + destinationFolder + "\"" + " " +"\"" + filename + "\"");
                p.StartInfo = info;
                p.Start();
                p.WaitForExit();

                if (p.ExitCode != 0)
                    throw new Exception("Decompressing failed. Maybe download is invalid.");
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        static bool is64BitProcess = (IntPtr.Size == 8);
        static bool is64BitOperatingSystem = is64BitProcess || InternalCheckIsWow64();

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process(
            [In] IntPtr hProcess,
            [Out] out bool wow64Process
        );

        public static bool InternalCheckIsWow64()
        {
            if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1) ||
                Environment.OSVersion.Version.Major >= 6)
            {
                using (Process p = Process.GetCurrentProcess())
                {
                    bool retVal;
                    if (!IsWow64Process(p.Handle, out retVal))
                    {
                        return false;
                    }
                    return retVal;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
