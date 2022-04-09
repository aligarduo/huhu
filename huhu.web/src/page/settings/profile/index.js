import React from 'react';
import { connect } from 'react-redux';
import store from '../../../redux/index';
import http from '../../../server/instance';
import servicePath from '../../../server/api';
import Toast from '../../../components/toast';

const up_input = React.createRef();
class Profile extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            flag: true,
            user_name: '',
            avatar_large: '',
            job_title: '',
            is_saveing: false, //保存中
        }
    }

    componentDidUpdate() {
        this.init();
    }

    init = () => {
        const { userinfo } = this.props;
        if (this.state.flag) {
            this.setState({
                flag: false,
                user_name: userinfo.user_name,
                avatar_large: userinfo.avatar_large,
                job_title: userinfo.job_title,
            })
        }
    }

    up_user_pic = () => {
        let mainBody = document.getElementsByClassName("settings_user_pic")[0];
        mainBody.click();
    }

    handleFile = () => {
        let cover = document.getElementsByClassName("settings_user_pic")[0];
        let files = cover.files;
        if (!files.length) return;
        let formData = new FormData();
        formData.append('cover', files[0], files[0].name);
        up_input.current.value = "";
        const _this = this;
        const Url = servicePath.get_imgurl_api__profile;
        http.post(
            Url,
            formData, {
            headers: { sign: false }
        }, true).then(function (callback_data) {
            if (callback_data.data.err_no !== 0) {
                Toast.info(callback_data.data.err_msg, 2500);
            }
            _this.setState(state => ({
                avatar_large: state.avatar_large = callback_data.data.data.main_url,
            }))
        }).catch(function (error) {
            alert(error)
        });
    }

    Save_changes = () => {
        let obj = {};
        obj.user_name = this.state.user_name;
        obj.avatar_large = this.state.avatar_large;
        obj.job_title = this.state.job_title;
        const _this = this;
        _this.setState({ is_saveing: true });
        const api = servicePath.user_api__user_update;
        http.post(api, obj, { sign: true, token: true }, false).then(function (callback) {
            if (callback.data.err_no !== 0) {
                Toast.info(callback.data.err_msg, 2500);
                _this.setState({ is_saveing: false });
                return;
            }
            store.dispatch({
                type: "USERINFO",
                userinfo: callback.data.data
            })
            _this.setState({ is_saveing: false });
            Toast.info('保存成功', 2500);
        })
    }

    render() {
        return (
            <div className="setting-profile-view">
                <div className="nav-text">个人资料</div>
                <div className="user-infos">
                    <div className="info-input">
                        <form className="form byte-form byte-form--label-right">
                            <div className="divide"></div>
                            <div className="byte-form-item">
                                <label className="byte-form-item__label _byte-item">用户名</label>
                                <div className="byte-form-item__content">
                                    <div className="byte-input byte-input--normal byte-input--suffixed">
                                        <input type="text"
                                            spellCheck="false"
                                            maxLength="20"
                                            className="byte-input__input byte-input__input--normal"
                                            defaultValue={this.state.user_name}
                                            onChange={(e) => this.setState({ user_name: e.target.value })}
                                        />
                                        <span className="byte-input__suffix">
                                            <span className="suffix">{this.state.user_name.length}/20</span>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div className="divide"></div>
                            <div className="byte-form-item">
                                <label className="byte-form-item__label">个性签名</label>
                                <div className="byte-form-item__content">
                                    <div className="textarea-input">
                                        <div className="textarea byte-input byte-input--normal">
                                            <textarea
                                                maxLength="200"
                                                rows="2"
                                                spellCheck="false"
                                                className="byte-input__textarea_se"
                                                defaultValue={this.state.job_title}
                                                onChange={(e) => this.setState({ job_title: e.target.value })}>
                                            </textarea>
                                            <span className="textarea-suffix">{this.state.job_title.length}/100</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </form>
                        <div className="divide"></div>
                        <button className="ui-btn save-btn primary large default"
                           disabled={this.state.is_saveing ? "disabled" : null}
                            onClick={() => this.Save_changes()}>
                            保存修改
                        </button>
                    </div>
                    <div className="avatar-input">
                        <div className="avatar-info">
                            <div className="setting-title h5-only">我的头像</div>
                            <div className="avatar-uploader uploader">
                                <div className="click-cover" onClick={() => this.up_user_pic()}>
                                    <i className="add-icon byte-icon byte-icon--plus-circle">
                                        <svg t="1561635709826" className="icon" viewBox="0 0 1024 1024" version="1.1" p-id="375017"><path d="M464.883436 464.883436V302.244035A23.732788 23.732788 0 0 1 488.616224 279.209271h46.069529a23.732788 23.732788 0 0 1 23.732788 23.034764v162.639401h162.6394a23.732788 23.732788 0 0 1 23.034765 23.732788v46.069529a23.732788 23.732788 0 0 1-23.034765 23.732788H558.418541v162.6394a23.732788 23.732788 0 0 1-23.732788 23.034765H488.616224a23.732788 23.732788 0 0 1-23.732788-23.034765V558.418541H302.244035A23.732788 23.732788 0 0 1 279.209271 534.685753V488.616224a23.732788 23.732788 0 0 1 23.034764-23.732788z m46.767552 465.581458a418.813906 418.813906 0 1 0-418.813906-418.813906 418.813906 418.813906 0 0 0 418.813906 418.813906z m0 92.837083a511.650988 511.650988 0 1 1 511.650989-511.650989 511.650988 511.650988 0 0 1-511.650989 511.650989z" p-id="375018"></path></svg>
                                    </i>
                                    <div className="click-text">点击修改头像</div>
                                </div>
                                <img src={this.state.avatar_large} alt="" className="setting-avatar" />
                            </div>
                            <div className="setting-title web-only">我的头像</div>
                            <div className="description">支持 jpg、png 且大小 3M 以内的图片</div>
                            <input
                                ref={up_input}
                                type="file"
                                className="settings_user_pic"
                                style={{ display: "none" }}
                                onChange={this.handleFile}
                            />
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}

const mapStateToProps = (state) => {
    return {
        userinfo: state.userinfo,
    }
}
export default connect(mapStateToProps)(Profile);