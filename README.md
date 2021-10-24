# leveldb-mcpe-dotnet
leveldb-mcpe for C#

## Projects
- **LevelDB-MCPE-Native:** native leveldb library (c, c++)
- **LevelDBMCPE.Net:** .NET libary which uses the native library (C#)
- **LibraryTest:** project to test the LevelDBMCPE.Net library (C#)

## Changes to leveldb-mcpe:
- export all functions in c.h
- function to set a specific compressor in leveldb_options_t
- support for a custom logger implementation
- support for setting a decompress allocator
