using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BlueGo.GUI;

/// <summary>
/// Main namespace of BlueGo application.
/// </summary>
namespace BlueGo
{
    /// <summary>
    /// Defines the platform. x86 for 32 Bit applications and x64 for 64 bit applications.
    /// </summary>
    public enum ePlatform
    {
        x64,
        x86
    }

    /// <summary>
    /// Defines the kind of compiler. Either Visual Studio 2010, Visual Studio 2012 or none.
    /// </summary>
    public enum eCompiler
    {
        Unknown,
        VS2010,
        VS2012,
        VS2013,
        VS2015,
        VS2019
    }

    /// <summary>
    /// Some helper functions to check if a certain compiler is available.
    /// </summary>
    public class Compiler
    {
        /// <summary>
        /// Checks if a certain compiler is present
        /// </summary>
        /// <param name="compiler">A specific compiler.</param>
        /// <returns>True if compiler is avialable, otherwise false.</returns>
        public static bool IsPresent(eCompiler compiler)
        {
            switch (compiler)
            {
                case eCompiler.VS2010:
                    if (Directory.Exists( PreferencesManager.Instance.VS2010Location))
                    {
                        return true;
                    }
                    break;

                case eCompiler.VS2012:
                    if (Directory.Exists(PreferencesManager.Instance.VS2012Location))
                    {
                        return true;
                    }
                    break;

                case eCompiler.VS2013:
                    if (Directory.Exists(PreferencesManager.Instance.VS2013Location))
                    {
                        return true;
                    }
                    break;

                case eCompiler.VS2015:
                    if (Directory.Exists(PreferencesManager.Instance.VS2015Location))
                    {
                        return true;
                    }
                    break;

                case eCompiler.VS2019:
                    if (Directory.Exists(PreferencesManager.Instance.VS2019Location))
                    {
                        return true;
                    }
                    break;

                default:
                    break;
            }

            return false;
        }

        /// <summary>
        /// Determines the file path of a certain compiler.
        /// </summary>
        /// <param name="compiler">The compiler for which the file system location should be determined.</param>
        /// <returns>Path to compiler.</returns>
        static public string GetCompilerPath(eCompiler compiler)
        {
            switch (compiler)
            {
                case eCompiler.VS2010:
                    {
                        if (Directory.Exists(PreferencesManager.Instance.VS2010Location))
                            return PreferencesManager.Instance.VS2010Location;
                    }
                    break;                    
         
                case eCompiler.VS2012:
                    {
                        if (Directory.Exists(PreferencesManager.Instance.VS2012Location))
                            return PreferencesManager.Instance.VS2012Location;
                    }
                    break;

                case eCompiler.VS2013:
                    {
                        if (Directory.Exists(PreferencesManager.Instance.VS2013Location))
                            return PreferencesManager.Instance.VS2013Location;
                    }
                    break;
                case eCompiler.VS2015:
                    {
                        if (Directory.Exists(PreferencesManager.Instance.VS2015Location))
                            return PreferencesManager.Instance.VS2015Location;
                    }
                    break;

                default:
                    break;
            }

            throw new Exception("Could not local compiler path.");
        }
    }
}
