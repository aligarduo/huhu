import React from 'react';
import browser_help from '../../utils/browser_help';
const { ScrollTop } = browser_help();

const open = () => {
    //判断当前为哪个客户端
    let browser = {
        versions: function () {
            let u = navigator.userAgent;
            return {
                trident: u.indexOf('Trident') > -1, //IE内核
                presto: u.indexOf('Presto') > -1, //opera内核
                webKit: u.indexOf('AppleWebKit') > -1, //苹果、谷歌内核
                gecko: u.indexOf('Gecko') > -1 && u.indexOf('KHTML') === -1,//火狐内核
                mobile: !!u.match(/AppleWebKit.*Mobile.*/), //是否为移动终端
                ios: !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/), //ios终端
                android: u.indexOf('Android') > -1 || u.indexOf('Adr') > -1, //android终端
                iPhone: u.indexOf('iPhone') > -1, //是否为iPhone或者QQHD浏览器
                iPad: u.indexOf('iPad') > -1, //是否iPad
                webApp: u.indexOf('Safari') === -1, //是否web应该程序，没有头部与底部
                weixin: u.indexOf('MicroMessenger') > -1, //是否微信 （2015-01-22新增）
                qq: u.match(/\sQQ/i) === " qq" //是否QQ
            };
        }(),
        language: (navigator.browserLanguage || navigator.language).toLowerCase()
    };

    let appInfo = {
        skip_open: 'huhu://',
        skip_download: '/app',
    };

    if (browser.versions.ios) {
        window.location.href = appInfo.skip_open;
        setTimeout(function () {
            window.location.href = appInfo.skip_download;
        }, 1000);
    } else if (browser.versions.android) {
        window.location.href = appInfo.skip_open;
        setTimeout(function () {
            window.location.href = appInfo.skip_download;
        }, 1000);
    }
}

const is_equipment = () => {
    const agents = [
        'Android',
        'iPhone',
        'iPad',
        'iPod',
    ];
    for (let i = 0; i < agents.length; i++) {
        if (navigator.userAgent.match(agents[i])) {
            return true;
        }
    }
    return false;
}

class OpenApp extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            states: true,
        }
    }
    componentDidMount() {
        //PC端
        window.addEventListener("scroll", () => { this.init() });
        //移动端
        window.addEventListener('touchmove', () => { this.init() });
    }
    init = () => {
        if (ScrollTop() >= 60) {
            this.setState({ states: false })
        } else {
            this.setState({ states: true })
        }
    }
    render() {
        return (
            <React.Fragment>
                {is_equipment() ? (
                    <div className={`open-button app-open-button ${this.state.states ? 'show' : 'hide'}`} onClick={open}>APP内打开</div>
                ) : null}
            </React.Fragment>
        )
    }
}

export default OpenApp;