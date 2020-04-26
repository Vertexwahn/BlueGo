using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace BlueGo
{
    class DetectSuperfluousIncludesDescription
    {
        public string projectName;
        public string destinationFolder;// Where to download and build boost?
        public eCompiler compilerType;
        public eOpenSceneGraphVersion version;
        public int coreCount;
        public ePlatform platform;
        public string withLibraries; // empty or for instance " --with-filesystem --with-signals"
        public string sourceFilePath;
    }

    class DetectSuperfluousIncludes : BuildProcess
    {
        public DetectSuperfluousIncludes(DetectSuperfluousIncludesDescription desc)
        {
            projectName = desc.projectName;
            destinationFolder = desc.destinationFolder;
            compilerType = desc.compilerType;
            coreCount = desc.coreCount;
            platform = desc.platform;
            withLibraries = desc.withLibraries;
            sourceFilePath = desc.sourceFilePath;
        }

        private bool runMSBuild(string destinationFolder)
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
                    sw.WriteLine("cd /D " + destinationFolder);
                    sw.WriteLine("msbuild " + projectName);
                }
            }

            readStandardOutput(p);
            readStandardError(p);

            p.WaitForExit();

            bool successfulCompilation = false;
            if (p.ExitCode == 0)
            {
                successfulCompilation = true;
            }
            
            p.Close();

            for (int i = 0; i < lastStandardOutput.Size; i++)
            {
                string lastOutput = lastStandardOutput.getLastItem();

                //message("debugOutput: (" + i + ")" + lastOutput );

                if (i == 5)
                {
                    if (lastOutput == "    0 Fehler")
                    {
                        message("No errors.");
                        return true;
                    }
                    else
                    {
                        message("Found errors.");
                        return false;
                    }
                }
            }

            return successfulCompilation;
        }


        public void FindSuperfluousIncludes()
        {
            // code should work
            if (!runMSBuild(destinationFolder))
            {
                message("Error: Project can not be compiled");
                return;
            }

            // no modify code to find superfluous includes
            string[] orginalSourceCode = System.IO.File.ReadAllLines(sourceFilePath);

            // find out in which lines something is included
            List<int> includeFileLineNumbers = new List<int>();
            
            for (int i = 0; i < orginalSourceCode.Length; i++)
            {
                if (orginalSourceCode[i].StartsWith("#include"))
                {
                    message("Include " + orginalSourceCode[i] + " found in line " + i);

                    includeFileLineNumbers.Add(i);
                }
            }

            List<int> superfluousIncludesLineNumbers = new List<int>();

            // now uncomment each include file to find out if it is needed or not in the current build configuration
            for (int i = 0; i < includeFileLineNumbers.Count; i++ )
            {
                string modifiedFileSource = "";

                // uncomment corresponding file
                for (int lineNumber = 0; lineNumber < orginalSourceCode.Length; lineNumber++ )
                {
                    if (lineNumber != includeFileLineNumbers[i])
                    {
                        modifiedFileSource += orginalSourceCode[lineNumber] + Environment.NewLine;
                    }
                    else
                    {
                        modifiedFileSource += "// " + orginalSourceCode[lineNumber] + Environment.NewLine; ;
                    }
                }

                // save file
                using (StreamWriter outfile = new StreamWriter(sourceFilePath))
                {
                    outfile.Write(modifiedFileSource);
                }


                // build it!
                if (runMSBuild(destinationFolder))
                {
                    superfluousIncludesLineNumbers.Add(includeFileLineNumbers[i]);
                }
            }

            // no mark all superfluous includes

            string modifiedFileSource2 = "";
            int si = 0;

            for (int lineNumber = 0; lineNumber < orginalSourceCode.Length; lineNumber++)
            {
                if (superfluousIncludesLineNumbers.Count > 0 &&
                    si < superfluousIncludesLineNumbers.Count &&
                    lineNumber == superfluousIncludesLineNumbers[si])
                {
                    si++;
                    modifiedFileSource2 += "// " + orginalSourceCode[lineNumber] + " // detected superfluous include" + Environment.NewLine;
                }
                else
                {
                    modifiedFileSource2 += orginalSourceCode[lineNumber] + Environment.NewLine;
                }
            }

            // save file
            using (StreamWriter outfile = new StreamWriter(sourceFilePath))
            {
                outfile.Write(modifiedFileSource2);
            }
        }

        string destinationFolder;// Where to download and build boost?
        eCompiler compilerType;
        string projectName;
        int coreCount;
        ePlatform platform;
        string withLibraries;
        string sourceFilePath;
    }
}
