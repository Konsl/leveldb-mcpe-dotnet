using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace LevelDBMCPE
{
    internal static class Library
    {
        public static string LibraryLocation { get; private set; }
        public static IntPtr LibraryHandle { get; private set; } = IntPtr.Zero;
        public static bool IsInitialized { get; private set; } = false;

        public static void Init()
        {
            if (!IsInitialized)
            {
                LibraryLocation = Path.Combine(Path.GetTempPath(), "LevelDB-MCPE.dll");
                if (!File.Exists(LibraryLocation))
                    using (FileStream fs = File.Create(LibraryLocation))
                    using (Stream src = Assembly.GetExecutingAssembly().GetManifestResourceStream("LevelDBMCPE.leveldb-mcpe.dll"))
                    {
                        src.CopyTo(fs);
                    }

                LibraryHandle = LoadLibrary(LibraryLocation);
                if (LibraryHandle == IntPtr.Zero)
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "LoadLibrary falied with path: " + LibraryLocation);

                InitMethods();

                AppDomain.CurrentDomain.ProcessExit += (object obj, EventArgs e) => Exit();

                IsInitialized = true;
            }
        }

        public static void Exit()
        {
            if (IsInitialized)
            {
                FreeLibrary(LibraryHandle);

                IsInitialized = false;
            }
        }

        private static void InitMethods()
        {
            _leveldb_open = (leveldb_open)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_open"), typeof(leveldb_open));
            _leveldb_close = (leveldb_close)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_close"), typeof(leveldb_close));
            _leveldb_put = (leveldb_put)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_put"), typeof(leveldb_put));
            _leveldb_delete = (leveldb_delete)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_delete"), typeof(leveldb_delete));
            _leveldb_write = (leveldb_write)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_write"), typeof(leveldb_write));
            _leveldb_get = (leveldb_get)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_get"), typeof(leveldb_get));
            _leveldb_create_iterator = (leveldb_create_iterator)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_create_iterator"), typeof(leveldb_create_iterator));
            _leveldb_create_snapshot = (leveldb_create_snapshot)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_create_snapshot"), typeof(leveldb_create_snapshot));
            _leveldb_release_snapshot = (leveldb_release_snapshot)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_release_snapshot"), typeof(leveldb_release_snapshot));
            _leveldb_property_value = (leveldb_property_value)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_property_value"), typeof(leveldb_property_value));
            _leveldb_approximate_sizes = (leveldb_approximate_sizes)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_approximate_sizes"), typeof(leveldb_approximate_sizes));
            _leveldb_compact_range = (leveldb_compact_range)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_compact_range"), typeof(leveldb_compact_range));
            _leveldb_destroy_db = (leveldb_destroy_db)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_destroy_db"), typeof(leveldb_destroy_db));
            _leveldb_repair_db = (leveldb_repair_db)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_repair_db"), typeof(leveldb_repair_db));
            _leveldb_iter_destroy = (leveldb_iter_destroy)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_iter_destroy"), typeof(leveldb_iter_destroy));
            _leveldb_iter_valid = (leveldb_iter_valid)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_iter_valid"), typeof(leveldb_iter_valid));
            _leveldb_iter_seek_to_first = (leveldb_iter_seek_to_first)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_iter_seek_to_first"), typeof(leveldb_iter_seek_to_first));
            _leveldb_iter_seek_to_last = (leveldb_iter_seek_to_last)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_iter_seek_to_last"), typeof(leveldb_iter_seek_to_last));
            _leveldb_iter_seek = (leveldb_iter_seek)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_iter_seek"), typeof(leveldb_iter_seek));
            _leveldb_iter_next = (leveldb_iter_next)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_iter_next"), typeof(leveldb_iter_next));
            _leveldb_iter_prev = (leveldb_iter_prev)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_iter_prev"), typeof(leveldb_iter_prev));
            _leveldb_iter_key = (leveldb_iter_key)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_iter_key"), typeof(leveldb_iter_key));
            _leveldb_iter_value = (leveldb_iter_value)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_iter_value"), typeof(leveldb_iter_value));
            _leveldb_iter_get_error = (leveldb_iter_get_error)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_iter_get_error"), typeof(leveldb_iter_get_error));
            _leveldb_writebatch_create = (leveldb_writebatch_create)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_writebatch_create"), typeof(leveldb_writebatch_create));
            _leveldb_writebatch_destroy = (leveldb_writebatch_destroy)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_writebatch_destroy"), typeof(leveldb_writebatch_destroy));
            _leveldb_writebatch_clear = (leveldb_writebatch_clear)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_writebatch_clear"), typeof(leveldb_writebatch_clear));
            _leveldb_writebatch_put = (leveldb_writebatch_put)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_writebatch_put"), typeof(leveldb_writebatch_put));
            _leveldb_writebatch_delete = (leveldb_writebatch_delete)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_writebatch_delete"), typeof(leveldb_writebatch_delete));
            _leveldb_writebatch_iterate = (leveldb_writebatch_iterate)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_writebatch_iterate"), typeof(leveldb_writebatch_iterate));
            _leveldb_options_create = (leveldb_options_create)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_options_create"), typeof(leveldb_options_create));
            _leveldb_options_destroy = (leveldb_options_destroy)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_options_destroy"), typeof(leveldb_options_destroy));
            _leveldb_options_set_comparator = (leveldb_options_set_comparator)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_options_set_comparator"), typeof(leveldb_options_set_comparator));
            _leveldb_options_set_filter_policy = (leveldb_options_set_filter_policy)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_options_set_filter_policy"), typeof(leveldb_options_set_filter_policy));
            _leveldb_options_set_create_if_missing = (leveldb_options_set_create_if_missing)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_options_set_create_if_missing"), typeof(leveldb_options_set_create_if_missing));
            _leveldb_options_set_error_if_exists = (leveldb_options_set_error_if_exists)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_options_set_error_if_exists"), typeof(leveldb_options_set_error_if_exists));
            _leveldb_options_set_paranoid_checks = (leveldb_options_set_paranoid_checks)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_options_set_paranoid_checks"), typeof(leveldb_options_set_paranoid_checks));
            _leveldb_options_set_env = (leveldb_options_set_env)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_options_set_env"), typeof(leveldb_options_set_env));
            _leveldb_options_set_info_log = (leveldb_options_set_info_log)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_options_set_info_log"), typeof(leveldb_options_set_info_log));
            _leveldb_options_set_write_buffer_size = (leveldb_options_set_write_buffer_size)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_options_set_write_buffer_size"), typeof(leveldb_options_set_write_buffer_size));
            _leveldb_options_set_max_open_files = (leveldb_options_set_max_open_files)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_options_set_max_open_files"), typeof(leveldb_options_set_max_open_files));
            _leveldb_options_set_cache = (leveldb_options_set_cache)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_options_set_cache"), typeof(leveldb_options_set_cache));
            _leveldb_options_set_block_size = (leveldb_options_set_block_size)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_options_set_block_size"), typeof(leveldb_options_set_block_size));
            _leveldb_options_set_block_restart_interval = (leveldb_options_set_block_restart_interval)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_options_set_block_restart_interval"), typeof(leveldb_options_set_block_restart_interval));
            _leveldb_options_set_compression = (leveldb_options_set_compression)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_options_set_compression"), typeof(leveldb_options_set_compression));
            _leveldb_options_set_compression_by_index = (leveldb_options_set_compression_by_index)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_options_set_compression_by_index"), typeof(leveldb_options_set_compression_by_index));
            _leveldb_comparator_create = (leveldb_comparator_create)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_comparator_create"), typeof(leveldb_comparator_create));
            _leveldb_comparator_destroy = (leveldb_comparator_destroy)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_comparator_destroy"), typeof(leveldb_comparator_destroy));
            _leveldb_filterpolicy_create = (leveldb_filterpolicy_create)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_filterpolicy_create"), typeof(leveldb_filterpolicy_create));
            _leveldb_filterpolicy_destroy = (leveldb_filterpolicy_destroy)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_filterpolicy_destroy"), typeof(leveldb_filterpolicy_destroy));
            _leveldb_filterpolicy_create_bloom = (leveldb_filterpolicy_create_bloom)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_filterpolicy_create_bloom"), typeof(leveldb_filterpolicy_create_bloom));
            _leveldb_readoptions_create = (leveldb_readoptions_create)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_readoptions_create"), typeof(leveldb_readoptions_create));
            _leveldb_readoptions_destroy = (leveldb_readoptions_destroy)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_readoptions_destroy"), typeof(leveldb_readoptions_destroy));
            _leveldb_readoptions_set_verify_checksums = (leveldb_readoptions_set_verify_checksums)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_readoptions_set_verify_checksums"), typeof(leveldb_readoptions_set_verify_checksums));
            _leveldb_readoptions_set_fill_cache = (leveldb_readoptions_set_fill_cache)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_readoptions_set_fill_cache"), typeof(leveldb_readoptions_set_fill_cache));
            _leveldb_readoptions_set_snapshot = (leveldb_readoptions_set_snapshot)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_readoptions_set_snapshot"), typeof(leveldb_readoptions_set_snapshot));
            _leveldb_writeoptions_create = (leveldb_writeoptions_create)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_writeoptions_create"), typeof(leveldb_writeoptions_create));
            _leveldb_writeoptions_destroy = (leveldb_writeoptions_destroy)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_writeoptions_destroy"), typeof(leveldb_writeoptions_destroy));
            _leveldb_writeoptions_set_sync = (leveldb_writeoptions_set_sync)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_writeoptions_set_sync"), typeof(leveldb_writeoptions_set_sync));
            _leveldb_cache_create_lru = (leveldb_cache_create_lru)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_cache_create_lru"), typeof(leveldb_cache_create_lru));
            _leveldb_cache_destroy = (leveldb_cache_destroy)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_cache_destroy"), typeof(leveldb_cache_destroy));
            _leveldb_create_default_env = (leveldb_create_default_env)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_create_default_env"), typeof(leveldb_create_default_env));
            _leveldb_env_destroy = (leveldb_env_destroy)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_env_destroy"), typeof(leveldb_env_destroy));
            _leveldb_logger_create = (leveldb_logger_create)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_logger_create"), typeof(leveldb_logger_create));
            _leveldb_logger_destroy = (leveldb_logger_destroy)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_logger_destroy"), typeof(leveldb_logger_destroy));
            _leveldb_free = (leveldb_free)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_free"), typeof(leveldb_free));
            _leveldb_major_version = (leveldb_major_version)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_major_version"), typeof(leveldb_major_version));
            _leveldb_minor_version = (leveldb_minor_version)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LibraryHandle, "leveldb_minor_version"), typeof(leveldb_minor_version));
        }

        #region Kernel32-Methods
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FreeLibrary(IntPtr hModule);
        #endregion

        #region LevelDB-Signatures
        /* DB operations */

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate IntPtr leveldb_open(IntPtr options, string name, char** errptr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void leveldb_close(IntPtr db);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_put(IntPtr db, IntPtr options, byte* key, IntPtr keylen, byte* val, IntPtr vallen, char** errptr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_delete(IntPtr db, IntPtr options, byte* key, IntPtr keylen, char** errptr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_write(IntPtr db, IntPtr options, IntPtr batch, char** errptr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate byte* leveldb_get(IntPtr db, IntPtr options, byte* key, IntPtr keylen, IntPtr* vallen, char** errptr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr leveldb_create_iterator(IntPtr db, IntPtr options);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr leveldb_create_snapshot(IntPtr db);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void leveldb_release_snapshot(IntPtr db, IntPtr snapshot);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate char* leveldb_property_value(IntPtr db, string propname);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_approximate_sizes(IntPtr db, int num_ranges, IntPtr* range_start_key, IntPtr* range_start_key_len, IntPtr* range_limit_key, IntPtr* range_limit_key_len, ulong* sizes);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_compact_range(IntPtr db, IntPtr start_key, IntPtr start_key_len, IntPtr limit_key, IntPtr limit_key_len);

        /* Management operations */

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_destroy_db(IntPtr options, string name, char** errptr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_repair_db(IntPtr options, string name, char** errptr);

        /* Iterator */

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_iter_destroy(IntPtr iter);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate byte leveldb_iter_valid(IntPtr iter);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_iter_seek_to_first(IntPtr iter);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_iter_seek_to_last(IntPtr iter);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_iter_seek(IntPtr iter, byte* k, IntPtr klen);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_iter_next(IntPtr iter);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_iter_prev(IntPtr iter);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate byte* leveldb_iter_key(IntPtr iter, IntPtr* klen);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate byte* leveldb_iter_value(IntPtr iter, IntPtr* vlen);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_iter_get_error(IntPtr iter, char** errptr);

        /* Write batch */

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate IntPtr leveldb_writebatch_create();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_writebatch_destroy(IntPtr wb);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_writebatch_clear(IntPtr wb);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_writebatch_put(IntPtr wb, byte* key, IntPtr klen, byte* val, IntPtr vlen);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_writebatch_delete(IntPtr wb, byte* key, IntPtr klen);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_writebatch_iterate(IntPtr wb, void* state, leveldb_writebatch_iterate_put put, leveldb_writebatch_iterate_deleted deleted);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_writebatch_iterate_put(IntPtr ptr, byte* k, IntPtr klen, byte* v, IntPtr vlen);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_writebatch_iterate_deleted(IntPtr ptr, byte* k, IntPtr klen);

        /* Options */

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate IntPtr leveldb_options_create();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_options_destroy(IntPtr options);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_options_set_comparator(IntPtr options, IntPtr comparator);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_options_set_filter_policy(IntPtr options, IntPtr filterpolicy);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_options_set_create_if_missing(IntPtr options, byte value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_options_set_error_if_exists(IntPtr options, byte value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_options_set_paranoid_checks(IntPtr options, byte value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_options_set_env(IntPtr options, IntPtr env);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_options_set_info_log(IntPtr options, IntPtr logger);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_options_set_write_buffer_size(IntPtr options, ulong size);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_options_set_max_open_files(IntPtr options, int max);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_options_set_cache(IntPtr options, IntPtr cache);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_options_set_block_size(IntPtr options, ulong size);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_options_set_block_restart_interval(IntPtr options, int value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_options_set_compression(IntPtr options, int compression);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_options_set_compression_by_index(IntPtr options, int compression, int index);

        /* Comparator */

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate IntPtr leveldb_comparator_create(void* state, leveldb_comparator_destructor destructor, leveldb_comparator_compare compare, leveldb_comparator_name name);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_comparator_destructor(IntPtr state);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate int leveldb_comparator_compare(IntPtr state, byte* a, IntPtr alen, byte* b, IntPtr blen);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate string leveldb_comparator_name(IntPtr state);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_comparator_destroy(IntPtr comparator);

        /* Filter policy */

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate IntPtr leveldb_filterpolicy_create(void* state, leveldb_filterpolicy_destructor destructor, leveldb_filterpolicy_create_filter create_filter, leveldb_filterpolicy_key_may_match key_may_match, leveldb_filterpolicy_name name);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_filterpolicy_destroy(IntPtr state);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_filterpolicy_destructor(IntPtr state);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate byte* leveldb_filterpolicy_create_filter(IntPtr state, byte** key_array, IntPtr* key_length_array, int num_keys, IntPtr* filter_length);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate byte leveldb_filterpolicy_key_may_match(IntPtr state, byte* key, IntPtr key_length, byte* filter, IntPtr filter_length);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate string leveldb_filterpolicy_name(IntPtr state);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate IntPtr leveldb_filterpolicy_create_bloom(int bits_per_key);

        /* Read options */

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate IntPtr leveldb_readoptions_create();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_readoptions_destroy(IntPtr readoptions);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_readoptions_set_verify_checksums(IntPtr readoptions, byte value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_readoptions_set_fill_cache(IntPtr readoptions, byte value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_readoptions_set_snapshot(IntPtr readoptions, IntPtr snapshot);

        /* Write options */

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate IntPtr leveldb_writeoptions_create();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_writeoptions_destroy(IntPtr writeoptions);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_writeoptions_set_sync(IntPtr writeoptions, byte value);

        /* Cache */

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate IntPtr leveldb_cache_create_lru(ulong capacity);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_cache_destroy(IntPtr cache);

        /* Env */

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate IntPtr leveldb_create_default_env();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_env_destroy(IntPtr env);

        /* Logger */

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate IntPtr leveldb_logger_create(
            void* state,
            leveldb_logger_destructor destructor,
            leveldb_logger_logv logv);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_logger_destroy(IntPtr logger);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_logger_destructor(IntPtr state);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_logger_logv(IntPtr state, string format, IntPtr ap);

        /* Utility */

        /* Calls free(ptr).
           REQUIRES: ptr was malloc()-ed and returned by one of the routines
           in this file.  Note that in certain cases (typically on Windows), you
           may need to call this routine instead of free(ptr) to dispose of
           malloc()-ed memory returned by this library. */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate void leveldb_free(void* ptr);

        /* Return the major version number for this release. */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate int leveldb_major_version();

        /* Return the minor version number for this release. */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate int leveldb_minor_version();
        #endregion

        #region LevelDB-Methods
        private static leveldb_open _leveldb_open;
        private static leveldb_close _leveldb_close;
        private static leveldb_put _leveldb_put;
        private static leveldb_delete _leveldb_delete;
        private static leveldb_write _leveldb_write;
        private static leveldb_get _leveldb_get;
        private static leveldb_create_iterator _leveldb_create_iterator;
        private static leveldb_create_snapshot _leveldb_create_snapshot;
        private static leveldb_release_snapshot _leveldb_release_snapshot;
        private static leveldb_property_value _leveldb_property_value;
        private static leveldb_approximate_sizes _leveldb_approximate_sizes;
        private static leveldb_compact_range _leveldb_compact_range;
        private static leveldb_destroy_db _leveldb_destroy_db;
        private static leveldb_repair_db _leveldb_repair_db;
        private static leveldb_iter_destroy _leveldb_iter_destroy;
        private static leveldb_iter_valid _leveldb_iter_valid;
        private static leveldb_iter_seek_to_first _leveldb_iter_seek_to_first;
        private static leveldb_iter_seek_to_last _leveldb_iter_seek_to_last;
        private static leveldb_iter_seek _leveldb_iter_seek;
        private static leveldb_iter_next _leveldb_iter_next;
        private static leveldb_iter_prev _leveldb_iter_prev;
        private static leveldb_iter_key _leveldb_iter_key;
        private static leveldb_iter_value _leveldb_iter_value;
        private static leveldb_iter_get_error _leveldb_iter_get_error;
        private static leveldb_writebatch_create _leveldb_writebatch_create;
        private static leveldb_writebatch_destroy _leveldb_writebatch_destroy;
        private static leveldb_writebatch_clear _leveldb_writebatch_clear;
        private static leveldb_writebatch_put _leveldb_writebatch_put;
        private static leveldb_writebatch_delete _leveldb_writebatch_delete;
        private static leveldb_writebatch_iterate _leveldb_writebatch_iterate;
        private static leveldb_options_create _leveldb_options_create;
        private static leveldb_options_destroy _leveldb_options_destroy;
        private static leveldb_options_set_comparator _leveldb_options_set_comparator;
        private static leveldb_options_set_filter_policy _leveldb_options_set_filter_policy;
        private static leveldb_options_set_create_if_missing _leveldb_options_set_create_if_missing;
        private static leveldb_options_set_error_if_exists _leveldb_options_set_error_if_exists;
        private static leveldb_options_set_paranoid_checks _leveldb_options_set_paranoid_checks;
        private static leveldb_options_set_env _leveldb_options_set_env;
        private static leveldb_options_set_info_log _leveldb_options_set_info_log;
        private static leveldb_options_set_write_buffer_size _leveldb_options_set_write_buffer_size;
        private static leveldb_options_set_max_open_files _leveldb_options_set_max_open_files;
        private static leveldb_options_set_cache _leveldb_options_set_cache;
        private static leveldb_options_set_block_size _leveldb_options_set_block_size;
        private static leveldb_options_set_block_restart_interval _leveldb_options_set_block_restart_interval;
        private static leveldb_options_set_compression _leveldb_options_set_compression;
        private static leveldb_options_set_compression_by_index _leveldb_options_set_compression_by_index;
        private static leveldb_comparator_create _leveldb_comparator_create;
        private static leveldb_comparator_destroy _leveldb_comparator_destroy;
        private static leveldb_filterpolicy_create _leveldb_filterpolicy_create;
        private static leveldb_filterpolicy_destroy _leveldb_filterpolicy_destroy;
        private static leveldb_filterpolicy_create_bloom _leveldb_filterpolicy_create_bloom;
        private static leveldb_readoptions_create _leveldb_readoptions_create;
        private static leveldb_readoptions_destroy _leveldb_readoptions_destroy;
        private static leveldb_readoptions_set_verify_checksums _leveldb_readoptions_set_verify_checksums;
        private static leveldb_readoptions_set_fill_cache _leveldb_readoptions_set_fill_cache;
        private static leveldb_readoptions_set_snapshot _leveldb_readoptions_set_snapshot;
        private static leveldb_writeoptions_create _leveldb_writeoptions_create;
        private static leveldb_writeoptions_destroy _leveldb_writeoptions_destroy;
        private static leveldb_writeoptions_set_sync _leveldb_writeoptions_set_sync;
        private static leveldb_cache_create_lru _leveldb_cache_create_lru;
        private static leveldb_cache_destroy _leveldb_cache_destroy;
        private static leveldb_create_default_env _leveldb_create_default_env;
        private static leveldb_env_destroy _leveldb_env_destroy;
        private static leveldb_logger_create _leveldb_logger_create;
        private static leveldb_logger_destroy _leveldb_logger_destroy;
        private static leveldb_free _leveldb_free;
        private static leveldb_major_version _leveldb_major_version;
        private static leveldb_minor_version _leveldb_minor_version;
        #endregion

        #region Public Library Methods
        public static IntPtr LevelDBOpen(IntPtr options, string name)
        {
            Init();
            unsafe
            {
                char* char_ptr = null;
                IntPtr ret = _leveldb_open(options, name, &char_ptr);
                if (char_ptr != null)
                    throw new LevelDBException(char_ptr);
                _leveldb_free(char_ptr);
                return ret;
            }
        }

        public static void LevelDBClose(IntPtr db)
        {
            Init();
            _leveldb_close(db);
        }

        public static void LevelDBPut(IntPtr db, IntPtr writeOptions, byte[] key, byte[] value)
        {
            Init();
            unsafe
            {
                char* char_ptr = null;
                fixed (byte* kPtr = key)
                fixed (byte* vPtr = value)
                {
                    _leveldb_put(db, writeOptions, kPtr, key.GetIntPtrLength(), vPtr, value.GetIntPtrLength(), &char_ptr);
                }
                if (char_ptr != null)
                    throw new LevelDBException(char_ptr);
                _leveldb_free(char_ptr);
            }
        }

        public static void LevelDBDelete(IntPtr db, IntPtr writeOptions, byte[] key)
        {
            Init();
            unsafe
            {
                char* char_ptr = null;
                fixed (byte* kPtr = key)
                {
                    _leveldb_delete(db, writeOptions, kPtr, key.GetIntPtrLength(), &char_ptr);
                }
                if (char_ptr != null)
                    throw new LevelDBException(char_ptr);
                _leveldb_free(char_ptr);
            }
        }

        public static void LevelDBWrite(IntPtr db, IntPtr writeOptions, IntPtr batch)
        {
            Init();
            unsafe
            {
                char* char_ptr = null;
                _leveldb_write(db, writeOptions, batch, &char_ptr);
                if (char_ptr != null)
                    throw new LevelDBException(char_ptr);
                _leveldb_free(char_ptr);
            }
        }

        public static byte[] LevelDBGet(IntPtr db, IntPtr options, byte[] key)
        {
            Init();
            unsafe
            {
                char* char_ptr = null;
                IntPtr valLength = IntPtr.Zero;
                byte* valPtr = null;
                fixed (byte* kPtr = key)
                {
                    valPtr = _leveldb_get(db, options, kPtr, key.GetIntPtrLength(), &valLength, &char_ptr);
                }
                if (char_ptr != null)
                    throw new LevelDBException(char_ptr);
                _leveldb_free(char_ptr);

                return Utils.PointerToArray(valPtr, valLength, true);
            }
        }

        public static IntPtr LevelDBCreateIterator(IntPtr db, IntPtr options)
        {
            Init();
            return _leveldb_create_iterator(db, options);
        }

        public static IntPtr LevelDBCreateSnapshot(IntPtr db)
        {
            Init();
            return _leveldb_create_snapshot(db);
        }

        public static void LevelDBReleaseSnapshot(IntPtr db, IntPtr snapshot)
        {
            Init();
            _leveldb_release_snapshot(db, snapshot);
        }

        public static string LevelDBPropertyValue(IntPtr db, string propname)
        {
            Init();
            unsafe
            {
                char* valuePtr = _leveldb_property_value(db, propname);
                if (valuePtr == null)
                    return null;
                StringBuilder value = new StringBuilder();
                if (valuePtr != null)
                    value.Append(new string(valuePtr));
                _leveldb_free(valuePtr);
                return value.ToString();
            }
        }

        public static ulong[] LevelDBApproximateSizes(IntPtr db, byte[][] start_keys, byte[][] limit_keys)
        {
            Init();
            if (start_keys.Length != limit_keys.Length)
                return null;

            IntPtr[] range_start_key = new IntPtr[start_keys.Length];
            GCHandle[] start_key_handles = new GCHandle[start_keys.Length];
            IntPtr[] range_start_key_len = new IntPtr[start_keys.Length];
            IntPtr[] range_limit_key = new IntPtr[start_keys.Length];
            GCHandle[] limit_key_handles = new GCHandle[start_keys.Length];
            IntPtr[] range_limit_key_len = new IntPtr[start_keys.Length];

            unsafe
            {
                for (int i = 0; i < start_keys.Length; i++)
                {
                    start_key_handles[i] = GCHandle.Alloc(start_keys[i], GCHandleType.Pinned);
                    fixed (byte* p = start_keys[i])
                        range_start_key[i] = (IntPtr)p;
                    range_start_key_len[i] = new IntPtr(start_keys[i]?.Length ?? 0);
                }

                for (int i = 0; i < start_keys.Length; i++)
                {
                    limit_key_handles[i] = GCHandle.Alloc(limit_keys[i], GCHandleType.Pinned);
                    fixed (byte* p = limit_keys[i])
                        range_limit_key[i] = (IntPtr)p;
                    range_limit_key_len[i] = new IntPtr(limit_keys[i]?.Length ?? 0);
                }

                ulong[] sizes = new ulong[range_start_key.Length];

                fixed (IntPtr* prsk = &range_start_key[0])
                fixed (IntPtr* prskl = &range_start_key_len[0])
                fixed (IntPtr* prlk = &range_limit_key[0])
                fixed (IntPtr* prlkl = &range_limit_key_len[0])
                fixed (ulong* psz = &sizes[0])
                    _leveldb_approximate_sizes(db, range_start_key.Length, prsk, prskl, prlk, prlkl, psz);

                for (int i = 0; i < start_keys.Length; i++)
                {
                    start_key_handles[i].Free();
                }

                for (int i = 0; i < limit_keys.Length; i++)
                {
                    limit_key_handles[i].Free();
                }

                return sizes;
            }
        }

        public static ulong[] LevelDBApproximateSizes(IntPtr db, string[] start_keys, string[] limit_keys)
        {
            Init();
            if (start_keys.Length != limit_keys.Length)
                return null;
            IntPtr[] range_start_key = new IntPtr[start_keys.Length];
            IntPtr[] range_start_key_len = new IntPtr[start_keys.Length];
            IntPtr[] range_limit_key = new IntPtr[start_keys.Length];
            IntPtr[] range_limit_key_len = new IntPtr[start_keys.Length];

            for (int i = 0; i < start_keys.Length; i++)
            {
                range_start_key[i] = Marshal.StringToHGlobalAnsi(start_keys[i]);
                range_start_key_len[i] = new IntPtr(start_keys[i]?.Length ?? 0);
            }

            for (int i = 0; i < limit_keys.Length; i++)
            {
                range_limit_key[i] = Marshal.StringToHGlobalAnsi(limit_keys[i]);
                range_limit_key_len[i] = new IntPtr(limit_keys[i]?.Length ?? 0);
            }

            ulong[] sizes = new ulong[range_start_key.Length];

            unsafe
            {
                fixed (IntPtr* prsk = &range_start_key[0])
                fixed (IntPtr* prskl = &range_start_key_len[0])
                fixed (IntPtr* prlk = &range_limit_key[0])
                fixed (IntPtr* prlkl = &range_limit_key_len[0])
                fixed (ulong* psz = &sizes[0])
                    _leveldb_approximate_sizes(db, range_start_key.Length, prsk, prskl, prlk, prlkl, psz);
            }

            for (int i = 0; i < start_keys.Length; i++)
            {
                Marshal.FreeHGlobal(range_start_key[i]);
            }

            for (int i = 0; i < limit_keys.Length; i++)
            {
                Marshal.FreeHGlobal(range_limit_key[i]);
            }

            return sizes;
        }

        public static void LevelDBCompactRange(IntPtr db, string startKey, string limitKey)
        {
            Init();

            IntPtr start_key = Marshal.StringToHGlobalAnsi(startKey);
            IntPtr start_key_len = new IntPtr(startKey?.Length ?? 0);
            IntPtr limit_key = Marshal.StringToHGlobalAnsi(limitKey);
            IntPtr limit_key_len = new IntPtr(limitKey?.Length ?? 0);

            _leveldb_compact_range(db, start_key, start_key_len, limit_key, limit_key_len);

            Marshal.FreeHGlobal(start_key);
            Marshal.FreeHGlobal(limit_key);
        }

        public static void LevelDBCompactRange(IntPtr db, byte[] startKey, byte[] limitKey)
        {
            Init();

            unsafe
            {
                fixed(byte* start_key = startKey)
                fixed (byte* limit_key = limitKey)
                    _leveldb_compact_range(db, (IntPtr)start_key, startKey.GetIntPtrLength(), (IntPtr)limit_key, limitKey.GetIntPtrLength());
            }
        }

        public static void LevelDBDestroyDB(IntPtr options, string name)
        {
            Init();
            unsafe
            {
                char* char_ptr = null;
                _leveldb_destroy_db(options, name, &char_ptr);
                if (char_ptr != null)
                    throw new LevelDBException(char_ptr);
                _leveldb_free(char_ptr);
            }
        }

        public static void LevelDBRepairDB(IntPtr options, string name)
        {
            Init();
            unsafe
            {
                char* char_ptr = null;
                _leveldb_repair_db(options, name, &char_ptr);
                if (char_ptr != null)
                    throw new LevelDBException(char_ptr);
                _leveldb_free(char_ptr);
            }
        }

        public static void LevelDBIterDestroy(IntPtr iter)
        {
            Init();
            _leveldb_iter_destroy(iter);
        }

        public static bool LevelDBIterValid(IntPtr iter)
        {
            Init();
            return _leveldb_iter_valid(iter) != 0;
        }

        public static void LevelDBIterSeekToFirst(IntPtr iter)
        {
            Init();
            _leveldb_iter_seek_to_first(iter);
        }

        public static void LevelDBIterSeekToLast(IntPtr iter)
        {
            Init();
            _leveldb_iter_seek_to_last(iter);
        }

        public static void LevelDBIterSeek(IntPtr iter, byte[] key)
        {
            Init();
            unsafe
            {
                fixed (byte* kPtr = key)
                    _leveldb_iter_seek(iter, kPtr, key.GetIntPtrLength());
            }
        }

        public static void LevelDBIterNext(IntPtr iter)
        {
            Init();
            _leveldb_iter_next(iter);
        }

        public static void LevelDBIterPrev(IntPtr iter)
        {
            Init();
            _leveldb_iter_prev(iter);
        }

        public static byte[] LevelDBIterKey(IntPtr iter)
        {
            Init();
            unsafe
            {
                IntPtr keyLength = IntPtr.Zero;
                byte* keyPtr = _leveldb_iter_key(iter, &keyLength);

                return Utils.PointerToArray(keyPtr, keyLength, false);
            }
        }

        public static byte[] LevelDBIterValue(IntPtr iter)
        {
            Init();
            unsafe
            {
                IntPtr valLength = IntPtr.Zero;
                byte* valPtr = _leveldb_iter_value(iter, &valLength);

                return Utils.PointerToArray(valPtr, valLength, false);
            }
        }

        public static string LevelDBIterGetError(IntPtr iter)
        {
            Init();
            unsafe
            {
                char* char_ptr = null;
                _leveldb_iter_get_error(iter, &char_ptr);
                StringBuilder value = new StringBuilder();
                if (char_ptr != null)
                    value.Append(Marshal.PtrToStringAnsi((IntPtr)char_ptr));
                else
                    value.Append("Success");
                _leveldb_free(char_ptr);
                return value.ToString();
            }
        }

        public static IntPtr LevelDBWriteBatchCreate()
        {
            Init();
            return _leveldb_writebatch_create();
        }

        public static void LevelDBWriteBatchDestroy(IntPtr wb)
        {
            Init();
            _leveldb_writebatch_destroy(wb);
        }

        public static void LevelDBWriteBatchClear(IntPtr wb)
        {
            Init();
            _leveldb_writebatch_clear(wb);
        }

        public static void LevelDBWriteBatchPut(IntPtr wb, byte[] key, byte[] value)
        {
            Init();
            unsafe
            {
                fixed (byte* pKey = key)
                fixed (byte* pValue = value)
                    _leveldb_writebatch_put(wb, pKey, key.GetIntPtrLength(), pValue, value.GetIntPtrLength());
            }
        }

        public static void LevelDBWriteBatchDelete(IntPtr wb, byte[] key)
        {
            Init();
            unsafe
            {
                fixed (byte* pKey = key)
                    _leveldb_writebatch_delete(wb, pKey, key.GetIntPtrLength());
            }
        }

        public delegate void LevelDBWriteBatchIteratePut(IntPtr state, byte[] key, byte[] value);
        public delegate void LevelDBWriteBatchIterateDeleted(IntPtr state, byte[] key);
        public static void LevelDBWriteBatchIterate(IntPtr wb, IntPtr state, LevelDBWriteBatchIteratePut put, LevelDBWriteBatchIterateDeleted deleted)
        {
            Init();
            unsafe
            {
                leveldb_writebatch_iterate_put unsafePut = (IntPtr ptr, byte* k, IntPtr klen, byte* v, IntPtr vlen) =>
                {
                    byte[] key = Utils.PointerToArray(k, klen, false);
                    byte[] value = Utils.PointerToArray(v, vlen, false);
                    put(ptr, key, value);
                };
                leveldb_writebatch_iterate_deleted unsafeDeleted = (IntPtr ptr, byte* k, IntPtr klen) =>
                {
                    byte[] key = Utils.PointerToArray(k, klen, false);
                    deleted(ptr, key);
                };
                _leveldb_writebatch_iterate(wb, (void*)state, unsafePut, unsafeDeleted);
            }
        }

        public static IntPtr LevelDBOptionsCreate()
        {
            Init();
            return _leveldb_options_create();
        }

        public static void LevelDBOptionsDestroy(IntPtr options)
        {
            Init();
            _leveldb_options_destroy(options);
        }

        public static void LevelDBOptionsSetComparator(IntPtr options, IntPtr comparator)
        {
            Init();
            _leveldb_options_set_comparator(options, comparator);
        }

        public static void LevelDBOptionsSetFilterPolicy(IntPtr options, IntPtr filterpolicy)
        {
            Init();
            _leveldb_options_set_filter_policy(options, filterpolicy);
        }

        public static void LevelDBOptionsSetCreateIfMissing(IntPtr options, bool value)
        {
            Init();
            _leveldb_options_set_create_if_missing(options, (byte)(value ? 1 : 0));
        }

        public static void LevelDBOptionsSetErrorIfExists(IntPtr options, bool value)
        {
            Init();
            _leveldb_options_set_error_if_exists(options, (byte)(value ? 1 : 0));
        }

        public static void LevelDBOptionsSetParanoidChecks(IntPtr options, bool value)
        {
            Init();
            _leveldb_options_set_paranoid_checks(options, (byte)(value ? 1 : 0));
        }

        public static void LevelDBOptionsSetEnv(IntPtr options, IntPtr env)
        {
            Init();
            _leveldb_options_set_env(options, env);
        }

        public static void LevelDBOptionsSetInfoLog(IntPtr options, IntPtr logger)
        {
            Init();
            _leveldb_options_set_info_log(options, logger);
        }

        public static void LevelDBOptionsSetWriteBufferSize(IntPtr options, ulong size)
        {
            Init();
            _leveldb_options_set_write_buffer_size(options, size);
        }

        public static void LevelDBOptionsSetMaxOpenFiles(IntPtr options, int max)
        {
            Init();
            _leveldb_options_set_max_open_files(options, max);
        }

        public static void LevelDBOptionsSetCache(IntPtr options, IntPtr cache)
        {
            Init();
            _leveldb_options_set_cache(options, cache);
        }

        public static void LevelDBOptionsSetBlockSize(IntPtr options, ulong size)
        {
            Init();
            _leveldb_options_set_block_size(options, size);
        }

        public static void LevelDBOptionsSetBlockRestartInterval(IntPtr options, int value)
        {
            Init();
            _leveldb_options_set_block_restart_interval(options, value);
        }

        public enum LevelDBCompression : int
        {
            NoCompression = 0,
            ZlibCompression = 2,
            RawZlibCompression = 4
        }

        public static void LevelDBOptionsSetCompression(IntPtr options, LevelDBCompression value)
        {
            Init();
            _leveldb_options_set_compression(options, (int)value);
        }

        public static void LevelDBOptionsSetCompressionByIndex(IntPtr options, LevelDBCompression value, int index)
        {
            Init();
            _leveldb_options_set_compression_by_index(options, (int)value, index);
        }

        public delegate string LevelDBComparatorName(IntPtr state);
        public delegate int LevelDBComparatorCompare(IntPtr state, byte[] a, byte[] b);
        public delegate void LevelDBComparatorDestructor(IntPtr state);
        public static IntPtr LevelDBComparatorCreate(IntPtr state, LevelDBComparatorDestructor destructor, LevelDBComparatorCompare compare, LevelDBComparatorName name)
        {
            Init();
            unsafe
            {
                leveldb_comparator_name unsafeName = (IntPtr ptr) =>
                {
                    return name(ptr);
                };
                leveldb_comparator_compare unsafeCompare = (IntPtr ptr, byte* a, IntPtr alen, byte* b, IntPtr blen) =>
                {
                    byte[] aArr = Utils.PointerToArray(a, alen, false);
                    byte[] bArr = Utils.PointerToArray(b, blen, false);
                    return compare(ptr, aArr, bArr);
                };
                leveldb_comparator_destructor unsafeDestructor = (IntPtr ptr) =>
                {
                    destructor(ptr);
                };
                return _leveldb_comparator_create((void*)state, unsafeDestructor, unsafeCompare, unsafeName);
            }
        }

        public static void LevelDBComparatorDestroy(IntPtr comparator)
        {
            Init();
            _leveldb_comparator_destroy(comparator);
        }

        public delegate string LevelDBFilterPolicyName(IntPtr state);
        public delegate byte[] LevelDBFilterPolicyCreateFilter(IntPtr state, byte[][] keys);
        public delegate bool LevelDBFilterPolicyKeyMayMatch(IntPtr state, byte[] key, byte[] filter);
        public delegate void LevelDBFilterPolicyDestructor(IntPtr state);
        public static IntPtr LevelDBFilterPolicyCreate(IntPtr state, LevelDBFilterPolicyDestructor destructor, LevelDBFilterPolicyCreateFilter createFilter, LevelDBFilterPolicyKeyMayMatch keyMayMatch, LevelDBFilterPolicyName name)
        {
            Init();
            unsafe
            {
                leveldb_filterpolicy_name unsafeName = (IntPtr ptr) =>
                {
                    return name(ptr);
                };
                leveldb_filterpolicy_create_filter unsafeCreateFilter = (IntPtr ptr, byte** key_array, IntPtr* key_length_array, int num_keys, IntPtr* filter_length) =>
                {
                    byte[][] keys = new byte[num_keys][];
                    for (int i = 0; i < num_keys; i++)
                    {
                        keys[i] = Utils.PointerToArray(key_array[i], key_length_array[i], false);
                    }
                    byte[] filter = createFilter(ptr, keys);
                    IntPtr gFilter = Marshal.AllocHGlobal(filter.Length);
                    Marshal.Copy(filter, 0, gFilter, filter.Length);
                    return (byte*)gFilter;
                };
                leveldb_filterpolicy_key_may_match unsafeKeyMayMatch = (IntPtr ptr, byte* key, IntPtr key_length, byte* filter, IntPtr filter_length) =>
                {
                    byte[] keyArr = Utils.PointerToArray(key, key_length, false);
                    byte[] filterArr = Utils.PointerToArray(filter, filter_length, false);
                    return (byte)(keyMayMatch(ptr, keyArr, filterArr) ? 1 : 0);
                };
                leveldb_filterpolicy_destructor unsafeDestructor = (IntPtr ptr) =>
                {
                    destructor(ptr);
                };
                return _leveldb_filterpolicy_create((void*)state, unsafeDestructor, unsafeCreateFilter, unsafeKeyMayMatch, unsafeName);
            }
        }

        public static void LevelDBFilterPolicyDestroy(IntPtr filterpolicy)
        {
            Init();
            _leveldb_filterpolicy_destroy(filterpolicy);
        }

        public static IntPtr LevelDBFilterPolicyCreateBloom(int bitsPerKey)
        {
            Init();
            return _leveldb_filterpolicy_create_bloom(bitsPerKey);
        }

        public static IntPtr LevelDBReadOptionsCreate()
        {
            Init();
            return _leveldb_readoptions_create();
        }

        public static void LevelDBReadOptionsDestroy(IntPtr readoptions)
        {
            Init();
            _leveldb_readoptions_destroy(readoptions);
        }

        public static void LevelDBReadOptionsSetVerifyChecksums(IntPtr readoptions, bool value)
        {
            Init();
            _leveldb_readoptions_set_verify_checksums(readoptions, (byte)(value ? 1 : 0));
        }

        public static void LevelDBReadOptionsSetFillCache(IntPtr readoptions, bool value)
        {
            Init();
            _leveldb_readoptions_set_fill_cache(readoptions, (byte)(value ? 1 : 0));
        }

        public static void LevelDBReadOptionsSetSnapshot(IntPtr readoptions, IntPtr snapshot)
        {
            Init();
            _leveldb_readoptions_set_snapshot(readoptions, snapshot);
        }

        public static IntPtr LevelDBWriteOptionsCreate()
        {
            Init();
            return _leveldb_writeoptions_create();
        }

        public static void LevelDBWriteOptionsDestroy(IntPtr writeoptions)
        {
            Init();
            _leveldb_writeoptions_destroy(writeoptions);
        }

        public static void LevelDBWriteOptionsSetSync(IntPtr writeoptions, bool value)
        {
            Init();
            _leveldb_writeoptions_set_sync(writeoptions, (byte)(value ? 1 : 0));
        }

        public static IntPtr LevelDBCacheCreateLRU(ulong capacity)
        {
            Init();
            return _leveldb_cache_create_lru(capacity);
        }

        public static void LevelDBCacheDestroy(IntPtr cache)
        {
            Init();
            _leveldb_cache_destroy(cache);
        }

        public static IntPtr LevelDBEnvCreateDefault()
        {
            Init();
            return _leveldb_create_default_env();
        }

        public static void LevelDBEnvDestroy(IntPtr env)
        {
            Init();
            _leveldb_env_destroy(env);
        }

        public delegate void LevelDBLoggerDestructor(IntPtr state);
        public delegate void LevelDBLoggerLogv(IntPtr state, string format, IntPtr ap);
        public static IntPtr LevelDBLoggerCreate(IntPtr state, LevelDBLoggerDestructor destructor, LevelDBLoggerLogv logv)
        {
            Init();
            unsafe
            {
                leveldb_logger_logv unsafeLogv = (IntPtr ptr, string format, IntPtr ap) =>
                {
                    logv(ptr, format, ap);
                };
                leveldb_logger_destructor unsafeDestructor = (IntPtr ptr) =>
                {
                    destructor(ptr);
                };
                return _leveldb_logger_create((void*)state, unsafeDestructor, unsafeLogv);
            }
        }

        public static void LevelDBLoggerDestroy(IntPtr logger)
        {
            Init();
            _leveldb_logger_destroy(logger);
        }

        public static void LevelDBFree(IntPtr ptr)
        {
            Init();
            unsafe
            {
                _leveldb_free((void*)ptr);
            }
        }

        public static Version LevelDBGetVersion()
        {
            Init();
            return new Version(_leveldb_major_version(), _leveldb_minor_version());
        }
        #endregion
    }
}
