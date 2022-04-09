import './detail.css';
import React from 'react';
import { connect } from 'react-redux'
import store from '../../redux/index';
import http from '../../server/instance';
import Loading from './content/loading';
import Content from './content';
import Recommended from './recommend';
import Sidebar from './sidebar';
import Suspended from './suspend';
import servicePath from '../../server/api';
import NotFound from '../../components/404/notfound';

class index extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            find: true,
        }
    }

    componentDidMount() {
        let id = this.props.match.params.id;
        this.detailed(id);
        this.get_comments_list(id);
    }

    //文章详细信息
    detailed = (id) => {
        let token = localStorage.getItem("passport_csrf_token");
        let flag = false;
        if (token) flag = true;
        const api = servicePath.content_api__detail;
        const _this = this;
        http.post(api, {
            "article_id": id
        }, { sign: false, token: flag }, true).then(function (callback) {
            if (callback.data.err_no === 0) {
                store.dispatch({
                    type: "ARTICLE_DETAILED",
                    article_detailed: callback.data,
                })
                return;
            } else if (callback.data.err_no === 1028) {
                _this.setState({ find: false });
                return;
            }
        })
    }

    //文章评论
    get_comments_list = (id) => {
        let token = localStorage.getItem("passport_csrf_token");
        let flag = false;
        if (token) flag = true;
        const api = servicePath.interact_api__comment_list;
        http.post(api, {
            cursor: "0",
            limit: 20,
            item_id: id,
        }, { sign: false, token: flag }, false).then(function (callback) {
            store.dispatch({
                type: "ARTICLE_COMMENTS",
                article_comments: callback.data,
            })
        })
    }

    render() {
        const { article_detailed } = this.props;
        document.title = article_detailed.data ? article_detailed.data.article_info.title : '很快就好...';
        return (
            <React.Fragment>
                {this.state.find ? (
                    <main className="container-detail main-container">
                        <div className="view column-view">
                            {article_detailed ?
                                (<React.Fragment>
                                    <Content></Content>
                                    <Recommended></Recommended>
                                    <Sidebar></Sidebar>
                                    <Suspended></Suspended>
                                </React.Fragment>) :
                                (<Loading></Loading>)
                            }
                        </div>
                    </main>
                ) : (<NotFound />)}
            </React.Fragment>
        )
    }
}

const mapStateToProps = (state) => {
    return {
        loading: state.loading,
        article_detailed: state.article_detailed,
    }
}
export default connect(mapStateToProps)(index);