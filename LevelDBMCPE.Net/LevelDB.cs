using System;
using System.Runtime.InteropServices;
using System.Text;

namespace LevelDBMCPE
{
    public static class LevelDB
    {
        public static Version Version
        {
            get
            {
                return Library.LevelDBGetVersion();
            }
        }

        public static byte[] StringToBytes(string str)
        {
            return Utils.StringToBytes(str);
        }

        public static string BytesToString(byte[] bytes)
        {
            return Utils.BytesToString(bytes);
        }
    }
}
