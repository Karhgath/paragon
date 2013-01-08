using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;

namespace Paragon.Infrastructure
{
    public class FileHandler
    {
        public static readonly ConcurrentDictionary<string, string> Cache = new ConcurrentDictionary<string, string>();
        private static readonly Lazy<FileSystemWatcher> Watcher = new Lazy<FileSystemWatcher>(System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

        private HttpServerUtilityBase Server { get; set; }

        public FileHandler(HttpServerUtilityBase server)
        {
            Server = server;
            
            if (!Watcher.IsValueCreated)
            {
                Watcher.Value.Path = Server.MapPath(@"~/App_Data/");
                Watcher.Value.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite;
                Watcher.Value.IncludeSubdirectories = true;
                Watcher.Value.Filter = "*.pgn";
                Watcher.Value.Changed += new FileSystemEventHandler(OnChanged);
                Watcher.Value.EnableRaisingEvents = true;
            }
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            var key = e.FullPath.Substring(e.FullPath.IndexOf(@"\App_Data\") + @"\App_Data\".Length).Replace(@"\", @"/");
            var text = System.IO.File.ReadAllText(e.FullPath);
            Cache.AddOrUpdate(key, text, (k, v) => text);
        }

        public string ReadHub(string realm, string hub)
        {
            var key = string.Format("{0}/{1}/{2}.{3}.pgn", "world", realm, hub, "hub");
            
            return Read(key);
        }

        private string Read(string key)
        {
            return Cache.GetOrAdd(key, (k) => System.IO.File.ReadAllText(Server.MapPath(@"~/App_Data/" + key)));
        }
    }
}