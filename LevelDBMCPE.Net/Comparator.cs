using System;
using System.Collections.Generic;

namespace LevelDBMCPE
{
    public class Comparator : NativeObject
    {
        private IntPtr State;
        private List<Delegate> delegates;

        protected Comparator(IntPtr handle, IntPtr state, List<Delegate> _delegates) : base(handle)
        {
            State = state;
            delegates = _delegates;
        }

        public static Comparator Create(IComparator comparator)
        {
            IntPtr state;
            do
                state = new IntPtr(Utils.Random.Next());
            while (states.ContainsKey(state));
            states[state] = comparator;
            List<Delegate> delegates = new List<Delegate>();
            return new Comparator(Library.LevelDBComparatorCreate(state, DestructorImpl, CompareImpl, NameImpl, delegates), state, delegates);
        }

        private static Dictionary<IntPtr, IComparator> states = new Dictionary<IntPtr, IComparator>();

        private static void DestructorImpl(IntPtr state)
        {
            if (states.ContainsKey(state))
                states[state].Destructor();
        }

        private static string NameImpl(IntPtr state)
        {
            if (states.ContainsKey(state))
                return states[state].GetName();
            return null;
        }

        private static int CompareImpl(IntPtr state, byte[] a, byte[] b)
        {
            if (states.ContainsKey(state))
                return states[state].Compare(a, b);
            return 0;
        }

        protected override void DisposeManagedResources()
        {
            states.Remove(State);
            delegates.Clear();
        }

        protected override void InternalClose()
        {
            EnsureNotDisposed();
            Library.LevelDBComparatorDestroy(NativeHandle);
            State = IntPtr.Zero;
        }
    }
}
