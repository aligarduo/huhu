import React from "react";
import { connect } from 'react-redux';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import marked from 'marked';
import store from '../../../../redux/index';
import Login from '../../../../components/login/index';
import http from '../../../../server/instance';
import Toast from '../../../../components/toast';
import servicePath from '../../../../server/api';
import timestamp_help from '../../../../utils/timestamp_help.js';
const { otherday } = timestamp_help();

class index extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            emoji_selector: false, //展开emoji列表
            publish_content: '', //评论内容
            input_onFocus: false, //输入框获得焦点
            reply_opt: null,
            article_comments_list: [],
            emoji: { plate: 1, sum: 137, path: 'm1', suffix: '.svg' },
        }
    }

    componentDidMount() {
        marked.setOptions({
            renderer: new marked.Renderer(),
            gfm: true,
            pedantic: false,
            sanitize: false,
            tables: true,
            breaks: true,
            smartLists: true,
            smartypants: true,
        })
        document.addEventListener('click', (e) => {
            this.setState({ emoji_selector: false });
        });
    }

    stopPropagation(e) {
        e.nativeEvent.stopImmediatePropagation();
    }

    //展开emoji列表
    cut_emoji_selector = (e) => {
        this.stopPropagation(e);
        this.setState({ emoji_selector: !this.state.emoji_selector });
    }

    //输入框获得焦点
    publish_onFocus = (e) => {
        if (e.target.className.indexOf("empty") > -1) {
            e.target.classList.remove("empty");
        }
        this.setState({ publish_content: e.target.innerHTML, input_onFocus: true });
    }

    //输入框失去焦点
    publish_onBlur = (e) => {
        this.setState({ publish_content: e.target.innerHTML, input_onFocus: false }, () => {
            if (this.state.publish_content === '' && e.target.className.indexOf("empty") <= -1) {
                e.target.classList.add("empty");
            }
        });
    }

    //点击_emoji
    click_emoji = (e) => {
        let img = `<img class="emoji" draggable="false" alt="" src="${e.target.currentSrc}">`;
        let str = this.state.publish_content + img;
        this.setState({ publish_content: str, input_onFocus: true });
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

    //发表评论
    make_comments = () => {
        //判断登录状态
        let token = localStorage.getItem("passport_csrf_token");
        if (!token) {
            Toast.info('请先登录', 2500);
            this.login_show_box();
            return;
        }
        if (this.state.publish_content === '') {
            Toast.info('请输入内容', 2500);
            this.setState({ input_onFocus: true });
            return;
        }
        const _this = this;
        const { userinfo } = this.props;
        let comment_list = document.getElementsByClassName('comment-list')[0];
        const api = servicePath.interact_api__comment_publish;
        http.post(api, {
            "comment_content": this.state.publish_content,
            "item_id": window.location.pathname.replace('/post/', '')
        }, { sign: true, token: true }, false).then(function (callback) {
            if (callback.data.err_no !== 0) {
                Toast.error(callback.data.err_msg, 2500);
                return;
            }
            let new_comment = `<div class="item"><div class="comment"><div class="user-popover-box popover"><a href="/user/${userinfo.user_id}" target="_blank" rel="noopener noreferrer" class="user-link"><img src=${userinfo.avatar_large} alt="" class="lazy avatar"></a></div><div class="comment-content-box comment-divider-line"><div class="meta-box"><div class="user-popover-box"><a href="/user/${userinfo.user_id}" target="_blank" rel="noopener noreferrer" class="username ellipsis"><span class="name">${userinfo.user_name}</span><span blank="true" class="rank"><img src=${require("../../../../assets/images/lv/lv" + userinfo.level + ".svg").default} alt=${`lv-${userinfo.level}`}></span></a></div><div class="position">${userinfo.job_title}</div><span class="divide"></span><time class="time">刚刚</time><span class="divide"></span><div class="delete">删除</div></div><div class="comment-content" data-id=${callback.data.data.comment_id}>${callback.data.data.comment_content}</div><div class="action-box"></div></div></div></div>`;
            comment_list.innerHTML = new_comment + comment_list.innerHTML;
            Toast.info('发表评论成功', 2500);
            _this.setState({ publish_content: '' });
        })
    }

    //del评论
    del_comments = () => {
        let del = document.getElementsByClassName("delete");
        for (let i = 0; i < del.length; i++) {
            del[i].onclick = (e) => {
                const api = servicePath.interact_api__comment_delete;
                http.post(api, {
                    "comment_id": e.target.parentNode.parentNode.children[1].getAttribute("data-id")
                }, { sign: true, token: true }, false).then(callback => {
                    if (callback.data.err_no !== 0) {
                        Toast.error(callback.data.err_msg, 2500);
                        return;
                    }
                    e.target.parentNode.parentNode.parentNode.parentNode.remove();
                    Toast.info('评论删除成功', 2500);
                })
            };
        }
    }
    componentDidUpdate() {
        this.del_comments();
    }
    componentWillUnmount() {
        this.del_comments();
    }

    emoji_cut = (i) => {
        switch (true) {
            case (i === 1): this.setState({
                emoji: { plate: 1, sum: 137, path: 'm1', suffix: '.svg' },
            }); break;
            case (i === 2): this.setState({
                emoji: { plate: 2, sum: 43, path: 'm2', suffix: '.png' },
            }); break;
            default: break;
        }
    }


    render() {
        const { userinfo, article_comments } = this.props;
        return (
            <React.Fragment>
                <div className="comment-list-box" id="comment-box">
                    {article_comments.data ? (
                        <div className={`comment-form${this.state.input_onFocus ? " focused" : ""}`}>
                            {userinfo ? (
                                <div className="avatar-box">
                                    <div className="avatar bounce-effect"
                                        style={{ backgroundImage: `url(${userinfo.avatar_large})` }}>
                                    </div>
                                </div>
                            ) : (undefined)}
                            <div className="form-box">
                                <div className="auth-card">
                                    <div className="input-box">
                                        <div suppressContentEditableWarning
                                            contentEditable="true"
                                            spellCheck="false"
                                            placeholder={this.state.publish_content === '' ? '输入评论' : ''}
                                            disabled="disabled"
                                            className="rich-input empty"
                                            onFocus={(e) => { this.publish_onFocus(e) }}
                                            onBlur={(e) => { this.publish_onBlur(e) }}
                                            onChange={(e) => console.log(e)}
                                            dangerouslySetInnerHTML={{ __html: this.state.publish_content }}>
                                        </div>
                                    </div>
                                </div>
                                <div className="action-box">
                                    <div className="emoji emoji-btn">
                                        <div className="emoji-box" onClick={(e) => this.cut_emoji_selector(e)}>
                                            <img src={require("../../../../assets/images/svg/emoji-btn.svg").default} alt="" className="lazy icon immediate" />
                                            <span>表情</span>
                                        </div>
                                        <div className="emoji-selector picker" style={{ display: this.state.emoji_selector ? "block" : "none" }} onClick={this.stopPropagation}>
                                            <div className="triangle"></div>
                                            <div className="emoji_header">
                                                {(() => {
                                                    let _emoji_header = [];
                                                    for (let i = 1; i <= 2; i++) {
                                                        _emoji_header.push(
                                                            <div key={i} className={`emotion_box${this.state.emoji["plate"] === i ? ' active' : ''}`} onClick={() => this.emoji_cut(i)}>
                                                                <img src={require("../../../../assets/images/png/emoji_icon_" + i + ".png").default} className="emoji-image" alt="" />
                                                            </div>
                                                        )
                                                    }
                                                    return _emoji_header;
                                                })()}
                                            </div>
                                            <div className="emoji-content">
                                                <div className="emoji-picker">
                                                    <div className="emojis">
                                                        <p className="emoji-title">全部</p>
                                                        <ul className="category">
                                                            {(() => {
                                                                let _emoji = [];
                                                                for (let i = 1; i <= this.state.emoji["sum"]; i++) {
                                                                    _emoji.push(
                                                                        <li className="item" key={i}>
                                                                            <img className="emoji" draggable="false" alt=""
                                                                                src={require("../../../../assets/images/emoji/" + this.state.emoji['path'] + "/emoji_" + i + this.state.emoji['suffix']).default}
                                                                                onClick={(e) => this.click_emoji(e)}
                                                                            />
                                                                        </li>
                                                                    )
                                                                }
                                                                return _emoji;
                                                            })()}
                                                        </ul>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="submit">
                                        <button className="submit-btn"
                                            onClick={() => this.make_comments()}>
                                            评论
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    ) : (undefined)}
                    <div className="comment-list">
                        {article_comments.data && article_comments.data.map((item, index) => {
                            return (
                                <div className="item" key={index}>
                                    <div className="comment">
                                        <div className="user-popover-box popover">
                                            <a href={`/user/${item.user_info.user_id}`} target="_blank" rel="noreferrer" className="user-link">
                                                <img src={item.user_info.avatar_large} alt="" className="lazy avatar bounce-effect" />
                                            </a>
                                        </div>
                                        <div className="comment-content-box comment-divider-line">
                                            <div className="meta-box">
                                                <div className="user-popover-box">
                                                    <a href={`/user/${item.user_info.user_id}`} target="_blank" rel="noreferrer" className="username ellipsis">
                                                        <span className="name">
                                                            {item.user_info.user_name}
                                                        </span>
                                                        <span blank="true" className="rank">
                                                            <img
                                                                src={require("../../../../assets/images/lv/lv" + item.user_info.level + ".svg").default}
                                                                alt={`lv-${item.user_info.level}`}
                                                            />
                                                        </span>
                                                    </a>
                                                </div>
                                                <div className="position">{item.user_info.job_title}</div>
                                                <span className="divide"></span>
                                                <time className="time">{otherday(item.comment_info.ctime)}</time>
                                                {parseInt(item.user_interact.user_id) === parseInt(item.comment_info.user_id) ? (
                                                    <>
                                                        <span className="divide"></span>
                                                        <div className="delete">删除</div>
                                                    </>
                                                ) : (undefined)}
                                            </div>
                                            <div className="comment-content" data-id={item.comment_info.comment_id} dangerouslySetInnerHTML={{ __html: marked(item.comment_info.comment_content) }}></div>
                                        </div>
                                    </div>
                                </div>
                            )
                        })}
                    </div>
                </div>
            </React.Fragment>
        )
    }
}

const mapStateToProps = (state) => {
    return {
        userinfo: state.userinfo,
        article_comments: state.article_comments,
    }
}
export default connect(mapStateToProps)(index);