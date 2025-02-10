using System.IO;

using Newtonsoft.Json;

namespace As.Applications.Data.Json
{
    public abstract class JsonBase<T> where T : class
    {
        public static T? GetData(
            string path,
            JsonSerializerSettings? jsonSettings = null)
        {
            using var sr = new StreamReader(path);
            return GetData(sr, jsonSettings);
        }

        public static T? GetData(
            StreamReader sr,
            JsonSerializerSettings? jsonSettings = null)
        {
            return Deserialise(sr.ReadToEnd(), jsonSettings);
        }

        public static T? Deserialise(
            string value,
            JsonSerializerSettings? jsonSettings = null)
        {
            return Jtransform<T>.DeserializeObject(value, jsonSettings);
        }

        public void PutData(
            string path,
            bool isEmptyToNull = false,
            JsonSerializerSettings? jsonSettings = null)
        {
            MoveBackup(path);
            using var sw = new StreamWriter(path);
            PutData(sw, isEmptyToNull, jsonSettings);
        }

        public void PutData(
            StreamWriter sw,
            bool isEmptyToNull = false,
            JsonSerializerSettings? jsonSettings = null)
        {
            sw.Write(Serialise(isEmptyToNull, jsonSettings));
            sw.Flush();
        }

        public string Serialise(
            bool isEmptyToNull = false,
            JsonSerializerSettings? jsonSettings = null)
        {
            return Jtransform<T>.SerializeObject(
                this,
                isEmptyToNull,
                jsonSettings);
        }

        protected static void MoveBackup(string f)
        {
            var b = f + "~";
            if (File.Exists(b)) File.Delete(b);
            if (File.Exists(f)) File.Move(f, b);
        }
    }
}
