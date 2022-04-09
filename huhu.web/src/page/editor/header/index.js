import React from 'react';
import Emitter from '../events';
import Usernav from './module/usernav';
import Tags from './module/tag';
import Cover from './module/cover';
import Summary from './module/summary';
import http from '../../../server/instance';
import Toast from '../../../components/toast';
import servicePath from '../../../server/api';
import url_help from '../../../utils/url_help';
const { urlparam } = url_help();


//跳转至草稿
const skip_draft = () => {
    const win = window.open('about:blank');
    win.location.href = "/editor/drafts"
}

class Header extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            estimate_flag: true, //判断标志
            editor_properties: '', //编辑器属性
            editor_properties_code: 0, //编辑器属性代号
            show_publish: false, //显示菜单
            title: '', //文章标题
            tags: '', //文章标签
            cover: '', //文章封面
            summary: '', //文章摘要
            editor_value: '', //文章内容
            save_as_draftID: 0, //另存为草稿id
        }
    }
    componentWillUnmount() {
        document.title = '乎乎 - 记录点点滴滴';
        Emitter.removeListener(this.eventEmitter);
    }
    componentDidMount() {
        //隐藏弹出层
        document.addEventListener('click', () => { this.setState({ show_publish: false }) });
        //键盘弹起触发
        document.addEventListener('keyup', (e) => { this.documentKeyup(e) });
        //订阅事件
        this.eventEmitter = Emitter.addListener('changeMessage', (msg) => { this.setState({ editor_value: msg }) });
        // 编辑器属性
        switch (this.props.property) {
            case 0://新文章
                document.title = `写文章 - 乎乎`;
                this.setState({ editor_properties: "发布", editor_properties_code: 310 });
                break;
            case 1://编辑草稿
                document.title = `编辑草稿 - ${this.props.info.title} - 乎乎`;
                this.FillingData(this.props.info);
                this.setState({ editor_properties: "发布", editor_properties_code: 311 });
                break;
            case 2://更新文章
                document.title = `更新文章 - ${this.props.info.title} - 乎乎`;
                this.FillingData(this.props.info);
                this.setState({ editor_properties: "更新", editor_properties_code: 312 });
                break;
            default: break;
        }
    }
    //阻止冒泡
    stopPropagation = (e) => {
        e.nativeEvent.stopImmediatePropagation();
    }
    //切换菜单
    showPublish = (e) => {
        this.stopPropagation(e);
        this.setState({ show_publish: !this.state.show_publish });
    }
    //键盘弹起触发
    documentKeyup = (e) => {
        this.estimate(urlparam(this.props.parentProps.match.url, 3));
    }
    //判断是新编写还是已有记录
    estimate = (property) => {
        if (property === "new") {
            if (this.state.estimate_flag) {
                this.setState({ estimate_flag: false })
                this.articleDraftCreate('direct');
            } else {
                this.articleDraftUpdate('direct');
            }
        } else {
            switch (this.state.editor_properties_code) {
                case 311: this.articleDraftUpdate('direct'); break;
                case 312:
                    if (this.state.save_as_draftID === 0) {
                        this.articleDraftCreate('save_as');
                    } else {
                        this.articleDraftUpdate('save_as');
                    };
                    break;
                default: break;
            }
        }
    }
    //收集数据源
    dataOrigin = (status) => {
        let obj = {};
        switch (status) {
            case 'draft': obj.draft_id = urlparam(window.location.href, 5); break;
            case 'article': obj.article_id = urlparam(window.location.href, 5); break;
            case 'save_as_draft': obj.draft_id = this.state.save_as_draftID; break;
            default: break;
        }
        obj.title = this.state.title;
        obj.cover_image = !this.state.cover || this.state.cover === '' ? '' : this.state.cover;;
        obj.mark_content = this.state.editor_value;
        obj.brief_content = this.state.summary;
        obj.tag_ids = this.state.tags.length > 0 ? (Array.isArray(this.state.tags) ? (this.state.tags) : ([this.state.tags])) : [''];
        obj.edit_type = "md";
        return obj;
    }
    //新增草稿
    articleDraftCreate = (way) => {
        const _this = this;
        let data = this.dataOrigin('no');
        const api = servicePath.content_api__article_draft_create;
        http.post(api, data, { sign: true, token: true }, false).then(res => {
            if (res.data.err_no !== 0) {
                Toast.error(res.data.err_msg, 3000);
                return;
            }
            switch (way) {
                case 'direct': window.history.replaceState(null, "new?v=2", res.data.data.draft_id); break;
                case 'save_as': _this.setState({ save_as_draftID: res.data.data.draft_id }); break;
                default: break;
            }
        })
    }
    //更新草稿
    articleDraftUpdate = (way) => {
        let data = null;
        switch (way) {
            case 'direct': data = this.dataOrigin('draft'); break;
            case 'save_as': data = this.dataOrigin('save_as_draft'); break;
            default: break;
        }
        const api = servicePath.content_api__article_draft_update;
        http.post(api, data, { sign: true, token: true }, false).then(res => {
            if (res.data.err_no !== 0) {
                Toast.error(res.data.err_msg, 3000)
                return;
            }
        })
    }
    //填充数据
    FillingData = (info) => {
        this.setState({
            title: info.title,
            tags: info.tag_ids,
            cover: info.cover_image,
            summary: info.brief_content,
        }, () => {
            Emitter.emit('editor_data', info.mark_content);
        })
    }
    //发布新文章
    publishArticle = () => {
        let obj = this.dataOrigin('draft');
        switch (true) {
            case (obj.draft_id === ""): Toast.error("初始化出错，请刷新重试！", 2500); return;
            case (obj.title === ""): Toast.error("请输入标题", 2500); return;
            case (obj.tag_ids[0] === ""): Toast.error("至少添加一个标签", 2500); return;
            case (obj.brief_content === ""): Toast.error("请输入文章摘要", 2500); return;
            case (obj.mark_content === ""): Toast.error("文章内容不能为空", 2500); return;
            default: break;
        }
        const api = servicePath.content_api__article_publish;
        http.post(api, obj, { sign: true, token: true }, false).then(res => {
            if (res.data.err_no === 0) {
                sessionStorage.setItem("published_succeed", JSON.stringify(res.data.data));
                window.location.href = "/published_succeed";
            } else if (res.data.err_no === 1051) {
                sessionStorage.setItem("published_error", JSON.stringify(res.data.data));
                window.location.href = "/published_error";
            } else {
                Toast.info(res.data.err_msg, 3000);
            }
        })
    }
    //更新文章
    updateArticle = () => {
        let obj = this.dataOrigin('article');
        switch (true) {
            case (obj.draft_id === ""): Toast.error("初始化出错，请刷新重试！", 2500); return;
            case (obj.title === ""): Toast.error("请输入标题", 2500); return;
            case (obj.brief_content === ""): Toast.error("请输入文章摘要", 2500); return;
            case (obj.mark_content === ""): Toast.error("文章内容不能为空", 2500); return;
            default: break;
        }
        const api = servicePath.content_api__article_update;
        http.post(api, obj, { sign: true, token: true }, false).then(res => {
            if (res.data.err_no !== 0) {
                Toast.info(res.data.err_msg, 3000)
            } else {
                sessionStorage.setItem("published_data", JSON.stringify(res.data.data));
                window.location.href = "/published";
            }
        })
    }
    //确定按钮
    confirmButton = () => {
        switch (this.state.editor_properties_code) {
            case 310: this.publishArticle(); break;
            case 311: this.publishArticle(); break;
            case 312: this.updateArticle(); break;
            default: break;
        }
    }


    render() {
        return (
            <header className="header editor-header">
                <input
                    placeholder="输入文章标题..."
                    spellCheck="false"
                    maxLength="80"
                    className="title-input"
                    value={this.state.title}
                    onChange={(e) => { this.setState({ title: e.target.value }) }}
                />
                <div className="right-box">
                    <div className="with-padding status-text">文章将自动保存至草稿箱</div>
                    <button className="with-padding xitu-btn xitu-btn-outline"
                        onClick={skip_draft}>
                        草稿箱
                    </button>
                    <div className="publish-popup with-padding">
                        <button
                            className="xitu-btn"
                            onClick={(e) => this.showPublish(e)}>
                            {this.state.editor_properties}
                        </button>
                        <div className="editor-panel"
                            style={{ display: this.state.show_publish ? 'block' : 'none' }}
                            onClick={this.stopPropagation}>
                            <div className="panel-title">发布文章</div>
                            <Tags value={this.state.tags} onChange={(e) => { this.setState({ tags: e.tags }) }} />
                            <Cover value={this.state.cover} onChange={(e) => { this.setState({ cover: e.cover_url }) }} />
                            <Summary value={this.state.summary} onChange={(e) => { this.setState({ summary: e.value }) }} />
                            <div className="footer">
                                <div className="btn-container">
                                    <button
                                        className="ui-btn btn line default"
                                        onClick={(e) => this.showPublish(e)}>
                                        取消
                                    </button>
                                    <button
                                        className="ui-btn btn primary default"
                                        onClick={() => this.confirmButton()}>
                                        确定
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <Usernav />
                </div>
            </header>
        )
    }
}

export default Header;