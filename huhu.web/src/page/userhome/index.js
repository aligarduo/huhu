import './style/userhome_1.css';
import './style/userhome_2.css';
import './style/userhome_3.css';
import './style/userhome_4.css';
import './style/userhome_5.css';
import React from 'react';
import { connect } from 'react-redux';
import Toast from '../../components/toast';
import store from '../../redux/index';
import http from '../../server/instance';
import Main from './module/main';
import Aside from './module/aside';
import servicePath from '../../server/api';
import browser_help from '../../utils/browser_help.js';

class UserHome extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            user_id: 0,
            cursor: 0,
            over_loading: true,
        }
    }

    componentDidMount() {
        this.setState({ user_id: this.props.match.params.user_id }, () => {
            this.user_Into_load(this.state.user_id);
            this.data_Into_load(this.state.user_id, this.state.cursor);
        });
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

    user_Into_load = (user_id) => {
        const api = servicePath.user_api__get_author_info;
        http.get(api, {
            params: { user_id: user_id }
        }, { sign: false, token: false }, false).then(function (callback) {
            if (callback.data.err_no !== 0) {
                Toast.error(callback.data.err_msg, 2500);
                return;
            }
            store.dispatch({
                type: "ARTICLE_AUTHORINFO",
                article_authorinfo: callback.data.data
            })
        })
    }

    data_Into_load = (user_id, cursor) => {
        const { user_homepage_article } = this.props;
        const _this = this;
        const api = servicePath.content_api__article_query_list;
        http.post(api, {
            cursor: cursor,
            limit: 20,
            user_id: user_id
        }, { sign: false, token: false }, false).then(function (callback) {
            if (callback.data.err_no === 0) {
                store.dispatch({
                    type: "OVERALL_DATA",
                    overall_data: callback.data,
                })
                store.dispatch({
                    type: "USER_HOMEPAGE_ARTICLE",
                    user_homepage_article: [...user_homepage_article, callback.data.data],
                })
                if (callback.data.has_more) {
                    _this.setState({ over_loading: true });
                }
            }
        })
    }

    reach_bottom = () => {
        const { PullOnLoading } = browser_help();
        PullOnLoading(60, () => {
            if (this.state.over_loading) {
                this.setState({ over_loading: false, cursor: this.state.cursor + 20 }, () => {
                    this.data_Into_load(this.state.user_id, this.state.cursor);
                });
            }
        });
    }
    

    render() {
        return (
            <main className="container main-container">
                <div className="view user-view">
                    <Main />
                    <Aside />
                </div>
            </main>
        )
    }
}


const mapStateToProps = (state) => {
    return {
        user_homepage_article: state.user_homepage_article,
    }
}
export default connect(mapStateToProps)(UserHome);