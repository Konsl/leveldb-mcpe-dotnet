using System;

namespace LevelDBMCPE
{
    public class ReadOptions : NativeObject
    {
        protected ReadOptions(IntPtr handle) : base(handle) { }

        public static ReadOptions Create()
        {
            return new ReadOptions(Library.LevelDBReadOptionsCreate());
        }

        protected override void InternalClose()
        {
            EnsureNotDisposed();
            Library.LevelDBReadOptionsDestroy(NativeHandle);
        }

        public bool VerifyChecksums
        {
            set
            {
                EnsureNotDisposed();
                Library.LevelDBReadOptionsSetVerifyChecksums(NativeHandle, value);
            }
        }

        public bool FillCache
        {
            set
            {
                EnsureNotDisposed();
                Library.LevelDBReadOptionsSetFillCache(NativeHandle, value);
            }
        }

        public Snapshot Snapshot
        {
            set
            {
                EnsureNotDisposed();
                Library.LevelDBReadOptionsSetSnapshot(NativeHandle, value);
            }
        }
    }
}
