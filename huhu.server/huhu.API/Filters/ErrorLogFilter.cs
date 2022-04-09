using huhu.Commom;
using huhu.Commom.Enums;
using huhu.Commom.Snowflake;
using huhu.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http.Filters;

namespace huhu.API.Filters
{
    public class ErrorLogFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext ExecutedContext)
        {
            //获取报文数据信息
            Dictionary<string,object> ActionParams = ExecutedContext.ActionContext.ActionArguments;
            string ActionName = ExecutedContext.ActionContext.ActionDescriptor.ActionName;
            string ControllerName = ExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string HttpRequestHeaders = ExecutedContext.ActionContext.Request.Headers.ToString();
            string UserAgent = ExecutedContext.ActionContext.Request.Headers.UserAgent.ToString();
            string HttpMethod = ExecutedContext.ActionContext.Request.Method.Method;
            string Exception = ExecutedContext.Exception.ToString();
            //雪花算法数据初始化
            Snowflake.Instance.SnowflakesInit(0, 0);
            //异常信息不为null
            if (ExecutedContext.Exception != null) {
                try {
                    //写入数据库
                    using (huhuLogEntities db = new huhuLogEntities()) {
                        log logs = new log {
                            log_id = Snowflake.Instance.NextId().ToString(),
                            controller_name = ControllerName + "Controlle",
                            action_name = ActionName,
                            action_param = GetCollections(ActionParams),
                            http_header = HttpRequestHeaders,
                            client_ip = NetUtil.GET_ClientIP(),
                            client_type = UserAgent,
                            http_method = HttpMethod,
                            exception = Exception,
                            attack_time = TimeUtil.GetCurrentTimestamp().ToString(),
                        };
                        db.logs.Add(logs);
                        db.SaveChanges();
                    }
                }
                catch (Exception ex) {
                    //写入txt文件
                    string Path = HttpContext.Current.Server.MapPath("/") + "Log\\" + Regex.Replace(DateTime.Now.ToShortDateString().ToString(), "/", "_") + "\\";
                    if (!FileUtil.FileExists(Path)) FileUtil.CreateFolder(Path);
                    string filename = Regex.Replace(DateTime.Now.ToLongTimeString().ToString(), ":", "_") + "__" + new Random().Next(100, 999) + ".txt";
                    using (StreamWriter sw = new StreamWriter(Path + filename)) {
                        string msg = string.Empty;
                        msg += $"controller_name：{ControllerName}Controller\n";
                        msg += $"action_name：{ActionName}\n";
                        msg += $"action_param：{GetCollections(ActionParams)}\n";
                        msg += $"http_header:{HttpRequestHeaders}\n";
                        msg += $"client_ip：{NetUtil.GET_ClientIP()}\n";
                        msg += $"client_type：{UserAgent}\n";
                        msg += $"http_method：{HttpMethod}\n";
                        msg += $"exception：{Exception}\n";
                        msg += $"attack_time：{TimeUtil.GetCurrentTimestamp()}";
                        sw.WriteLine("===================== 捕获异常开始 =====================\n");
                        sw.WriteLine("==================== 数据库触发异常 =====================\n");
                        sw.WriteLine(ex);
                        sw.WriteLine("\n====================== 访问触发异常 ======================\n");
                        sw.WriteLine(msg);
                        sw.WriteLine("\n====================== 捕获异常结束 ======================\n\n");
                    }
                }
            }

            ResultMsg result = new ResultMsg();
            ExecutedContext.Response = JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SERVER_ERROR, Descripion.GetDescription(ResultCode.SERVER_ERROR)));
            base.OnException(ExecutedContext);
        }

        #region 获取Action参数
        /// <summary>
        /// 获取Action参数
        /// </summary>
        /// <param name="Collections"></param>
        /// <returns></returns>
        private static string GetCollections(Dictionary<string, object> Collections)
        {
            string Parameters = string.Empty;
            if (Collections == null || Collections.Count == 0) {
                return Parameters;
            }
            foreach (string key in Collections.Keys) {
                Parameters += string.Format("{0}={1}&", key, Collections[key]);
            }
            if (!string.IsNullOrWhiteSpace(Parameters) && Parameters.EndsWith("&")) {
                Parameters = Parameters.Substring(0, Parameters.Length - 1);
            }
            return Parameters;
        }

        #endregion
    }
}