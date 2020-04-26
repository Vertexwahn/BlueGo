using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace BlueGo.UnitTest
{
    /// <summary>
    ///     The DownloadHelperTest contains tests to validate URLs that are used to download zip
    /// </summary>
    [TestFixture]
    public class DownloadHelperTest
    {
        #region Methods ------------------------------------------------------------------------------------------------------------------------

        #region Public -------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     CreateBoostInfoListTest validates the Boost download URLs 
        /// </summary>
        [Test]
        public void CreateBoostInfoListTest()
        {
            List<BoostInfo> boostInfoList = BoostInfo.CreateInfoList();
            foreach(BoostInfo boostInfo in boostInfoList)
            {
                Assert.IsTrue(DownloadHelper.RemoteFileExists(boostInfo.DownloadURL));
            }
        }

        /// <summary>
        ///     CreateQtInfoListTest validates the Qt download URLs 
        /// </summary>
        [Test]
        public void CreateQtInfoListTest()
        {
            List<QtInfo> qtInfoList = QtInfo.CreateInfoList();
            foreach (QtInfo qtInfo in qtInfoList)
            {
                Assert.IsTrue(DownloadHelper.RemoteFileExists(qtInfo.DownloadURL));
            }
        }

        /// <summary>
        ///     CreateOpenSceneGraphInfoListTest validates the OSG download URLs 
        /// </summary>
        [Test]
        public void CreateOpenSceneGraphInfoListTest()
        {
            List<OpenSceneGraphInfo> openSceneGraphInfoList = OpenSceneGraphInfo.CreateInfoList();
            foreach (OpenSceneGraphInfo openSceneGraphInfo in openSceneGraphInfoList)
            {
                Assert.IsTrue(DownloadHelper.RemoteFileExists(openSceneGraphInfo.DownloadURL));
            }
        }

        /// <summary>
        ///     CreateLibLASInfoListTest validates the libLAS download URLs 
        /// </summary>
        [Test]
        public void CreateLibLASInfoListTest()
        {
            List<LibLASInfo> libLASInfoList = LibLASInfo.CreateInfoList();
            foreach (LibLASInfo libLASInfo in libLASInfoList)
            {
                Assert.IsTrue(DownloadHelper.RemoteFileExists(libLASInfo.DownloadURL));
            }
        }

        /// <summary>
        ///     CreatePCLInfoListTest validates the PCL download URLs 
        /// </summary>
        [Test]
        public void CreatePCLInfoListTest()
        {
            List<PCLInfo> pclInfoList = PCLInfo.CreateInfoList();
            foreach (PCLInfo pclInfo in pclInfoList)
            {
                Assert.IsTrue(DownloadHelper.RemoteFileExists(pclInfo.DownloadURL));
            }
        }

        /// <summary>
        ///     CreateFlannInfoListTest validates the Flann download URLs 
        /// </summary>
        [Test]
        public void CreateFlannInfoListTest()
        {
            List<FlannInfo> flannInfoList = FlannInfo.CreateInfoList();
            foreach (FlannInfo flannInfo in flannInfoList)
            {
                Assert.IsTrue(DownloadHelper.RemoteFileExists(flannInfo.DownloadURL));
            }
        }

        /// <summary>
        ///     CreateVTKInfoListTest validates the VTK download URLs 
        /// </summary>
        [Test]
        public void CreateVTKInfoListTest()
        {
            List<VTKInfo> vtkInfoList = VTKInfo.CreateInfoList();
            foreach (VTKInfo vtkInfo in vtkInfoList)
            {
                Assert.IsTrue(DownloadHelper.RemoteFileExists(vtkInfo.DownloadURL));
            }
        }

        /// <summary>
        ///     CreateEigenInfoListTest validates the Eigen download URLs 
        /// </summary>
        [Test]
        public void CreateEigenInfoListTest()
        {
            List<EigenInfo> eigenInfList = EigenInfo.CreateInfoList();
            foreach (EigenInfo eigenInfo in eigenInfList)
            {
                Assert.IsTrue(DownloadHelper.RemoteFileExists(eigenInfo.DownloadURL));
            }
        }

        /// <summary>
        ///     RemoteFileExistsEmptyURLTest validates empty URL
        /// </summary>
        [Test]
        public void RemoteFileExistsEmptyURLTest()
        {
            Assert.IsFalse(DownloadHelper.RemoteFileExists(string.Empty));
        }

        /// <summary>
        ///     RemoteFileExistsRandomURLTest validates the Random download URL
        /// </summary>
        [Test]
        public void RemoteFileExistsRandomURLTest()
        {
            Assert.IsFalse(DownloadHelper.RemoteFileExists("http://www.google.com/random.zip"));
        }

        /// <summary>
        ///     RemoteFileExistsNullTest asserts null value URL
        /// </summary>
        [Test]
        public void RemoteFileExistsNullTest()
        {
            Assert.IsFalse(DownloadHelper.RemoteFileExists(null));
        }

        #endregion //Public

        #endregion //Methods
    }
}