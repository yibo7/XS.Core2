using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XS.Core2.HttpServer
{
    public class XsHttpServer
    {
        private System.Net.HttpListener httpListener;
        private Dictionary<string, Func<HttpListenerRequest, HttpListenerResponse, string>> routes = new Dictionary<string, Func<HttpListenerRequest, HttpListenerResponse, string>>();
        //private Dictionary<string, Func<HttpListenerRequest, Stream>> streamRoutes = new Dictionary<string, Func<HttpListenerRequest, Stream>>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="HostUrl">监听端口，比如8080</param>
        public XsHttpServer(int port,bool IsLoclHost = true)
        {
            httpListener = new System.Net.HttpListener();
            httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            var url = string.Concat("http://localhost:", port, "/"); // 仅本地访问,不需要管理员权限
            if (!IsLoclHost)
            {
                url = string.Concat("http://+:", port, "/");  // 允许外网访问,需要管理员权限
            } 
                
            httpListener.Prefixes.Add(url);
            
        }

        public void AddRoute(string path, Func<HttpListenerRequest, HttpListenerResponse, string> handler)
        {
            routes.Add(path, handler);
        }
 

        public void Start()
        {
            httpListener.Start();
            httpListener.BeginGetContext(new AsyncCallback(GetContextCallBack), httpListener);
            //Task.Run(() => StartListening());
        }

        public void Stop()
        {
            httpListener.Stop();
        }


        private void GetContextCallBack(IAsyncResult ar)
        {
            httpListener = ar.AsyncState as HttpListener;

            HttpListenerContext context = httpListener.EndGetContext(ar);
            httpListener.BeginGetContext(new AsyncCallback(GetContextCallBack), httpListener);


            HttpListenerResponse response = context.Response;
            // 统一设置 Content-Type 为 text/plain 并指定 charset=utf-8
            response.ContentType = "text/plain; charset=utf-8";
            HttpListenerRequest request = context.Request;

            response.KeepAlive = false;

            string responseString = string.Empty; 

            if (routes.ContainsKey(request.Url.AbsolutePath))
            {
                responseString = routes[request.Url.AbsolutePath](request,response);
            }
            else
            {
                responseString = "Invalid request 404";
                response.StatusCode = 404;

            }
            if (!string.IsNullOrWhiteSpace(responseString))
            {
                var buffer = Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);
                response.Close();
            }           
            
        } 
    }
}
