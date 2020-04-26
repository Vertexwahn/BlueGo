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
    class BuildBoostTest
    {
        [Test]
        public void DownloadAndBuildBoost1_52_0_VS2010_x64()
        {
            try
            {
                string destinationFolder = GlobalVariables.destinationFolder + @"DownloadAndBuildBoost1_52_0_VS2010_x64\";
                
                eCompiler compiler = eCompiler.VS2010;

                boostBuildProcessDescripton bbpd = new boostBuildProcessDescripton();
                bbpd.boostVersion = eBoostVersion.Boost1_52_0;
                bbpd.compilerType = compiler;
                bbpd.coreCount = 4;
                bbpd.destinationFolder = destinationFolder;
                BoostBuildProcess boostBuildProcess = new BoostBuildProcess(bbpd);

                //boostBuildProcess.Message += new MessageEventHandler(boostBuildProcess_Message);
                //boostBuildProcess.StandardOutputMessage += new StandardOutputMessageEventHandler(boostBuildProcess_StandardOutputMessage);
                //boostBuildProcess.StandardErrorMessage += new StandardErrorMessageEventHandler(boostBuildProcess_StandardErrorMessage);
                //boostBuildProcess.Finished += new FinishedEventHandler(boostBuildProcess_Finished);

                //Thread workerThread = new Thread(boostBuildProcess.DownloadAndBuildBoost_x64);
                //workerThread.Start();

                boostBuildProcess.DownloadAndBuild();
            }
            catch (System.Exception ex)
            {
                Assert.True(false);
            }
        }
    }
}
