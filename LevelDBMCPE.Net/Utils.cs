using System;
using System.Linq;

namespace LevelDBMCPE
{
    internal static class Utils
    {
        public static Random Random = new Random();

        public static IntPtr GetIntPtrLength(this Array array)
        {
            if (IntPtr.Size == 4)
                return (IntPtr)array.Length;
            return (IntPtr)array.LongLength;
        }

        public static unsafe T[] PointerToArray<T>(T* pointer, IntPtr length, bool levelDBFree) where T : unmanaged
        {
            return PointerToArray(pointer, (long)length, levelDBFree);
        }

        public static unsafe T[] PointerToArray<T>(T* pointer, long length, bool levelDBFree) where T : unmanaged
        {
            if (pointer == null)
                return null;
            T[] array = new T[length];
            for (long i = 0; i < length; i++)
            {
                array[i] = pointer[i];
            }
            if (levelDBFree)
                Library.LevelDBFree((IntPtr)pointer);
            return array;
        }

        public static bool ArraysAreEqual<T>(T[] a, T[] b) where T : IComparable<T>
        {
            if (a is null)
            {
                if (b is null)
                    return true;
                return false;
            }
            if (a.Length != b.Length)
                return false;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i].CompareTo(b[i]) != 0)
                    return false;
            }
            return true;
        }

        public static byte[] StringToBytes(string str)
        {
            return str.Select((char c) => unchecked((byte)c)).ToArray();
        }

        public static string BytesToString(byte[] bytes)
        {
            return new string(bytes.Select((byte b) => unchecked((char)b)).ToArray());
        }
    }
}
