using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LevelDBMCPE
{
    public class DB : NativeObject
    {
        private DB(IntPtr handle) : base(handle) { }

        internal List<Iterator> iterators = new List<Iterator>();
        public ReadOnlyCollection<Iterator> Iterators { get => iterators.AsReadOnly(); }

        internal List<Snapshot> snapshots = new List<Snapshot>();
        public ReadOnlyCollection<Snapshot> Snapshots { get => snapshots.AsReadOnly(); }

        public Options Options { get; internal set; }

        public static DB Open(string path, Options options = null)
        {
            if (options is null)
                options = Options.Create();
            options.EnsureNotDisposed();
            return new DB(Library.LevelDBOpen(options, path)) { Options = options };
        }

        protected override void DisposeManagedResources()
        {
            foreach (var iter in iterators.ToArray())
            {
                iter.Dispose();
            }
            iterators.Clear();
            foreach (var snapshot in snapshots.ToArray())
            {
                snapshot.Dispose();
            }
            snapshots.Clear();
            Options.Dispose();
            Options = null;
        }

        protected override void InternalClose()
        {
            EnsureNotDisposed();
            Library.LevelDBClose(NativeHandle);
        }

        public void Put(byte[] key, byte[] value, WriteOptions writeOptions = null)
        {
            EnsureNotDisposed();
            if (writeOptions is null)
                writeOptions = WriteOptions.Create();
            writeOptions.EnsureNotDisposed();
            Library.LevelDBPut(NativeHandle, writeOptions, key, value);
        }

        public void Delete(byte[] key, WriteOptions writeOptions = null)
        {
            EnsureNotDisposed();
            if (writeOptions is null)
                writeOptions = WriteOptions.Create();
            writeOptions.EnsureNotDisposed();
            Library.LevelDBDelete(NativeHandle, writeOptions, key);
        }

        public void Write(WriteBatch batch, WriteOptions writeOptions = null)
        {
            EnsureNotDisposed();
            if (writeOptions is null)
                writeOptions = WriteOptions.Create();
            writeOptions.EnsureNotDisposed();
            Library.LevelDBWrite(NativeHandle, writeOptions, batch);
        }

        public byte[] Get(byte[] key, ReadOptions readOptions = null)
        {
            EnsureNotDisposed();
            if (readOptions is null)
                readOptions = ReadOptions.Create();
            readOptions.EnsureNotDisposed();
            return Library.LevelDBGet(NativeHandle, readOptions, key);
        }

        public Iterator CreateIterator(ReadOptions readOptions = null)
        {
            EnsureNotDisposed();
            if (readOptions is null)
                readOptions = ReadOptions.Create();
            readOptions.EnsureNotDisposed();
            return Iterator.Create(this, readOptions);
        }

        public Snapshot CreateSnapshot()
        {
            EnsureNotDisposed();
            return Snapshot.Create(this);
        }

        public void ReleaseSnapshot(Snapshot snapshot)
        {
            EnsureNotDisposed();
            snapshot.EnsureNotDisposed();
            snapshot.Release();
        }

        public string GetPropertyValue(string name)
        {
            EnsureNotDisposed();
            return Library.LevelDBPropertyValue(NativeHandle, name);
        }

        public ulong ApproximateSize(string keyName)
        {
            EnsureNotDisposed();

            return ApproximateSize(Utils.StringToBytes(keyName));
        }

        public ulong ApproximateSize(byte[] keyName)
        {
            EnsureNotDisposed();

            ReadOptions readOptions = ReadOptions.Create();
            readOptions.VerifyChecksums = false;
            Snapshot snapshot = CreateSnapshot();
            readOptions.Snapshot = snapshot;

            Iterator iter = CreateIterator(readOptions);

            for (iter.SeekToFirst(); iter.IsValid; iter.Next())
            {
                byte[] key = iter.Key;
                if (Utils.ArraysAreEqual(keyName, key))
                    break;
            }

            if (!iter.IsValid)
                return 0;

            iter.Next();

            ulong size;

            if (iter.IsValid)
            {
                size = ApproximateSizes(new byte[][] { keyName }, new byte[][] { iter.Key })[0];
            }
            else
            {
                byte[] limitKey = new byte[keyName.Length + 1];
                for (int i = 0; i < limitKey.Length; i++)
                    limitKey[i] = 0xFF;

                size = ApproximateSizes(new byte[][] { keyName }, new byte[][] { limitKey })[0];
            }

            iter.Destroy();
            ReleaseSnapshot(snapshot);
            readOptions.Destroy();

            return size;
        }

        public ulong[] ApproximateSizes(string[] startKeys, string[] limitKeys)
        {
            EnsureNotDisposed();
            if (startKeys is null || limitKeys is null)
                return null;
            int count = Math.Min(startKeys.Length, limitKeys.Length);
            return Library.LevelDBApproximateSizes(NativeHandle, startKeys.Take(count).ToArray(), limitKeys.Take(count).ToArray());
        }

        public ulong[] ApproximateSizes(byte[][] startKeys, byte[][] limitKeys)
        {
            EnsureNotDisposed();
            if (startKeys is null || limitKeys is null)
                return null;
            int count = Math.Min(startKeys.Length, limitKeys.Length);
            return Library.LevelDBApproximateSizes(NativeHandle, startKeys.Take(count).ToArray(), limitKeys.Take(count).ToArray());
        }

        public void CompactRange(string startKey, string limitKey)
        {
            EnsureNotDisposed();
            Library.LevelDBCompactRange(NativeHandle, startKey, limitKey);
        }

        public void CompactRange(byte[] startKey, byte[] limitKey)
        {
            EnsureNotDisposed();
            Library.LevelDBCompactRange(NativeHandle, startKey, limitKey);
        }

        public static void DestroyDB(string path, Options options)
        {
            if (options is null)
                options = Options.Create();
            options.EnsureNotDisposed();
            Library.LevelDBDestroyDB(options, path);
        }

        public static void RepairDB(string path, Options options)
        {
            if (options is null)
                options = Options.Create();
            options.EnsureNotDisposed();
            Library.LevelDBRepairDB(options, path);
        }
    }
}
