import React from 'react';
import { Link } from 'react-router-dom';
import url_help from '../../../utils/url_help';
const { urlparam } = url_help();

class Navigation extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            // nav_List: [{
            //     url: '/',
            //     content: '首页',
            // }, {
            //     url: '/topic',
            //     content: '发现',
            // }, {
            //     url: '/video',
            //     content: '视频',
            // }, {
            //     url: '/let-me-answer',
            //     content: '我来回答',
            // }],
            nav_List: [{
                url: '/',
                content: '首页',
            }],
            nav: '',
            small_menu_state: false,
        }
    }

    componentDidMount() {
        document.addEventListener('click', (e) => {
            this.setState({
                small_menu_state: false
            })
        });
        this.setState({ nav: urlparam(window.location.href, 3) })
    }

    //阻止冒泡
    stopPropagation(e) {
        e.nativeEvent.stopImmediatePropagation();
    }

    small_menu = (e) => {
        this.stopPropagation(e);
        this.setState({
            small_menu_state: !this.state.small_menu_state
        });
    }

    render() {
        return (
            <li className="main-nav-list">
                <div className="phone-show-menu" onClick={(e) => this.small_menu(e)}>
                    {(() => {
                        let nav = null;
                        this.state.nav_List.forEach((item) => {
                            if (this.state.nav === item.url.replace("/", "")) {
                                nav = <span>{item.content}</span>;
                            }
                        })
                        if (nav === null) nav = <span>首页</span>;
                        return nav;
                    })()}
                    <svg width="12" height="12" viewBox="0 0 12 12" fill="none" xmlns="http://www.w3.org/2000/svg" className={`unfold-icon${this.state.small_menu_state ? ' active' : ''}`} ><path d="M2.45025 4.82431C2.17422 4.49957 2.40501 4.00049 2.83122 4.00049H9.16878C9.59498 4.00049 9.82578 4.49957 9.54975 4.82431L6.38097 8.55229C6.1813 8.78719 5.8187 8.78719 5.61903 8.55229L2.45025 4.82431Z"></path></svg>
                </div>
                <ul className={`phone-hide${this.state.small_menu_state ? ' show' : ''}`} >
                    {this.state.nav_List.map((item, index) => {
                        return (
                            <li key={index}
                                className={`header-nav-item link-item${this.state.nav === item.url.replace("/", "") ? ' active' : ''}`}>
                                <Link to={item.url}>{item.content}</Link>
                            </li>
                        )
                    })}
                </ul>
            </li>
        )
    }
}

export default Navigation;