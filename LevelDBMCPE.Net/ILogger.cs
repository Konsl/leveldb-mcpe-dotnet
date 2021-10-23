using System;
using System.IO;

namespace LevelDBMCPE
{
    public interface ILogger
    {
        void Destructor();
        void Logv(string format, Stream ap);
    }
}
