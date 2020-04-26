using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using System.Net;

namespace BlueGo
{
    namespace UnitTest
    {
        [TestFixture]
        public class DownloadTest
        {
            [Test]
            public void DownloadBoost()
            {
                try
                {
                    eBoostVersion boostVersion = eBoostVersion.Boost1_52_0;

                    string destinationFolder = GlobalVariables.destinationFolder +  @"DownloadBoost\";

                    if (!Directory.Exists(destinationFolder))
                    {
                        Directory.CreateDirectory(destinationFolder);
                    }

                    string boostDownloadURL = BoostInfo.GetDownloadURL(boostVersion);
                    string boostZIPFilename = BoostInfo.GetBoostZipFileName(boostVersion);

                    // Download boost
                    // Check if URL is valid
                    bool validUrl = DownloadHelper.RemoteFileExists(boostDownloadURL);
                    Assert.True(validUrl);

                    DownloadHelper.DownloadFileFromURL(boostDownloadURL, destinationFolder + boostZIPFilename);

                    if (boostVersion == eBoostVersion.Boost1_51_0)
                    {
                        FileInfo fi = new FileInfo(destinationFolder + boostZIPFilename);
                        long fileLength = fi.Length;

                        Assert.True(fileLength == 91365553);
                    }

                    if (boostVersion == eBoostVersion.Boost1_52_0)
                    {
                        FileInfo fi = new FileInfo(destinationFolder + boostZIPFilename);
                        long fileLength = fi.Length;

                        Assert.True(fileLength == 95342510);
                    }

                    // Delete File
                    System.IO.File.Delete(destinationFolder + boostZIPFilename);
                }
                catch (System.Exception ex)
                {
                    Assert.True(false);
                }
            }
        } // end class DownloadTest
    } // end namespace UnitTest
} // end namespace BlueGo
