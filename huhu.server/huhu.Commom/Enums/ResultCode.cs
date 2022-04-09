using System.ComponentModel;

namespace huhu.Commom.Enums
{
    /// <summary>
    /// 状态代码
    /// </summary>
    public enum ResultCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("success")]
        SUCCESS = 0,
        /// <summary>
        /// 服务器内部错误
        /// </summary>
        [Description("error")]
        SERVER_ERROR = 1,
        /// <summary>
        /// sign缺少字段
        /// </summary>
        [Description("sign缺少字段")]
        SIGN_LACK_FIELD = 1001,
        /// <summary>
        /// sign请求超时
        /// </summary>
        [Description("sign请求超时")]
        SIGN_REQUEST_TIMEOUT = 1002,
        /// <summary>
        /// sign校验失败
        /// </summary>
        [Description("sign校验失败")]
        SIGN_CHECK_FAILURE = 1003,
        /// <summary>
        /// csrf_token校验失败
        /// </summary>
        [Description("csrf_token校验失败")]
        CSRF_TOKEN_CHECK_FAILURE = 1004,
        /// <summary>
        /// 请求没有权限
        /// </summary>
        [Description("请求没有权限")]
        CSRF_REQUEST_NOT_AUTHORIZED = 1005,
        /// <summary>
        /// 参数错误
        /// </summary>
        [Description("参数错误")]
        PARAMETER_ERROR = 1006,
        /// <summary>
        /// 手机号格式错误
        /// </summary>
        [Description("手机号格式错误")]
        PHONE_FORMAT_IS_WRONG = 1007,
        /// <summary>
        /// 验证码错误
        /// </summary>
        [Description("验证码错误")]
        VCODE_WRONG = 1008,
        /// <summary>
        /// 该帐号因涉及异常操作被限制登录
        /// </summary>
        [Description("该帐号因涉及异常操作被限制登录")]
        ACCOUNT_ABNORMAL_LIMIT = 1009,
        /// <summary>
        /// 该帐号因涉及发布违规内容被限制登录
        /// </summary>
        [Description("该帐号因涉及发布违规内容被限制登录")]
        ACCOUNT_VIOLATION_LIMIT = 1010,
        /// <summary>
        /// 该帐号因多次涉及违规行为被永久关闭
        /// </summary>
        [Description("该帐号因多次涉及违规行为被永久关闭")]
        ACCOUNT_VIOLATION_CLOSED = 1011,
        /// <summary>
        /// 账号已被注销
        /// </summary>
        [Description("账号已被注销")]
        ACCOUNT_IS_CANCELLED = 1012,
        /// <summary>
        /// 帐号或密码错误
        /// </summary>
        [Description("帐号或密码错误")]
        ACCOUNT_OR_PASS_INCORRECT = 1013,
        /// <summary>
        /// 不支持该请求
        /// </summary>
        [Description("不支持该请求")]
        REQUEST_IS_NOT_SUPPORTED = 1014,
        /// <summary>
        /// 不支持此链接
        /// </summary>
        [Description("不支持此链接")]
        LINK_NOT_SUPPORTED = 1015,
        /// <summary>
        /// 此链接无法正常访问
        /// </summary>
        [Description("此链接无法正常访问")]
        INVALID_LINK = 1016,
        /// <summary>
        /// 内容为空
        /// </summary>
        [Description("内容为空")]
        CONTENT_IS_NULL = 1017,
        /// <summary>
        /// 此收藏集名称已存在
        /// </summary>
        [Description("此收藏集名称已存在")]
        COLLECTION_IDENTICAL = 1018,
        /// <summary>
        /// 此收藏集不存在,请先创建收藏集
        /// </summary>
        [Description("此收藏集不存在,请先创建收藏集")]
        COLLECTION_NOT_EXIST = 1019,
        /// <summary>
        /// 此收藏集下不存在该项目
        /// </summary>
        [Description("此收藏集下不存在该项目")]
        COLLECTION_NOT_EXIST_ITEM = 1020,
        /// <summary>
        /// 请勿重复操作
        /// </summary>
        [Description("请勿重复操作")]
        REPETITIVE_OPERATION = 1021,
        /// <summary>
        /// 操作失败，请先进行添加项目
        /// </summary>
        [Description("操作失败，请先进行添加项目")]
        OPERATION_FAILURE = 1022,
        /// <summary>
        /// 草稿不存在
        /// </summary>
        [Description("草稿不存在")]
        DRAFT_NOT_EXIST = 1023,
        /// <summary>
        /// 请先登录
        /// </summary>
        [Description("请先登录")]
        PLEASE_LOGIN_FIRST = 1024,
        /// <summary>
        /// 标签不存在
        /// </summary>
        [Description("标签不存在")]
        TAG_NOT_EXIST = 1025,
        /// <summary>
        /// 邮箱格式错误
        /// </summary>
        [Description("邮箱格式错误")]
        EMAIL_FORMAT_IS_WRONG = 1026,
        /// <summary>
        /// 不支持图片格式
        /// </summary>
        [Description("不支持图片格式")]
        IMAGE_FORMAT_NOT_SUPPORTED = 1027,
        /// <summary>
        /// 文章不存在或已被删除
        /// </summary>
        [Description("文章不存在或已被删除")]
        ARTICLE_NOT_EXIST_OR_DELETED = 1028,
        /// <summary>
        /// 评论不存在或已被删除
        /// </summary>
        [Description("评论不存在或已被删除")]
        COMMENTS_NOT_EXIST = 1029,
        /// <summary>
        /// 回复不存在或已被删除
        /// </summary>
        [Description("回复不存在或已被删除")]
        REPLY_NOT_EXIST = 1030,
        /// <summary>
        /// 用户不存在
        /// </summary>
        [Description("用户不存在")]
        USER_NOT_EXIST = 1031,
        /// <summary>
        /// 操作类型错误
        /// </summary>
        [Description("操作类型错误")]
        OPERATION_TYPE_ERROR = 1032,
        /// <summary>
        /// 标签名已存在
        /// </summary>
        [Description("标签名称已存在")]
        TAG_NAME_ALREADY_EXISTS = 1035,
        /// <summary>
        /// 手机号已存在
        /// </summary>
        [Description("手机号已存在")]
        MOBILE_NAME_ALREADY_EXISTS = 1036,
        /// <summary>
        /// 验证码下发失败
        /// </summary>
        [Description("验证码下发失败")]
        ISSUED_ERROR = 1037,
        /// <summary>
        /// 话题圈子名称已存在
        /// </summary>
        [Description("话题圈子名称已存在")]
        CIRCLE_NAME_ALREADY_EXISTS = 1038,
        /// <summary>
        /// 话题圈子不存在
        /// </summary>
        [Description("话题圈子不存在")]
        TOPIC_CIRCLE_DOES_NOT_EXIST = 1039,
        /// <summary>
        /// 话题不存在
        /// </summary>
        [Description("话题不存在")]
        TOPIC_DOES_NOT_EXIST = 1040,
        /// <summary>
        /// 选项已存在
        /// </summary>
        [Description("选项已存在")]
        OPTION_ALREADY_EXISTS = 1041,
        /// <summary>
        /// 选项不存在
        /// </summary>
        [Description("选项不存在")]
        OPTION_ALREADY_NOT_EXIST = 1042,
        /// <summary>
        /// 关注对象不能是自己
        /// </summary>
        [Description("对象不能是自己")]
        CANT_FOCUS_ON_YOURSELF = 1043,
        /// <summary>
        /// 此广告不存在
        /// </summary>
        [Description("此广告不存在")]
        ADVERTISING_DOES_NOT_EXIST = 1044,
        /// <summary>
        /// 此管理员不存在
        /// </summary>
        [Description("此管理员不存在")]
        ADMIN_NOT_EXIST = 1045,
        /// <summary>
        /// 此管理员账号因违规被超级管理员关闭
        /// </summary>
        [Description("此管理员账号因违规被超级管理员关闭")]
        ADMINISTRATOR_SHUTS_IT_DOWN = 1046,
        /// <summary>
        /// 二维码未失效
        /// </summary>
        [Description("二维码未失效")]
        QR_NO_FAILURE = 1047,
        /// <summary>
        /// 二维码认证中
        /// </summary>
        [Description("二维码认证中")]
        QR_AUTHENTICATING = 1048,
        /// <summary>
        /// 二维码已失效
        /// </summary>
        [Description("二维码已失效")]
        QR_VOKSI = 1049,
        /// <summary>
        /// 大小超过限制
        /// </summary>
        [Description("大小超过限制")]
        SIZE_OUT_OF_LIMIT = 1050,
        /// <summary>
        /// 内容包含敏感词
        /// </summary>
        [Description("内容包含敏感词")]
        CONTENT_CONTAINS_SENSITIVE_WORDS = 1051,

    }
}