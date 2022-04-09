import React from 'react'
import { Link } from 'react-router-dom';
import Toast from '../../../../components/toast';
import http from '../../../../server/instance';
import servicePath from '../../../../server/api';

class index extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            user_List: [],
        };
    }

    componentDidMount() {
        this.ranking(3);
    }

    ranking = (capa) => {
        const _this = this;
        const api = servicePath.user_api__author_ranking;
        http.get(api, {
            params: {
                capacity: capa,
            },
            sign: false,
            token: true
        }, false).then(function (callback) {
            if (callback.data.err_no !== 0) {
                Toast.error(callback.data.err_msg, 3000);
                return;
            }
            _this.setState({ user_List: callback.data.data });
        })
    }

    render() {
        return (
            <div className="sidebar-block user-block">
                <div className="recommend-author-block sticky-author-block">
                    <header className="user-block-header">🎖️作者榜</header>
                    <div className="user-list">

                        {(() => {
                            if (this.state.user_List.length === 0) {
                                return (
                                    <div className="byte-loading">
                                        <div className="byte-loading__spinner">
                                            <i className="circular byte-icon byte-icon--loading">
                                                <svg t="1553157954893" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="2605">
                                                    <path d="M980.752 313.697c-25.789-60.972-62.702-115.725-109.713-162.736-47.012-47.011-101.764-83.924-162.736-109.713C645.161 14.542 578.106 1 509 1c-2.242 0-4.48 0.015-6.715 0.043-16.567 0.211-29.826 13.812-29.615 30.38 0.209 16.438 13.599 29.618 29.99 29.618l0.39-0.002c1.98-0.026 3.963-0.039 5.95-0.039 61.033 0 120.224 11.947 175.93 35.508 53.82 22.764 102.162 55.359 143.683 96.879s74.115 89.862 96.88 143.683C949.054 392.776 961 451.967 961 513c0 16.568 13.432 30 30 30s30-13.432 30-30c0-69.106-13.541-136.162-40.248-199.303z" p-id="2606"></path>
                                                </svg>
                                            </i>
                                        </div>
                                    </div>
                                )
                            } else if (this.state.user_List.length > 0) {
                                return (
                                    this.state.user_List.map((item, index) => {
                                        return (
                                            <div key={index} className="item">
                                                <div className="link">
                                                    <Link to={`/user/${item.user_id}`} target="_blank" rel="noreferrer">
                                                        <div className="avatar bounce-effect" onClick={this.showMenu}
                                                            style={{ backgroundImage: `url(${item.avatar_large ? item.avatar_large : ''})` }}>
                                                        </div>
                                                    </Link>
                                                    <div className="user-info">
                                                        <Link to={`/user/${item.user_id}`} target="_blank" rel="noreferrer" className="username">
                                                            <span className="ranking-name">{item.user_name}</span>
                                                            <span className="ranking-rank">
                                                                <img
                                                                    src={require("../../../../assets/images/lv/lv" + item.level + ".svg").default}
                                                                    alt={`lv-${item.level}`}
                                                                />
                                                            </span>
                                                        </Link>
                                                        <div className="position">{item.job_title}</div>
                                                    </div>
                                                </div>
                                            </div>
                                        )
                                    })
                                )
                            }
                        })()}

                        <Link key={index} to={"/asain/authors"} className="item" target="_blank" rel="noreferrer">
                            <div className="more">
                                <span>完整榜单</span>
                                <i className="icon ion-chevron-right"></i>
                            </div>
                        </Link>

                    </div>
                </div>
            </div>
        )
    }
}

export default index;