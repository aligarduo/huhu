using huhu.Commom;
using huhu.Commom.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace huhu.API.Controllers
{
    /// <summary>
    /// 图片处理
    /// </summary>
    public class TosController : ApiController
    {
        #region 配置
        /// <summary>
        /// 主域名
        /// </summary>
        private static readonly string MainFileServer = ConfigurationManager.AppSettings["MainFileServer"].ToString();
        /// <summary>
        /// 备用域名
        /// </summary>
        private static readonly string BackupFileServer = ConfigurationManager.AppSettings["BackupFileServer"].ToString();
        /// <summary>
        /// 存放路径
        /// </summary>
        private static readonly string DiskStoragePath = ConfigurationManager.AppSettings["DiskStoragePath"].ToString();
        /// <summary>
        /// 头像图片尺寸
        /// </summary>
        private static readonly string UserPicSize = ConfigurationManager.AppSettings["UserPicSize"].ToString();
        /// <summary>
        /// 头像图片存放文件夹路径
        /// </summary>
        private static readonly string UserPicFolderPath = ConfigurationManager.AppSettings["UserPicFolderPath"].ToString();
        /// <summary>
        /// 原图图片尺寸
        /// </summary>
        private static readonly string OriginalSize = ConfigurationManager.AppSettings["OriginalSize"].ToString();
        /// <summary>
        /// 原图图片存放文件夹路径
        /// </summary>
        private static readonly string OriginalFolderPath = ConfigurationManager.AppSettings["OriginalFolderPath"].ToString();
        /// <summary>
        /// 封面图片尺寸
        /// </summary>
        private static readonly string CoverSize = ConfigurationManager.AppSettings["CoverSize"].ToString();
        /// <summary>
        /// 封面图片存放文件夹路径
        /// </summary>
        private static readonly string CoverFolderPath = ConfigurationManager.AppSettings["CoverFolderPath"].ToString();
        /// <summary>
        /// 文字水印字体颜色
        /// </summary>
        private static readonly string WaterMarkFontColor = ConfigurationManager.AppSettings["WaterMarkFontColor"].ToString();
        #endregion


        [HttpPost]
        [Route("image/get_img_url")]
        public HttpResponseMessage FormatConversion([FromUri] string method, [FromUri] bool wmark) {
            ResultMsg result = new ResultMsg();
            object obj;
            switch (method) {
                //原图
                case "original": obj = ProcessData(OriginalSize, OriginalFolderPath, wmark); break;
                //封面
                case "cover": obj = ProcessData(CoverSize, CoverFolderPath, wmark); break;
                //头像
                case "profile": obj = ProcessData(UserPicSize, UserPicFolderPath, wmark); break;
                //没有匹配
                default: return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            return JsonUtil.ToJson(obj);
        }


        /// <summary>
        /// 开始处理数据
        /// </summary>
        /// <param name="PicSize"></param>
        /// <param name="FolderPath"></param>
        /// <param name="WaterMark"></param>
        /// <returns></returns>
        private object ProcessData(string PicSize, string FolderPath, bool WaterMark) {
            ResultMsg result = new ResultMsg();

            string filePath = DiskStoragePath + "\\" + FolderPath; //存放路径
            string fileSuffix = WaterMark ? "mark.webp" : "no-mark.webp"; //后缀
            string fileName = Guid.NewGuid().ToString("N") + "~" + fileSuffix; //文件名

            HttpFileCollection Files = HttpContext.Current.Request.Files;
            if (Files.AllKeys.Any()) {
                using (HttpClient client = new HttpClient()) {
                    HttpContextBase HttpContext = (HttpContextBase)Request.Properties["MS_HttpContext"];
                    Stream file_stream = HttpContext.Request.Files[0].InputStream;
                    //不支持图片格式
                    if (!ImageUtil.IsWebImage(HttpContext.Request.Files[0].ContentType)) {
                        return result.SetResultMsg((int)ResultCode.IMAGE_FORMAT_NOT_SUPPORTED, Descripion.GetDescription(ResultCode.IMAGE_FORMAT_NOT_SUPPORTED));
                    }
                    // 图片超过4M
                    if (HttpContext.Request.Files[0].ContentLength > 4 * 1024 * 1024) {
                        return result.SetResultMsg((int)ResultCode.SIZE_OUT_OF_LIMIT, Descripion.GetDescription(ResultCode.SIZE_OUT_OF_LIMIT));
                    }
                    //裁剪
                    List<string> picsize_list = new List<string>(PicSize.Split('*'));
                    if (picsize_list[0] != "none" && picsize_list[1] != "none") {
                        file_stream = CutImageUtil.ZoomAuto(file_stream, double.Parse(picsize_list[0]), double.Parse(picsize_list[1]), 100);
                    }
                    //添加水印
                    if (WaterMark) {
                        file_stream = WaterMarkUtil.AddImageSignText(file_stream, "@乎乎技术社区", 9, 100, 13, WaterMarkFontColor);
                    }
                    //创建文件夹
                    if (!FileUtil.FileExists(filePath)) FileUtil.CreateFolder(filePath);
                    //保存文件
                    Image img = Image.FromStream(file_stream);
                    img.Save(filePath + "\\" + fileName);
                    //返回路径
                    string proc = FolderPath.Replace("\\", "/") + "/" + fileName;
                    //释放资源
                    file_stream.Close();
                    file_stream.Dispose();
                    //返回链接
                    object Url = new {
                        main_url = MainFileServer + proc,
                        backup_url = BackupFileServer + proc,
                    };
                    return result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), Url);
                }
            }
            return result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR));
        }


    }
}