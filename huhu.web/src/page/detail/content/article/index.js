import './style/css_1.css';
import React from 'react';
import { connect } from 'react-redux';
import mediumZoom from 'medium-zoom';
import './style/cur.css';
import copy from 'copy-to-clipboard';
import marked from 'marked';
import hljs from "highlight.js";
import 'highlight.js/styles/a11y-light.css';
import Toast from '../../../../components/toast';
import http from '../../../../server/instance';
import servicePath from '../../../../server/api';
import Prompt from '../../../../components/prompt';
import timestamp_help from '../../../../utils/timestamp_help.js';
import numbers_help from '../../../../utils/numbers_help.js';
const { yeardate } = timestamp_help();
const { number_divide_thousands } = numbers_help();

let rendererMD = new marked.Renderer();
class index extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            is_follow: false, //是否关注
            confirm_dialog: false, //删除确认弹窗标志
            item_article_id: null, //等待删除的草稿ID
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
            highlight: function (code) {
                return hljs.highlightAuto(code).value;
            }
        });
        //重写a标签，在新标签打开
        rendererMD.link = function (href, title, text) {
            return '<a href=/skiplink?target=' + href + ' title="' + href + '" target="_blank">' + text + '</a>';
        }
        this.init();
        document.addEventListener("copy", (e) => {
            this.oncopy(e);
        });
    }

    init = () => {
        const { article_detailed } = this.props;
        if (article_detailed.err_no === 0) {
            this.setState({
                is_follow: article_detailed.data.user_interact.is_follow,
            }, () => {
                let p = document.getElementsByClassName('markdown-body')[0].querySelectorAll('img');
                mediumZoom(p, {
                    margin: 24,
                    background: 'rgb(255 255 255)',
                    scrollOffset: 0,
                    metaClick: false
                });
            })
        }
    }
    componentDidUpdate() {
        let n = document.getElementsByTagName("pre").length;
        for (let i = 0; i < n; i++) {
            let a = document.getElementsByTagName("pre")[i].getElementsByTagName("code")[0];
            let b = a.className;
            a.setAttribute("lang", b.replace('language-', ''));

            let span = document.createElement("span");
            span.className = "copy-code-btn";
            span.innerHTML = "复制代码";
            span.onclick = (e) => this.copy(e);
            a.append(span);
        }
    }
    copy = (e) => {
        copy(e.target.parentNode.innerText.replace('复制代码', ''));
        Toast.info('代码复制成功', 2500);
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
    //跳转编辑
    JumpEdit = (id) => {
        const win = window.open('about:blank');
        win.location.href = `/editor/update/${id}`;
    }
    //开启弹窗
    deletes = (article_id) => {
        document.body.style.overflow = 'hidden';
        this.setState({ confirm_dialog: true, item_article_id: article_id });
    }
    //取消删除
    cancel_delete = () => {
        document.body.style.overflow = 'inherit';
        this.setState({ confirm_dialog: false, item_article_id: null });
    }
    //确认删除
    affirm_delete = () => {
        const _this = this;
        const api = servicePath.content_api__article_delete;
        http.post(api, {
            "article_id": this.state.item_article_id,
        }, { sign: true, token: true }, false).then(function (callback) {
            if (callback.data.err_no !== 0) {
                Toast.error(callback.data.err_msg, 3000);
                return;
            }
            _this.setState({
                confirm_dialog: false,
                item_article_id: null,
            });
            document.body.style.overflow = 'inherit';
            window.location.replace("/");
        })
    }
    //版权信息
    copyright = (a, b) => {
        return [
            '著作权归作者所有。',
            '商业转载请联系作者获得授权，非商业转载请注明出处。',
            '作者：' + a,
            '链接：' + b,
            '来源：乎乎',
        ]
    }
    //版权声明
    oncopy = (event) => {
        const { article_detailed } = this.props;
        // clipboardData 对象是为通过编辑菜单、快捷菜单和快捷键执行的编辑操作所保留的，也就是你复制或者剪切内容
        let clipboardData = event.clipboardData || window.clipboardData;
        // 如果未复制或者未剪切，则return出去
        if (!clipboardData) { return; }
        // Selection对象，表示用户选择的文本范围或光标的当前位置。
        // 声明一个变量接收 -- 用户输入的剪切或者复制的文本转化为字符串
        let text = window.getSelection().toString();
        if (text && text.length > 47) {
            // 如果文本存在，首先取消文本的默认事件
            event.preventDefault();
            let user_name = article_detailed.data.author_user_info.user_name;
            let url = window.location.protocol + '//' + window.location.host + window.location.pathname;
            clipboardData.setData('text/plain', text + '\n\n\n' + this.copyright(user_name, url).join("\n") + '\n');
        }
    }


    render() {
        const { article_detailed, userinfo } = this.props;
        return (
            <React.Fragment>
                <article className="article">

                    <h1 className="article-title">
                        {article_detailed ? (article_detailed.data.article_info.title) : null}
                    </h1>

                    <div className="author-info-block author-info-block-one">
                        <a href={article_detailed ? '/user/' + article_detailed.data.author_user_info.user_id : ''} target="_blank" rel="noreferrer" className="avatar-link">
                            <div className="avatar bounce-effect"
                                style={{ backgroundImage: `url(${article_detailed ? article_detailed.data.author_user_info.avatar_large : ''})` }}>
                            </div>
                        </a>
                        <div className="author-info-box">
                            <div className="author-name">
                                <a href={article_detailed ? '/user/' + article_detailed.data.author_user_info.user_id : ''} target="_blank" rel="noreferrer" className="username ellipsis">
                                    <span className="name" style={{ maxWidth: "calc(100%-50px)" }}>
                                        {article_detailed ? article_detailed.data.author_user_info.user_name : ''}
                                    </span>
                                    <span blank="true" className="rank">
                                        {article_detailed ? (
                                            <img src={require("../../../../assets/images/lv/lv" + article_detailed.data.author_user_info.level + ".svg").default}
                                                alt={`lv-${article_detailed.data.author_user_info.level}`}
                                            />
                                        ) : null}
                                    </span>
                                </a>
                            </div>
                            <div className="meta-box">
                                <time className="time">{article_detailed ? yeardate('YYYY年mm月dd日', article_detailed.data.article_info.ctime) : ''}</time>
                                <span className="views-count">{article_detailed ? '阅读 ' + number_divide_thousands(article_detailed.data.article_info.view_count) : ''}</span>
                                {userinfo && article_detailed ? (
                                    userinfo.user_id === article_detailed.data.author_user_info.user_id ? (
                                        <React.Fragment>
                                            <span className="compile" onClick={() => this.JumpEdit(article_detailed.data.article_id)}>编辑</span>
                                            <span className="deletes" onClick={() => this.deletes(article_detailed.data.article_id)}>删除</span>
                                        </React.Fragment>
                                    ) : null
                                ) : null}
                            </div>
                        </div>
                        {userinfo && article_detailed &&
                            userinfo.user_id !== article_detailed.data.author_user_info.user_id ? (
                            <button className={`follow-button${this.state.is_follow ? ' followed' : ''}`} onClick={(e) => this.follow(e)}>
                                <span className={`icon ${this.state.is_follow ? 'icon-followed' : 'icon-follow'}`}></span>
                                <span>{this.state.is_follow ? '取消关注' : '关注'}</span>
                            </button>
                        ) : null}
                    </div>

                    {article_detailed && article_detailed.data.article_info.cover_image !== "" ? (
                        <img src={article_detailed.data.article_info.cover_image} alt="" className="lazy article-hero" />
                    ) : null}
                    <div className="article-content">
                        <div className="markdown-body" id="markdown-body"
                            dangerouslySetInnerHTML={{ __html: marked(article_detailed.data.article_info.mark_content, { renderer: rendererMD }) }}>
                        </div>
                    </div>

                    <Prompt
                        title="删除文章"
                        cuewords="删除后不可恢复，确认删除此文章吗？"
                        confirm_dialog={this.state.confirm_dialog}
                        cancel_delete={() => { this.cancel_delete() }}
                        affirm_delete={() => { this.affirm_delete() }}>
                    </Prompt>
                </article>
            </React.Fragment >
        )
    }
}

const mapStateToProps = (state) => {
    return {
        userinfo: state.userinfo,
        article_detailed: state.article_detailed,
    }
}
export default connect(mapStateToProps)(index);