using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelDBMCPE
{
    public class Comparator : NativeObject
    {
        private IntPtr State;

        protected Comparator(IntPtr handle, IntPtr state) : base(handle)
        {
            State = state;
        }

        public static Comparator Create(IComparator comparator)
        {
            IntPtr state;
            do
                state = new IntPtr(Utils.Random.Next());
            while (states.ContainsKey(state));
            states[state] = comparator;
            return new Comparator(Library.LevelDBComparatorCreate(state, DestructorImpl, CompareImpl, NameImpl), state);
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

        protected override void InternalClose()
        {
            EnsureNotDisposed();
            Library.LevelDBComparatorDestroy(NativeHandle);
            states.Remove(State);
            State = IntPtr.Zero;
        }
    }
}
