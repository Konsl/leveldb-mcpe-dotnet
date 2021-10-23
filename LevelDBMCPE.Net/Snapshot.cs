using System;

namespace LevelDBMCPE
{
    public class Snapshot : NativeObject
    {
        public DB DB { get; internal set; }

        protected Snapshot(IntPtr handle, DB db) : base(handle)
        {
            DB = db;
        }

        public static Snapshot Create(DB db)
        {
            if (db is null)
                throw new ArgumentNullException(nameof(db));
            db.EnsureNotDisposed();

            return new Snapshot(Library.LevelDBCreateSnapshot(db), db);
        }

        public void Release() => Dispose();

        protected override void InternalClose()
        {
            EnsureNotDisposed();
            Library.LevelDBReleaseSnapshot(DB, NativeHandle);
        }
    }
}
