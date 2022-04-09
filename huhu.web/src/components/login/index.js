import './login.css';
import React from 'react';
import ReactDOM from 'react-dom';
import Cartoon from './module/cartoon';
import PhoneLogin from './module/phonelogin';
import PassWordLogin from './module/passwordlogin';
import ResetPassWord from './module/resetpassword';
import ThirdPartyLogin from './module/thirdpartylogin';
import RichScan from './module/richscan';

class index extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            login_method: "phone_login",  //登录类型
            cartoon_state: "normal",  //卡通动画
            oauth: false,  //第三方登录
            oauth_title: null, //第三方登录标题
        }
    }

    //关闭登录窗口
    close_login_box = () => {
        const div = document.getElementsByClassName("user-permissions")[0];
        ReactDOM.unmountComponentAtNode(div);
        document.body.style.overflow = ''; //恢复滚动条
    }

    // 切换登录方式
    clickable = (comment) => {
        this.setState({ login_method: comment.login_method });
    }

    // 切换第三方登录方式
    oauth_clickable = (val) => {
        this.setState({ login_method: val.login_method, oauth_title: val.oauth_title });
    }

    // 切换卡通
    cartoon = (type) => {
        switch (type.state) {
            case 'text': this.setState({ cartoon_state: 'greeting' }); break;
            case 'password': this.setState({ cartoon_state: 'blindfold' }); break;
            case 'normal': this.setState({ cartoon_state: 'normal' }); break;
            default: break;
        }
    }

    render() {
        return (
            <div className="auth-modal-box">
                <div className="auth-form">
                    <Cartoon state={this.state.cartoon_state} />
                    <i title="关闭"
                        className="close-btn-g ion-close-round"
                        onClick={this.close_login_box}>
                    </i>
                    <div className="login-panel">
                        {(() => {
                            switch (this.state.login_method) {
                                case 'phone_login':
                                    return <PhoneLogin clickable={this.clickable} statetype={this.cartoon} />;
                                case 'password_login':
                                    return <PassWordLogin clickable={this.clickable} statetype={this.cartoon} />;
                                case 'reset_password':
                                    return <ResetPassWord clickable={this.clickable} statetype={this.cartoon} />;
                                case 'richscan':
                                    return <RichScan clickable={this.clickable} statetype={this.state.oauth_title} />;
                                default: return undefined;
                            }
                        })()}
                    </div>
                    <ThirdPartyLogin method={this.state.login_method} oauth_clickable={(val) => this.oauth_clickable(val)} />
                    <div className="agreement-box">
                        注册登录即表示同意
                        <a href="/term/huhu-terms" target="_blank" rel="noopener noreferrer"> 用户协议</a>
                        、
                        <a href="/term/privacy" target="_blank" rel="noopener noreferrer">隐私政策</a>
                    </div>
                </div>
            </div>
        )
    }
}

export default index;