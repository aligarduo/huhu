import './header.css';
import React from 'react';
import Navigation from './module/navigation';
import Search from './module/search';
import Menu from './module/menu';
import Notification from './module/notification';
import Loginbtn from './module/loginbtn';
import { connect } from 'react-redux';
import store from '../../redux/index';
import http from '../../server/instance';
import servicePath from '../../server/api';
import Toast from '../toast';

class index extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            oldTop: 0,
            newTop: 0,
            login_state: false,
        }
        this.onScroll = this.Scroll.bind(this);
    }

    componentDidMount() {
        document.onscroll = this.Scroll;
        //判断登录状态
        let token = localStorage.getItem("passport_csrf_token");
        if (token) {
            this.setState({ login_state: true });
            this.GetUserInfo();
        }
    }

    GetUserInfo = () => {
        const _this = this;
        const api = servicePath.userinfo_api;
        http.get(api, {
            params: {},
            sign: false,
            token: true
        }, false).then(function (callback) {
            if (callback.data.err_no !== 0) {
                Toast.error("登录失效，请重新登录", 2500);
                localStorage.removeItem('passport_csrf_token');
                _this.setState({ login_state: false });
                return;
            }
            store.dispatch({
                type: "USERINFO",
                userinfo: callback.data.data
            })
        })
    }

    // 自己造轮子，导航栏效果
    Scroll = () => {
        var scrollTop = document.documentElement.scrollTop || window.pageYOffset || document.body.scrollTop;
        this.setState({ newTop: scrollTop })
        if (this.state.newTop > this.state.oldTop) {
            //console.log("下")
            if (scrollTop > 180) {
                if (this.state.newTop - this.state.oldTop > 20) {
                    store.dispatch({
                        type: "SCROLL",
                        scroll: false
                    })
                }
            }
        }
        else {
            //console.log("上")
            if (this.state.oldTop - this.state.newTop > 20) {
                store.dispatch({
                    type: "SCROLL",
                    scroll: true
                })
            }
        }
        this.setState({ oldTop: this.state.newTop })
    }

    render() {
        const { userinfo } = this.props;
        return (
            <div className="main-header-box">
                <header className={`main-header${this.props.scroll ? " visible" : ""}`}>
                    <div className="container main-container-s">
                        <a href="/" className="logo">
                            <img src={require('../../assets/images/logo/logo_icon.svg').default} alt="乎乎" className="logo-img" />
                            <img src={require('../../assets/images/logo/logo_icon_mobile.svg').default} alt="乎乎" className="mobile" />
                        </a>
                        <nav className="main-nav">
                            <ul className="header-nav-list main-nav-s">
                                <Navigation></Navigation>
                                <Search></Search>
                                {this.state.login_state ? (
                                    <React.Fragment>
                                        <Notification />
                                        <Menu userinfo={userinfo} />
                                    </React.Fragment>
                                ) : (<Loginbtn />)}
                            </ul>
                        </nav>
                    </div>
                </header>
            </div>
        )
    }
}

const mapStateToProps = (state) => {
    return {
        scroll: state.scroll,
        userinfo: state.userinfo,
    }
}
export default connect(mapStateToProps)(index);