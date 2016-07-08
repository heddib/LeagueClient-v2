using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IWshRuntimeLibrary;

namespace Installer {
    public class KappaInstaller {
        public static readonly string AppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Kappa");

        public static void Main(string[] args) {
            Console.CursorVisible = false;
            Console.WriteLine();
            Console.WriteLine("  Extracting...");

            var src = Assembly.GetExecutingAssembly().GetManifestResourceStream("Installer.Data.zip");
            var dst = Path.Combine(AppData, "Application");
            var toDelete = new[] { dst, Path.Combine(AppData, "cache") };

            foreach (string path in toDelete) {
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
            }

            Directory.CreateDirectory(dst);

            int count = 0;
            using (var zip = new ZipArchive(src)) {
                foreach (var entry in zip.Entries) {
                    var path = Path.Combine(dst, entry.FullName);
                    entry.ExtractToFile(path, true);
                    count++;

                    Console.WriteLine($"    {count} / {zip.Entries.Count}");
                    Console.CursorTop--;
                }
            }

            var shell = new WshShell();
            var linkFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Kappa.lnk");
            var lnk = (IWshShortcut) shell.CreateShortcut(linkFile);

            lnk.Description = "Kappa";
            lnk.WorkingDirectory = dst;
            lnk.TargetPath = Path.Combine(dst, "Kappa.exe");
            lnk.IconLocation = lnk.TargetPath;
            lnk.Save();
        }
    }
}
