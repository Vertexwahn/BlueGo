using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using BlueGo.Util;

namespace BlueGo
{
    enum eBoostVersion
    {
        Unknown,
        Boost1_44_0,
        Boost1_51_0,
        Boost1_52_0,
        Boost1_53_0,
        Boost1_54_0,
        Boost1_55_0,
        Boost1_56_0,
        Boost1_57_0,
        Boost1_58_0,
        Boost1_59_0,
		Boost1_60_0,
        Boost1_61_0,
        Boost1_62_0,
        Boost1_63_0,
        FromSourceSVN
    }

    // Can be used to define which combinations of Boost, Compiler and Platform work together
    class SupportedLibraryPlatformEntry
    {
        eBoostVersion boostVersion;
        eCompiler compilerVersion;
        ePlatform platform;
    }

    class BoostInfo
    {
        BoostInfo(string filename, string downloadURL, eBoostVersion version)
        {
            this.zipFilename = filename;
            downloadURLDestinations.Add(downloadURL);
            this.version = version;
        }

        BoostInfo(string filename, List<string> downloadURLDestinations, eBoostVersion version)
        {
            this.zipFilename = filename;
            foreach(string url in downloadURLDestinations)
            {
                this.downloadURLDestinations.Add(url);
            }
          
            this.version = version;
        }

        // Searches for a valid URL
        public string DownloadURL
        {
            get
            {
                foreach (string url in downloadURLDestinations)
                {
                    if (DownloadHelper.RemoteFileExists(url))
                    { 
                        return url;
                    }
                    else
                    {
                        MessageBox.Show(url + "does not exist. Trying next URL if available.");
                    }
                }

                return downloadURLDestinations[0];
            }
        }

        public void AddDownloadURLDestination(string url)
        {
            downloadURLDestinations.Add(url);
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
                if (zipFilename.ToLower().Contains(".zip"))
                {
                    return zipFilename.Substring(0, zipFilename.Length - 4);
                }
                else
                {
                    return BoostHelper.BoostFromSourceSVNFolderName;
                }
            }
        }

        public BlueGo.eBoostVersion Version
        {
            get { return version; }
            set { version = value; }
        }
           
        public static BoostInfo GetBoostInfo(eBoostVersion version)
        {
            foreach (BoostInfo info in CreateInfoList())
            {
                if (info.version == version)
                {
                    return new BoostInfo(info.ZIPFilename, info.downloadURLDestinations, info.version); // hand back a copy
                }
            }

            throw new Exception("Unknown boost version.");
        }

        public static List<BoostInfo> CreateInfoList()
        {
            List<BoostInfo> list = new List<BoostInfo>();

            list.Add(new BoostInfo(
                "FromSourceSVN",
                @"http://svn.boost.org/svn/boost/trunk/",
                eBoostVersion.FromSourceSVN
            ));

            list.Add(new BoostInfo(
                "boost_1_44_0.zip",
                @"http://heanet.dl.sourceforge.net/project/boost/boost/1.44.0/boost_1_44_0.zip",
                eBoostVersion.Boost1_44_0
            ));

            list.Add(new BoostInfo(
                "boost_1_51_0.zip",
                @"http://heanet.dl.sourceforge.net/project/boost/boost/1.51.0/boost_1_51_0.zip",
                eBoostVersion.Boost1_51_0
            ));

            list.Add(new BoostInfo(
                "boost_1_52_0.zip",
                @"http://heanet.dl.sourceforge.net/project/boost/boost/1.52.0/boost_1_52_0.zip",
                eBoostVersion.Boost1_52_0
            ));

            list.Add(new BoostInfo(
                "boost_1_53_0.zip",
                @"http://heanet.dl.sourceforge.net/project/boost/boost/1.53.0/boost_1_53_0.zip",
                eBoostVersion.Boost1_53_0
            ));

            list.Add(new BoostInfo(
               "boost_1_54_0.zip",
               @"http://heanet.dl.sourceforge.net/project/boost/boost/1.54.0/boost_1_54_0.zip",
               eBoostVersion.Boost1_54_0
             ));

            list.Add(new BoostInfo(
               "boost_1_55_0.zip",
               @"http://heanet.dl.sourceforge.net/project/boost/boost/1.55.0/boost_1_55_0.zip",
               eBoostVersion.Boost1_55_0
             ));

            list.Add(new BoostInfo(
               "boost_1_56_0.zip",
               @"http://downloads.sourceforge.net/project/boost/boost/1.56.0/boost_1_56_0.zip?r=http%3A%2F%2Fsourceforge.net%2Fprojects%2Fboost%2Ffiles%2Fboost%2F1.56.0%2F&ts=1431175803&use_mirror=netcologne",
               eBoostVersion.Boost1_56_0
             ));

            list.Add(new BoostInfo(
                   "boost_1_57_0.zip",
                   @"https://www.cms.bgu.tum.de/oip/BlueGo/boost_1_57_0.zip",
                   eBoostVersion.Boost1_57_0
                 )
             );

            list.Add(new BoostInfo(
                    "boost_1_58_0.zip",
                    @"https://www.cms.bgu.tum.de/oip/BlueGo/boost_1_58_0.zip",
                    eBoostVersion.Boost1_58_0
                )
            );

			{
				List<string> urls = new List<string>();
				urls.Add(@"http://netcologne.dl.sourceforge.net/project/boost/boost/1.59.0/boost_1_59_0.zip");
				urls.Add(@"http://freefr.dl.sourceforge.net/project/boost/boost/1.59.0/boost_1_59_0.zip");
				urls.Add(@"http://kent.dl.sourceforge.net/project/boost/boost/1.59.0/boost_1_59_0.zip");
				urls.Add(@"http://iweb.dl.sourceforge.net/project/boost/boost/1.59.0/boost_1_59_0.zip");

				BoostInfo biBoost1_59_0 = new BoostInfo(
					"boost_1_59_0.zip",
					urls,
					eBoostVersion.Boost1_59_0
				);

				list.Add(biBoost1_59_0);				
			}

			{
				List<string> urls = new List<string>();
				urls.Add(@"http://iweb.dl.sourceforge.net/project/boost/boost/1.60.0/boost_1_60_0.zip");
				urls.Add(@"http://netcologne.dl.sourceforge.net/project/boost/boost/1.60.0/boost_1_60_0.zip");
				urls.Add(@"http://netassist.dl.sourceforge.net/project/boost/boost/1.60.0/boost_1_60_0.zip");

				BoostInfo biBoost1_60_0 = new BoostInfo(
					"boost_1_60_0.zip",
					urls,
					eBoostVersion.Boost1_60_0
				);

				list.Add(biBoost1_60_0);	
			}

            {
                List<string> urls = new List<string>();
                urls.Add(@"https://dl.dropboxusercontent.com/u/59547297/Libraries/boost_1_61_0.zip");
                urls.Add(@"http://vorboss.dl.sourceforge.net/project/boost/boost/1.61.0/boost_1_61_0.zip");
                urls.Add(@"http://freefr.dl.sourceforge.net/project/boost/boost/1.61.0/boost_1_61_0.zip");

                BoostInfo biBoost1_61_0 = new BoostInfo(
                    "boost_1_61_0.zip",
                    urls,
                    eBoostVersion.Boost1_61_0
                );

                list.Add(biBoost1_61_0);
            }

            {
                List<string> urls = new List<string>();
                urls.Add(@"https://dl.dropboxusercontent.com/u/59547297/Libraries/boost_1_62_0.zip");
                urls.Add(@"http://netcologne.dl.sourceforge.net/project/boost/boost/1.62.0/boost_1_62_0.zip");
                urls.Add(@"http://netassist.dl.sourceforge.net/project/boost/boost/1.62.0/boost_1_62_0.zip");

                BoostInfo biBoost1_62_0 = new BoostInfo(
                    "boost_1_62_0.zip",
                    urls,
                    eBoostVersion.Boost1_62_0
                );

                list.Add(biBoost1_62_0);
            }

            {
                List<string> urls = new List<string>();
                urls.Add(@"https://netix.dl.sourceforge.net/project/boost/boost/1.63.0/boost_1_63_0.zip");
                urls.Add(@"https://dl.dropboxusercontent.com/content_link/XCF6E7mw1RFsyyakII4LVbhB7v4y0ocnPTqzOTTuZM6SIkXuFkWBuc2exlXquuPo/file?dl=1");

                BoostInfo biBoost1_63_0 = new BoostInfo(
                    "boost_1_63_0.zip",
                    urls,
                    eBoostVersion.Boost1_63_0
                );

                list.Add(biBoost1_63_0);
            }

            return list;
        }
  
        public static string BoostVersionToString(eBoostVersion version)
        {
            switch (version)
            {
                case eBoostVersion.Boost1_44_0:
                    return "1.44.0";
                case eBoostVersion.Boost1_51_0:
                    return "1.51.0";
                case eBoostVersion.Boost1_52_0:
                    return "1.52.0";
                case eBoostVersion.Boost1_53_0:
                    return "1.53.0";
                case eBoostVersion.Boost1_54_0:
                    return "1.54.0";
                case eBoostVersion.Boost1_55_0:
                    return "1.55.0";
                case eBoostVersion.Boost1_56_0:
                    return "1.56.0";
                case eBoostVersion.Boost1_57_0:
                    return "1.57.0";
                case eBoostVersion.Boost1_58_0:
                    return "1.58.0";
                case eBoostVersion.Boost1_59_0:
                    return "1.59.0";
				case eBoostVersion.Boost1_60_0:
                    return "1.60.0";
                case eBoostVersion.Boost1_61_0:
                    return "1.61.0";
                case eBoostVersion.Boost1_62_0:
                    return "1.62.0";
                case eBoostVersion.Boost1_63_0:
                    return "1.63.0";
                case eBoostVersion.FromSourceSVN:
                    return "From Source (SVN)";
            }

            throw new Exception("Unknown boost version");
        }
        
        public static string GetDownloadURL(eBoostVersion version)
        {
            foreach (BoostInfo info in CreateInfoList())
            {
                if(info.version == version)
                {
                    return info.DownloadURL;
                }
            }

            throw new Exception("Unknown boost version.");
        }

        public static string GetBoostZipFileName(eBoostVersion version)
        {
            foreach (BoostInfo bi in CreateInfoList())
            {
                if (bi.version == version)
                {
                    return bi.ZIPFilename;
                }
            }

            throw new Exception("Unknown boost version");
        }

        string          zipFilename;
        List<string>    downloadURLDestinations = new List<string>(); // a list of different download urls - sometimes a download destination disappears
        eBoostVersion   version;
    }    

    class boostBuildProcessDescripton
    {
        public string destinationFolder;// Where to download and build boost?
        public eCompiler compilerType;
        public eBoostVersion boostVersion;
        public int coreCount;
        public ePlatform platform;
        public string withLibraries; // empty or for instance " --with-filesystem --with-signals"
        public String B2CommandLineArguments;
    }

    class BoostBuildProcess : BuildProcess
    {
        public BoostBuildProcess(boostBuildProcessDescripton bbpd)
        {
            destinationFolder = bbpd.destinationFolder;
            compilerType = bbpd.compilerType;
            boostVersion = bbpd.boostVersion;
            coreCount = bbpd.coreCount;
            platform = bbpd.platform;
            withLibraries = bbpd.withLibraries;
            B2commandLineArguments = bbpd.B2CommandLineArguments;
        }

        public void DownloadAndBuild()
        {
            try
            {
                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }
                
                // Boost.IOStreams Zlib filters see http://stackoverflow.com/questions/2629421/how-to-use-boost-in-visual-studio-2010
                bool bBuildBoostIOStreams = false;
                if (bBuildBoostIOStreams)
                {
                    message("Downloading zlib127...");
                    DownloadHelper.DownloadFileFromURL(@"http://zlib.net/zlib127.zip", destinationFolder + "zlib127.zip");
                    message("Start to unzip zlib127...");
                    // Unzip zlib127
                    SevenZip.Decompress(destinationFolder + "/" + "zlib127.zip", destinationFolder);

                    string ZLIB_SOURCE = destinationFolder + "zlib127";
                    string ZLIB_INCLUDE = destinationFolder + "zlib127";
                }

                message("Downloading boost...");

                string boostDownloadURL = BoostInfo.GetDownloadURL(boostVersion);
                string boostZIPFilename = BoostInfo.GetBoostZipFileName(boostVersion);
                if (boostVersion == eBoostVersion.FromSourceSVN)
                {
                    string boostFromSourceSVNFolderPath = Path.Combine(destinationFolder, BoostHelper.BoostFromSourceSVNFolderName);
                    DownloadHelper.CheckOutFromSourceSVN("svn", boostDownloadURL, boostFromSourceSVNFolderPath);
                }
                else
                {
                    DownloadHelper.DownloadFileFromURL(boostDownloadURL, destinationFolder + boostZIPFilename);

                    message("Start to unzip boost...");

                    if (boostVersion == eBoostVersion.Boost1_51_0)
                    {
                        FileInfo fi = new FileInfo(destinationFolder + boostZIPFilename);
                        long fileLength = fi.Length;

                        if (fileLength != 91365553)
                        {
                            MessageBox.Show("Invalid file size of " + boostZIPFilename + " Size in bytes should be 91365553");
                        }
                    }

                    // Unzip Boost 
                    SevenZip.Decompress(destinationFolder + "/" + boostZIPFilename, destinationFolder);

                    message("boost has been unzipped!");
                }

                if(boostVersion == eBoostVersion.Boost1_54_0 || compilerType == eCompiler.VS2013)
                {
                    // patch boost!
                    // https://svn.boost.org/trac/boost/attachment/ticket/8750/vc12.patch
              
                }
				
				if(boostVersion == eBoostVersion.Boost1_58_0 && compilerType == eCompiler.VS2015)
                {
                    message("Patching boost 1.58.0 config file to support VS2015.");
                    // patch boost!
                    // http://stackoverflow.com/questions/30760889/c-c-unknown-compiler-version-while-compiling-boost-with-msvc-14-0-vs-2015/30959156#30959156
                    File.Copy(@"boost1.58.0patch\vc14_visualc.hpp", destinationFolder + @"\boost_1_58_0\boost\config\compiler\visualc.hpp", true);              
                }

                message("start building boost...");

                // Build boost
                if (boostVersion == eBoostVersion.Boost1_51_0 || boostVersion == eBoostVersion.Boost1_44_0)
                {
                    buildBoost_1_51_0_x64(destinationFolder);
                }
                else
                {
                    bootstrapBoost_1_52_0_AndLaterVersion();
                    buildBoost_1_52_0_AndLaterVersion();
                }

                // remove downloaded file
                if (File.Exists(Path.Combine(destinationFolder, boostZIPFilename)))
                {
                    System.IO.File.Delete(Path.Combine(destinationFolder, boostZIPFilename));
                }

                message("boost successfully built!");
                              

                OnFinished();
            }
            catch (Exception ex)
            {
                message(string.Empty);
                OnFailure();
                MessageBox.Show(ex.ToString());
            }
        }

        private void buildBoost_1_51_0_x64(string destinationFolder)
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
                    if (compilerType == eCompiler.VS2010)
                    {
                        eBoostVersion version = boostVersion;
                        string extractFolderName = BoostInfo.GetBoostInfo(version).ExtractFolderName;

                        sw.WriteLine("cd /D " + destinationFolder + extractFolderName); //"boost_1_51_0-x64");
                        sw.WriteLine("bootstrap");
                        sw.WriteLine(@".\b2 -j" + Math.Max(2, coreCount) + " --toolset=msvc-10.0 address-model=64 --build-type=complete stage");
                    }
                    else
                    {
                        eBoostVersion version = boostVersion;
                        string extractFolderName = BoostInfo.GetBoostInfo(version).ExtractFolderName;

                        // go to boost root directory
                        sw.WriteLine("cd /D " + destinationFolder + extractFolderName);

                        // should work like this 

                        //sw.WriteLine("bootstrap.bat");
                        //sw.WriteLine(@".\b2 -j" + Math.Max(2, coreCount) + " --toolset=msvc-11.0 address-model=64 --build-type=complete stage"); // todo find out number of cores...

                        // workaround

                        // change directory to boost_1_51_0-x64\tools\build\v2
                        string path = System.Windows.Forms.Application.StartupPath;
                        if (!File.Exists(Path.Combine(destinationFolder, extractFolderName, "b2.exe")) && File.Exists(Path.Combine(path, "b2.exe")))
                        {
                            File.Copy(Path.Combine(path, "b2.exe"), Path.Combine(destinationFolder, extractFolderName, "b2.exe"));
                        }

                        sw.WriteLine(@".\b2 -j" + Math.Max(2, coreCount) + " --toolset=msvc-11.0 address-model=64 --build-type=complete stage"); // todo find out number of cores...
                    }
                }
            }

            readStandardOutput(p);
            readStandardError(p);

            p.WaitForExit();
            p.Close();
        }

        private void bootstrapBoost_1_52_0_AndLaterVersion()
        {
            Process p = new Process();

            ProcessStartInfo info = null;

            if (compilerType == eCompiler.VS2015 ||
                compilerType == eCompiler.VS2013)
            {
                if (platform == ePlatform.x64)
                {
                    info = new ProcessStartInfo("cmd.exe", @"%comspec% /k """ + Compiler.GetCompilerPath(compilerType) + @"\VC\vcvarsall.bat"" amd64");
                }
                else if (platform == ePlatform.x86)
                {
                    info = new ProcessStartInfo("cmd.exe", @"%comspec% /k """ + Compiler.GetCompilerPath(compilerType) + @"\VC\vcvarsall.bat"" x86");
                }
                else
                {
                    throw new Exception("Unknown platform.");
                }
            }
            else if(compilerType == eCompiler.VS2012)
            {
                info = new ProcessStartInfo("cmd.exe");
            }
            else
            {
                throw new Exception("Unknown compiler.");
            }

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
                    eBoostVersion version = boostVersion;
                    string extractFolderName = BoostInfo.GetBoostInfo(version).ExtractFolderName;
                    if (Directory.Exists(destinationFolder + extractFolderName + @"\tools\build\v2"))
                    {
                        sw.WriteLine("cd /D " + destinationFolder + extractFolderName + @"\tools\build\v2");                        
                    }
                    else
                    {
                        sw.WriteLine("cd /D " + destinationFolder + extractFolderName);
                    }
                    sw.WriteLine("bootstrap");                    
                }
            }

            readStandardOutput(p);
            readStandardError(p);

            p.WaitForExit();
            p.Close();
        }

        private void buildBoost_1_52_0_AndLaterVersion()
        {
            Process p = new Process();
            ProcessStartInfo info = CreateVisualStudioCommandPromptProcessStartInfo(compilerType, platform);

            info.RedirectStandardInput = true;
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.CreateNoWindow = true;

            p.StartInfo = info;
            p.Start();

            using (StreamWriter sw = p.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    eBoostVersion version = boostVersion;
                    string extractFolderName = BoostInfo.GetBoostInfo(version).ExtractFolderName;

                    if (!File.Exists(Path.Combine(destinationFolder, extractFolderName, "b2.exe")) && File.Exists(Path.Combine(destinationFolder, extractFolderName, @"tools\build\v2", "b2.exe")))
                    {
                        File.Copy(Path.Combine(destinationFolder, extractFolderName, @"tools\build\v2", "b2.exe"), Path.Combine(destinationFolder, extractFolderName, "b2.exe"));
                    }

                    sw.WriteLine("cd /D " + destinationFolder + extractFolderName);

                    if(platform == ePlatform.x64)
                    {
                        if(compilerType == eCompiler.VS2015)
                        {
                            if(version == eBoostVersion.Boost1_58_0)
                                sw.WriteLine(@".\b2 -j" + Math.Max(2, coreCount) + " --build-type=complete stage" + withLibraries);
                            else
                                sw.WriteLine(@".\b2 -j" + Math.Max(2, coreCount) + " --toolset=msvc-14.0 address-model=64 --build-type=complete stage" + withLibraries);
                        }
                        if (compilerType == eCompiler.VS2013)
                        {
                            sw.WriteLine(@".\b2 -j" + Math.Max(2, coreCount) + " --toolset=msvc-12.0 address-model=64 --build-type=complete stage" + withLibraries); // -sBZIP2_SOURCE=C:\thirdparty\vs2012\x64\zlib-1.2.7");
                        }
                        else if (compilerType == eCompiler.VS2012)
                            sw.WriteLine(@".\b2 -j" + Math.Max(2, coreCount) + " --toolset=msvc-11.0 address-model=64 --build-type=complete stage" + withLibraries); // -sBZIP2_SOURCE=C:\thirdparty\vs2012\x64\zlib-1.2.7");
                        else
                            sw.WriteLine(@".\b2 -j" + Math.Max(2, coreCount) + " --toolset=msvc-10.0 address-model=64 --build-type=complete stage" + withLibraries);
                    }
                    else
                    {
                        System.Diagnostics.Debug.Assert(platform == ePlatform.x86);

                        if (compilerType == eCompiler.VS2013)
                            sw.WriteLine(@".\b2 -j" + Math.Max(2, coreCount) + " --toolset=msvc-12.0 address-model=32 --build-type=complete stage" + withLibraries);
                        else if (compilerType == eCompiler.VS2012)
                            sw.WriteLine(@".\b2 -j" + Math.Max(2, coreCount) + " --toolset=msvc-11.0 address-model=32 --build-type=complete stage" + withLibraries);
                        else
                            sw.WriteLine(@".\b2 -j" + Math.Max(2, coreCount) + " --toolset=msvc-10.0 address-model=32 --build-type=complete stage" + withLibraries);
                    }
                }
            }

            readStandardOutput(p);
            readStandardError(p);

            p.WaitForExit();
            p.Close();
        }

        private void buildBoost1_49_0_gcc(string destinationFolder)
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
                    sw.WriteLine("cd /D " + destinationFolder + "boost_1_49_0-x86");
                    sw.WriteLine("bootstrap.bat gcc");
                    sw.WriteLine(@".\b2 -j" + Math.Max(2, coreCount) + " --toolset=gcc address-model=32 --build-type=complete stage"); // todo find out number of cores...
                }
            }

            readStandardOutput(p);
            readStandardError(p);

            p.WaitForExit();
            p.Close();
        }

        string destinationFolder; // Where to download and build boost?
        eCompiler compilerType;
        eBoostVersion boostVersion;
        int coreCount;
        ePlatform platform;
        string withLibraries;
        String B2commandLineArguments;
    }
}