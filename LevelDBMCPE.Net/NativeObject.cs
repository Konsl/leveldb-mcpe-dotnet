using log4net;
using System;

namespace LevelDBMCPE
{
    public abstract class NativeObject : IDisposable
    {
        protected readonly ILog Log;

        public IntPtr NativeHandle { get; internal set; } = IntPtr.Zero;

        protected NativeObject(IntPtr handle)
        {
            Log = LogManager.GetLogger(GetType());
            Log.Debug($"NativeObject({handle})");
            NativeHandle = handle;
        }

        ~NativeObject()
        {
            Dispose(false);
        }

        public static implicit operator IntPtr(NativeObject obj)
        {
            return obj.NativeHandle;
        }

        protected abstract void InternalClose();
        protected abstract void DisposeManagedResources();

        public void Destroy() => Dispose();
        public void Close() => Dispose();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                Log.Debug($"Dispose({disposing})");

                if (disposing)
                    DisposeManagedResources();

                Log.Debug($"InternalClose()");
                InternalClose();
                NativeHandle = IntPtr.Zero;
            }
        }

        internal void EnsureNotDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        public bool IsDisposed => NativeHandle == IntPtr.Zero;
    }
}
