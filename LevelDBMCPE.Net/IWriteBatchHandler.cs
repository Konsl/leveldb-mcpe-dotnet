namespace LevelDBMCPE
{
    public interface IWriteBatchHandler
    {
        void Put(byte[] key, byte[] value);
        void Delete(byte[] key);
    }
}
