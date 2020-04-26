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
    public delegate void MessageEventHandler(object sender, string message);
    public delegate void StandardOutputMessageEventHandler(object sender, string message);
    public delegate void StandardErrorMessageEventHandler(object sender, string message);
    public delegate void FinishedEventHandler(object sender);
    public delegate void FailureEventHandler(object sender);

    class BuildProcess
    {
        // Used to show the user some hints like "Started downloading..."
        public event MessageEventHandler Message;

        // Used to redirect standard output messages
        public event StandardOutputMessageEventHandler StandardOutputMessage;

        // TODO: it is not clear for users when to use Message and when to use StandardOutputMessage
        // offer just writeMessage or message function or Log function which takes a enum type {error info trace verbose}
        // 1. remove message function 2. rename StandardOutputMessage

        // Used to redirect standard error output.
        public event StandardErrorMessageEventHandler StandardErrorMessage;

        // Called when the build process has finished.
        public event FinishedEventHandler Finished;

        // Called when the build process has encountered some exception.
        public event FailureEventHandler Failure;

        public BuildProcess()
        {
            lastStandardOutput = new RingBuffer(10);
        }
        
        protected void readStandardOutput(Process p)
        {
            using (StreamReader sr = p.StandardOutput)
            {
                while (sr.BaseStream.CanRead)
                {
                    string line = sr.ReadLine();

                    if (line == null)
                        break;

                    lastStandardOutput.addItem(line);

                    writeStandardOutputMessage(line);
                }
            }
        }

        protected void readStandardError(Process p)
        {
            using (StreamReader sr = p.StandardError)
            {
                while (sr.BaseStream.CanRead)
                {
                    string test = sr.ReadLine();

                    if (test == null)
                        break;

                    writeStandardErrorMessage(test);
                }
            }
        }      

        protected void message(string msg)
        {
            if (Message != null)
                Message(this, msg);
        }

        protected void writeStandardOutputMessage(string message)
        {
            if (StandardOutputMessage != null)
            {
                StandardOutputMessage(this, message);
            }
        }

        protected void writeStandardErrorMessage(string message)
        {
            if (StandardOutputMessage != null)
            {
                StandardErrorMessage(this, message);
            }
        }

        protected void OnFinished()
        {
            if (Finished != null)
            {
                Finished(this);
            }
        }

        protected void OnFailure()
        {
            if (Failure != null)
            {
                Failure(this);
            }
        }

        public ProcessStartInfo CreateVisualStudioCommandPromptProcessStartInfo(eCompiler compilerType, ePlatform platformType)
        {
            ProcessStartInfo info = null;

            string compilerPath = Compiler.GetCompilerPath(compilerType);

            if (platformType == ePlatform.x64)
            {
                info = new ProcessStartInfo("cmd.exe", @"%comspec% /k """ + compilerPath + @"\VC\vcvarsall.bat"" amd64");
            }
            else if (platformType == ePlatform.x86)
            {
                info = new ProcessStartInfo("cmd.exe", @"%comspec% /k """ + compilerPath + @"\VC\vcvarsall.bat"" x86");
            }
            else
            {
                throw new Exception("Unknown Platform type.");
            }

            return info;
        }

        protected RingBuffer lastStandardOutput;
    }
}