using huhu.Commom;
using huhu.Commom.Enums;
using System;
using System.Configuration;
using System.Net.Http;
using System.Web.Http;

namespace huhu.API.Controllers
{
    /// <summary>
    /// APP扫一扫登录
    /// </summary>
    public class RichScanController : ApiController
    {
        /// <summary>
        /// 获取二维码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("passport/web/richscan")]
        public HttpResponseMessage RichScan() {
            //初始化结果集
            ResultMsg result = new ResultMsg();
            //初始化变量
            object Guid = GUIDUtil.GuidTo19();
            string RichScanQRExpirationTime = ConfigurationManager.AppSettings["RichScanQRExpirationTime"].ToString();
            string redis_value = string.Format("richscan:{0}", Guid);
            //初始化redis
            RedisUtil redis = new RedisUtil();
            if (redis.StringGet(redis_value) != null) {
                redis.KeyDelete(redis_value);
            }
            redis.StringSet(redis_value, "1");
            redis.SetExpire(redis_value, DateTime.Now.AddSeconds(double.Parse(RichScanQRExpirationTime)));
            //整合数据体
            object return_result = new {
                uuid = Guid.ToString(),
                expires = RichScanQRExpirationTime,
                start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                expire_time = DateTime.Now.AddSeconds(int.Parse(RichScanQRExpirationTime)).ToString("yyyy-MM-dd HH:mm:ss")
            };
            //返回结果
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), return_result));
        }

        /// <summary>
        /// 刷新状态
        /// </summary>
        /// <param name="uuid">UUID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("passport/web/richscan_connect")]
        public HttpResponseMessage RichScan_Connect([FromUri] string uuid) {
            //初始化结果集
            ResultMsg result = new ResultMsg();
            if (string.IsNullOrEmpty(uuid)) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            //定义变量
            string Description = string.Empty; int Code = 0;
            //初始化redis
            RedisUtil redis = new RedisUtil();
            string _uuid = redis.StringGet(string.Format("richscan:{0}", uuid));
            if (_uuid != null) {
                switch (_uuid) {
                    case "1": Code = (int)ResultCode.QR_NO_FAILURE; Description = Descripion.GetDescription(ResultCode.QR_NO_FAILURE); break;
                    case "2": Code = (int)ResultCode.QR_AUTHENTICATING; Description = Descripion.GetDescription(ResultCode.QR_AUTHENTICATING); break;
                }
            } else {
                Code = (int)ResultCode.QR_VOKSI;
                Description = Descripion.GetDescription(ResultCode.QR_VOKSI);
            }
            //整合数据体
            object return_result = new { uuid, code = Code, msg = Description };
            //返回结果
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), return_result));
        }

        /// <summary>
        /// 二维码扫码回调
        /// </summary>
        /// <param name="uuid">UUID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("passport/web/richscan_syntony")]
        public HttpResponseMessage RichScan_Syntony([FromUri] string uuid) {
            // 初始化结果集
            ResultMsg result = new ResultMsg();
            //初始化redis
            RedisUtil redis = new RedisUtil();
            string RichScanQRExpirationTime = ConfigurationManager.AppSettings["RichScanQRExpirationTime"].ToString();
            string redis_value = string.Format("richscan:{0}", uuid);
            //redis结果不为null则修改值
            if (redis.StringGet(redis_value) != null) {
                redis.KeyDelete(redis_value);
                redis.StringSet(redis_value, "2");
                redis.SetExpire(redis_value, DateTime.Now.AddSeconds(double.Parse(RichScanQRExpirationTime)));
            }
            //返回结果
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS)));
        }



    }
}
