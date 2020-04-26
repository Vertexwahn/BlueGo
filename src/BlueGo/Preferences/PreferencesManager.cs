using System;
using System.Drawing;
using System.IO;

namespace BlueGo
{
    class PreferencesManager
    {
        static Preferences m_Preferences;

        public static Preferences Instance
        {
            get
            {
                if (m_Preferences == null)
                {
                    m_Preferences = new Preferences();
                }

                return m_Preferences;
            }
        }
    }
}