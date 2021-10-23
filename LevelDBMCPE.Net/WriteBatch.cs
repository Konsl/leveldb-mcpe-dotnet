using System;
using System.Collections.Generic;

namespace LevelDBMCPE
{
    public class WriteBatch : NativeObject
    {
        protected WriteBatch(IntPtr handle) : base(handle) { }

        public static WriteBatch Create()
        {
            return new WriteBatch(Library.LevelDBWriteBatchCreate());
        }

        protected override void InternalClose()
        {
            EnsureNotDisposed();
            Library.LevelDBWriteBatchDestroy(NativeHandle);
        }

        public void Clear()
        {
            EnsureNotDisposed();
            Library.LevelDBWriteBatchClear(NativeHandle);
        }

        public void Put(byte[] key, byte[] value)
        {
            EnsureNotDisposed();
            Library.LevelDBWriteBatchPut(NativeHandle, key, value);
        }

        public void Delete(byte[] key)
        {
            EnsureNotDisposed();
            Library.LevelDBWriteBatchDelete(NativeHandle, key);
        }

        public void Iterate(IWriteBatchHandler handler)
        {
            IntPtr state;
            do
                state = new IntPtr(Utils.Random.Next());
            while (states.ContainsKey(state));
            states[state] = handler;
            Library.LevelDBWriteBatchIterate(NativeHandle, state, IteratePutImpl, IterateDeleteImpl);
            states.Remove(state);
        }

        private static Dictionary<IntPtr, IWriteBatchHandler> states = new Dictionary<IntPtr, IWriteBatchHandler>();

        private static void IteratePutImpl(IntPtr state, byte[] key, byte[] value)
        {
            if (states.ContainsKey(state))
                states[state].Put(key, value);
        }

        private static void IterateDeleteImpl(IntPtr state, byte[] key)
        {
            if (states.ContainsKey(state))
                states[state].Delete(key);
        }
    }
}
