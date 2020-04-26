using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using System.Threading;

namespace BlueGo.UnitTest
{ 
    [TestFixture]
    class OpenCASCADETest
    {
        [Test]
        public void DownloadAndBuildOpenCascade()
        {
            try
            {
                string destinationFolder = GlobalVariables.destinationFolder + @"OpenCascade\";
                
                eCompiler compiler = eCompiler.VS2010;

                openCASCADEBuildProcessDescripton bbpd = new openCASCADEBuildProcessDescripton();
                bbpd.version = eOpenCASCADEVersion.OpenCASCADE_2_4_3;
                bbpd.compilerType = compiler;
                bbpd.coreCount = 4;
                bbpd.destinationFolder = destinationFolder;
                OpenCASCADEBuildProcess buildProcess = new OpenCASCADEBuildProcess(bbpd);

                //boostBuildProcess.Message += new MessageEventHandler(boostBuildProcess_Message);
                //boostBuildProcess.StandardOutputMessage += new StandardOutputMessageEventHandler(boostBuildProcess_StandardOutputMessage);
                //boostBuildProcess.StandardErrorMessage += new StandardErrorMessageEventHandler(boostBuildProcess_StandardErrorMessage);
                //boostBuildProcess.Finished += new FinishedEventHandler(boostBuildProcess_Finished);

                //Thread workerThread = new Thread(boostBuildProcess.DownloadAndBuildBoost_x64);
                //workerThread.Start();

                buildProcess.DownloadAndBuild();
            }
            catch (System.Exception ex)
            {
                Assert.True(false);
            }
        }
    }
}
