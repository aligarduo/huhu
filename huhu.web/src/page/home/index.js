import './home.css';
import React from 'react';
import { connect } from 'react-redux';
import NProgress from 'nprogress';
import browser_help from '../../utils/browser_help.js';
import http from '../../server/instance';
import servicePath from '../../server/api';
import store from '../../redux/index';
import Main from './main';
import Aside from './aside';
import Loading from '../../components/loading';

class home extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            cursor: 0,
            over_loading: true,
            active: 0,
            has_more: true,
        };
    }

    componentDidMount() {
        NProgress.start();
        //重置redux
        this.reset_redux();
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
        NProgress.done();
    }
    //重置redux
    reset_redux = () => {
        store.dispatch({
            type: "RECOMMEND_FEED",
            recommend_feed: [],
        })
    }
    //到达底部
    reach_bottom = () => {
        const { PullOnLoading } = browser_help();
        const { recommend_feed } = this.props;
        PullOnLoading(60, () => {
            if (this.state.has_more && this.state.over_loading && recommend_feed.length > 0) {
                this.setState({ over_loading: false, cursor: this.state.cursor + 20 }, () => {
                    this.recommend_all(this.state.cursor);
                });
            }
        });
    }
    //推荐
    recommend_all = (cursor) => {
        let token = localStorage.getItem("passport_csrf_token");
        let flag = false;
        if (token) flag = true;
        const { recommend_feed } = this.props;
        const _this = this;
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
                if (!callback.data.has_more) {
                    _this.setState({ has_more: false, over_loading: false });
                }
                _this.setState({ over_loading: true });
            }
        })
    }



    render() {
        const { recommend_feed } = this.props;
        return (
            <main className="container main-container with-view-nav">
                <div className="index-view">
                    <div className="timeline-content">
                        <Main></Main>
                        <Aside></Aside>
                        {(() => {
                            if (recommend_feed.length > 0 && this.state.has_more && this.state.over_loading) {
                                return (
                                    <div className="bott-box">
                                        <Loading />
                                    </div>
                                )
                            }
                        })()}
                    </div>
                </div>
            </main>
        )
    }
}

const mapStateToProps = (state) => {
    return {
        recommend_feed: state.recommend_feed,
    }
}
export default connect(mapStateToProps)(home);