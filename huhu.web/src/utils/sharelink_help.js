/**
 * 分享新浪微博
 * @param {[type]} title [分享标题]
 * @param {[type]} url   [分享url链接，默认当前页面]
 * @param {[type]} pic   [分享图片]
 */
const share_weibo = (title, url, pic) => {
    let param = {
        url: url || window.location.href,
        type: '3',
        count: '1', //是否显示分享数，1显示(可选)
        appkey: '', //申请的应用appkey,显示分享来源(可选)
        title: title || '', //分享的文字内容(可选，默认为所在页面的title)
        pic: pic || '', //分享图片的路径(可选)
        ralateUid: '', //关联用户的UID，分享微博会@该用户(可选)
        rnd: new Date().valueOf()
    }
    let temp = [];
    for (var p in param) {
        temp.push(p + '=' + encodeURIComponent(param[p] || ''))
    }
    let targetUrl = 'http://service.weibo.com/share/share.php?' + temp.join('&');
    window.open(targetUrl, 'sinaweibo', 'height=520, width=720');
}

/**
 * 
 * @param {[type]} url   [分享url链接，默认当前页面链接]
 * @param {[type]} title [分享标题]
 * @param {[type]} pic   [分享图片]
 */
const share_qq = (url, title, pic) => {
    let param = {
        url: url || window.location.href,
        desc: '', //分享理由
        title: title || '', //分享标题(可选)
        summary: '', //分享描述(可选)
        pics: pic || '', //分享图片(可选)
        flash: '', //视频地址(可选)
        site: '' //分享来源 (可选)
    };
    let s = [];
    for (var i in param) {
        s.push(i + '=' + encodeURIComponent(param[i] || ''));
    }
    let targetUrl = "http://connect.qq.com/widget/shareqq/iframe_index.html?" + s.join('&');
    window.open(targetUrl, 'qq', 'height=520, width=720');
}

/**
 * 
 * @param {[type]} url   [分享url链接，默认当前页面链接]
 */
const share_weixin = (url) => {
    let u = url || window.location.href;
    let targetUrl = '/share_weixin?url=' + u;
    window.open(targetUrl, 'weixin', 'height=520, width=720');
}

const sharelink_help = () => {
    return {
        share_weibo,
        share_qq,
        share_weixin,
    }
}
export default sharelink_help;