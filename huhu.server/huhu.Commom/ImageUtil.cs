namespace huhu.Commom
{
    public class ImageUtil
    {
        /// <summary>
        /// 文件类型
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static bool IsWebImage(string contentType) {
            if (contentType == "image/pjpeg" || 
                contentType == "image/jpeg" || 
                contentType == "image/gif" || 
                contentType == "image/bmp" ||
                contentType == "image/webp" ||
                contentType == "image/png") {
                return true;
            } else {
                return false;
            }
        }
    }
}
