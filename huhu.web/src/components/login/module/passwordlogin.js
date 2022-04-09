import React from 'react';
import Toast from '../../toast';
import http from '../../../server/instance';
import servicePath from '../../../server/api';
import encrypt_help from '../../../utils/encrypt_help';
const { md5_16 } = encrypt_help();

class PasswordLogin extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            account: "", //账号
            password: "", //密码
            login_state: false, //登录状态
        }
    }

    //切换登录方式
    clickable = (val) => {
        if (this.props.clickable) {
            this.props.clickable({ login_method: val })
        }
    }
    //切换卡通状态
    cartoon = (type) => {
        if (this.props.statetype) {
            this.props.statetype({ state: type.target.type })
        }
    }
    //还原卡通状态
    cartoon_restore = () => {
        if (this.props.statetype) {
            this.props.statetype({ state: 'normal' })
        }
    }
    //输入事件
    inputChange = (e, type) => {
        switch (type) {
            case "account": this.setState({ account: e.target.value }); break;
            case "password": this.setState({ password: e.target.value }); break;
            default: break;
        }
    }
    //点击登录
    login = () => {
        let obj = {};
        obj.account = this.state.account;
        obj.password = md5_16(this.state.password);
        switch (true) {
            case (this.state.account.length === 0): Toast.error("账号不为空", 2500); return;
            case (this.state.password.length === 0): Toast.error("密码不能为空", 2500); return;
            default: break;
        }
        const _this = this;
        this.setState({ login_state: true });
        const api = servicePath.cipher_login_api;
        http.post(api, obj, { sign: true, token: false }, false).then(function (callback) {
            if (callback.data.err_no !== 0) {
                Toast.error(callback.data.err_msg, 2500);
                _this.setState({ login_state: false });
                return;
            }
            localStorage.setItem('passport_csrf_token', callback.data.token);
            window.location.reload()
        })
    }


    render() {
        return (
            <React.Fragment>
                <h1 className="login-title">账密登录</h1>
                <div className="input-group">
                    <div className="login-input-box">
                        <input name="loginPhoneOrEmail" maxLength="64" autoComplete="off" placeholder="邮箱/手机号（国际号码加区号）"
                            className="input account-input"
                            onFocus={this.cartoon.bind(this)}
                            onBlur={this.cartoon_restore}
                            onChange={(e) => this.inputChange(e, "account")}
                        />
                    </div>
                    <div className="login-input-box">
                        <input name="loginPassword" autoComplete="off" type="password" maxLength="64" placeholder="请输入密码" className="input"
                            onFocus={this.cartoon.bind(this)}
                            onBlur={this.cartoon_restore}
                            onChange={(e) => this.inputChange(e, "password")}
                        />
                    </div>
                </div>
                <button className="btn" disabled={this.state.login_state ? "disabled" : ""} onClick={() => this.login()}>{this.state.login_state ? "登录中..." : "登录"}</button>
                <div className="prompt-box">
                    <span className="clickable"
                        onClick={this.clickable.bind(this, 'phone_login')}>
                        手机登录
                    </span>
                    <span className="right clickable"
                        onClick={this.clickable.bind(this, 'reset_password')}>
                        忘记密码
                    </span>
                </div>
            </React.Fragment>
        )
    }
}

export default PasswordLogin;