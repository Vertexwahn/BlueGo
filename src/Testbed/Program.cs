using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Testbed
{
    class Foo
    {
        //StreamWriter _outputStream;

        // Called asynchronously with a line of data
        private void OnDataReceived(object Sender, DataReceivedEventArgs e)
        {
            if ((e.Data != null) )
            {
                //_outputStream.WriteLine(e.Data);
                Console.WriteLine(e.Data);
            }
        }

        // http://stackoverflow.com/questions/139593/processstartinfo-hanging-on-waitforexit-why
        public void BuildQt2()
        {
            using (Process process = new Process())
            {
                int timeout = 1000 * 30;

                string compilerPath = @"C:\Program Files (x86)\Microsoft Visual Studio 10.0";
                process.StartInfo = new ProcessStartInfo("cmd.exe", @"%comspec% /k """ + compilerPath + @"\VC\vcvarsall.bat"" amd64");
                //process.StartInfo.FileName = filename;
                //process.StartInfo.Arguments = arguments;
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
                            Console.WriteLine(e.Data);
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
                            Console.WriteLine(e.Data);
                        }
                    };

                    process.Start();

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    using (StreamWriter sw = process.StandardInput)
                    {
                        if (sw.BaseStream.CanWrite)
                        {
                            sw.WriteLine("cd /D " + @"C:\thirdparty\vs2010\x64\qt-everywhere-opensource-src-5.0.2");
                            sw.WriteLine("configure -developer-build -opensource -opengl desktop -nomake examples -nomake tests");
                            sw.WriteLine("y");
                        }
                    }

                    if (process.WaitForExit(timeout) &&
                        outputWaitHandle.WaitOne(timeout) &&
                        errorWaitHandle.WaitOne(timeout))
                    {
                        // Process completed. Check process.ExitCode here.
                    }
                    else
                    {
                        // Timed out.
                    }
                }
            }
        }

        public void BuildQt()
        {
            try
            {
                ProcessStartInfo info = null;

                string compilerPath = @"C:\Program Files (x86)\Microsoft Visual Studio 10.0";

                Process p = new Process();
                info = new ProcessStartInfo("cmd.exe", @"%comspec% /k """ + compilerPath + @"\VC\vcvarsall.bat"" amd64");

                info.RedirectStandardInput = true;
                info.UseShellExecute = false;
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;

                p.StartInfo = info;

                p.OutputDataReceived += new DataReceivedEventHandler(OnDataReceived);
                p.ErrorDataReceived += new DataReceivedEventHandler(OnDataReceived);

                p.Start();
                p.BeginOutputReadLine();


                using (StreamWriter sw = p.StandardInput)
                {
                    if (sw.BaseStream.CanWrite)
                    {
                        sw.WriteLine("cd /D " + @"C:\thirdparty\vs2010\x64\qt-everywhere-opensource-src-5.0.2");
                      
                        

                        sw.WriteLine("configure -developer-build -opensource -opengl desktop -nomake examples -nomake tests");
                      
                        sw.WriteLine("y");
                    }
                }

                
               // p.WaitForExit();
                p.WaitForExit();
                //TODO error checks here

            }
            catch (Exception e)
            {
                throw e;
            }
        }// function
    }

    class Program
    {
        static void Main(string[] args)
        {
            Foo f = new Foo();
            f.BuildQt2();
            int x;
        }
    }
}
