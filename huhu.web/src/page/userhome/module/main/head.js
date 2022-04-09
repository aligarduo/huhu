import React from 'react';
import { connect } from 'react-redux';
import Toast from '../../../../components/toast';
import http from '../../../../server/instance';
import servicePath from '../../../../server/api';
import numbers_help from '../../../../utils/numbers_help.js';
const { number_divide_thousands } = numbers_help();

class Head extends React.Component {
    componentDidUpdate() {
        const { article_authorinfo } = this.props;
        let username = article_authorinfo.user_name, title = '';
        username && username !== '' ? title = `${username} 的个人主页 - 文章` : title = '加载中';
        document.title = title;
    }
    Jump_settings = () => {
        window.location.href = "/user/settings/profile";
    }
    follow = (e) => {
        let n = e.target.className.indexOf('followed');
        if (e.target.className === '') {
            n = e.target.parentNode.className.indexOf('followed');
        }
        const do_api = servicePath.interact_api__follow_do;
        const undo_api = servicePath.interact_api__follow_undo;
        switch (true) {
            case (n > -1): this.follow_do_undo(undo_api, 'undo'); break;
            case (n <= -1): this.follow_do_undo(do_api, 'do'); break;
            default: break;
        }
    }
    follow_do_undo = (api, start) => {
        const { article_detailed } = this.props;
        const _this = this;
        http.post(api, {
            id: article_detailed.data.author_user_info.user_id,
            type: 1
        }, { sign: true, token: true }, false).then(function (callback) {
            if (callback.data.err_no !== 0) {
                Toast.info(callback.data.err_msg, 2500);
                return;
            }
            switch (start) {
                case "undo": _this.setState({ is_follow: false }); break;
                case "do": _this.setState({ is_follow: true }); break;
                default: break;
            }
        })
    }

    render() {
        const { article_authorinfo, userinfo } = this.props;
        return (
            <div className="user-info-block block shadow">
                <div className="lazy avatar bounce-effect"
                    style={{ backgroundImage: `url(${article_authorinfo ? article_authorinfo.avatar_large : ''})` }}>
                </div>
                <div className="info-box-home">
                    <div className="top">
                        <h1 className="username">
                            {article_authorinfo ? article_authorinfo.user_name : ''}
                            <div className="rank">
                                {article_authorinfo ? (
                                    <img
                                        src={require("../../../../assets/images/lv/lv" + article_authorinfo.level + ".svg").default}
                                        alt={`lv-${article_authorinfo.level}`}
                                    />
                                ) : null}
                            </div>
                        </h1>
                    </div>
                    {article_authorinfo && article_authorinfo.job_title !== "" ? (
                        <div className="intro">
                            <svg t="1637056042361" className="icon" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="44925" width="32" height="32"><path d="M893.44 325.461333v176.128H135.509333V325.461333a56.490667 56.490667 0 0 1 20.309334-46.933333 68.266667 68.266667 0 0 1 51.2-17.066667H358.4v-76.970666a49.322667 49.322667 0 0 1 10.069333-29.354667A36.522667 36.522667 0 0 1 392.533333 143.36h248.32a36.522667 36.522667 0 0 1 24.746667 11.776 48.128 48.128 0 0 1 10.069333 29.354667v76.288h146.602667a102.4 102.4 0 0 1 51.2 17.066666 56.32 56.32 0 0 1 19.968 47.616z m0 217.258667v211.456a77.141333 77.141333 0 0 1-20.138667 52.736 68.266667 68.266667 0 0 1-51.2 17.066667H206.336a101.376 101.376 0 0 1-51.2-17.066667 94.378667 94.378667 0 0 1-20.309333-52.736V542.72h283.648v70.485333c0 5.802667 5.12 11.776 5.12 17.066667 5.12 5.973333 10.069333 5.973333 20.138666 5.973333h136.533334a26.794667 26.794667 0 0 0 20.138666-4.778666c5.12-5.802667 10.069333-11.776 5.12-17.066667v-71.68zM408.405333 249.173333h217.258667v-58.88H408.405333z m161.621334 293.546667v58.709333h-111.104V542.72z" fill="#FFBE61" p-id="44926"></path></svg>
                            <span className="content">{article_authorinfo.job_title}</span>
                        </div>
                    ) : null}
                </div>
                <div className="action-box-home">

                    <div className="stat-action-block">
                        {article_authorinfo ? (
                            <div className="stat-item">
                                <div className="item">
                                    <span>关注</span>
                                    <span className="count">{number_divide_thousands(article_authorinfo.followee_count)}</span>
                                </div>
                                <div className="item">
                                    <span>粉丝</span>
                                    <span className="count">{number_divide_thousands(article_authorinfo.fans_count)}</span>
                                </div>
                                <div className="item">
                                    <span>获赞</span>
                                    <span className="count">{number_divide_thousands(article_authorinfo.got_digg_count)}</span>
                                </div>
                            </div>
                        ) : null}
                        {userinfo && article_authorinfo ? (
                            userinfo.user_id === article_authorinfo.user_id ? (
                                <button className="setting-btn btn" onClick={() => this.Jump_settings()}><span>设置</span></button>
                            ) : null
                        ) : null}
                    </div>

                    {userinfo && article_authorinfo ? (
                        userinfo.user_id === article_authorinfo.user_id ? (
                            <button className="setting-btn btn fit" onClick={() => this.Jump_settings()}><span>编辑个人资料</span></button>
                        ) : null
                    ) : null}

                </div>
            </div>
        )
    }
}

const mapStateToProps = (state) => {
    return {
        userinfo: state.userinfo,
        article_authorinfo: state.article_authorinfo,
    }
}
export default connect(mapStateToProps)(Head);