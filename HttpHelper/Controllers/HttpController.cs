using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HttpHelper.Controllers
{
    public class HttpController : Controller
    {
        public IActionResult HttpIndex()
        {
            return View();
        }

        public IActionResult HttpRequest(HttpRequest httpRequest)
        {
            List<Task> taskList = new List<Task>();
            if (httpRequest != null)
            {
                Dictionary<string, string> headers = new Dictionary<string, string>();
                if (!string.IsNullOrWhiteSpace(httpRequest.Accept))
                {
                    headers.Add("Accept", httpRequest.Accept);
                }
                if (!string.IsNullOrWhiteSpace(httpRequest.Authorization))
                {
                    headers.Add("Authorization", httpRequest.Authorization);
                }
                //if (!string.IsNullOrWhiteSpace(httpRequest.ContentType))
                //{
                //    headers.Add("Content-Type", httpRequest.ContentType);
                //}
                if (!string.IsNullOrWhiteSpace(httpRequest.Origin))
                {
                    headers.Add("Origin", httpRequest.Origin);
                }
                if (!string.IsNullOrWhiteSpace(httpRequest.Referer))
                {
                    headers.Add("Referer", httpRequest.Referer);
                }
                if (!string.IsNullOrWhiteSpace(httpRequest.UserAgent))
                {
                    headers.Add("User-Agent", httpRequest.UserAgent);
                }
                if (!string.IsNullOrWhiteSpace(httpRequest.customHeaders))
                {
                    //List<CustomHeaders> customHeadersList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CustomHeaders>>(httpRequest.customHeaders);
                    //foreach (CustomHeaders customHeaders in customHeadersList)
                    //{
                    //    if (!string.IsNullOrWhiteSpace(customHeaders.Key) && !string.IsNullOrWhiteSpace(customHeaders.Value))
                    //    {
                    //        headers.Add(customHeaders.Key, customHeaders.Value);
                    //    }
                    //}

                }
                for (int i = 0; i < httpRequest.ConcurrencyTimes; i++)
                {
                    if (httpRequest.RequestMethod == ERequestMethod.Get)
                    {
                        taskList.Add(HttpHelper.HttpGetAsync("", headers));
                        //HttpHelper.HttpGetAsync("", headers).Result
                    }
                    else
                    {
                        taskList.Add(HttpHelper.HttpPostAsync(httpRequest.Url, httpRequest.PostData, httpRequest.ContentType, httpRequest.TimeOut, headers));
                    }
                }
            }
            Task.WaitAll(taskList.ToArray());
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (Task item in taskList)
            {
                string Result = (item.GetType().GetProperty("Result").GetValue(item)).ToString();
                sb.AppendLine(Result);
            }

            return Ok(sb.ToString());

        }
    }

    public class HttpRequest
    {
        /// <summary>
        /// 请求路径
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 并发次数
        /// </summary>
        public int ConcurrencyTimes { get; set; } = 1;
        public int TimeOut { get; set; } = 30;

        /// <summary>
        /// 请求类型
        /// </summary>
        public ERequestMethod RequestMethod { get; set; }
        public string PostData { get; set; }
        public string Accept { get; set; }
        public string Authorization { get; set; }
        public string ContentType { get; set; }
        public string Origin { get; set; }
        public string Referer { get; set; }
        public string UserAgent { get; set; }
        public string customHeaders { get; set; }

    }

    public class CustomHeaders
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public enum ERequestMethod
    {
        Get = 1,
        Post = 2
    }
}