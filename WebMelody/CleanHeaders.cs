using System;
using System.Web;
using System.Collections.Generic;
namespace WebMelody
{
    public class CleanHeaders : IHttpModule
    {
        private static List<string> _headers = new List<string>();
        public static void AddHeader(string header)
        {
            if(header != null && header != string.Empty)
                _headers.Add(header);
        }
        public void Dispose()
        {
            //clean-up code here.
        }
        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += PreSendRequestHeaders;
        }
        public void PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpApplication context = sender as HttpApplication;
            if (context != null)
                RemoveHeaders(context);
        }
        private static void RemoveHeaders(HttpApplication context)
        {
            foreach(var header in _headers)
                context.Response.Headers.Remove(header);
        }
    }
}