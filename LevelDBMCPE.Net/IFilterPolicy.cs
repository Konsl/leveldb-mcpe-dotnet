namespace LevelDBMCPE
{
    public interface IFilterPolicy
    {
        string GetName();
        byte[] CreateFilter(byte[][] keys);
        bool KeyMayMatch(byte[] key, byte[] filter);
        void Destructor();
    }
}
