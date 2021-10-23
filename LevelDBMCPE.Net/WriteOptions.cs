using System;

namespace LevelDBMCPE
{
    public class WriteOptions : NativeObject
    {
        protected WriteOptions(IntPtr handle) : base(handle) { }

        public static WriteOptions Create()
        {
            return new WriteOptions(Library.LevelDBWriteOptionsCreate());
        }

        protected override void InternalClose()
        {
            EnsureNotDisposed();
            Library.LevelDBWriteOptionsDestroy(NativeHandle);
        }

        public bool Sync
        {
            set
            {
                EnsureNotDisposed();
                Library.LevelDBWriteOptionsSetSync(NativeHandle, value);
            }
        }
    }
}
