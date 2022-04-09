import onlysign_help from '../utils/onlysign_help';
const { snowflake } = onlysign_help();

const ip = "https://api.huhu.chat/";
// const ip = "https://localhost:44385/";
// const ip = "http://127.0.0.1:44385/";
const aid = 2608;
const suffix = `?aid=${aid}&uuid=${snowflake()}`;

const servicePath = {
    recommend_api__recommend_all_feed: ip + "recommend_api/v1/article/recommend_all_feed" + suffix, //全部文章推荐
    msg_code_api: ip + "passport/web/msg_code", //获取登录验证码
    reset_code_api: ip + "passport/web/reset_code", //获取更改密码验证码
    login_api: ip + "passport/web/sms_login" + suffix, //手机验证码登录
    cipher_login_api: ip + "passport/web/cipher_login" + suffix, //账号密码登录
    reset_password_api: ip + "passport/web/reset_password" + suffix, //重置密码
    userinfo_api: ip + "user_api/v1/user/get" + suffix, //用户个人基本信息
    logout_api: ip + "passport/web/logout", //用户个退出登录
    content_api__detail: ip + "content_api/v1/article/detail" + suffix, //文章详细
    interact_api__digg_save_api: ip + "interact_api/v1/digg/save" + suffix, //点赞
    interact_api__digg_cancel_api: ip + "interact_api/v1/digg/cancel" + suffix, //取消点赞
    interact_api__follow_do: ip + "interact_api/v1/follow/do" + suffix, //关注作者
    interact_api__follow_undo: ip + "interact_api/v1/follow/undo" + suffix, //取消关注作者
    interact_api__comment_list: ip + "interact_api/v1/comment/list" + suffix, //文章评论
    tag_api__query_tag_list: ip + "tag_api/v1/tag/query_tag_list" + suffix, //文章标签

    get_imgurl_api__original_wmark: ip + "image/get_img_url?method=original&wmark=true", //处理图片-原图加水印
    get_imgurl_api__original_nowmark: ip + "image/get_img_url?method=original&wmark=false", //处理图片-原图不加水印
    get_imgurl_api__cover: ip + "image/get_img_url?method=cover&wmark=false", //处理图片-封面
    get_imgurl_api__profile: ip + "image/get_img_url?method=profile&wmark=false", //处理图片-头像

    content_api__article_draft_create: ip + "content_api/v1/article_draft/create" + suffix, //新增草稿
    content_api__article_publish: ip + "content_api/v1/article/publish" + suffix, //发布文章
    content_api__article_editor: ip + "content_api/v1/article/editor" + suffix, //编辑校验-用户发布后更新文章
    content_api__article_draft_query_list: ip + "content_api/v1/article_draft/query_list" + suffix, //草稿列表
    content_api__article_draft_delete: ip + "content_api/v1/article_draft/delete" + suffix, //删除草稿
    content_api__article_draft_update: ip + "content_api/v1/article_draft/update" + suffix, //更新草稿
    content_api__article_draft_detail: ip + "content_api/v1/article_draft/detail" + suffix, //查看草稿详细
    user_api__get_author_info: ip + "user_api/v1/user/get" + suffix, //作者信息
    content_api__article_query_list: ip + "content_api/v1/article/query_list" + suffix, //获取用户发布的文章
    content_api__query_adverts: ip + "content_api/v1/advert/query_adverts" + suffix, //广告
    content_api__query_adverts_id: ip + "content_api/v1/advert/query" + suffix, //广告
    content_api__article_hot: ip + "content_api/v1/article/hot" + suffix, //热榜
    content_api__article_update: ip + "content_api/v1/article/update" + suffix, //更新文章
    content_api__article_delete: ip + "content_api/v1/article/delete" + suffix, //删除文章
    interact_api__collection_list: ip + "interact_api/v1/collection/list" + suffix, //收藏集列表
    interact_api__collection_add: ip + "interact_api/v1/collection/add" + suffix, //创建收藏集
    interact_api__collect_add: ip + "interact_api/v1/collect/add" + suffix, //加入收藏
    interact_api__collect_delete: ip + "interact_api/v1/collect/add" + suffix, //取消收藏
    interact_api__comment_publish: ip + "interact_api/v1/comment/publish" + suffix, //发表评论
    interact_api__comment_delete: ip + "interact_api/v1/comment/delete" + suffix, //删除评论
    search_api__search: ip + "search_api/v1/search" + suffix, //搜索
    user_api__author_ranking: ip + "user_api/v1/author/ranking" + suffix, //排行榜
    user_api__user_update: ip + "user_api/v1/user/update" + suffix, //更新个人信息
}

export default servicePath;