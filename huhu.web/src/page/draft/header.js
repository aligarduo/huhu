import './draft.css'
import React from 'react';
import { connect } from 'react-redux'
import { Link } from 'react-router-dom';
import Toast from '../../components/toast'
import store from '../../redux';
import http from '../../server/instance'
import servicePath from '../../server/api'

class Header extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            show_menu: false
        }
        this.showMenu = this.showMenu.bind(this);
    }

    //阻止冒泡
    stopPropagation(e) {
        e.nativeEvent.stopImmediatePropagation();
    }

    showMenu(e) {
        this.stopPropagation(e);
        this.setState({
            show_menu: !this.state.show_menu
        });
    }

    componentDidMount() {
        //在document上绑定点击事件，隐藏弹出层
        document.addEventListener('click', () => {
            this.setState({
                show_menu: false
            })
        });
        //判断登录状态
        let token = localStorage.getItem("passport_csrf_token");
        if (token) {
            this.GetUserInfo();
        } else {
            Toast.error("初始化失败，请检查登录设置", 3000);
            return;
        }
    }

    GetUserInfo = () => {
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
        })
    }

    //跳转
    redact = () => {
        window.location.href = "/editor/drafts/new?v=2";
    }

    render() {
        const { userinfo } = this.props;
        return (
            <header className="draft-header">
                <div className="draft-header-content">
                    <Link to={`/`} className="draft-logo"></Link>
                    <button className="draft-new-button" onClick={() => this.redact()}>写文章</button>
                    <nav className="draft-navigator">
                        <div className="draft-toggle-btn" onClick={this.showMenu}
                            style={{ backgroundImage: `url(${userinfo ? userinfo.avatar_large : ''})` }}>
                        </div>
                        <div className="draft-menu root-menu" style={{ display: this.state.show_menu ? 'block' : 'none' }}>
                            <div className="item-group">
                                <a href="/editor/drafts/new" className="item-g">
                                    <svg width="16" height="16" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg" className="fengwei"><path fillRule="evenodd" clipRule="evenodd" d="M14.3335 13.6696C14.5176 13.6696 14.6668 13.8188 14.6668 14.0029V14.6696C14.6668 14.8537 14.5176 15.0029 14.3335 15.0029H1.66683C1.48273 15.0029 1.3335 14.8537 1.3335 14.6696V14.0029C1.3335 13.8188 1.48273 13.6696 1.66683 13.6696H14.3335Z" fill="#86909C"></path><path fillRule="evenodd" clipRule="evenodd" d="M9.8175 1.52987L9.77386 1.48969C9.51193 1.26901 9.1201 1.28199 8.87345 1.52864L8.40329 1.9988L8.40492 2.00037L7.27453 3.13954L10.5036 6.3751L12.1057 4.77295C12.3656 4.51309 12.3661 4.09193 12.107 3.83138L9.8175 1.52987ZM9.50415 7.3746L6.27569 4.14613L1.33325 9.12694V11.6696L1.33508 11.7194C1.36053 12.0643 1.64846 12.3363 1.99992 12.3363H4.5426L9.50415 7.3746Z" fill="#86909C"></path></svg>
                                    <span>写文章</span>
                                </a>
                                <a href="/editor/drafts" className="item-g">
                                    <svg width="16" height="16" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg" className="fengwei"><path fillRule=" evenodd" clipRule="evenodd" d="M2 1.66663C2 1.11434 2.44772 0.666626 3 0.666626H11L14 3.80948V14.3333C14 14.8856 13.5523 15.3333 13 15.3333H3C2.44772 15.3333 2 14.8856 2 14.3333V1.66663ZM4.66675 9.66667C4.66675 9.29848 4.96522 9 5.33341 9H8.00008C8.36827 9 8.66675 9.29848 8.66675 9.66667C8.66675 10.0349 8.36827 10.3333 8.00008 10.3333H5.33341C4.96522 10.3333 4.66675 10.0349 4.66675 9.66667ZM5.33341 6.33337C4.96522 6.33337 4.66675 6.63185 4.66675 7.00004C4.66675 7.36823 4.96522 7.66671 5.33341 7.66671H10.6667C11.0349 7.66671 11.3334 7.36823 11.3334 7.00004C11.3334 6.63185 11.0349 6.33337 10.6667 6.33337H5.33341Z" fill="#86909C"></path><path fillRule="evenodd" clipRule="evenodd" d="M10.3999 1.5L13.3999 4.5H10.3999V1.5Z" fill="white"></path></svg>
                                    <span>草稿</span>
                                </a>
                            </div>
                            <div className="item-group">
                                <Link to={`${userinfo ? '/user/' + userinfo.user_id : ''}`} className="item-g">
                                    <svg width="16" height="16" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg" className="fengwei"><path fillRule="evenodd" clipRule="evenodd" d="M11.3333 4.33333C11.3333 2.49238 9.84095 1 8 1C6.15905 1 4.66667 2.49238 4.66667 4.33333C4.66667 6.17428 6.15905 7.66667 8 7.66667C9.84095 7.66667 11.3333 6.17428 11.3333 4.33333ZM14 12.6667C14 10.0952 11.8409 8.66667 10 8.66667H6C4.15905 8.66667 2 10.0974 2 12.6667V14.6667C2 15.0349 2.29848 15.3333 2.66667 15.3333H13.3333C13.7015 15.3333 14 15.0349 14 14.6667V12.6667Z" fill="#86909C"></path></svg>
                                    <span>我的主页</span>
                                </Link>
                                <Link to={`${userinfo ? '/user/' + userinfo.user_id + '/likes' : ''}`} className="item-g">
                                    <svg width="16" height="16" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg" className="fengwei"><path fillRule="evenodd" clipRule="evenodd" d="M8.89173 0.830916L9.01335 0.887911C9.82399 1.27873 11.0012 2.06767 11.0012 3.33425C11.0012 3.83643 10.7783 4.50245 10.3324 5.33231H12.8585C14.8478 5.33231 15.3047 7.08822 14.8478 8.40394L12.8585 13.6407C12.6904 14.2498 12.1306 14.6723 11.4918 14.6723H3.32845V5.33231H4.6931L7.65981 1.11199C7.84713 0.81396 8.35279 0.585483 8.89173 0.830916ZM2.33333 5.33333V14.6667H1V5.33333H2.33333Z" fill="#86909C"></path></svg>
                                    <span>我赞过的</span>
                                </Link>
                                <Link to={`${userinfo ? '/user/' + userinfo.user_id + '/collections' : ''}`} className="item-g">
                                    <svg width="16" height="16" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg" className="fengwei"><path fillRule="evenodd" clipRule="evenodd" d="M4.07409 14.913C3.78885 15.0502 3.44641 14.9301 3.30922 14.6449C3.26321 14.5492 3.24467 14.4427 3.25568 14.3371L3.67332 10.3317C3.69013 10.1705 3.63789 10.0097 3.52953 9.88917L0.813641 6.86787C0.602045 6.63248 0.621335 6.27013 0.856725 6.05854C0.931303 5.9915 1.02217 5.9452 1.12024 5.92427L5.09331 5.07635C5.25183 5.04251 5.38861 4.94314 5.46977 4.80283L7.50394 1.28624C7.66242 1.01226 8.013 0.918629 8.28698 1.07711C8.37379 1.12732 8.4459 1.19943 8.49611 1.28624L10.5303 4.80283C10.6114 4.94314 10.7482 5.04251 10.9067 5.07635L14.8798 5.92427C15.1894 5.99033 15.3867 6.29482 15.3207 6.60436C15.2997 6.70243 15.2534 6.7933 15.1864 6.86787L12.4705 9.88917C12.3622 10.0097 12.3099 10.1705 12.3267 10.3317L12.7444 14.3371C12.7772 14.6519 12.5486 14.9337 12.2338 14.9665C12.1282 14.9775 12.0216 14.959 11.926 14.913L8.24843 13.1442C8.09143 13.0687 7.90862 13.0687 7.75162 13.1442L4.07409 14.913Z" fill="#86909C"></path></svg>
                                    <span>我的收藏</span>
                                </Link>
                            </div>
                        </div>
                    </nav>
                </div>
            </header>
        )
    }
}

const mapStateToProps = (state) => {
    return {
        userinfo: state.userinfo,
    }
}
export default connect(mapStateToProps)(Header);