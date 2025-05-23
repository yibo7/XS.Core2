//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Net;
//using System.Net.Mime;
//using System.Reflection;
//using System.Text;
//namespace WalletMiddleware.Http
//{
//    public class ApiHttpListener
//    {
//        private HttpListener httpListener;
//        public ApiHttpListener(int port)
//        {

//            try
//            {
//                httpListener = new HttpListener();
//                httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
//                var url = string.Concat("http://+:", port, "/");
//                httpListener.Prefixes.Add(url);
//                httpListener.Start();
//                httpListener.BeginGetContext(new AsyncCallback(GetContextCallBack), httpListener);

//            }
//            catch (Exception e)
//            {

//                throw;
//            }

//        }


//        private void GetContextCallBack(IAsyncResult ar)
//        {
//            httpListener = ar.AsyncState as HttpListener;

//            HttpListenerContext context = httpListener.EndGetContext(ar);
//            httpListener.BeginGetContext(new AsyncCallback(GetContextCallBack), httpListener);


//            HttpListenerResponse response = context.Response;
//            HttpListenerRequest request = context.Request;

//            response.KeepAlive = false;
//            //request.Url

//            string sFileName = request.Url.AbsolutePath;//Console.WriteLine(context.Request.Url.PathAndQuery);

//            try
//            {
//                StringBuilder sbMessage = new StringBuilder();

//                if (sFileName.Equals("/"))
//                {
//                    sbMessage.Append("WalletMiddleware 系统1.0");
//                }
//                else if (sFileName.Equals("/v"))
//                {

//                    sbMessage.Append("WalletMiddleware 1.0.0");

//                }
//                else if (sFileName.Equals("/fees"))//ctid=1&to=abc&amount=10&speed=1
//                {
//                    string userid = Settings.Instance.WcfUserId;
//                    int CoinTypeId = XS.Core.XsUtils.StrToInt(request.QueryString["ctid"]);
//                    string coinType = AppStaticData.CoinTypes[CoinTypeId].fShortName;
//                    string from = AppStaticData.HotWallets[CoinTypeId].Address;
//                    string to = request.QueryString["to"];
//                    decimal amount = XS.Core.XsUtils.StrToDecimal(request.QueryString["amount"], 0);
//                    string password = Settings.Instance.WcfUserPass;
//                    bool isFromCold = false;
//                    int speed = XS.Core.XsUtils.StrToInt(request.QueryString["speed"]);
//                    string valstr =
//                        $"amount={amount}&coinType={coinType}&from={from}&isFromCold={isFromCold}&password={password}&speed={speed}&to={to}&userid={userid}";
//                    string signature = HashHelper.GetHashStr(valstr);
//                    var rzData = WcfInst.Instance.GetTransactionMinerFee(userid, coinType, from, to, amount, password, isFromCold, speed, signature);

//                    string rzTem = "isok: {0},msg: '{1}',fees:{2}";
//                    sbMessage.Append("{");
//                    if (rzData.IsSucess)
//                    {
//                        if (rzData.Data.IsValidated)
//                        {
//                            sbMessage.AppendFormat(rzTem, true, "成功", rzData.Data.CurrentSelectMinerFee);
//                        }
//                        else
//                        {
//                            sbMessage.AppendFormat(rzTem, false, rzData.Data.Message, 0);
//                        }
//                    }
//                    else
//                    {
//                        sbMessage.AppendFormat(rzTem, false, "请求接口出错", 0);

//                    }
//                    sbMessage.Append("}");

//                }
//                else
//                {
//                    response.StatusCode = 404;
//                    response.StatusDescription = "Could not find the requested page";
//                    response.ProtocolVersion = new Version("1.1");
//                    response.Close();
//                    return;
//                }

//                // Use the encoding from the response if one has been set.
//                // Otherwise, use UTF8.
//                System.Text.Encoding encoding = response.ContentEncoding;
//                if (encoding == null)
//                {
//                    encoding = System.Text.Encoding.Default;
//                    response.ContentEncoding = encoding;
//                }
//                byte[] buffer = encoding.GetBytes(sbMessage.ToString());
//                response.ContentLength64 = buffer.Length;
//                response.StatusCode = (int)HttpStatusCode.OK;
//                response.StatusDescription = "OK";
//                response.ProtocolVersion = new Version("1.1");
//                response.ContentType = "text/html";
//                // Don't keep the TCP connection alive; 
//                // We don't expect multiple requests from the same client.

//                HttpServerEventArgs Args = new HttpServerEventArgs();
//                Args.Url = context.Request.UserHostAddress;
//                Args.IP = context.Request.ServiceName;

//                // Write the response body.
//                System.IO.Stream stream = response.OutputStream;
//                stream.Write(buffer, 0, buffer.Length);
//                stream.Close();
//                response.Close();


//                OnRequest(null, Args);


//            }
//            catch (Exception e)
//            {
//                XS.Core.Log.ErrorLog.Error(e);
//                XS.Core.Log.ErrorLog.ErrorFormat("来自{0}接收请求出错:{1}", sFileName, e.Message);
//            }

//        }

//        public void Stop()
//        {
//            httpListener.Stop();
//        }

//        public event EventHandler<HttpServerEventArgs> RequestEvent;
//        public void OnRequest(object sender, HttpServerEventArgs arg)
//        {
//            if (!Equals(RequestEvent, null))
//            {
//                RequestEvent(sender, arg);
//            }
//        }

//    }
//}