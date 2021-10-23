using System;
using System.Runtime.InteropServices;

namespace LevelDBMCPE
{
    [Serializable]
    public class LevelDBException : Exception
    {
        public LevelDBException() : base("Success"){ }
        public unsafe LevelDBException(char* status) : base(Marshal.PtrToStringAnsi((IntPtr)status)) { }
        public LevelDBException(string status) : base(status) { }
        protected LevelDBException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
