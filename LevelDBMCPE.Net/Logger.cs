using System;
using System.Collections.Generic;

namespace LevelDBMCPE
{
    public class Logger : NativeObject
    {
        private IntPtr State;

        protected Logger(IntPtr handle, IntPtr state) : base(handle)
        {
            State = state;
        }

        public static Logger Create(ILogger logger)
        {
            IntPtr state;
            do
                state = new IntPtr(Utils.Random.Next());
            while (states.ContainsKey(state));
            states[state] = logger;
            return new Logger(Library.LevelDBLoggerCreate(state, DestructorImpl, LogvImpl), state);
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

        protected override void InternalClose()
        {
            EnsureNotDisposed();
            Library.LevelDBLoggerDestroy(NativeHandle);
            states.Remove(State);
            State = IntPtr.Zero;
        }
    }
}
