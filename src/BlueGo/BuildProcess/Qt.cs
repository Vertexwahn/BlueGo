using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace BlueGo
{
    enum eQtVersion
    {
        Qt4_8_3,
        Qt4_8_4,
        Qt5_0_0,
        Qt5_0_1,
        Qt5_0_2,
        Qt5_1_0,
        Qt5_1_1,
        Qt5_2_0,
        Qt5_2_1,
        Qt5_3_0,
        Qt5_3_1,
        Qt5_3_2,
        Qt5_4_0,
        Qt5_4_1,
        Qt5_5_0,
        Qt5_5_1,
        Qt5_14_0,
        Qt5_14_1,
    }

    class QtInfo
    {
        QtInfo(string filename, string downloadURL, eQtVersion version)
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

        public eQtVersion Version
        {
            get { return version; }
            set { version = value; }
        }

        public static QtInfo GetQtInfo(eQtVersion version)
        {
            foreach (QtInfo info in CreateInfoList())
            {
                if (info.version == version)
                {
                    return new QtInfo(info.ZIPFilename, info.downloadURL, info.version); // hand back a copy
                }
            }

            throw new Exception("Unknown Qt version.");
        }

        public static List<QtInfo> CreateInfoList()
        {
            List<QtInfo> list = new List<QtInfo>();

            // TODO: move this Data to an XML file

            list.Add(new QtInfo(
                "qt-everywhere-opensource-src-4.8.3.zip",
                @"http://download.qt-project.org/archive/qt/4.8/4.8.3/qt-everywhere-opensource-src-4.8.3.zip",
                eQtVersion.Qt4_8_3
            ));

            list.Add(new QtInfo(
                "qt-everywhere-opensource-src-4.8.4.zip",
                @"http://download.qt-project.org/archive/qt/4.8/4.8.4/qt-everywhere-opensource-src-4.8.4.zip",
                eQtVersion.Qt4_8_4
            ));

            list.Add(new QtInfo(
                "qt-everywhere-opensource-src-5.0.0.zip",
                @"http://download.qt-project.org/official_releases/qt/5.0/5.0.0/single/qt-everywhere-opensource-src-5.0.0.zip",
                eQtVersion.Qt5_0_0)
            );

            list.Add(new QtInfo(
               "qt-everywhere-opensource-src-5.0.1.zip",
               @"http://download.qt-project.org/official_releases/qt/5.0/5.0.1/single/qt-everywhere-opensource-src-5.0.1.zip",
               eQtVersion.Qt5_0_1)
            );

            list.Add(new QtInfo(
               "qt-everywhere-opensource-src-5.0.2.zip",
               @"http://download.qt-project.org/official_releases/qt/5.0/5.0.2/single/qt-everywhere-opensource-src-5.0.2.zip",
               eQtVersion.Qt5_0_2)
            );

            list.Add(new QtInfo(
               "qt-everywhere-opensource-src-5.1.0.zip",
               @"http://download.qt-project.org/official_releases/qt/5.1/5.1.0/single/qt-everywhere-opensource-src-5.1.0.zip",
               eQtVersion.Qt5_1_0)
            );

            list.Add(new QtInfo(
                "qt-everywhere-opensource-src-5.1.1.zip",
                @"http://download.qt-project.org/official_releases/qt/5.1/5.1.1/single/qt-everywhere-opensource-src-5.1.1.zip",
                eQtVersion.Qt5_1_1)
            );

            list.Add(new QtInfo(
                "qt-everywhere-opensource-src-5.2.0.zip",
                @"http://download.qt-project.org/official_releases/qt/5.2/5.2.0/single/qt-everywhere-opensource-src-5.2.0.zip",
               // @"http://vertexwahn.de/thirdparty/qt-everywhere-opensource-src-5.2.0.zip",
                eQtVersion.Qt5_2_0)
            );

            list.Add(new QtInfo(
              "qt-everywhere-opensource-src-5.2.1.zip",
              @"http://download.qt-project.org/official_releases/qt/5.2/5.2.1/single/qt-everywhere-opensource-src-5.2.1.zip",
              eQtVersion.Qt5_2_1)
            );

            list.Add(new QtInfo(
              "qt-everywhere-opensource-src-5.3.0.zip",
              @"http://download.qt-project.org/official_releases/qt/5.3/5.3.0/single/qt-everywhere-opensource-src-5.3.0.zip",
              eQtVersion.Qt5_3_0)
            );

            list.Add(new QtInfo(
              "qt-everywhere-opensource-src-5.3.1.zip",
              @"http://download.qt-project.org/official_releases/qt/5.3/5.3.1/single/qt-everywhere-opensource-src-5.3.1.zip",
              eQtVersion.Qt5_3_1)
            );

            list.Add(new QtInfo(
              "qt-everywhere-opensource-src-5.3.2.zip",
              @"http://download.qt-project.org/official_releases/qt/5.3/5.3.2/single/qt-everywhere-opensource-src-5.3.2.zip",
              eQtVersion.Qt5_3_2)
            );

            list.Add(new QtInfo(
              "qt-everywhere-opensource-src-5.4.0.zip",
              @"http://download.qt-project.org/official_releases/qt/5.4/5.4.0/single/qt-everywhere-opensource-src-5.4.0.zip",
              eQtVersion.Qt5_4_0)
            );

            list.Add(new QtInfo(
              "qt-everywhere-opensource-src-5.4.0.zip",
              @"http://download.qt.io/official_releases/qt/5.4/5.4.1/single/qt-everywhere-opensource-src-5.4.1.zip",
              eQtVersion.Qt5_4_1)
            );

            list.Add(new QtInfo(
                "qt-everywhere-opensource-src-5.5.0.zip",
                @"http://download.qt.io/official_releases/qt/5.5/5.5.0/single/qt-everywhere-opensource-src-5.5.0.zip",
                eQtVersion.Qt5_5_0)
            );

            list.Add(new QtInfo(
               "qt-everywhere-opensource-src-5.5.1.zip",
               @"http://download.qt.io/official_releases/qt/5.5/5.5.1/single/qt-everywhere-opensource-src-5.5.1.zip",
               eQtVersion.Qt5_5_1)
            );


            list.Add(new QtInfo(
               "qt-everywhere-src-5.14.0.zip",
               @"http://download.qt-project.org/official_releases/qt/5.14/5.14.0/single/qt-everywhere-src-5.14.0.zip",
               eQtVersion.Qt5_14_0)
            );


            list.Add(new QtInfo(
               "qt-everywhere-src-5.14.1.zip",
               @"http://download.qt-project.org/official_releases/qt/5.14/5.14.1/single/qt-everywhere-src-5.14.1.zip",
               eQtVersion.Qt5_14_1)
            );

            return list;
        }

        public static string TransformVersionToString(eQtVersion version)
        {
            switch (version)
            {
                case eQtVersion.Qt4_8_3:
                    return "4.8.3";
                case eQtVersion.Qt4_8_4:
                    return "4.8.4";
                case eQtVersion.Qt5_0_0:
                    return "5.0.0";
                case eQtVersion.Qt5_0_1:
                    return "5.0.1";
                case eQtVersion.Qt5_0_2:
                    return "5.0.2";
                case eQtVersion.Qt5_1_0:
                    return "5.1.0";
                case eQtVersion.Qt5_1_1:
                    return "5.1.1";
                case eQtVersion.Qt5_2_0:
                    return "5.2.0";
                case eQtVersion.Qt5_2_1:
                    return "5.2.1";
                case eQtVersion.Qt5_3_0:
                    return "5.3.0";
                case eQtVersion.Qt5_3_1:
                    return "5.3.1";
                case eQtVersion.Qt5_3_2:
                    return "5.3.2";
                case eQtVersion.Qt5_4_0:
                    return "5.4.0";
                case eQtVersion.Qt5_4_1:
                    return "5.4.1";
                case eQtVersion.Qt5_5_0:
                    return "5.5.0";
                case eQtVersion.Qt5_5_1:
                    return "5.5.1";
                case eQtVersion.Qt5_14_0:
                    return "5.14.0";
                case eQtVersion.Qt5_14_1:
                    return "5.14.1";
            }

            throw new Exception("Unknown Qt version");
        }

        public static string GetDownloadURL(eQtVersion version)
        {
            foreach (QtInfo info in CreateInfoList())
            {
                if (info.version == version)
                {
                    return info.DownloadURL;
                }
            }

            throw new Exception("Unknown Qt version.");
        }

        public static string GetZipFileName(eQtVersion version)
        {
            foreach (QtInfo info in CreateInfoList())
            {
                if (info.version == version)
                {
                    return info.ZIPFilename;
                }
            }

            throw new Exception("Unknown Qt version.");
        }
        
        string zipFilename;
        string downloadURL;
        eQtVersion version;
    }

    class qtBuildProcessDescripton
    {
        public string destinationFolder;// Where to download and build qt?
        public eCompiler compilerType;
        public eQtVersion qtVersion;
        public ePlatform platform;
        public string withLibraries;
        public String qtCommandLineArguments;
        public bool downloadThirdPartyLibraryEveryTime;        
    }

    class QtBuildProcess : BuildProcess
    {
        string destinationFolder;
        eCompiler compilerType;
        eQtVersion qtVersion;
        ePlatform platform;
        string withLibraries;
        String qtCommandLineArguments;
        bool downloadThirdPartyLibraryEveryTime;

        public QtBuildProcess(qtBuildProcessDescripton qbpd)
        {
            destinationFolder = qbpd.destinationFolder;
            compilerType = qbpd.compilerType;
            qtVersion = qbpd.qtVersion;
            platform = qbpd.platform;
            withLibraries = qbpd.withLibraries;
            qtCommandLineArguments = qbpd.qtCommandLineArguments;            
            downloadThirdPartyLibraryEveryTime = qbpd.downloadThirdPartyLibraryEveryTime;
        }

        public void DownloadAndBuild()
        {
            try
            {
                if (!Directory.Exists(destinationFolder))
                {
                   Directory.CreateDirectory(destinationFolder);
                }
                               
                message("downloading QT");

                string QtDownloadURL = QtInfo.GetDownloadURL(qtVersion);
                string QtZipFilename = QtInfo.GetZipFileName(qtVersion);

                if (!File.Exists(destinationFolder + QtZipFilename) || downloadThirdPartyLibraryEveryTime)
                {
                    // Download Qt
                    DownloadHelper.DownloadFileFromURL(QtDownloadURL, destinationFolder + QtZipFilename);

                    message("extracting QT");
                }

                // Extract Qt
                SevenZip.Decompress(destinationFolder + QtZipFilename, destinationFolder);

                // patch qt
                if (compilerType == eCompiler.VS2012 && qtVersion == eQtVersion.Qt4_8_3)
                {
                    // patch Qt 4.8.3
                    message("patching Qt 4.8.3");

                    System.Reflection.Assembly a = System.Reflection.Assembly.GetEntryAssembly();
                    string baseDir = System.IO.Path.GetDirectoryName(a.Location);
                    string qmakeConfigPath = baseDir + @"\qt4.8.3vs2012patch\qmake.conf";

                    if (!File.Exists(qmakeConfigPath))
                        throw new Exception("File " + qmakeConfigPath + " does not exist");

                    File.Copy(
                        qmakeConfigPath,
                        destinationFolder + @"qt-everywhere-opensource-src-4.8.3\mkspecs\win32-msvc2010\qmake.conf", true);

                    File.Copy(
                      @"qt4.8.3vs2012patch\HashSet.h",
                      destinationFolder + @"qt-everywhere-opensource-src-4.8.3\src\3rdparty\webkit\Source\JavaScriptCore\wtf\HashSet.h", true);
                }

                // patch qt
                if (compilerType == eCompiler.VS2012 && qtVersion == eQtVersion.Qt4_8_4)
                {
                    // patch Qt 4.8.4
                    message("patching Qt 4.8.4");

                    System.Reflection.Assembly a = System.Reflection.Assembly.GetEntryAssembly();
                    string baseDir = System.IO.Path.GetDirectoryName(a.Location);
                    string qmakeConfigPath = baseDir + @"\qt4.8.3vs2012patch\qmake.conf";

                    if (!File.Exists(qmakeConfigPath))
                        throw new Exception("File " + qmakeConfigPath + " does not exist");

                    File.Copy(
                        qmakeConfigPath,
                        destinationFolder + @"qt-everywhere-opensource-src-4.8.4\mkspecs\win32-msvc2010\qmake.conf", true);

                    File.Copy(
                      @"qt4.8.3vs2012patch\HashSet.h",
                      destinationFolder + @"qt-everywhere-opensource-src-4.8.4\src\3rdparty\webkit\Source\JavaScriptCore\wtf\HashSet.h", true);
                }

                message("configure qt");

                if (qtVersion == eQtVersion.Qt4_8_3 ||
                   qtVersion == eQtVersion.Qt4_8_4)
                {
                    configureQT4();
                }
                else
                {
                    configureQT5();
                }

                message("building qt");

                buildQT();

                // remove downloaded file
                if (File.Exists(Path.Combine(destinationFolder, QtZipFilename)))
                {
                    System.IO.File.Delete(Path.Combine(destinationFolder, QtZipFilename));
                }

                message("Finished buidling qt  - - - DONE");

                OnFinished();
            }
            catch(Exception ex)
            {
                message(string.Empty);
                OnFailure();
                MessageBox.Show(ex.ToString());
            }
        }

        private void configureQT4()
        {
            eCompiler compiler = compilerType;
  
            Process p = new Process();
            ProcessStartInfo info = CreateVisualStudioCommandPromptProcessStartInfo(compilerType, platform);
         
            info.RedirectStandardInput = true;
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.CreateNoWindow = true;

            p.StartInfo = info;
            p.Start();

            string extractFolderName = QtInfo.GetQtInfo(qtVersion).ExtractFolderName;

            using (StreamWriter sw = p.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("cd /D " + destinationFolder + extractFolderName);

                    if (compiler == eCompiler.VS2010 || qtVersion == eQtVersion.Qt4_8_3)// Qt 4.8.3 does not know platform msvc2012
                    {
                        sw.WriteLine("configure -mp -opensource -nomake demos -nomake examples -platform win32-msvc2010" + qtCommandLineArguments + withLibraries);
                    }
                    else
                    {
                        sw.WriteLine("configure -mp -opensource -nomake demos -nomake examples -platform win32-msvc2012" + qtCommandLineArguments + withLibraries);
                    }

                    sw.WriteLine("y");
                }
            }

            readStandardOutput(p);
            readStandardError(p);

            p.WaitForExit();
            p.Close();
        }

        private void configureQT5()
        {
            using (Process process = new Process())
            {
                process.StartInfo = CreateVisualStudioCommandPromptProcessStartInfo(compilerType, platform);
  
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardInput = true;

                StringBuilder output = new StringBuilder();
                StringBuilder error = new StringBuilder();

                using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
                using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
                {
                    process.OutputDataReceived += (sender, e) =>
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

                    process.ErrorDataReceived += (sender, e) =>
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

                    process.Start();

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    using (StreamWriter sw = process.StandardInput)
                    {
                        if (sw.BaseStream.CanWrite)
                        {
                            string extractFolderName = QtInfo.GetQtInfo(qtVersion).ExtractFolderName;

                            sw.WriteLine("cd /D " + destinationFolder + extractFolderName);
                            if (compilerType == eCompiler.VS2010)
                            {
                                sw.WriteLine("configure -developer-build -opensource -nomake examples -nomake tests" + qtCommandLineArguments + withLibraries);
                            }
                            else
                            {
                                sw.WriteLine("configure -developer-build -opensource -nomake examples -nomake tests" + qtCommandLineArguments + withLibraries);
                            }

                            sw.WriteLine("y");
                        }
                    }
                    
                    int timeoutIn30Seconds = 1000 * 60 * 30; // 30 minutes
                    if (process.WaitForExit(timeoutIn30Seconds) &&
                        outputWaitHandle.WaitOne(timeoutIn30Seconds) &&
                        errorWaitHandle.WaitOne(timeoutIn30Seconds))
                    {
                        // Process completed. Check process.ExitCode here.
                        //process.ExitCode
                    }
                    else
                    {
                        // Timed out.
                        throw new Exception("Configure Qt5: TimeOut");
                    }
                }
            }
        }

        private void buildQT()
        {
            using (Process process = new Process())
            {
                process.StartInfo = CreateVisualStudioCommandPromptProcessStartInfo(compilerType, platform);

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardInput = true;

                StringBuilder output = new StringBuilder();
                StringBuilder error = new StringBuilder();

                using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
                using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
                {
                    process.OutputDataReceived += (sender, e) =>
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

                    process.ErrorDataReceived += (sender, e) =>
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

                    process.Start();

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    using (StreamWriter sw = process.StandardInput)
                    {
                        if (sw.BaseStream.CanWrite)
                        {
                            string extractFolderName = QtInfo.GetQtInfo(qtVersion).ExtractFolderName;

                            sw.WriteLine("cd /D " + destinationFolder + extractFolderName);
                            sw.WriteLine(@"nmake");

                            //sw.WriteLine("y");
                        }
                    }

                    int timeoutInOneDay = 1000 * 60 * 60 * 24; // 1 day
                    if (process.WaitForExit(timeoutInOneDay) &&
                        outputWaitHandle.WaitOne(timeoutInOneDay) &&
                        errorWaitHandle.WaitOne(timeoutInOneDay))
                    {
                        // Process completed. Check process.ExitCode here.
                        //process.ExitCode
                    }
                    else
                    {
                        // Timed out.
                        throw new Exception("Build Qt5: TimeOut");
                    }
                }
            }
        }
    }
}