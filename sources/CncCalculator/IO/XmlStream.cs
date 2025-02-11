using System.IO;
using System.Xml.Serialization;

using As.Applications.Validation;

namespace As.Applications.IO
{
    public static class XmlStream
    {
        public const string EXTENSION = ".xml";

        /// <summary>
        /// Read data from file, optional: create if missing,
        /// if T : <see cref="IXmlProcessing">IXmlProcessing</see> then call IXmlProcessing.XmlReadProcess after reading
        /// if T : <see cref="IChanged">IChanged</see> then call IChanged.ResetChanged.after reading (and optional XmlReadProcess).
        /// </summary>
        /// <param name="path">Path to file with content</param>
        /// <param name="result">Reference to content location</param>
        /// <param name="create_if_missing">True: create file if it is missing; False: skip reading if it is missing and return no result</param>
        /// <param name="noexcept">True: try to capture and logg all exceptions; False: let exceptions pass.</param>
        /// <returns>True if a content is available (read or created), false if no content is available</returns>
        public static bool Read<T>(
            string path,
            out T result,
            bool create_if_missing,
            bool noexcept = false)
            where T : class, new()
        {
            try
            {
                T? v = null;
                if (File.Exists(path))
                {
                    XmlSerializer serializer = new(typeof(T));
                    using var stream = new StreamReader(path);
                    v = serializer.Deserialize(stream) as T;
                    if (v != null) TryIXmlProcessReadProcess(ref v);
                }
                else if (create_if_missing)
                {
                    result = new();
                    return Write(
                        path,
                        result,
                        create_backup: false,
                        noexcept: false);
                }
                result = v!;
                if (v == null) return false;
                TryIChangedReset(ref v);
                return true;
            }
            catch
            {
                if (noexcept)
                {
                    result = default!;
                    return false;
                }
                throw;
            }
        }

        /// <summary>
        /// Write machine settings to file,
        /// it T : <see cref="IXmlProcessing">IXmlProcessing</see> then call IXmlProcessing.XmlWritePrepare before actual writing
        /// if T : <see cref="IChanged">IChanged</see> then call IChanged.ResetChanged after writing.
        /// </summary>
        /// <param name="path">Path to file with setting content</param>
        /// <param name="value">Reference to content</param>
        /// <param name="noexcept">True: try to capture and logg all exceptions; False: let exceptions pass.</param>
        /// <param name="create_backup">True (default); try create backup file; False: no backup file.</param>
        /// <returns>True if content was writen; False otherwise</returns>
        public static bool Write<T>(
            string path,
            T? value,
            bool create_backup,
            bool noexcept = false)
            where T : class
        {
            try
            {
                ArgumentNullException.ThrowIfNull(value);

                var p = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(p) && !Directory.Exists(p))
                {
                    Directory.CreateDirectory(p);
                }
                if (create_backup) TryCreateBackup(path);

                TryIXmlProcessWritePrepare(ref value);
                XmlSerializer serializer = new(typeof(T));
                using var stream = new StreamWriter(path);
                serializer.Serialize(stream, value);
                TryIXmlProcessWriteProcess(ref value);

                TryIChangedReset(ref value);
                return true;
            }
            catch
            {
                if (noexcept) return false;
                throw;
            }
        }

        static void TryCreateBackup(string path)
        {
            if (File.Exists(path))
            {
                // prepare move to *.*~
                var can_move = true;
                var c = path + "~";
                if (File.Exists(c))
                {
                    // try delete old *.*~.
                    try { File.Delete(c); }
                    catch { can_move = false; }
                }
                if (can_move)
                {
                    // move *.* to *.*~
                    try { File.Move(path, c); }
                    catch { /* never mind */ }
                }
            }
        }

        static void TryIChangedReset<T>(ref T value) where T : class
        {
            if (typeof(T).GetInterface(nameof(IChanged)) == null) return;

            var f = typeof(T).GetMethod(nameof(IChanged.ResetChanged))
                ?? typeof(IChanged).GetMethod(nameof(IChanged.ResetChanged));
            f?.Invoke(value, null);
        }

        static void TryIXmlProcessWritePrepare<T>(ref T value) where T : class
        {
            if (typeof(T).GetInterface(nameof(IXmlProcessing)) == null) return;

            var f = typeof(T).GetMethod(nameof(IXmlProcessing.XmlWritePrepare))
                ?? typeof(IXmlProcessing).GetMethod(nameof(IXmlProcessing.XmlWritePrepare));
            f?.Invoke(value, null);
        }

        static void TryIXmlProcessWriteProcess<T>(ref T value) where T : class
        {
            if (typeof(T).GetInterface(nameof(IXmlProcessing)) == null) return;

            var f = typeof(T).GetMethod(nameof(IXmlProcessing.XmlWriteProcess))
                ?? typeof(IXmlProcessing).GetMethod(nameof(IXmlProcessing.XmlWriteProcess));
            f?.Invoke(value, null);
        }

        static void TryIXmlProcessReadProcess<T>(ref T value) where T : class
        {
            if (typeof(T).GetInterface(nameof(IXmlProcessing)) == null) return;

            var f = typeof(T).GetMethod(nameof(IXmlProcessing.XmlReadProcess))
                ?? typeof(IXmlProcessing).GetMethod(nameof(IXmlProcessing.XmlReadProcess)); 
            f?.Invoke(value, null);
        }
    }
}
