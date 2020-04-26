using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace BlueGo
{
    namespace Util
    {
        /// <summary>
        ///     Enum for Visual Studio Product version
        /// </summary>
        public enum eVisualStudioProduct
        {
            Visual_Studio_10_2010,
            Visual_Studio_11_2012,
            Visual_Studio_12_2013,
            Visual_Studio_14_2015
        }

        /// <summary>
        /// Used to crypt passwords.
        /// </summary>
        public class Cryptomat
        {
            public static string EncryptMessage(string plainMessage, string password)
            {
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
                des.IV = new byte[8];
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, new byte[0]);
                des.Key = pdb.CryptDeriveKey("RC2", "MD5", 128, new byte[8]);
                MemoryStream ms = new MemoryStream(plainMessage.Length * 2);
                CryptoStream encStream = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainMessage);
                encStream.Write(plainBytes, 0, plainBytes.Length);
                encStream.FlushFinalBlock();
                byte[] encryptedBytes = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(encryptedBytes, 0, (int)ms.Length);
                encStream.Close();
                return Convert.ToBase64String(encryptedBytes);
            }

            public static string DecryptMessage(string encryptedBase64, string password)
            {
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
                des.IV = new byte[8];
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, new byte[0]);
                des.Key = pdb.CryptDeriveKey("RC2", "MD5", 128, new byte[8]);
                byte[] encryptedBytes = Convert.FromBase64String(encryptedBase64);
                MemoryStream ms = new MemoryStream(encryptedBase64.Length);
                CryptoStream decStream = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                decStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                decStream.FlushFinalBlock();
                byte[] plainBytes = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(plainBytes, 0, (int)ms.Length);
                decStream.Close();
                return Encoding.UTF8.GetString(plainBytes);
            }  
        }

        public class Tuple<T, U, V>
        {
            public Tuple(T first, U second, V third)
            {
                m_First = first;
                m_Second = second;
                m_Third = third;
            }

            private T m_First;
            private U m_Second;
            private V m_Third;

            public T First
            {
                get { return m_First; }
                set { m_First = value; }
            }
            
            public U Second
            {
                get { return m_Second; }
                set { m_Second = value; }
            }
            
            public V Third
            {
                get { return m_Third; }
                set { m_Third = value; }
            }
        }

        public static class Helper
        {
            public static List<T> FindDuplicates<T>(List<T> inputList)
            {
                Dictionary<T, int> uniqueStore = new Dictionary<T, int>();
                List<T> finalList = new List<T>();
                foreach (T currValue in inputList)
                {
                    if (!uniqueStore.ContainsKey(currValue))  // 
                    {
                        uniqueStore.Add(currValue, 0);  
                    }
                    else
                    {
                        finalList.Add(currValue);
                    }
                }
                return finalList;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="inputList"></param>
            /// <returns></returns>
            static List<string> removeDuplicates(List<string> inputList)
            {
                Dictionary<string, int> uniqueStore = new Dictionary<string, int>();
                List<string> finalList = new List<string>();
                foreach (string currValue in inputList)
                {
                    if (!uniqueStore.ContainsKey(currValue))
                    {
                        uniqueStore.Add(currValue, 0);
                        finalList.Add(currValue);
                    }
                }
                return finalList;
            }

            /// <summary>
            /// Da die Klasse TimeSpan keine Methode .ToString mit einem Format String anbietet, kann man 
            /// diesen Umweg gehen.
            /// </summary>
            /// <param name="timeSpan"></param>
            /// <param name="formatString"></param>
            /// <returns></returns>
            public static string ToString(TimeSpan timeSpan, string formatString)
            {
                DateTime tmp = new DateTime() + timeSpan;
                return tmp.ToString(formatString); 
            }

            /// <summary>
            /// Wird von der Methode FindItemTextIndex geworfen, wenn das entsprechende Item nicht gefunden
            /// werden konnte.
            /// </summary>
            public class FindItemTextIndexNotFoundException : Exception
            {
                /// <summary>
                /// Initialisiert eine neue Instanz der NotFoundException-Klasse mit einer angegebenen
                //  Fehlermeldung.
                /// </summary>
                /// <param name="message">Die Meldung, in der der Fehler beschrieben wird.</param>
                public FindItemTextIndexNotFoundException(string message)
                    : base(message)
                {

                }
            }

            /// <summary>
            /// Liefert den Index eines bestimmten Items innerhalb einer ComboBox, das als String gegeben ist.
            /// </summary>
            /// <example>
            ///     <code>
            ///         comboBox1.Items.Clear();
            ///         comboBox1.Items.Add("1ter Eintrag");
            ///         comboBox1.Items.Add("2ter Eintrag");
            ///         comboBox1.Items.Add("3ter Eintrag");
            ///         comboBox1.Items.Add("4ter Eintrag");
            ///         
            ///         FindComboBoxIndex("2ter Eintrag") liefert 1 
            ///    </code>
            /// </example>
            /// <param name="comboBox">comboBox, in der gesucht werden soll.</param>
            /// <param name="itemText">String, dessen Index in der ComboBox ermittelt werden soll.</param>
            /// <returns>Index des gefundenen Elements.</returns>
            public static int FindItemTextIndex(ComboBox comboBox, string itemText)
            {
                System.Diagnostics.Trace.Assert(comboBox != null);
                System.Diagnostics.Trace.Assert(itemText != null);

                for (int index = 0; index < comboBox.Items.Count; index++)
                {
                    try
                    {
                        string comboBoxItemText = (string)(comboBox.Items[index]);
                        if (comboBoxItemText == itemText)
                        {
                            return index;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.Assert(false);
                        MessageBox.Show(ex.ToString());
                    }
                }

                throw new FindItemTextIndexNotFoundException(
                    "Item mit dem Text \"" + itemText + "\" konnte nicht gefunden werden.");
            }

            /// <summary>
            ///     Gets the Visual Studio product name as a string
            /// </summary>
            /// <param name="vsProduct">
            ///     Enum value for which string value is required
            /// </param>
            /// <returns>
            ///     Visual Studio product name as a string
            /// </returns>
            public static string GetVisualStudioProductName(eVisualStudioProduct vsProduct)
            {
                switch (vsProduct)
                {
                    case eVisualStudioProduct.Visual_Studio_10_2010:
                        return "Visual Studio 10";

                    case eVisualStudioProduct.Visual_Studio_11_2012:
                        return "Visual Studio 11";

                    case eVisualStudioProduct.Visual_Studio_12_2013:
                        return "Visual Studio 12";

                    case eVisualStudioProduct.Visual_Studio_14_2015:
                        return "Visual Studio 14";
                }

                throw new Exception("Unknown Visual Studio Product");
            }                  
        }
    
        /// <summary>
        ///     Helper class for Boost
        /// </summary>
        public static class BoostHelper
        {
            /// <summary>
            ///     Boost svn checkout folder name
            /// </summary>
            public const string BoostFromSourceSVNFolderName = "boost_FromSourceSVN";             
        }
    
        /// <summary>
        ///     DependencyChecker class checks dependencies for the features 
        ///     i.e checks for cmake, msbuild etc.
        /// </summary>
        public static class DependencyChecker
        {
            private const string CMakeNotInstalledMessage = "CMake is not available on the path or is not installed.\n\n" +
                       "If CMake is installed, configure the cmake exe path in BlueGo under File->Preferences->BuildDependencies\n\n";
            private const string MSBuildNotInstalledMessage = "MSBuild is not available on the path or is not installed.\n" +
                        "Please install latest version of MSBuild.\n\n" +
                        "If MSBuild is installed, configure the msbuild exe path in BlueGo under File->Preferences->BuildDependencies\n\n";
        
            /// <summary>
            ///     Checks comman dependencies like cmake & msbuild.
            /// </summary>
            public static void CheckCommanDependency()
            {
                if (!Executable.IsCMakeInstalled())
                {
                    throw new Exception(CMakeNotInstalledMessage);
                }

                if (!Executable.IsMSBuildInstalled())
                {
                    throw new Exception(MSBuildNotInstalledMessage);
                }
            }
        }

        /// <summary>
        ///     Folder class defines standard folder name as constant
        /// </summary>
        public static class Folder
        {
            /// <summary>
            ///     Bin folder name
            /// </summary>
            public const string BinFolderName = "bin";

            /// <summary>
            ///     Lib folder name
            /// </summary>
            public const string LibFolderName = "lib";

            /// <summary>
            ///     Release folder name
            /// </summary>
            public const string ReleaseFolderName = "Release";

            /// <summary>
            ///     Source folder name
            /// </summary>
            public const string SourceFolderName = "src";

            /// <summary>
            ///     Cpp folder name
            /// </summary>
            public const string CppFolderName = "cpp";
        }
    }
}