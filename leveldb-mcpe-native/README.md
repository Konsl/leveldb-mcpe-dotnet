This project can be used to build the Mojang LevelDB branch as a DLL out of the box in Visual Studio 2015.

How to compile:
* After cloning the repo, enter the following command to download LevelDB-MCPE and ZLib sources: `git submodules update --init`
* Open `LevelDB-MCPE.sln` in Visual Studio 2015 (haven't tested 2017).
* Set the desired configuration (Release for normal use, Debug for debugging), and platform (Win32 for 32-bit, x64 for 64-bit).
* Build the solution.
* The compiled DLL will be located in: BuildOutput\\(platform)\\(configuration)\\leveldb-mcpe.dll
