using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BlueGo.GUI;

namespace BlueGo
{
    // http://www.cmake.org/cmake/help/v2.8.8/cmake.html#command:find_package
    class Package
    {
        public Package(string package)
        {
            package_ = package;
        }

        string package_;
        string version_;
        string components_;
    }
}
