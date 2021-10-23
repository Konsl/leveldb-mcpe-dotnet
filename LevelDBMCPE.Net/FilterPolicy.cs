using System;
using System.Collections.Generic;

namespace LevelDBMCPE
{
    public class FilterPolicy : NativeObject
    {
        private IntPtr State;

        protected FilterPolicy(IntPtr handle, IntPtr state) : base(handle)
        {
            State = state;
        }

        public static FilterPolicy Create(IFilterPolicy filterPolicy)
        {
            IntPtr state;
            do
                state = new IntPtr(Utils.Random.Next());
            while (states.ContainsKey(state) || state == IntPtr.Zero);
            states[state] = filterPolicy;
            return new FilterPolicy(Library.LevelDBFilterPolicyCreate(state, DestructorImpl, CreateFilterImpl, KeyMayMatchImpl, NameImpl), state);
        }

        public static FilterPolicy CreateBloom(int bitsPerKey)
        {
            return new FilterPolicy(Library.LevelDBFilterPolicyCreateBloom(bitsPerKey), IntPtr.Zero);
        }

        private static Dictionary<IntPtr, IFilterPolicy> states = new Dictionary<IntPtr, IFilterPolicy>();

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

        private static byte[] CreateFilterImpl(IntPtr state, byte[][] keys)
        {
            if (states.ContainsKey(state))
                return states[state].CreateFilter(keys);
            return null;
        }

        private static bool KeyMayMatchImpl(IntPtr state, byte[] key, byte[] filter)
        {
            if (states.ContainsKey(state))
                return states[state].KeyMayMatch(key, filter);
            return false;
        }

        protected override void InternalClose()
        {
            EnsureNotDisposed();
            Library.LevelDBFilterPolicyDestroy(NativeHandle);
            if (states.ContainsKey(State))
                states.Remove(State);
            State = IntPtr.Zero;
        }
    }
}
