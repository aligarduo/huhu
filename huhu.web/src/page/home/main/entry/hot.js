import React from 'react';
import { connect } from 'react-redux';
import store from '../../../../redux/index';
import http from '../../../../server/instance';
import servicePath from '../../../../server/api';
import Toast from '../../../../components/toast';
import Loading from '../../../../components/loading';
import numbers_help from '../../../../utils/numbers_help';
const { number_divide_thousands } = numbers_help();

class Hot extends React.Component {
    componentDidMount() {
        this.hot_list(20);
    }
    hot_list = (capacity) => {
        const api = servicePath.content_api__article_hot;
        http.get(api, {
            params: {
                capacity: capacity,
            },
            sign: false,
            token: true,
        }, true).then(function (callback) {
            if (callback.data.err_no !== 0) {
                Toast.info(callback.data.err_msg, 2500);
                return;
            }
            store.dispatch({
                type: "ARTICLE_HOT",
                article_hot: callback.data.data
            })
        })
    }

    

    render() {
        const { article_hot } = this.props;
        return (
            <div className='body-list-wrap'>
                {(() => {
                    switch (true) {
                        case (article_hot.length === 0): return <Loading />;
                        case (article_hot.length > 0):
                            return (
                                article_hot.map((item, index) => {
                                    return (
                                        <li className="hot entry-item" key={index}>
                                            <div className="hot-content">
                                                <div className='hot-index'>{index + 1}</div>
                                                <div className="hot-content-wrapper">
                                                    <div className="content-main">
                                                        <div className="title-row">
                                                            <a href={'/post/' + item.article_info.article_id} target="_blank" rel="noreferrer" className="hot-title">
                                                                <span className="text-highlight">{item.article_info.title}</span>
                                                            </a>
                                                        </div>
                                                        <div className="hot-abstract">
                                                            <a href={'/post/' + item.article_info.article_id} target="_blank" rel="noreferrer">{item.article_info.brief_content}</a>
                                                        </div>
                                                        <ul className="action-list jh-timeline-action-area">
                                                            <li className="item hot">
                                                                <i></i>
                                                                <span>{number_divide_thousands(item.heat_out)} 次浏览</span>
                                                            </li>
                                                        </ul>
                                                    </div>
                                                    {item.article_info.cover_image === '' ? null : (
                                                        <div className="lazyload-wrapper">
                                                            <a href={'/post/' + item.article_info.article_id} target="_blank" rel="noreferrer">
                                                                <img src={item.article_info.cover_image} alt="" className="lazy thumb" />
                                                            </a>
                                                        </div>
                                                    )}
                                                </div>
                                            </div>
                                        </li>
                                    )
                                })
                            )
                        default: break;
                    }
                })()}
                {article_hot.length > 0 ? <div className='footer-list-wrap'>没有更多内容了</div> : null}
            </div>
        )
    }
}

const mapStateToProps = (state) => {
    return {
        article_hot: state.article_hot,
    }
}
export default connect(mapStateToProps)(Hot);