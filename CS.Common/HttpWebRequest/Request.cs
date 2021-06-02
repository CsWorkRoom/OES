using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CS.Common.ApiRequest
{
    /// <summary>
    /// 提供HttpWebRequest请求的相关封装.
    /// 本接口所提供的方法均不含异常拦截处理，请在调用的主方法中去拦截请求异常.
    /// </summary>
    public static class Request
    {
        #region post 请求
        public enum PostType
        {
            /// <summary>
            /// 表单模式，传入参数格式如：roleId=1&uid=2
            /// </summary>
            FROM = 0,
            /// <summary>
            /// JSON格式字符串，格式如：{k:v,k2:v2,k3:{kk1:vv1}}
            /// </summary>
            JSON = 1,
            /// <summary>
            /// XML模式
            /// </summary>
            XML = 2
        }
        public static string PostHttp(string url, string body, PostType type = PostType.FROM)
        {
            string resStr = string.Empty;
            switch (type)
            {
                case PostType.FROM:
                    resStr = PostForm(url, body);
                    break;
                case PostType.JSON:
                    resStr = PostJson(url, body);
                    break;
                case PostType.XML:
                    resStr = PostXml(url, body);
                    break;
                default:
                    resStr = PostForm(url, body);
                    break;
            }
            return resStr;
        }

        #region post请求几种方式(私有)
        /// <summary>
        /// POST表单
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        private static string PostForm(string url, string body)
        {
            string resStr = string.Empty;
            byte[] bs = Encoding.UTF8.GetBytes(body);
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = bs.Length;

            using (Stream reqStream = myRequest.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }

            using (HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse())
            {
                StreamReader sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                resStr = sr.ReadToEnd();
                sr.Close();
            }
            myRequest.Abort();
            return resStr;
        }

        /// <summary>
        /// POST XML
        /// </summary>
        /// <param name="url">请求url(不含参数)</param>
        /// <param name="body">请求body. soap"text/xml; charset=utf-8"xml字符串</param>
        /// <returns></returns>
        private static string PostXml(string url, string body)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            httpWebRequest.ContentType = "text/xml;charset=utf-8";
            httpWebRequest.Method = "POST";
            //httpWebRequest.Timeout = timeout;//设置超时
            httpWebRequest.Headers.Add("SOAPAction", "http://tempuri.org/mediate");

            byte[] btBodys = Encoding.UTF8.GetBytes(body);
            httpWebRequest.ContentLength = btBodys.Length;
            httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

            #region 取消异常拦截
            //HttpWebResponse httpWebResponse;
            //try
            //{
            //    httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            //}
            //catch (WebException ex)
            //{
            //    httpWebResponse = (HttpWebResponse)ex.Response;
            //}
            #endregion

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8);
            string responseContent = streamReader.ReadToEnd();

            httpWebResponse.Close();
            streamReader.Close();
            httpWebRequest.Abort();

            return responseContent;
        }

        /// <summary>
        /// POST json
        /// </summary>
        /// <param name="url"></param>
        /// <param name="JSONData"></param>
        /// <returns></returns>
        private static string PostJson(string url, string JSONData)
        {
            string result = string.Empty;
            //byte[] bs = Encoding.UTF8.GetBytes(body);
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/json";

            using (var streamWriter = new StreamWriter(myRequest.GetRequestStream()))
            {
                streamWriter.Write(JSONData);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)myRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            httpResponse.Close();
            myRequest.Abort();
            return result;
        }
        #endregion

        #endregion

        #region get请求
        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="url">请求url(不含参数)</param>
        /// <param name="postDataStr">参数部分：roleId=1&uid=2</param>
        /// <param name="timeout">等待时长(毫秒)</param>
        /// <returns></returns>
        public static string GetHttp(string url, string postDataStr, int timeout = 2000)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.Timeout = timeout;//等待

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            response.Close();
            request.Abort();
            return retString;
        }
        #endregion

        #region 文件传输请求
        /// <summary>
        /// 传输文件到指定接口
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filePath">文件物理路径</param>
        /// <returns></returns>
        public static string PostFile(string url, string filePath)
        {
            string resStr = string.Empty;

            // 初始化HttpWebRequest
            HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(url);

            // 封装Cookie
            Uri uri = new Uri(url);
            Cookie cookie = new Cookie("Name", DateTime.Now.Ticks.ToString());
            CookieContainer cookies = new CookieContainer();
            cookies.Add(uri, cookie);
            httpRequest.CookieContainer = cookies;

            if (!File.Exists(filePath))
            {
                return "文件不存在";
            }
            FileInfo file = new FileInfo(filePath);

            // 生成时间戳
            string strBoundary = "----------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundaryBytes = Encoding.ASCII.GetBytes(string.Format("\r\n--{0}--\r\n", strBoundary));

            // 填报文类型
            httpRequest.Method = "Post";
            httpRequest.Timeout = 1000 * 120;
            httpRequest.ContentType = "multipart/form-data; boundary=" + strBoundary;

            // 封装HTTP报文头的流
            StringBuilder sb = new StringBuilder();
            sb.Append("--");
            sb.Append(strBoundary);
            sb.Append(Environment.NewLine);
            sb.Append("Content-Disposition: form-data; name=\"");
            sb.Append("file");
            sb.Append("\"; filename=\"");
            sb.Append(file.Name);
            sb.Append("\"");
            sb.Append(Environment.NewLine);
            sb.Append("Content-Type: ");
            sb.Append("multipart/form-data;");
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sb.ToString());

            // 计算报文长度
            long length = postHeaderBytes.Length + file.Length + boundaryBytes.Length;
            httpRequest.ContentLength = length;

            // 将报文头写入流
            Stream requestStream = httpRequest.GetRequestStream();
            requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    requestStream.Write(buffer, 0, bytesRead);
                }
            }

            // 将报文尾部写入流
            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            // 关闭流
            requestStream.Close();

            using (HttpWebResponse myResponse = (HttpWebResponse)httpRequest.GetResponse())
            {
                StreamReader sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                resStr = sr.ReadToEnd();
                sr.Close();
                //Console.WriteLine("反馈结果" + responseString);
            }
            httpRequest.Abort();
            return resStr;
        }
        #endregion
    }
}
