using System;
using System.Collections.Generic;

namespace LevelDBMCPE
{
    public class Logger : NativeObject
    {
        private IntPtr State;
        private List<Delegate> delegates;

        protected Logger(IntPtr handle, IntPtr state, List<Delegate> _delegates) : base(handle)
        {
            State = state;
            delegates = _delegates;
        }

        public static Logger Create(ILogger logger)
        {
            IntPtr state;
            do
                state = new IntPtr(Utils.Random.Next());
            while (states.ContainsKey(state));
            states[state] = logger;
            List<Delegate> delegates = new List<Delegate>();
            return new Logger(Library.LevelDBLoggerCreate(state, DestructorImpl, LogvImpl, delegates), state, delegates);
        }

        private static Dictionary<IntPtr, ILogger> states = new Dictionary<IntPtr, ILogger>();

        private static void DestructorImpl(IntPtr state)
        {
            if (states.ContainsKey(state))
                states[state].Destructor();
        }

        private static void LogvImpl(IntPtr state, string format, IntPtr ap)
        {
            if (states.ContainsKey(state))
                states[state].Logv(format, new UnsafeMemoryStream(ap));
        }

        protected override void DisposeManagedResources()
        {
            states.Remove(State);
            delegates.Clear();
        }

        protected override void InternalClose()
        {
            EnsureNotDisposed();
            Library.LevelDBLoggerDestroy(NativeHandle);
            State = IntPtr.Zero;
        }
    }
}
