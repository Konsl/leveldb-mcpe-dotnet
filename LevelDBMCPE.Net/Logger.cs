using System;

namespace LevelDBMCPE
{
    public class Logger : NativeObject
    {
        protected Logger(IntPtr handle) : base(handle) { }

        protected override void InternalClose()
        {
            EnsureNotDisposed();
        }
    }
}
