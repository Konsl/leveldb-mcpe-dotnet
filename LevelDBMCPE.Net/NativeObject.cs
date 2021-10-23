using System;

namespace LevelDBMCPE
{
    public abstract class NativeObject : IDisposable
    {
        public IntPtr NativeHandle { get; internal set; } = IntPtr.Zero;

        protected NativeObject(IntPtr handle)
        {
            NativeHandle = handle;
        }

        ~NativeObject()
        {
            if (!IsDisposed)
                Dispose();
        }

        public static implicit operator IntPtr(NativeObject obj)
        {
            return obj.NativeHandle;
        }

        protected abstract void InternalClose();

        public void Destroy() => Dispose();
        public void Close() => Dispose();

        public void Dispose()
        {
            try
            {
                InternalClose();
            }
            catch { }
            NativeHandle = IntPtr.Zero;
        }

        internal void EnsureNotDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        public bool IsDisposed => NativeHandle == IntPtr.Zero;
    }
}
