using System;

namespace LevelDBMCPE
{
    public class Env : NativeObject
    {
        protected Env(IntPtr handle) : base(handle) { }

        public static Env CreateDefault()
        {
            return new Env(Library.LevelDBEnvCreateDefault());
        }

        protected override void InternalClose()
        {
            EnsureNotDisposed();
            Library.LevelDBEnvDestroy(NativeHandle);
        }
    }
}
