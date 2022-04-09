import React from 'react';
import { connect } from 'react-redux'

class Account extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            popup: false,
            popup_warm: 0,
            popup_warm_content: [
                '目前PC端暂不支持换绑功能，如您申请更换手机绑定，请前往乎乎app 我-设置-更换手机绑定进行换绑操作，给您带来的不便，我们深感抱歉。如果您有任何建议和反馈，可以发送邮箱到 2966994290@qq.com 联系我们。',
                '目前PC端暂不支持重置功能，如您申请密码重置，请前往乎乎app 我-设置-密码重置进行重置操作，给您带来的不便，我们深感抱歉。如果您有任何建议和反馈，可以发送邮箱到 2966994290@qq.com 联系我们。',
                '目前PC端暂不支持注销功能，如您申请账号注销，请前往乎乎app 我-设置-账号注销进行注销操作，给您带来的不便，我们深感抱歉。如果您有任何建议和反馈，可以发送邮箱到 2966994290@qq.com 联系我们。',
            ]
        }
    }

    warm = (select) => {
        this.setState({ popup: !this.state.popup, popup_warm: select });
    }

    render() {
        const { userinfo } = this.props;
        return (
            <div className="setting-account-view">
                <div className="nav-text">账号设置</div>
                <ul className="setting-list">
                    <li className="setting-item">
                        <span className="setting-title">手机</span>
                        <div className="setting-input-box">
                            <span className="empty-account">{userinfo.phone}</span>
                            <div className="setting-action-box">
                                <button className="setting-btn" onClick={()=>this.warm(0)}>换绑</button>
                            </div>
                        </div>
                    </li>
                    <li className="setting-item">
                        <span className="setting-title">密码</span>
                        <div className="setting-input-box">
                            <span className="empty-account"></span>
                            <div className="setting-action-box">
                                <button className="setting-btn" onClick={()=>this.warm(1)}>重置</button>
                            </div>
                        </div>
                    </li>
                    <li className="setting-item">
                        <span className="setting-title">账号注销</span>
                        <div className="setting-input-box">
                            <span className="empty-account"></span>
                            <div className="setting-action-box">
                                <button className="setting-btn" onClick={()=>this.warm(2)}>注销</button>
                            </div>
                        </div>
                    </li>
                </ul>

                {this.state.popup ? (
                    <div className="account-delete-modal-box">
                        <div className="logout-panel">
                            <div className="warm-title">温馨提示</div>
                            <div className="content">
                                <div className="content-text">
                                    {this.state.popup_warm_content[this.state.popup_warm]}
                                </div>
                                <button onClick={()=>this.warm(0)}>我知道了</button>
                            </div>
                        </div>
                    </div>
                ) : (undefined)}

            </div>
        )
    }
}

const mapStateToProps = (state) => {
    return {
        userinfo: state.userinfo,
    }
}
export default connect(mapStateToProps)(Account);