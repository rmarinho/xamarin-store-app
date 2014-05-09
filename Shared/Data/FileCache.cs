﻿using System;
using System.Threading.Tasks;
#if NETFX_CORE

using Windows.Security.Cryptography;
#else
using System.Security.Cryptography;
#endif
using System.Text;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Net.Http;

namespace XamarinStore
{
    public class FileCache
    {

        public static string SaveLocation;
        public static async Task<string> Download(string url)
        {
            if (string.IsNullOrEmpty(SaveLocation))
                throw new Exception("Save location is required");
            var fileName = md5(url);

            return await Download(url, fileName);
        }

        static object locker = new object();
        public static async Task<string> Download(string url, string fileName)
        {
            try
            {
                var path = Path.Combine(SaveLocation, fileName);
                //TODO
#if NETFX_CORE
#else
                if (File.Exists(path))
                    return path;
#endif


                await GetDownload(url, path);

                return path;
            }
            catch (Exception ex)
            {
                //TODO: Console.WriteLine (ex);
                return "";
            }
        }

        static Dictionary<string, Task> downloadTasks = new Dictionary<string, Task>();
        static Task GetDownload(string url, string fileName)
        {
            lock (locker)
            {
                Task task;
                if (downloadTasks.TryGetValue(fileName, out task))
                    return task;
                #if NETFX_CORE
#else
                var client = new WebClient();
                downloadTasks.Add(fileName, task = client.DownloadFileTaskAsync(url, fileName));
#endif
                      return task;

            }
        }
        static void removeTask(string fileName)
        {
            lock (locker)
            {
                downloadTasks.Remove(fileName);
            }
        }

#if NETFX_CORE

#else
        static MD5CryptoServiceProvider checksum = new MD5CryptoServiceProvider ();
#endif

        static int hex(int v)
        {
            if (v < 10)
                return '0' + v;
            return 'a' + v - 10;
        }

        static string md5(string input)
        {
#if NETFX_CORE
            return input;
           /// string md5Hex = CryptographicBuffer.EncodeToHexString(input)
#else
			var bytes = checksum.ComputeHash (Encoding.UTF8.GetBytes (input));
			var ret = new char [32];
			for (int i = 0; i < 16; i++){
				ret [i*2] = (char)hex (bytes [i] >> 4);
				ret [i*2+1] = (char)hex (bytes [i] & 0xf);
			}
			return new string (ret);
#endif
        }
    }
}

