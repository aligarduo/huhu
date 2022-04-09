import React from 'react';
import { connect } from 'react-redux';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import Toast from '../../../components/toast';
import http from '../../../server/instance';
import servicePath from '../../../server/api';
import store from '../../../redux/index';
import Login from '../../../components/login/index';
import sharelink_help from '../../../utils/sharelink_help.js';
const { share_weibo, share_qq, share_weixin } = sharelink_help();

class index extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            init_flag: true, //侧边栏加载标志
            article_digg: 0, //点赞数量
            article_is_digg: null, //点赞状态
            create_collection: false, //创建收藏集
            collection_list: [], //收藏集数据源
            new_collection_name: null, //新建收藏集名称
            show_collection_list: false, //收藏集列表
            is_collect: null, //文章是否收藏
        }
    }

    //显示收藏集列表
    showCollectionList(e) {
        //阻止冒泡
        e.nativeEvent.stopImmediatePropagation();
        this.setState({ show_collection_list: true });
    }

    //隐藏收藏集列表
    hideCollectionList(e) {
        e = e || window.event;
        if (e.stopPropagation) {
            //W3C阻止冒泡
            e.stopPropagation();
        } else {
            //IE阻止冒泡
            e.cancelBubble = true;
        }
        this.setState({ show_collection_list: false });
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
    digg = (e) => {
        //判断登录状态
        let token = localStorage.getItem("passport_csrf_token");
        if (!token) {
            this.login_show_box();
            return;
        }
        const digg_save_api = servicePath.interact_api__digg_save_api;
        const digg_cancel_api = servicePath.interact_api__digg_cancel_api;
        let n = e.target.className.indexOf('active');
        switch (true) {
            case (n <= -1): this.digg_save_cancel(digg_save_api, 'save'); break;
            case (n > -1): this.digg_save_cancel(digg_cancel_api, 'cancel'); break;
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

    //跳转到评论区
    comment = () => {
        let anchorElement = document.getElementById("comment-box")
        if (anchorElement) {
            anchorElement.scrollIntoView({ block: 'start', behavior: "smooth" })
        }
    }

    //获取收藏集列表
    get_collection_list = (e) => {
        //判断登录状态
        let token = localStorage.getItem("passport_csrf_token");
        if (!token) {
            this.login_show_box();
            return;
        }
        this.showCollectionList(e);
        const { article_detailed } = this.props;
        const _this = this;
        const api = servicePath.interact_api__collection_list;
        http.post(api, {
            cursor: "0",
            limit: 20,
            article_id: article_detailed ? article_detailed.data.article_id : 0,
        }, { sign: false, token: true }, false).then(function (callback) {
            if (callback.data.err_no !== 0) {
                Toast.info("网络繁忙，请稍后重试", 2500);
                return;
            }
            _this.setState({ collection_list: callback.data.data })
        })
    }

    //显示输入框
    create_collection = () => {
        this.setState({ create_collection: true });
    }

    //输入完成时
    inputChange = (e) => {
        this.setState({ new_collection_name: e.target.value });
    }

    //创建收藏集
    new_create = () => {
        const _this = this;
        const api = servicePath.interact_api__collection_add;
        http.post(api, {
            collection_name: this.state.new_collection_name,
        }, { sign: true, token: true }, false).then(function (callback) {
            if (callback.data.err_no !== 0) {
                Toast.info("网络繁忙，请稍后重试", 2500);
                return;
            }
            let obj = {};
            obj.collection_id = callback.data.collection_id;
            obj.collection_name = callback.data.collection_name;
            obj.post_article_count = 0;
            _this.setState({
                collection_list: [..._this.state.collection_list, callback.data],
            })
        })
    }

    //收藏
    collect = (collection_id, index) => {
        let _document = document.getElementsByClassName("collection-list")[0]
            .getElementsByTagName("li")[index]
            .getElementsByClassName("item-block")[1]
            .getElementsByTagName("button")[0];
        const add_api = servicePath.interact_api__collect_add;
        const delete_api = servicePath.interact_api__collect_delete;
        let n = _document.className.indexOf('start');
        switch (true) {
            case (n > -1): this.collect_add_delete(add_api, collection_id, 'add', _document); break;
            case (n <= -1): this.collect_add_delete(delete_api, collection_id, 'delete', _document); break;
            default: break;
        }
    }

    //加入收藏与取消收藏
    collect_add_delete = (api, collection_id, start, _document) => {
        const { article_detailed } = this.props;
        const _this = this;
        http.post(api, {
            collection_id: collection_id,
            item_id: article_detailed.data.article_id,
        }, { sign: true, token: true }, false).then(function (callback) {
            if (callback.data.err_no !== 0) {
                Toast.info(callback.data.err_msg, 2500);
                return;
            }
            switch (start) {
                case "add":
                    _this.setState({ is_collect: true });
                    _document.classList.remove("start");
                    break;
                case "delete":
                    _this.setState({ is_collect: false });
                    _document.classList.add("start");
                    break;
                default: break;
            }
        })
    }

    componentDidMount() {
        const { article_detailed } = this.props;
        //一次获取数据就好，防止抱死
        if (article_detailed && this.state.article_is_digg === null) {
            this.setState({
                article_is_digg: article_detailed.data.user_interact.is_digg,
                article_digg: article_detailed.data.article_info.digg_count,
            })
        }
        if (article_detailed && this.state.is_collect === null) {
            this.setState({
                is_collect: article_detailed.data.user_interact.is_collect,
            })
        }
    }

    share_link = (type) => {
        switch (true) {
            case (type === 'weibo'): share_weibo(); break;
            case (type === 'qq'): share_qq(); break;
            case (type === 'weixin'): share_weixin(); break;
            default: break;
        }
    }

    render() {
        const { article_detailed } = this.props;
        return (
            <div className="article-suspended-panel">
                <div className={`like-btn panel-btn like-adjust with-badge${this.state.article_is_digg ? ' active' : ''}`}
                    badge={this.state.article_digg}
                    onClick={(e) => this.digg(e)}>
                </div>
                <div className="comment-btn panel-btn comment-adjust with-badge"
                    badge={article_detailed ? article_detailed.data.article_info.comment_count : 0}
                    onClick={(e) => this.comment(e)}>
                </div>
                <div className={`collect-btn panel-btn${this.state.is_collect ? ' active' : ''}`} onClick={(e) => this.get_collection_list(e)}>
                    <div className="collect-popup-box" style={{ display: this.state.show_collection_list ? 'block' : 'none' }}>
                        <div className="mask" onClick={(e) => this.hideCollectionList(e)}></div>
                        <div className="collect-popup right">
                            <div className="collect-title">添加到收藏集</div>
                            <ul className="collection-list">
                                {this.state.collection_list.length > 0 &&
                                    this.state.collection_list.map((item, index) => {
                                        return (
                                            <li key={index} className="item" onClick={() => this.collect(item.collection_id, index)}>
                                                <div className="item-block">
                                                    <div className="collect-title">{item.collection_name}</div>
                                                    <div className="entry-count">{item.post_article_count}</div>
                                                </div>
                                                <div className="item-block">
                                                    <button className={`collect-icon collected${item.is_collect ? '' : ' start'}`}></button>
                                                </div>
                                            </li>
                                        )
                                    })}
                            </ul>
                            <div className="collect-action-box">
                                {this.state.create_collection ? (
                                    <div className="new-form">
                                        <input
                                            maxlength="20"
                                            className="collect-title-input"
                                            onChange={(e) => this.inputChange(e)}
                                        />
                                        <button
                                            className="submit-btn"
                                            onClick={() => this.new_create()}>
                                            添加
                                        </button>
                                    </div>
                                ) : (<button className="new-btn" onClick={() => this.create_collection()}>新建收藏集</button>)}
                            </div>
                        </div>
                    </div>
                </div>
                <div className="report-btn share-btn panel-btn"></div>
                <div className="share-title">分享</div>
                <div className="weibo-btn panel-btn" onClick={() => this.share_link("weibo")}></div>
                <div className="qq-btn panel-btn" onClick={() => this.share_link("qq")}></div>
                <div className="wechat-btn panel-btn" onClick={() => this.share_link("weixin")}></div>
            </div>
        )
    }
}

const mapStateToProps = (state) => {
    return {
        article_detailed: state.article_detailed,
    }
}
export default connect(mapStateToProps)(index);