using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http.Headers;

namespace Kappa.BackEnd.Server {
    public abstract class HttpService {
        public string BaseUrl { get; }
        protected HttpService(string baseUrl) {
            BaseUrl = baseUrl;
        }

        protected static void HandleFile(HttpListenerContext context, string file) {
            context.Response.ContentType = GetMimeType(file);
            context.Response.Headers["Accept-Ranges"] = "bytes";

            if (!File.Exists(file)) {
                context.Response.StatusCode = 404;
            } else {
                try {
                    using (var read = File.OpenRead(file))
                        HandleStream(context, read);
                    context.Response.StatusCode = 200;
                } catch {
                    context.Response.StatusCode = 500;
                }
            }
        }

        protected static void HandleStream(HttpListenerContext context, Stream src, long offset = 0, long length = -1) {
            if (context.Request.Headers["Range"] != null) {
                var range = context.Request.Headers["Range"].Split('=')[1];
                var split = range.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                long start = int.Parse(split[0]);
                long end = split.Length > 1 ? int.Parse(split[1]) : src.Length;
                if (length >= 0) end = Math.Min(end, length);

                context.Response.StatusCode = 206;
                context.Response.Headers["Content-Range"] = new ContentRangeHeaderValue(start, end - 1).ToString();

                src.Seek(offset + start, SeekOrigin.Begin);
                src.CopyToLength(context.Response.OutputStream, end - start);
            } else if (length < 0) {
                src.Seek(offset, SeekOrigin.Begin);
                src.CopyTo(context.Response.OutputStream);
            } else {
                src.Seek(offset, SeekOrigin.Begin);
                src.CopyToLength(context.Response.OutputStream, length);
            }
        }

        public abstract bool Handle(HttpListenerContext context);

        public static string GetMimeType(string file) {
            return new Dictionary<string, string> {
                [".html"] = "text/html",
                [".css"] = "text/css",
                [".js"] = "application/javascript",
                [".map"] = "application/json",

                [".svg"] = "image/svg+xml",
                [".jpg"] = "image/jpeg",
                [".png"] = "image/png",
                [".webm"] = "video/webm",
                [".ogg"] = "audio/ogg",

                [".ttf"] = "application/octet-stream"
                //MPEG requires specialized codes :(
                //[".mp4"] = "video/mp4",
                //[".mp3"] = "audio/mp3",
            }[Path.GetExtension(file)];
        }
    }
}
