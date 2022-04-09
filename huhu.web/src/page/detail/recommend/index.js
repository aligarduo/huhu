import React from 'react';
import http from '../../../server/instance';
import servicePath from '../../../server/api';
import browser_help from '../../../utils/browser_help.js';
import timestamp_help from '../../../utils/timestamp_help.js';
const { otherday } = timestamp_help();

class index extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            flag: true,
            recommend: [],
            cursor: 0,
            over_loading: true,
        }
    }

    componentDidMount() {
        if (this.state.flag) {
            this.recommend_all(0);
            this.setState({ flag: false })
        }
        //滑倒底部自动加载
        const _this = this;
        //PC端
        window.addEventListener("scroll", () => {
            //到达底部
            _this.reach_bottom();
        });
        //移动端
        window.addEventListener('touchmove', () => {
            //到达底部
            _this.reach_bottom();
        });
    }
    //推荐
    recommend_all = (cursor) => {
        const _this = this;
        const api = servicePath.recommend_api__recommend_all_feed
        http.post(api, {
            cursor: cursor,
            limit: 15
        }, { sign: false, token: false }, false).then(function (callback) {
            if (callback.data.err_no === 0) {
                _this.setState({ recommend: [..._this.state.recommend, callback.data.data] });
                if (callback.data.has_more) {
                    _this.setState({ over_loading: true });
                }else{
                    _this.setState({ over_loading: false });
                }
            }
        })
    }
    //到达底部
    reach_bottom = () => {
        const { PullOnLoading } = browser_help();
        PullOnLoading(60, () => {
            if (this.state.over_loading) {
                this.setState({ over_loading: false, cursor: this.state.cursor + 20 }, () => {
                    this.recommend_all(this.state.cursor);
                });
            }
        });
    }


    render() {
        return (
            <React.Fragment>
                {this.state.recommend.length <= 0 ? null : (
                    <div className="main-area recommended-area">
                        <div className="recommended-entry-list-title">相关文章推荐</div>
                        <ul className="recommended-entry-list shadow">
                            {this.state.recommend.map((_item) => {
                                return _item.map((item, index) => {
                                    return (
                                        <li className="recommend-item" key={index}>
                                            <div className="entry-box">
                                                <div className="recommend-entry">
                                                    <div className="entry-link">
                                                        <div className="recommend-content-box">
                                                            <div className="meta-container">
                                                                <div className="user-message">
                                                                    <a href={`/user/${item.item_info.author_user_info.user_id}`} target="_blank" rel="noopener noreferrer" className="userbox">
                                                                        <div className="user-popover-box">{item.item_info.author_user_info.user_name}</div>
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
                                                                        <a href={`/post/${item.item_info.article_info.article_id}`} target="_blank" rel="noopener noreferrer" className="title">
                                                                            <span className="text-highlight">{item.item_info.article_info.title}</span>
                                                                        </a>
                                                                    </div>
                                                                    <div className="abstract">
                                                                        <a href={`/post/${item.item_info.article_info.article_id}`} target="_blank" rel="noopener noreferrer">
                                                                            {item.item_info.article_info.brief_content}
                                                                        </a>
                                                                    </div>
                                                                    <ul className="action-list jh-timeline-action-area">
                                                                        <li className="item view">
                                                                            <i></i>
                                                                            <span>{item.item_info.article_info.got_view_count} 次浏览</span>
                                                                        </li>
                                                                        <li className="item like">
                                                                            <i></i>
                                                                            <span>{item.item_info.article_info.got_digg_count} 次点赞</span>
                                                                        </li>
                                                                        <li className="item comment">
                                                                            <i></i>
                                                                            <span>{item.item_info.article_info.got_comment_count} 条评论</span>
                                                                        </li>
                                                                    </ul>
                                                                </div>
                                                                {item.item_info.article_info.cover_image !== "" ? (
                                                                    <a href={`/post/${item.item_info.article_info.article_id}`} target="_blank" rel="noopener noreferrer">
                                                                        <img src={item.item_info.article_info.cover_image} alt="" className="lazy thumb" />
                                                                    </a>
                                                                ) : (undefined)}
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </li>
                                    )
                                })
                            })}
                        </ul>
                    </div>
                )}
            </React.Fragment>
        )
    }
}

export default index;