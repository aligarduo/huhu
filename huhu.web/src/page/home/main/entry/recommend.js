import React from 'react';
import { connect } from 'react-redux';
import http from '../../../../server/instance';
import servicePath from '../../../../server/api';
import store from '../../../../redux/index';
import LazyLoad from 'react-lazyload';
import Loading from '../../../../components/loading';
import Empty from '../../../../components/empty';

import Video from '../../../video';

import numbers_help from '../../../../utils/numbers_help.js';
import timestamp_help from '../../../../utils/timestamp_help.js';
const { number_divide_thousands } = numbers_help();
const { otherday } = timestamp_help();

class index extends React.Component {
    componentDidMount() {
        //推荐
        this.recommend_all(0);
    }
    //推荐
    recommend_all = (cursor) => {
        let token = localStorage.getItem("passport_csrf_token");
        let flag = false;
        if (token) flag = true;
        const { recommend_feed } = this.props;
        const api = servicePath.recommend_api__recommend_all_feed
        http.post(api, {
            cursor: cursor,
            limit: 20
        }, { sign: false, token: flag }, true).then(function (callback) {
            if (callback.data.err_no === 0) {
                store.dispatch({
                    type: "RECOMMEND_FEED",
                    recommend_feed: [...recommend_feed, callback.data.data],
                })
            }
        })
    }
    onmouseover = (e, author_user_info) => {
        let html = `
            <div class='userinfo'>
                <div class='left avatar' style='background-image: url(${author_user_info.avatar_large})'></div>
                <div class='right'>
                    <span class="name">${author_user_info.user_name}</span>
                    <span class="rank"><img src="/static/media/lv6.fe0c3496.svg" alt="lv-6" /></span>
                    <div class='position'>${author_user_info.job_title}</div>
                </div>
            </div>`
        let div = document.createElement('div');
        div.className = "user-details shadow";
        div.innerHTML = html;
        e.target.parentNode.parentNode.parentNode.appendChild(div);
    }
    onmouseout = (e) => {
        let parentNode = e.target.parentNode.parentNode.parentNode;
        let lastChild = e.target.parentNode.parentNode.parentNode.lastChild;
        parentNode.removeChild(lastChild);
    }



    render() {
        const { recommend_feed } = this.props;
        return (
            <React.Fragment>
                <div className="entry-list-wrap">
                    {(() => {
                        switch (true) {
                            case (recommend_feed.length === 0): return <Loading />;
                            case (recommend_feed[0].length === 0): return <Empty />;
                            case (recommend_feed[0].length > 0):
                                return (
                                    <ul className="entry-list">
                                        {recommend_feed && recommend_feed.map((item) => {
                                            return item.map((item_data, index) => {
                                                return (

                                                    // <li key={index} className="entry-item">
                                                    //     <Video></Video>
                                                    // </li>



                                                    <li key={index} className="entry-item">
                                                        <div className="entry-content">
                                                            <div className="meta-container">
                                                                <div className="user-message">
                                                                    <a href={'/user/' + item_data.item_info.author_user_info.user_id} target="_blank" rel="noreferrer" className="userbox">
                                                                        <div className="user-popover-box"
                                                                            onMouseOver={(e) => this.onmouseover(e, item_data.item_info.author_user_info)}
                                                                            onMouseOut={(e) => this.onmouseout(e)}>
                                                                            {item_data.item_info.author_user_info.user_name}
                                                                        </div>
                                                                    </a>
                                                                </div>
                                                                <div className="dividing"></div>
                                                                <div className="date">
                                                                    {otherday(item_data.item_info.article_info.ctime)}
                                                                </div>
                                                                <div className="dividing"></div>
                                                                <div className="tag_list">
                                                                    {item_data.item_info.tags.map((tag_item, tag_index) => {
                                                                        return (
                                                                            <div className="tag" key={tag_index}>
                                                                                {tag_item.tag_name}
                                                                                <i className="point"></i>
                                                                            </div>
                                                                        )
                                                                    })}
                                                                </div>
                                                            </div>
                                                            <div className="content-wrapper">
                                                                <div className="content-main">
                                                                    <div className="title-row">
                                                                        <a href={'/post/' + item_data.item_info.article_id}
                                                                            target="_blank"
                                                                            rel="noreferrer"
                                                                            className="title">
                                                                            <span className="text-highlight">{item_data.item_info.article_info.title}</span>
                                                                        </a>
                                                                    </div>
                                                                    <div className="abstract">
                                                                        <a href={'/post/' + item_data.item_info.article_id} target="_blank" rel="noopener noreferrer">
                                                                            {item_data.item_info.article_info.brief_content}
                                                                        </a>
                                                                    </div>
                                                                    <ul
                                                                        className="action-list jh-timeline-action-area">
                                                                        <li className="item view">
                                                                            <i></i>
                                                                            <span>{number_divide_thousands(item_data.item_info.article_info.got_view_count)} 次浏览</span>
                                                                        </li>
                                                                        <li className={`item like${item_data.item_info.user_interact.is_digg ? " active" : ""}`}>
                                                                            <i></i>
                                                                            <span>{number_divide_thousands(item_data.item_info.article_info.got_digg_count)} 次点赞</span>
                                                                        </li>
                                                                        <li className="item comment">
                                                                            <i></i>
                                                                            <span>{number_divide_thousands(item_data.item_info.article_info.got_comment_count)} 条评论</span>
                                                                        </li>
                                                                    </ul>
                                                                </div>
                                                                {item_data.item_info.article_info.cover_image ? (
                                                                    <LazyLoad scroll={true} offset={200} height={100}>
                                                                        <a href={'/post/' + item_data.item_info.article_id} target="_blank" rel="noreferrer">
                                                                            <img src={item_data.item_info.article_info.cover_image} alt=""
                                                                                className="lazy thumb"
                                                                                onError={(e) => {
                                                                                    e.nativeEvent.target.offsetParent.classList.add('load-error');
                                                                                    e.nativeEvent.target.classList.remove('lazy');
                                                                                    e.nativeEvent.target.classList.add('pic-load-error');
                                                                                    e.nativeEvent.target.src = require('../../../../assets/images/svg/image-loading-failed.svg').default;
                                                                                }}
                                                                            />
                                                                        </a>
                                                                    </LazyLoad>
                                                                ) : (undefined)}
                                                            </div>
                                                        </div>
                                                    </li>
                                                )
                                            })
                                        })}
                                    </ul>
                                )
                            default: break;
                        }
                    })()}
                </div>

            </React.Fragment>
        )
    }
}

const mapStateToProps = (state) => {
    return {
        recommend_feed: state.recommend_feed
    }
}
export default connect(mapStateToProps)(index);