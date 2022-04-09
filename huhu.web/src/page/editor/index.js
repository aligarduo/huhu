import './style/app01.css'
import './style/app02.css'
import './style/theme01.css'
import './style/theme02.css'
import React from 'react';
import Header from "./header/index";
import Main from "./main/index";
import http from '../../server/instance';
import servicePath from '../../server/api';
import Toast from '../../components/toast';
import url_help from '../../utils/url_help';
const { urlparam } = url_help();

class Editor extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            login_state: false,
            property: 0,
            article_info: null,
        }
    }

    componentDidMount() {
        let token = localStorage.getItem("passport_csrf_token");
        if (!token) {
            alert("初始化失败，请检查登录设置");
            return;
        }
        let _urlparam = urlparam(this.props.match.url, 3);
        switch (urlparam(this.props.match.url, 2)) {
            case 'drafts': this.DraftDetail(_urlparam); break;
            case 'update': this.articleDetail(_urlparam); break;
            default: break;
        }
    }
    //草稿详细
    DraftDetail = (draft_id) => {
        if (draft_id === 'new') return this.setState({ login_state: true });
        const _this = this;
        const api = servicePath.content_api__article_draft_detail;
        http.post(api, {
            "draft_id": draft_id,
        }, { sign: true, token: true }, false).then(res => {
            if (res.data.err_no !== 0) {
                Toast.error(res.data.err_msg, 3000)
                return;
            }
            _this.setState({ property: 1, article_info: res.data.data, login_state: true });
        })
    }
    //文章详细
    articleDetail = (article_id) => {
        const api = servicePath.content_api__article_editor;
        const _this = this;
        http.post(api, {
            "article_id": article_id,
        }, { sign: false, token: true }, false).then(function (callback) {
            if (callback.data.err_no !== 0) {
                Toast.error(callback.data.err_msg, 3000)
                return;
            }
            _this.setState({ property: 2, article_info: callback.data.data.article_info, login_state: true });
        })
    }


    render() {
        return (
            <div id="huhu-web-editor">
                <div className="edit-draft">
                    <div className="markdown-editor">
                        {this.state.login_state ? (
                            <React.Fragment>
                                <Header
                                    parentProps={this.props}
                                    property={this.state.property}
                                    info={this.state.article_info} />
                                <Main />
                            </React.Fragment>
                        ) : null}
                    </div>
                </div>
                <div className="main-alert-list"></div>
            </div>
        )
    }
}

export default Editor;