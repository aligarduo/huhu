import React from 'react';

class ThirdPartyLogin extends React.Component {
    constructor(props) {
        super(props);
        this.state = {}
    }

    clickable = (type, val) => {
        if (this.props.oauth_clickable) {
            this.props.oauth_clickable({ login_method: type, oauth_title: val })
        }
    }

    QQ_Login = () => {
        let url = "https://graph.qq.com/oauth2.0/authorize";
        let params = "?";
        params += "response_type=code&";  //获取access_token
        params += "client_id=101995093&"; //应用appid
        //回调地址
        //本地调试
        //params += "redirect_uri=http://3c875j1492.wicp.vip/passport/thirdparty/qq_auth&";
        //服务器
        params += "redirect_uri=https://api.huhu.chat/passport/thirdparty/qq_auth&";
        params += "state=2068";
        window.open(url + params, 'QQ', 'height=520, width=720');
    }

    componentDidMount = () => {
        window.addEventListener("message", (e) => {
            let token = e.data;
            if (token && token !== "") {
                localStorage.setItem('passport_csrf_token', token);
                window.location.reload();
            }
        });
    }


    render() {
        return (
            <div className="oauth-box" style={{ display: (this.props.method === 'password_login') ? 'block' : 'none' }} >
                <div className="oauth">
                    <div className="oauth-bg" onClick={() => this.QQ_Login()}>
                        <img title="QQ" alt="QQ" className="oauth-btn" src={require('../../../assets/images/svg/qq-icon.svg').default} />
                    </div>
                    <div className="oauth-bg" onClick={() => this.clickable('richscan', '微信')}>
                        <img title="微信" alt="微信" className="oauth-btn" src={require('../../../assets/images/svg/wechat-icon.svg').default} />
                    </div>
                    <div className="oauth-bg">
                        <img title="微博" alt="微博" className="oauth-btn" src={require('../../../assets/images/svg/weibo-icon.svg').default} />
                    </div>
                    {/* <div className="oauth-bg" onClick={() => this.clickable('richscan', '乎乎APP')}>
                        <img title="乎乎APP扫一扫" alt="乎乎APP扫一扫" className="oauth-btn" src={require('../../../assets/images/svg/richscan.svg').default} />
                    </div> */}
                </div>
            </div>
        )
    }
}

export default ThirdPartyLogin;