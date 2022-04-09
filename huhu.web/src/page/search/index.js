import './search.css';
import React from 'react';
import { connect } from 'react-redux';
import marked from 'marked';
import http from '../../server/instance';
import servicePath from '../../server/api';
import store from '../../redux/index';
import LazyLoad from 'react-lazyload';
import Loading from '../../components/loading';
import Empty from '../../components/empty';
import numbers_help from '../../utils/numbers_help.js';
import timestamp_help from '../../utils/timestamp_help.js';
const { number_divide } = numbers_help();
const { otherday } = timestamp_help();

class Search extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            key_word: '',
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
        this.setState({ key_word: decodeURI(this.props.history.location.search.substr(7)) }, () => {
            this.search(this.state.key_word);
        })
    }

    search = (query) => {
        const { search } = this.props;
        const api = servicePath.search_api__search;
        http.post(api, {
            "cursor": "0",
            "limit": 20,
            "key_word": query,
            "id_type": 1,
            "sort_type": 2
        }, { sign: false, token: false }, true).then(function (callback) {
            if (callback.data.err_no === 0) {
                store.dispatch({
                    type: "SEARCH",
                    search: [...search, callback.data.data],
                })
            }
        })
    }

    highlight = (title) => {
        title = title.toLowerCase()
        let key = this.state.key_word.toLowerCase();
        let index = title.lastIndexOf(key);
        let before = title.substring(0, index);
        let after = title.substring(index + key.length, title.length);
        let middle = title.match(RegExp(key, 'ig'));
        middle = middle === null ? "" : "<em>" + middle + "</em>"
        let res = before + middle + after;
        return res;
    }


    render() {
        const { search } = this.props;
        return (
            <div className='main-container'>
                <div className="search-entry">
                    {search[0] ? search[0].length === 0 ? <Empty /> : (
                        <ul className="search-entry-body shadow">
                            {search && search.map((item) => {
                                return item.map((item_data, index) => {
                                    return (
                                        <li key={index} className="search-item">
                                            <div className="content-box">
                                                <div className="meta-container">
                                                    <div className="user-message">
                                                        <a href={'/user/' + item_data.item_info.author_user_info.user_id} target="_blank" rel="noreferrer" className="userbox">
                                                            <div className="user-popover-box">
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
                                                                    <a href={'/tag/' + tag_item.tag_id} className="tag">
                                                                        {tag_item.tag_name}
                                                                    </a>
                                                                    <i className="point"></i>
                                                                </div>
                                                            )
                                                        })}
                                                    </div>
                                                </div>
                                                <div className="content-wrapper">
                                                    <div className="content-main">
                                                        <div className="title-row">
                                                            <a href={'/post/' + item_data.item_info.article_id} target="_blank" rel="noreferrer" className="search-title">
                                                                <span dangerouslySetInnerHTML={{ __html: marked(this.highlight(item_data.item_info.article_info.title)) }}></span>
                                                            </a>
                                                        </div>
                                                        <div className="abstract">
                                                            <a href={'/post/' + item_data.item_info.article_id} target="_blank" rel="noreferrer" className="search-body">
                                                                <span dangerouslySetInnerHTML={{ __html: marked(this.highlight(item_data.item_info.article_info.brief_content)) }}></span>
                                                            </a>
                                                        </div>
                                                        <ul
                                                            className="action-list jh-timeline-action-area">
                                                            <li className="item view">
                                                                <i></i>
                                                                <span>{number_divide(item_data.item_info.article_info.got_view_count)}</span>
                                                            </li>
                                                            <li className={`item like${item_data.item_info.user_interact.is_digg ? " active" : ""}`}>
                                                                <i></i>
                                                                <span>{number_divide(item_data.item_info.article_info.got_digg_count)}</span>
                                                            </li>
                                                            <li className="item comment">
                                                                <i></i>
                                                                <span>{number_divide(item_data.item_info.article_info.got_comment_count)}</span>
                                                            </li>
                                                        </ul>
                                                    </div>
                                                    {item_data.item_info.article_info.cover_image ? (
                                                        <LazyLoad scroll={true} offset={200} height={100}>
                                                            <a href={'/post/' + item_data.item_info.article_id} target="_blank" rel="noreferrer">
                                                                <img src={item_data.item_info.article_info.cover_image} className="lazy thumb" alt="" />
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
                    ) : <Loading />}
                </div>
            </div>
        )
    }
}

const mapStateToProps = (state) => {
    return {
        search: state.search,
    }
}
export default connect(mapStateToProps)(Search);