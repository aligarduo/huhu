import './draft.css';
import React from 'react';
import Header from './header';
import Toast from '../../components/toast';
import http from '../../server/instance';
import servicePath from '../../server/api';
import Loading from '../../components/loading';
import Empty from '../../components/empty';
import Prompt from '../../components/prompt';
import browser_help from '../../utils/browser_help.js';
import timestamp_help from '../../utils/timestamp_help.js';
const { yeardate } = timestamp_help();

class Draft extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            article_draft_count: 0, //草稿总数
            article_draft: [], //草稿列表
            confirm_dialog: false, //删除确认弹窗标志
            item_draft_id: null, //等待删除的草稿ID
            over_loading: true, //加载标志
            cursor: 0, //加载索引
        }
    }

    componentDidMount() {
        document.title = '文章草稿 - 乎乎';
        this.Query_List(this.state.cursor);
        document.addEventListener('click', (e) => {
            let item = document.getElementsByClassName('draft-more-list');
            for (let i = 0; i < item.length; i++) {
                item[i].style.display = "none";
            }
        });
        //PC端
        window.addEventListener("scroll", () => {
            //到达底部
            this.reach_bottom();
        });
    }
    componentWillUnmount() {
        document.title = '乎乎 - 记录点点滴滴';
    }
    //草稿列表
    Query_List = (cursor) => {
        const _this = this;
        const api = servicePath.content_api__article_draft_query_list;
        http.post(api, {
            "cursor": cursor,
            "limit": 20
        }, { sign: false, token: true }, false).then(function (callback) {
            if (callback.data.err_no !== 0) {
                Toast.error(callback.data.err_msg, 3000);
                return;
            }
            _this.setState({
                article_draft_count: callback.data.count,
                article_draft: [..._this.state.article_draft, callback.data.data],
            });
            if (callback.data.has_more) {
                _this.setState({ over_loading: true });
            }
        })
    }
    //到达底部
    reach_bottom = () => {
        const { PullOnLoading } = browser_help();
        PullOnLoading(30, () => {
            if (this.state.over_loading) {
                this.setState({ over_loading: false, cursor: this.state.cursor + 20 });
                this.Query_List(this.state.cursor);
            }
        });
    }
    //阻止冒泡
    stopPropagation = (e) => {
        e.nativeEvent.stopImmediatePropagation();
    }
    //显示更多小菜单
    showMore = (e) => {
        let item = document.getElementsByClassName('draft-more-list');
        for (let i = 0; i < item.length; i++) {
            if (item[i] !== e.target.lastElementChild) {
                item[i].style.display = "none";
            }
        }
        this.stopPropagation(e);
        try {
            if (e.target.lastElementChild.style.display === 'inline') {
                e.target.lastElementChild.style.display = "none";
            } else {
                e.target.lastElementChild.style.display = "inline";
            }
        } catch { return }
    }
    //跳转到编辑
    redact = (draft_id) => {
        window.location.href = "/editor/drafts/" + draft_id;
    }
    //删除弹窗
    delete = (draft_id) => {
        document.body.style.overflow = 'hidden';
        this.setState({ confirm_dialog: true, item_draft_id: draft_id });
    }
    //取消删除
    cancel_delete = () => {
        document.body.style.overflow = 'inherit';
        this.setState({ confirm_dialog: false, item_draft_id: null });
    }
    //确认删除
    affirm_delete = () => {
        const _this = this;
        const api = servicePath.content_api__article_draft_delete;
        http.post(api, {
            "draft_id": this.state.item_draft_id
        }, { sign: true, token: true }, false).then(function (callback) {
            if (callback.data.err_no !== 0) {
                Toast.error(callback.data.err_msg, 3000);
                return;
            }
            _this.state.article_draft.forEach((_item) => {
                _item.forEach((item, index) => {
                    if (_this.state.item_draft_id === item.draft_id) {
                        _item.splice(index, 1);
                    }
                })
            })
            _this.setState({
                confirm_dialog: false,
                item_draft_id: 0,
                article_draft_count: _this.state.article_draft_count - 1,
            });
            document.body.style.overflow = 'inherit';
        })
    }


    render() {
        return (
            <div className="_draft">
                <Header />
                <main className="draft-main">
                    <div className="draft-list shadow">
                        <div className="draft-list-header">
                            <div className="list-header-item active">文章草稿（{this.state.article_draft_count}）</div>
                        </div>
                        <ul className="draft-lists">
                            {(() => {
                                switch (true) {
                                    case (this.state.article_draft.length === 0): return <Loading />;
                                    case (this.state.article_draft[0].length === 0): return <Empty />;
                                    case (this.state.article_draft[0].length > 0):
                                        return this.state.article_draft.map((_item) => {
                                            return _item.map((item, index) => {
                                                return (
                                                    <li className="draft-item" key={index}>
                                                        <div className="draft">
                                                            <a href={`/editor/drafts/${item.draft_id}`} className="draft-title">{item.title === "" ? "无标题" : item.title}</a>
                                                            <div className="draft-info-box">
                                                                <div className="item-date">{yeardate('YYYY年mm月dd日 HH:MM', item.ctime)}</div>
                                                                <div className="menus ion-ios-more" onClick={(e) => this.showMore(e)}>
                                                                    <ul className="menu-list shadow draft-more-list" style={{ display: 'none' }}>
                                                                        <li className="draft-item" onClick={() => this.redact(item.draft_id)}>编辑</li>
                                                                        <li className="draft-item" onClick={() => this.delete(item.draft_id)}>删除</li>
                                                                    </ul>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </li>
                                                )
                                            })
                                        })
                                    default: break;
                                }
                            })()}
                        </ul>
                        <Prompt
                            title="删除文章"
                            cuewords="删除后不可恢复，确认删除此文章吗？"
                            confirm_dialog={this.state.confirm_dialog}
                            cancel_delete={() => { this.cancel_delete() }}
                            affirm_delete={() => { this.affirm_delete() }}>
                        </Prompt>
                    </div>
                </main>
            </div>
        )
    }
}

export default Draft;