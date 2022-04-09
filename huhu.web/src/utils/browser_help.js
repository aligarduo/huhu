/**
 * 滚动条距离顶部距离
 * @returns 滚动条距离顶部距离
 */
const ScrollTop = () => {
    var scrollTop = 0, bodyScrollTop = 0, documentScrollTop = 0;
    if (document.body) {
        bodyScrollTop = document.body.scrollTop;
    }
    if (document.documentElement) {
        documentScrollTop = document.documentElement.scrollTop;
    }
    scrollTop = (bodyScrollTop - documentScrollTop > 0) ? bodyScrollTop : documentScrollTop;
    return scrollTop;
}

/**
 * 滚动条高度
 * @returns 滚动条高度
 */
const ScrollHeight = () => {
    var scrollHeight = 0, bodyScrollHeight = 0, documentScrollHeight = 0;
    if (document.body) {
        bodyScrollHeight = document.body.scrollHeight;
    }

    if (document.documentElement) {
        documentScrollHeight = document.documentElement.scrollHeight;
    }
    scrollHeight = (bodyScrollHeight - documentScrollHeight > 0) ? bodyScrollHeight : documentScrollHeight;
    return scrollHeight;
}

/**
 * 可视化窗口高度
 * @returns 可视化窗口高度
 */
 const WindowHeight = () => {
    var windowHeight = 0;
    if (document.compatMode === "CSS1Compat") {
        windowHeight = document.documentElement.clientHeight;
    } else {
        windowHeight = document.body.clientHeight;
    }
    return windowHeight;
}

/**
 * 上拉加载
 * @param {Number} drift 偏移量
 * @param {Function} callback 回调函数
 */
const PullOnLoading = (drift, callback) => {
    if (ScrollHeight() <= WindowHeight() + ScrollTop() + drift) {
        if (typeof callback === "function") {
            callback();
        } else {
            console.log("第二个参数要是一个函数");
        }
    }
}

const browser_help = () => {
    return {
        ScrollTop,
        ScrollHeight,
        WindowHeight,
        PullOnLoading,
    }
}
export default browser_help;