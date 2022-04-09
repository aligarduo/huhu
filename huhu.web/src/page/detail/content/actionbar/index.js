import React from 'react';
import { connect } from 'react-redux';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import copy from 'copy-to-clipboard';
import Toast from '../../../../components/toast';
import http from '../../../../server/instance';
import servicePath from '../../../../server/api';
import store from '../../../../redux/index';
import Login from '../../../../components/login/index';

class index extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            equipment: false, //是否是移动设备
            article_is_digg: null,
            article_digg: 0,
        }
    }
    componentDidMount() {
        const agents = ['Android', 'webOS', 'iPhone', 'iPad', 'iPod', 'BlackBerry', 'Windows Phone'];
        for (let i = 0; i < agents.length; i++) {
            if (navigator.userAgent.match(agents[i])) {
                this.setState({ equipment: true });
            }
        }
        const { article_detailed } = this.props;
        if (article_detailed) {
            this.setState({
                article_is_digg: article_detailed.data.user_interact.is_digg,
                article_digg: article_detailed.data.article_info.digg_count,
            })
        }
    }
    // 弹出登录窗口
    login_show_box() {
        const div = document.getElementsByClassName("user-permissions")[0]
        ReactDOM.render(
            <Provider store={store}>
                <Login />
            </Provider>, div
        );
        document.body.style.overflow = 'hidden';
    }
    //点赞
    digg = () => {
        let token = localStorage.getItem("passport_csrf_token");
        if (!token) {
            this.login_show_box();
            return;
        }
        const digg_save_api = servicePath.interact_api__digg_save_api;
        const digg_cancel_api = servicePath.interact_api__digg_cancel_api;
        let is_digg = this.state.article_is_digg;

        switch (true) {
            case (is_digg === false): this.digg_save_cancel(digg_save_api, 'save'); break;
            case (is_digg === true): this.digg_save_cancel(digg_cancel_api, 'cancel'); break;
            default: break;
        }
    }
    //点赞与取消
    digg_save_cancel = (api, state) => {
        const _this = this;
        const { article_detailed } = this.props;
        http.post(api, {
            item_id: article_detailed.data.article_id,
            item_type: 1
        }, { sign: true, token: true }, false).then(function (callback) {
            if (callback.data.err_no !== 0) {
                Toast.info(callback.data.err_msg, 2500);
                return;
            }
            switch (state) {
                case 'save': _this.setState({
                    article_is_digg: true,
                    article_digg: _this.state.article_digg + 1,
                }); break;
                case 'cancel': _this.setState({
                    article_is_digg: false,
                    article_digg: _this.state.article_digg - 1,
                }); break;
                default: break;
            }
        })
    }

    skip_comment = () => {
        let anchorElement = document.getElementById("comment-box")
        if (anchorElement) {
            anchorElement.scrollIntoView({ block: 'start', behavior: "smooth" })
        }
    }

    copyUrl = () => {
        const url = window.location.href;
        copy(url);
        Toast.info("链接已复制，快去分享给好友吧！", 2500);
    }


    render() {
        const { article_detailed } = this.props;
        return (
            <React.Fragment>
                {this.state.equipment ? (
                    <React.Fragment>
                        <div className="bottom-action-box action-bar">
                            <div className={`praise-action bar-action${this.state.article_is_digg ? ' active' : ''}`} onClick={() => this.digg()}>
                                <div className="action-title-box">
                                    {this.state.article_is_digg ? (
                                        <img src={require('../../../../assets/images/svg/got-digg.svg').default} alt="" />
                                    ) : (
                                        <img src={require('../../../../assets/images/svg/digg.svg').default} alt="" />
                                    )}
                                    <span className={`action-title${this.state.article_is_digg ? ' active' : ''}`}>
                                        {(() => {
                                            let article_digg = this.state.article_digg;
                                            return article_digg === 0 ? '点赞' : article_digg
                                        })()}
                                    </span>
                                </div>
                            </div>
                            <div className="comment-action bar-action">
                                <div className="action-title-box">
                                    <img src={require('../../../../assets/images/svg/comment-icon.svg').default} alt="" />
                                    <span className="action-title" onClick={() => this.skip_comment()}>
                                        {(() => {
                                            let comment = article_detailed.data.article_info.comment_count;
                                            return comment === 0 ? '评论' : comment
                                        })()}
                                    </span>
                                </div>
                            </div>
                            <div className="collect-action bar-action" onClick={() => this.copyUrl()}>
                                <div className="action-title-box">
                                    <img src={require('../../../../assets/images/svg/transmit.svg').default} alt="" />
                                    <span className="action-title">分享</span>
                                </div>
                            </div>
                        </div>
                    </React.Fragment>
                ) : (undefined)}
            </React.Fragment>
        )
    }
}

const mapStateToProps = (state) => {
    return {
        article_detailed: state.article_detailed,
    }
}
export default connect(mapStateToProps)(index);