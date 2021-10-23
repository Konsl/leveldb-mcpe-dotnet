using System;

namespace LevelDBMCPE
{
    public class Cache : NativeObject
    {
        protected Cache(IntPtr handle) : base(handle) { }

        public static Cache CreateLRU(ulong capacity)
        {
            return new Cache(Library.LevelDBCacheCreateLRU(capacity));
        }

        protected override void DisposeManagedResources() { }

        protected override void InternalClose()
        {
            EnsureNotDisposed();
            Library.LevelDBCacheDestroy(NativeHandle);
        }
    }
}
