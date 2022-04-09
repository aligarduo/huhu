import React from 'react';
import { connect } from 'react-redux'
import Toast from '../../../../components/toast';
import http from '../../../../server/instance';
import Head from './head.js'
import Navigation from './navigation.js'
import Loading from '../../../../components/loading';
import Empty from '../../../../components/empty';
import servicePath from '../../../../server/api';
import Prompt from '../../../../components/prompt';
import numbers_help from '../../../../utils/numbers_help.js';
import timestamp_help from '../../../../utils/timestamp_help.js';
const { number_divide } = numbers_help();
const { otherday } = timestamp_help();

class Index extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            confirm_dialog: false, //删除确认弹窗标志
            item_article_id: null, //等待删除的草稿ID
        }
    }
    componentDidMount() {
        document.addEventListener('click', () => {
            let item = document.getElementsByClassName('user-more-list');
            for (let i = 0; i < item.length; i++) {
                item[i].style.display = "none";
            }
        });
    }
    //阻止冒泡
    stopPropagation = (e) => {
        e.nativeEvent.stopImmediatePropagation();
    }
    //显示更多小菜单
    showMore = (e) => {
        let item = document.getElementsByClassName('user-more-list');
        for (let i = 0; i < item.length; i++) {
            if (item[i] !== e.target.nextSibling) {
                item[i].style.display = "none";
            }
        }
        this.stopPropagation(e);
        if (e.target.nextSibling.style.display === 'inline') {
            e.target.nextSibling.style.display = "none";
        } else {
            e.target.nextSibling.style.display = "inline";
        }
    }
    //跳转到编辑
    redact = (article_id) => {
        window.location.href = "/editor/update/" + article_id;
    }
    //删除弹窗
    delete = (article_id) => {
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
        const { user_homepage_article } = this.props;
        const api = servicePath.content_api__article_delete;
        http.post(api, {
            "article_id": this.state.item_article_id,
        }, { sign: true, token: true }, false).then(function (callback) {
            if (callback.data.err_no === 0) {
                user_homepage_article.forEach((_item) => {
                    _item.forEach((item, index) => {
                        if (_this.state.item_article_id === item.item_info.article_id) {
                            _item.splice(index, 1);
                        }
                    })
                })
            } else {
                Toast.error(callback.data.err_msg, 3000);
            }
            _this.setState({
                confirm_dialog: false,
                item_article_id: 0,
            });
            document.body.style.overflow = 'inherit';
        })
    }


    render() {
        const { user_homepage_article, article_authorinfo, userinfo } = this.props;
        return (
            <div className="major-area">
                <Head />
                <div className="list-block shadow">
                    <div className="detail-list">
                        <Navigation />
                        <div className="list-body">
                            <div className="post-list-box">
                                <ul className="entry-list">
                                    {(() => {
                                        switch (true) {
                                            case (user_homepage_article.length === 0): return <Loading />;
                                            case (user_homepage_article[0].length === 0): return <Empty />;
                                            case (user_homepage_article[0].length > 0):
                                                return user_homepage_article.map((_item) => {
                                                    return _item.map((item, index) => {
                                                        return (
                                                            <li className="user-home-item" key={index}>
                                                                <div className="content-box">
                                                                    <div className="meta-container">
                                                                        <div className="user-message">
                                                                            <a href={`/user/${item.item_info.author_user_info.user_id}`} target="_blank" rel="noreferrer" className="userbox">
                                                                                <div className="user-popover-box">
                                                                                    {item.item_info.author_user_info.user_name}
                                                                                </div>
                                                                            </a>
                                                                        </div>
                                                                        <div className="dividing"></div>
                                                                        <div className="date">{otherday(item.item_info.article_info.ctime)}</div>
                                                                        <div className="dividing"></div>
                                                                        <div className="tag_list">
                                                                            {item.item_info.tags.map((tag_item, tag_index) => {
                                                                                return (
                                                                                    <div className="tag" key={tag_index}>
                                                                                        <div className="tag">
                                                                                            {tag_item.tag_name}
                                                                                        </div>
                                                                                        <i className="point"></i>
                                                                                    </div>
                                                                                )
                                                                            })}
                                                                        </div>
                                                                    </div>
                                                                    <div className="content-wrapper">
                                                                        <div className="content-main">
                                                                            <div className="title-row">
                                                                                <a href={`/post/${item.item_info.article_info.article_id}`} target="_blank" rel="noreferrer" title={item.item_info.article_info.title} className="title">
                                                                                    <span className="text-highlight">{item.item_info.article_info.title}</span>
                                                                                </a>
                                                                            </div>
                                                                            <div className="abstract">
                                                                                <a href={`/post/${item.item_info.article_info.article_id}`} target="_blank" rel="noreferrer">
                                                                                    {item.item_info.article_info.brief_content}
                                                                                </a>
                                                                            </div>
                                                                            <ul className="action-list jh-timeline-action-area">
                                                                                <li className="item view">
                                                                                    <i></i>
                                                                                    <span>{number_divide(item.item_info.article_info.got_view_count)} 次浏览</span>
                                                                                </li>
                                                                                <li className={`item like${item.item_info.user_interact.is_digg ? ' active' : ''}`}>
                                                                                    <i></i>
                                                                                    <span>{number_divide(item.item_info.article_info.got_digg_count)} 次点赞</span>
                                                                                </li>
                                                                                <li className="item comment">
                                                                                    <i></i>
                                                                                    <span>{number_divide(item.item_info.article_info.got_comment_count)} 条评论</span>
                                                                                </li>

                                                                                {userinfo && article_authorinfo ? (
                                                                                    userinfo.user_id === article_authorinfo.user_id ? (
                                                                                        <li className="item more">
                                                                                            <i onClick={(e) => this.showMore(e)}></i>
                                                                                            <ul className="more-list user-more-list" style={{ display: 'none' }}>
                                                                                                <li className="item" onClick={() => this.redact(item.item_info.article_info.article_id)}>编辑</li>
                                                                                                <li className="item" onClick={() => this.delete(item.item_info.article_info.article_id)}>删除</li>
                                                                                            </ul>
                                                                                        </li>
                                                                                    ) : null
                                                                                ) : null}


                                                                            </ul>
                                                                        </div>
                                                                        {item.item_info.article_info.cover_image !== "" ? (
                                                                            <a href={`/post/${item.item_info.article_info.article_id}`} target="_blank" rel="noreferrer">
                                                                                <img src={item.item_info.article_info.cover_image} alt="" className="lazy thumb" />
                                                                            </a>
                                                                        ) : (undefined)}
                                                                    </div>
                                                                </div>
                                                            </li>
                                                        )
                                                    })
                                                });
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
        article_authorinfo: state.article_authorinfo,
        user_homepage_article: state.user_homepage_article,
    }
}
export default connect(mapStateToProps)(Index);