using System;

namespace LevelDBMCPE
{
    public class Iterator : NativeObject
    {
        public DB DB { get; internal set; }
        public ReadOptions ReadOptions { get; internal set; }

        protected Iterator(IntPtr handle, DB db) : base(handle)
        {
            DB = db;
            DB.iterators.Add(this);
        }

        public static Iterator Create(DB db, ReadOptions readOptions)
        {
            db.EnsureNotDisposed();
            readOptions.EnsureNotDisposed();
            return new Iterator(Library.LevelDBCreateIterator(db, readOptions), db) { ReadOptions = readOptions };
        }

        protected override void InternalClose()
        {
            EnsureNotDisposed();
            Library.LevelDBIterDestroy(NativeHandle);
            DB.iterators.Remove(this);
        }

        protected override void DisposeManagedResources()
        {
            ReadOptions.Dispose();
            ReadOptions = null;
        }

        public bool IsValid
        {
            get
            {
                EnsureNotDisposed();
                return Library.LevelDBIterValid(NativeHandle);
            }
        }

        public void SeekToFirst()
        {
            EnsureNotDisposed();
            Library.LevelDBIterSeekToFirst(NativeHandle);
        }

        public void SeekToLast()
        {
            EnsureNotDisposed();
            Library.LevelDBIterSeekToLast(NativeHandle);
        }

        public void Seek(string key)
        {
            EnsureNotDisposed();
            Library.LevelDBIterSeek(NativeHandle, Utils.StringToBytes(key));
        }

        public void Seek(byte[] key)
        {
            EnsureNotDisposed();
            Library.LevelDBIterSeek(NativeHandle, key);
        }

        public void Next()
        {
            EnsureNotDisposed();
            Library.LevelDBIterNext(NativeHandle);
        }

        public void Prev()
        {
            EnsureNotDisposed();
            Library.LevelDBIterPrev(NativeHandle);
        }

        public byte[] Key
        {
            get
            {
                EnsureNotDisposed();
                return Library.LevelDBIterKey(NativeHandle);
            }
        }

        public byte[] Value
        {
            get
            {
                EnsureNotDisposed();
                return Library.LevelDBIterValue(NativeHandle);
            }
        }

        public string Error
        {
            get
            {
                EnsureNotDisposed();
                return Library.LevelDBIterGetError(NativeHandle);
            }
        }
    }
}
