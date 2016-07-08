using Kappa.Riot.Domain.TeambuilderDraft;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Kappa {
    public static class Extensions {
        public static void CopyToLength(this Stream input, Stream output, long bytes) {
            byte[] buffer = new byte[32768];
            int read;
            while (bytes > 0 && (read = input.Read(buffer, 0, (int) Math.Min(buffer.Length, bytes))) > 0) {
                output.Write(buffer, 0, read);
                bytes -= read;
            }
        }

        public static T ReadStruct<T>(this Stream stream) where T : struct {
            var sz = Marshal.SizeOf(typeof(T));
            var buffer = new byte[sz];
            while (sz > 0) sz -= stream.Read(buffer, 0, sz);
            var pinnedBuffer = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            var structure = (T) Marshal.PtrToStructure(
                pinnedBuffer.AddrOfPinnedObject(), typeof(T));
            pinnedBuffer.Free();
            return structure;
        }

        public static int WriteStruct<T>(this Stream stream, T t) where T : struct {
            var bytes = t.ToByteArray();
            stream.Write(bytes, 0, bytes.Length);
            return bytes.Length;
        }

        public static byte[] ToByteArray<T>(this T t) where T : struct {
            var size = Marshal.SizeOf(typeof(T));
            var bytes = new byte[size];

            var ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(t, ptr, true);
            Marshal.Copy(ptr, bytes, 0, size);
            Marshal.FreeHGlobal(ptr);

            return bytes;
        }

        public static T[] Extend<T>(this T[] src, params T[] t) {
            if (src == null) throw new ArgumentNullException(nameof(src));
            return Concat(src, false, t).ToArray();
        }

        public static ChampSelectAction GetCurrentAction(this ChampSelectState state) {
            return (state?.CurrentActionIndex ?? -1) < 0 ? null : state.Actions[state.CurrentActionIndex]?.Single();
        }

        private static IEnumerable<T> Concat<T>(IEnumerable<T> source, bool insertAtStart, params T[] extra) {
            if (insertAtStart)
                foreach (var t in extra)
                    yield return t;
            foreach (var e in source)
                yield return e;
            // ReSharper disable once InvertIf
            if (!insertAtStart)
                foreach (var t in extra)
                    yield return t;
        }

        public static byte[] GetBytes(this string str, Encoding encoding = null) {
            return (encoding ?? Encoding.UTF8).GetBytes(str);
        }

        public static int EndOf(this string str, string search, int start = 0) {
            int index = str.IndexOf(search, start, StringComparison.Ordinal);
            if (index != -1) index += search.Length;
            return index;
        }

        public static Task<int> WaitForExitAsync(this Process p) {
            var tcs = new TaskCompletionSource<int>();
            p.EnableRaisingEvents = true;
            p.Exited += (s, e) => tcs.SetResult(p.ExitCode);
            return tcs.Task;
        }
    }
}
