import React from 'react';
import { connect } from 'react-redux';
import Toast from '../../../../components/toast'
import store from '../../../../redux/index';
import http from '../../../../server/instance'
import servicePath from '../../../../server/api'

class UserNav extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            show_nav: false
        }
        this.showNav = this.showNav.bind(this);
    }

    componentDidMount() {
        // 在 document 上绑定点击事件，隐藏弹出层
        document.addEventListener('click', (e) => {
            this.setState({
                show_nav: false
            })
        });
        this.GetUserInfo();
    }

    GetUserInfo = () => {
        const t = this;
        const api = servicePath.userinfo_api;
        http.get(api, {
            params: {},
            sign: false,
            token: true
        }, false).then(function (callback) {
            if (callback.data.err_no !== 0) {
                Toast.error(callback.data.err_msg, 3000);
                return;
            }
            store.dispatch({
                type: "USERINFO",
                userinfo: callback.data.data
            })
            t.setState({ userinfo: callback.data.data });
        })
    }

    // 封装后的阻止冒泡功能
    stopPropagation(e) {
        e.nativeEvent.stopImmediatePropagation();
    }

    showNav(e) {
        this.stopPropagation(e);
        this.setState({
            show_nav: !this.state.show_nav
        });
    }

    render() {
        const { userinfo } = this.props;
        //数据源
        let dataList = [
            [
                {
                    url: "/editor/drafts/new?v=2",
                    designation: "写文章",
                },
                {
                    url: "/editor/drafts",
                    designation: "草稿",
                }
            ], [
                {
                    url: `/user/${userinfo.user_id}`,
                    designation: "我的主页",
                },
                {
                    url: `/user/${userinfo.user_id}/likes`,
                    designation: "我喜欢的",
                }
                ,
                {
                    url: `/user/${userinfo.user_id}/collections`,
                    designation: "我的收藏",
                }
            ]
        ]

        return (
            <nav className="navigator main-navigator with-padding">
                <div className="user-toggle-btn" onClick={this.showNav}
                    style={{ backgroundImage: `url(${userinfo ? userinfo.avatar_large : ''})` }}>
                </div>
                <div className="menu root-menu"
                    style={{ display: this.state.show_nav ? 'block' : 'none' }}
                    onClick={this.stopPropagation}>
                    {
                        dataList.map((item, index) => {
                            return (
                                <div key={index} className="item-group">{
                                    item.map((t, i) => {
                                        return <a key={i} href={t.url} className="item">{t.designation}</a>
                                    })
                                }</div>
                            )
                        })
                    }
                </div>
            </nav>
        )
    }
}

const mapStateToProps = (state) => {
    return {
        userinfo: state.userinfo,
    }
}
export default connect(mapStateToProps)(UserNav);