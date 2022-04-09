import React from 'react';
import Toast from '../../toast';
import http from '../../../server/instance';
import servicePath from '../../../server/api';
import area_code_list from '../area_code.json';
import encrypt_help from '../../../utils/encrypt_help';
import regular_help from '../../../utils/regular_help';
const { md5_8, md5_16 } = encrypt_help();
const { regularPhone } = regular_help();

class ResetPassword extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            dropdown_selected: 0,
            input_focus: false,
            dropdown_pannel: false,
            deftime: 60,
            area_code: 86,
            mobile: '',
            vcode: '',
            new_password: '',
            button_text: '获取验证码',
            button_state: true,
            login_state: false, //登录状态
        }
    }

    clickable = (val) => {
        if (this.props.clickable) {
            this.props.clickable({ login_method: val })
        }
    }
    input_focus = () => {
        this.setState({ input_focus: true })
    }
    input_blur = () => {
        this.setState({ input_focus: false })
    }
    dropdown_pannel = () => {
        this.setState({ dropdown_pannel: this.state.dropdown_pannel ? false : true })
    }
    dropdown_item_selected(i) {
        this.setState({
            dropdown_selected: i,
            area_code: area_code_list[i].area_code,
            dropdown_pannel: false
        })
    }
    cartoon = (type) => {
        if (this.props.statetype) {
            this.props.statetype({ state: type.target.type })
        }
    }
    cartoon_restore = () => {
        if (this.props.statetype) {
            this.props.statetype({ state: 'normal' })
        }
    }
    inputChange = (e, type) => {
        switch (type) {
            case "mobile": this.setState({ mobile: e.target.value }); break;
            case "vcode": this.setState({ vcode: e.target.value }); break;
            case "new_password": this.setState({ new_password: e.target.value }); break;
            default: break;
        }
    }
    // 校验手机号码格式
    is_mobile = () => {
        if (this.state.mobile !== "" && this.state.mobile !== null && !this.state.area_code) {
            this.input.focus();
            Toast.info("请输入手机号码", 3000);
            return false;
        }
        return regularPhone(this.state.mobile);
    }
    // 获取验证码
    send_vcode = () => {
        if (!this.is_mobile()) return Toast.info("请输入正确的手机号码", 3000);
        let mobile = "+" + this.state.area_code + this.state.mobile;
        const api = servicePath.reset_code_api;
        const start_countdown = this.start_countdown;
        http.get(api, {
            params: {
                account_sdk_source: "web",
                mobile: mobile
            },
            sign: true, token: false
        }, false).then(function (callback) {
            if (callback.data.err_no !== 0) {
                Toast.error(callback.data.err_msg, 3000);
                return;
            }
            start_countdown();
        })
    }
    // 开始倒计时
    start_countdown = () => {
        this.count_down();
        let telephone = this.state.mobile;
        telephone = telephone.substring(0, 3) + '****' + telephone.substring(7);
        Toast.info("验证码已发送至" + telephone, 3000);
    }
    // 倒计时
    count_down = () => {
        if (this.state.deftime > 0) {
            setTimeout(() => {
                let time = this.state.deftime - 1;
                this.setState({ button_text: this.state.deftime + ' 秒后重新发送', deftime: time, button_state: false })
                this.count_down()
            }, 1000)
        } else {
            this.setState({ button_text: '获取验证码', deftime: 60, button_state: true })
        }
    }
    Reset_password = () => {
        switch (true) {
            case (this.state.mobile === ""): Toast.error("请输入手机号", 2500); return;
            case (this.state.vcode === ""): Toast.error("请填写短信验证码", 2500); return;
            case (this.state.new_password === ""): Toast.error("密码长度不能小于6位", 2500); return;
            default: break;
        }
        if (!this.is_mobile()) return Toast.info("请输入正确的手机号码", 3000);
        let obj = {};
        obj.mobile = "+" + this.state.area_code + this.state.mobile;
        obj.code = md5_8(this.state.vcode);
        obj.password = md5_16(this.state.new_password);
        const _this = this;
        this.setState({ login_state: true });
        const api = servicePath.reset_password_api;
        http.post(api, obj, { sign: true, token: false }, false).then(function (callback) {
            if (callback.data.err_no !== 0) {
                Toast.error(callback.data.err_msg, 3000);
                _this.setState({ login_state: false });
                return;
            }
            _this.clickable('password_login');
            Toast.info("密码重置成功，请使用手机号+新密码登录。", 2500);
        })
    }


    render() {
        return (
            <React.Fragment>
                <div className="reset-password-box">
                    <h1 className="login-title">手机重置密码</h1>
                    <div className="input-group">
                        <div className={`login-input-box dropdown-box${this.state.input_focus ? ' focus' : ''}`}
                            onFocus={this.input_focus.bind(this)}
                            onBlur={this.input_blur.bind(this)}>
                            <div className="dropdown-input-container input-dropdown">
                                <div className="dropdown-input-box">
                                    <input autoComplete="off" defaultValue="86" />
                                    <img src={require('../../../assets/images/svg/pull-down-gray.svg').default} alt="" className="arrow"
                                        style={{ transform: (this.state.dropdown_pannel) ? "rotate(180deg)" : "rotate(0deg)" }}
                                        onClick={this.dropdown_pannel.bind(this)} />
                                </div>
                                {this.state.dropdown_pannel ? (
                                    <div className="dropdown-pannel" >
                                        <ul>
                                            {area_code_list.map((item, index) => {
                                                return (
                                                    <li key={index}
                                                        className={`item${index === this.state.dropdown_selected ? ' selected' : ''}`}
                                                        onClick={this.dropdown_item_selected.bind(this, index)}>
                                                        <span>{item.area}</span>
                                                        <span>+{item.area_code}</span>
                                                    </li>
                                                )
                                            })}
                                        </ul>
                                    </div>
                                ) : null}
                            </div>
                            <input
                                name="mobile"
                                autoComplete="off"
                                placeholder="请输入手机号码"
                                className="input number-input"
                                ref={(input) => this.input = input}
                                onFocus={this.cartoon.bind(this)}
                                onBlur={this.cartoon_restore}
                                onChange={(e) => this.inputChange(e, "mobile")}
                            />
                        </div>
                        <div className="login-input-box">
                            <input
                                autoComplete="off"
                                name="registerSmsCode"
                                maxLength="4"
                                placeholder="验证码"
                                className="input"
                                onChange={(e) => this.inputChange(e, "vcode")}
                            />
                            <button className="send-vcode-btn" disabled={(this.state.button_state) ? "" : "disabled"}
                                onClick={() => this.send_vcode()}>
                                {this.state.button_text}
                            </button>
                        </div>
                        <div className="login-input-box">
                            <input name="loginPassword"
                                autoComplete="off"
                                type="password"
                                maxLength="64"
                                placeholder="请输入新密码"
                                className="input"
                                onFocus={this.cartoon.bind(this)}
                                onBlur={this.cartoon_restore}
                                onChange={(e) => this.inputChange(e, "new_password")}
                            />
                        </div>
                    </div>
                    <button className="btn"
                        disabled={this.state.login_state ? "disabled" : ""}
                        onClick={() => this.Reset_password()}>
                        修改
                    </button>
                    <div className="prompt-box">
                        <span className="clickable"
                            onClick={this.clickable.bind(this, 'password_login')}>
                            账密登录
                        </span>
                    </div>
                </div>
            </React.Fragment>
        )
    }
}

export default ResetPassword;