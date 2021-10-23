namespace LevelDBMCPE
{
    public interface IComparator
    {
        string GetName();
        int Compare(byte[] a, byte[] b);
        void Destructor();
    }
}
