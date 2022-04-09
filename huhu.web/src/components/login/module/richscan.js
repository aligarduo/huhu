import React from 'react'
import QRCode from 'qrcode.react';
import http from '../../../server/instance';
import Toast from '../../../components/toast';

class RichScan extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            Verify_Status: 1, //二维码验证状态
            QRCode_UUID: 0, //UUID
            QRCode_URL: 'https://www.huhu.chat/app?uuid=', //二维码跳转地址
            set_timeout: null,
        }
    }
    componentDidMount() {
        this.init();
    }
    componentWillUnmount() {
        clearTimeout(this.state.set_timeout)
    }
    //初始化
    init = () => {
        const _this = this;
        const api = "https://api.huhu.chat/passport/web/richscan";
        http.get(api, {
            params: {},
            sign: false,
            token: false
        }, false).then(function (callback) {
            if (callback.data.err_no !== 0) {
                Toast.error('网络繁忙，请稍后重试', 3000);
                return;
            }
            _this.setState({
                Verify_Status: 2,
                QRCode_UUID: callback.data.data.uuid,
            }, () => {
                _this.RefreshStatus();
            });
        })
    }
    //刷新状态
    RefreshStatus = () => {
        const api = "https://api.huhu.chat/passport/web/richscan_connect";
        http.get(api, {
            params: { uuid: this.state.QRCode_UUID },
            sign: false,
            token: false
        }, false).then(function (callback) {
            if (callback.data.err_no !== 0) {
                Toast.error('网络繁忙，请稍后重试', 3000);
                return;
            }
        })
        this.setState({
            set_timeout: setTimeout(() => {
                this.RefreshStatus();
            }, 2000)
        })
    }
    //切换登录方式
    clickable = () => {
        if (this.props.clickable) {
            this.props.clickable({ login_method: "password_login" });
        }
    }
    render() {
        return (
            <div className='richscan'>
                <div title='返回' className='getback' onClick={() => this.clickable()}>
                    <svg t="1639374389537" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="3772" width="24" height="24"><path d="M224.32 505.6a31.936 31.936 0 0 1 10.88-19.84l222.08-222.08a32 32 0 0 1 45.12 45.12l-169.28 169.28H768a32 32 0 0 1 0 64H333.12l169.28 169.28a32 32 0 1 1-45.12 45.44l-224-224a31.968 31.968 0 0 1-8.96-27.2z" p-id="3773" fill="#a8a8a8"></path></svg>
                </div>
                <div className="qr-box">
                    {(() => {
                        switch (this.state.Verify_Status) {
                            case 1: return (
                                <div className="pswp_preloader_icn">
                                    <div className="pswp_preloader_cut">
                                        <div className="pswp_preloader_donut"></div>
                                    </div>
                                </div>
                            )
                            case 2: return (
                                <QRCode
                                    value={this.state.QRCode_URL}
                                    size={160}
                                    fgColor={this.props.statetype === "微信" ? "#24DB5A" : "#1e80ff"}
                                    bgColor="#ffffff"
                                    renderAs="canvas"
                                    className="qr"
                                />
                            )
                            case 3: return (
                                <div className='waiting'>
                                    <svg t="1639409927665" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="2340" width="84" height="84"><path d="M512 85.333333c235.648 0 426.666667 191.018667 426.666667 426.666667s-191.018667 426.666667-426.666667 426.666667S85.333333 747.648 85.333333 512 276.352 85.333333 512 85.333333z m-74.965333 550.4L346.453333 545.152a42.666667 42.666667 0 1 0-60.330666 60.330667l120.704 120.704a42.666667 42.666667 0 0 0 60.330666 0l301.653334-301.696a42.666667 42.666667 0 1 0-60.288-60.330667l-271.530667 271.488z" fill="#52C41A" p-id="2341"></path></svg>
                                </div>
                            )
                            default: break;
                        }
                    })()}
                </div>
                {this.state.Verify_Status === 3 ? (
                    <div className='hint'>扫码成功，请在手机上确认</div>
                ) : (
                    <div className='hint'>请使用「{this.props.statetype}」扫一扫登录</div>
                )}
            </div>
        )
    }
}
export default RichScan;