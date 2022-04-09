import React from 'react';
import Toast from '../../toast';
import http from '../../../server/instance';
import servicePath from '../../../server/api';
import area_code_list from '../area_code.json';
import encrypt_help from '../../../utils/encrypt_help';
const { md5_8 } = encrypt_help();

class PhoneLogin extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            dropdown_selected: 0,
            input_focus: false,
            dropdown_pannel: false,
            mobile: null,
            vcode: null,
            area_code: 86,
            deftime: 60,
            button_text: '获取验证码',
            button_state: true,
            login_state: false, //登录状态
        }
    }

    componentDidMount() {
        this.input.focus(); //初始化输入焦点定位到输入框
    }
    // 切换登录方式
    clickable = () => {
        if (this.props.clickable) {
            this.props.clickable({ login_method: "password_login" });
        }
    }
    // 输入框获得焦点
    input_focus = () => {
        this.setState({ input_focus: true });
    }
    // 输入框失去焦点
    input_blur = () => {
        this.setState({ input_focus: false });
    }
    // 区号下拉框
    dropdown_pannel = () => {
        this.setState({ dropdown_pannel: this.state.dropdown_pannel ? false : true });
    }
    // 选中区号
    dropdown_item_selected = (i) => {
        this.setState({
            dropdown_selected: i,
            area_code: area_code_list[i].area_code,
            dropdown_pannel: false
        })
    }
    // 卡通改变
    cartoon = (type) => {
        if (this.props.statetype) {
            this.props.statetype({ state: type.target.type });
        }
    }
    // 卡通改变
    cartoon_restore = () => {
        if (this.props.statetype) {
            this.props.statetype({ state: 'normal' });
        }
    }
    // 手机号码输入完成
    inputChange = (e) => {
        this.setState({ mobile: e.target.value });
    }
    // 验证码输入完成
    input_vcode_Change = (e) => {
        this.setState({ vcode: e.target.value });
    }
    // 校验手机号码格式
    is_mobile = () => {
        if (this.state.mobile !== "" && this.state.mobile !== null && !this.state.area_code) {
            this.input.focus();
            Toast.error("请输入手机号码", 2500);
            return false;
        }
        let regex = /^((\+)?86|((\+)?86)?)0?1[23456789]\d{9}$/;
        if (!regex.test(this.state.mobile)) {
            Toast.error("请输入正确的手机号码", 2500);
            return false;
        }
        return true;
    }
    // 获取验证码
    send_vcode = () => {
        if (!this.is_mobile()) return;
        let mobile = "+" + this.state.area_code + this.state.mobile;
        const api = servicePath.msg_code_api;
        const start_countdown = this.start_countdown;
        http.get(api, {
            params: {
                account_sdk_source: "web",
                mobile: mobile
            },
            sign: true,
            token: false
        }, false).then(function (callback) {
            if (callback.data.err_no !== 0) {
                Toast.error(callback.data.err_msg, 2500);
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
        Toast.info("验证码已发送至" + telephone, 2500);
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
    // 提交数据，开始登录
    start_login = () => {
        if (!this.is_mobile()) return;
        if (this.state.vcode === "" || this.state.vcode === null) {
            Toast.error("请输入验证码", 2500);
            return;
        }
        let mobiles = "+" + this.state.area_code + this.state.mobile;
        const api = servicePath.login_api;
        const _this = this;
        this.setState({ login_state: true });
        http.post(api, {
            mobile: mobiles,
            code: md5_8(this.state.vcode),
        }, { sign: true, token: false }, false).then(function (callback) {
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
                <h1 className="login-title" >手机登录</h1>
                <div className="input-group" >
                    <div className={`login-input-box dropdown-box${this.state.input_focus ? ' focus' : ''}`}
                        onFocus={this.input_focus.bind(this)}
                        onBlur={this.input_blur.bind(this)} >
                        <div className="dropdown-input-container input-dropdown" >
                            <div className="dropdown-input-box" >
                                <input autoComplete="off"
                                    readOnly="readonly"
                                    key={this.state.area_code}
                                    defaultValue={this.state.area_code}
                                    onFocus={() => { this.setState({ dropdown_pannel: true }) }} />
                                <img src={require('../../../assets/images/svg/pull-down-gray.svg').default}
                                    alt=""
                                    className="arrow"
                                    style={{ transform: (this.state.dropdown_pannel) ? "rotate(180deg)" : "rotate(0deg)" }}
                                    onClick={this.dropdown_pannel.bind(this)}
                                />
                            </div>
                            {this.state.dropdown_pannel ?
                                (<div className="dropdown-pannel" >
                                    <ul > {
                                        area_code_list.map((item, index) => {
                                            return (
                                                <li key={index}
                                                    className={`item${index === this.state.dropdown_selected ? ' selected' : ''}`}
                                                    onClick={this.dropdown_item_selected.bind(this, index)} >
                                                    <span>{item.area} </span><span >+{item.area_code}</span>
                                                </li>
                                            )
                                        })
                                    } </ul>
                                </div>
                                ) : (undefined)}
                        </div>
                        <input name="mobile"
                            autoComplete="off"
                            placeholder="请输入手机号码"
                            className="input number-input"
                            ref={(input) => this.input = input}
                            onFocus={this.cartoon.bind(this)}
                            onBlur={this.cartoon_restore}
                            onChange={(e) => this.inputChange(e)} />
                    </div>
                    <div className="login-input-box" >
                        <input autoComplete="off"
                            name="registerSmsCode"
                            maxLength="4"
                            placeholder="验证码"
                            className="input"
                            onChange={(e) => this.input_vcode_Change(e)} />
                        <button className="send-vcode-btn" disabled={(this.state.button_state) ? "" : "disabled"}
                            onClick={this.send_vcode} >
                            {this.state.button_text}
                        </button>
                    </div>
                </div>
                <button className="btn"
                    disabled={this.state.login_state ? "disabled" : ""}
                    onClick={this.start_login.bind(this)} >
                    登录
                </button>
                <div className="prompt-box" >
                    <span className="clickable" onClick={this.clickable} >更多登录方式</span>
                </div>
            </React.Fragment>
        )
    }
}

export default PhoneLogin;