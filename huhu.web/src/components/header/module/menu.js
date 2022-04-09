import React from 'react';
import Toast from '../../toast'
import http from '../../../server/instance'
import servicePath from '../../../server/api'

class Menu extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            show_menu: false,
        }
        this.showMenu = this.showMenu.bind(this);
    }

    componentDidMount() {
        //在document上绑定点击事件，隐藏弹出层
        document.addEventListener('click', (e) => {
            this.setState({
                show_menu: false
            })
        });
    }

    //退出登录
    login_out() {
        let msg = window.confirm("确定退出登录吗？每一片贫瘠的土地都需要坚定的挖掘者！");
        if (msg) {
            const api = servicePath.logout_api;
            http.get(api, {
                params: {},
                sign: true,
                token: true
            }, false).then(function (callback) {
                if (callback.data.err_no === 0) {
                    localStorage.removeItem('passport_csrf_token');
                    window.location.reload();
                }
            })
        }
    }

    //阻止冒泡
    stopPropagation(e) {
        e.nativeEvent.stopImmediatePropagation();
    }

    //显示菜单
    showMenu(e) {
        this.stopPropagation(e);
        this.setState({
            show_menu: !this.state.show_menu,
        });
    }

    is_login = () => {
        let token = localStorage.getItem("passport_csrf_token");
        if (token)
            return true;
        else
            return false;
    }

    //写文章
    write_article(e) {
        if (!this.is_login()) {
            Toast.error("初始化失败，请检查登录设置。", 2500);
            return;
        }
        this.stopPropagation(e);
        window.location.href = "/editor/drafts/new?v=2";
    }

    editor_drafts(e) {
        if (!this.is_login()) {
            Toast.error("初始化失败，请检查登录设置。", 2500);
            return;
        }
        this.stopPropagation(e);
        window.location.href = "/editor/drafts";
    }


    render() {
        const { userinfo } = this.props;
        return (
            <React.Fragment>
                <li className="nav-item">
                    <div className="avatar immediate" onClick={this.showMenu}
                        style={{ backgroundImage: `url(${userinfo ? userinfo.avatar_large : ''})` }}>
                    </div>
                    {userinfo && this.state.show_menu ? (
                        <ul className="nav-menu user-dropdown-list no-touch not-select">
                            <div className="nav-menu-item-group">
                                <li className="nav-menu-item">
                                    <a onClick={this.write_article.bind(this)}>
                                        <svg width="16" height="16" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg" className="fengwei">
                                            <path fillRule="evenodd" clipRule="evenodd" d="M14.3335 13.6696C14.5176 13.6696 14.6668 13.8188 14.6668 14.0029V14.6696C14.6668 14.8537 14.5176 15.0029 14.3335 15.0029H1.66683C1.48273 15.0029 1.3335 14.8537 1.3335 14.6696V14.0029C1.3335 13.8188 1.48273 13.6696 1.66683 13.6696H14.3335Z" fill="#86909C"></path>
                                            <path fillRule="evenodd" clipRule="evenodd" d="M9.8175 1.52987L9.77386 1.48969C9.51193 1.26901 9.1201 1.28199 8.87345 1.52864L8.40329 1.9988L8.40492 2.00037L7.27453 3.13954L10.5036 6.3751L12.1057 4.77295C12.3656 4.51309 12.3661 4.09193 12.107 3.83138L9.8175 1.52987ZM9.50415 7.3746L6.27569 4.14613L1.33325 9.12694V11.6696L1.33508 11.7194C1.36053 12.0643 1.64846 12.3363 1.99992 12.3363H4.5426L9.50415 7.3746Z" fill="#86909C"></path>
                                        </svg>
                                        <span>写文章</span>
                                    </a>
                                </li>
                                <li className="nav-menu-item">
                                    <a onClick={this.editor_drafts.bind(this)}>
                                        <svg width="16" height="16" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg" className="fengwei">
                                            <path fillRule=" evenodd" clipRule="evenodd" d="M2 1.66663C2 1.11434 2.44772 0.666626 3 0.666626H11L14 3.80948V14.3333C14 14.8856 13.5523 15.3333 13 15.3333H3C2.44772 15.3333 2 14.8856 2 14.3333V1.66663ZM4.66675 9.66667C4.66675 9.29848 4.96522 9 5.33341 9H8.00008C8.36827 9 8.66675 9.29848 8.66675 9.66667C8.66675 10.0349 8.36827 10.3333 8.00008 10.3333H5.33341C4.96522 10.3333 4.66675 10.0349 4.66675 9.66667ZM5.33341 6.33337C4.96522 6.33337 4.66675 6.63185 4.66675 7.00004C4.66675 7.36823 4.96522 7.66671 5.33341 7.66671H10.6667C11.0349 7.66671 11.3334 7.36823 11.3334 7.00004C11.3334 6.63185 11.0349 6.33337 10.6667 6.33337H5.33341Z" fill="#86909C"></path>
                                            <path fillRule="evenodd" clipRule="evenodd" d="M10.3999 1.5L13.3999 4.5H10.3999V1.5Z" fill="white"></path>
                                        </svg>
                                        <span>草稿箱</span>
                                    </a>
                                </li>
                            </div>
                            <div className="nav-menu-item-group">
                                <li className="nav-menu-item">
                                    <a href={`${userinfo ? '/user/' + userinfo.user_id : ''}`} className="link-icon">
                                        <svg width="16" height="16" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg" className="fengwei">
                                            <path fillRule="evenodd" clipRule="evenodd" d="M11.3333 4.33333C11.3333 2.49238 9.84095 1 8 1C6.15905 1 4.66667 2.49238 4.66667 4.33333C4.66667 6.17428 6.15905 7.66667 8 7.66667C9.84095 7.66667 11.3333 6.17428 11.3333 4.33333ZM14 12.6667C14 10.0952 11.8409 8.66667 10 8.66667H6C4.15905 8.66667 2 10.0974 2 12.6667V14.6667C2 15.0349 2.29848 15.3333 2.66667 15.3333H13.3333C13.7015 15.3333 14 15.0349 14 14.6667V12.6667Z" fill="#86909C" data-v-8c1a0f22=""></path>
                                        </svg>
                                        <span>我的主页</span>
                                    </a>
                                </li>
                                <li className="nav-menu-item">
                                    <a href={`${userinfo ? '/user/' + userinfo.user_id + '/likes' : ''}`}>
                                        <svg width="16" height="16" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg" className="fengwei">
                                            <path fillRule="evenodd" clipRule="evenodd" d="M8.89173 0.830916L9.01335 0.887911C9.82399 1.27873 11.0012 2.06767 11.0012 3.33425C11.0012 3.83643 10.7783 4.50245 10.3324 5.33231H12.8585C14.8478 5.33231 15.3047 7.08822 14.8478 8.40394L12.8585 13.6407C12.6904 14.2498 12.1306 14.6723 11.4918 14.6723H3.32845V5.33231H4.6931L7.65981 1.11199C7.84713 0.81396 8.35279 0.585483 8.89173 0.830916ZM2.33333 5.33333V14.6667H1V5.33333H2.33333Z" fill="#86909C"></path>
                                        </svg>
                                        <span>我赞过的</span>
                                    </a>
                                </li>
                                <li className="nav-menu-item">
                                    <a href={`${userinfo ? '/user/' + userinfo.user_id + '/collections' : ''}`}>
                                        <svg width="16" height="16" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg" className="fengwei">
                                            <path fillRule="evenodd" clipRule="evenodd" d="M4.07409 14.913C3.78885 15.0502 3.44641 14.9301 3.30922 14.6449C3.26321 14.5492 3.24467 14.4427 3.25568 14.3371L3.67332 10.3317C3.69013 10.1705 3.63789 10.0097 3.52953 9.88917L0.813641 6.86787C0.602045 6.63248 0.621335 6.27013 0.856725 6.05854C0.931303 5.9915 1.02217 5.9452 1.12024 5.92427L5.09331 5.07635C5.25183 5.04251 5.38861 4.94314 5.46977 4.80283L7.50394 1.28624C7.66242 1.01226 8.013 0.918629 8.28698 1.07711C8.37379 1.12732 8.4459 1.19943 8.49611 1.28624L10.5303 4.80283C10.6114 4.94314 10.7482 5.04251 10.9067 5.07635L14.8798 5.92427C15.1894 5.99033 15.3867 6.29482 15.3207 6.60436C15.2997 6.70243 15.2534 6.7933 15.1864 6.86787L12.4705 9.88917C12.3622 10.0097 12.3099 10.1705 12.3267 10.3317L12.7444 14.3371C12.7772 14.6519 12.5486 14.9337 12.2338 14.9665C12.1282 14.9775 12.0216 14.959 11.926 14.913L8.24843 13.1442C8.09143 13.0687 7.90862 13.0687 7.75162 13.1442L4.07409 14.913Z" fill="#86909C"></path>
                                        </svg>
                                        <span>我的收藏</span>
                                    </a>
                                </li>
                            </div>
                            <div className="nav-menu-item-group">
                                <li className="nav-menu-item">
                                    <a href="/user/settings/profile">
                                        <svg width="16" height="16" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg" className="fengwei">
                                            <path fillRule="evenodd" clipRule="evenodd" d="M7.99989 0.727295C8.53915 0.727295 9.07116 0.786648 9.58901 0.903237L9.88786 0.970519L10.8198 3.09506L13.1143 2.84422L13.3217 3.07014C14.0479 3.86109 14.5927 4.80447 14.9121 5.83475L15.0021 6.12519L14.8231 6.371L13.6368 8.00002L15.0021 9.87485L14.9121 10.1653C14.5927 11.1956 14.0479 12.139 13.3217 12.9299L13.1143 13.1558L10.8198 12.905L9.88786 15.0295L9.58901 15.0968C9.07116 15.2134 8.53915 15.2727 7.99989 15.2727C7.46051 15.2727 6.92838 15.2134 6.41041 15.0967L6.11154 15.0294L5.98852 14.7489L5.17999 12.905L2.88551 13.1558L2.67809 12.9299C1.95184 12.1389 1.40698 11.1954 1.08759 10.1649L0.997559 9.87446L1.17661 9.62865L2.36299 8.00002L0.997559 6.12559L1.08759 5.83512C1.40698 4.80469 1.95184 3.86118 2.67809 3.07014L2.88551 2.84422L5.17999 3.09505L6.11154 0.970623L6.41041 0.903319C6.92838 0.786676 7.46051 0.727295 7.99989 0.727295ZM7.99996 4.96969C9.66782 4.96969 11.0187 6.32704 11.0187 7.99999C11.0187 9.67293 9.66782 11.0303 7.99996 11.0303C6.3321 11.0303 4.9812 9.67293 4.9812 7.99999C4.9812 6.32704 6.3321 4.96969 7.99996 4.96969ZM6.19336 7.99999C6.19336 6.99518 7.00287 6.1818 8 6.1818C8.99713 6.1818 9.80663 6.99518 9.80663 7.99999C9.80663 9.00479 8.99713 9.81817 8 9.81817C7.00287 9.81817 6.19336 9.00479 6.19336 7.99999Z" fill="#86909C"></path>
                                        </svg>
                                        <span>设置</span>
                                    </a>
                                </li>
                                <li className="nav-menu-item more">
                                    <a>
                                        <svg width="16" height="16" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg" className="fengwei">
                                            <path fillRule="evenodd" clipRule="evenodd" d="M8.00008 0.666626C12.0502 0.666626 15.3334 3.94987 15.3334 7.99996C15.3334 12.05 12.0502 15.3333 8.00008 15.3333C3.94999 15.3333 0.666748 12.05 0.666748 7.99996C0.666748 3.94987 3.94999 0.666626 8.00008 0.666626ZM8.33333 10.6667C8.51743 10.6667 8.66667 10.8159 8.66667 11V11.6667C8.66667 11.8508 8.51743 12 8.33333 12H7.66667C7.48257 12 7.33333 11.8508 7.33333 11.6667V11C7.33333 10.8159 7.48257 10.6667 7.66667 10.6667H8.33333ZM7.90531 4C8.69777 4 9.3561 4.19191 9.88032 4.57573C10.4045 4.95955 10.6666 5.52817 10.6666 6.28159C10.6666 6.7436 10.5527 7.13275 10.3248 7.44904C10.1915 7.64095 9.93555 7.88617 9.55685 8.1847L9.18341 8.47789C8.98004 8.63782 8.84504 8.8244 8.77842 9.03763C8.76029 9.09582 8.74435 9.204 8.73061 9.36216C8.71566 9.53444 8.57145 9.66667 8.39853 9.66667H7.65226C7.46817 9.66667 7.31893 9.51743 7.31893 9.33333L7.31928 9.31796L7.32034 9.30262C7.36327 8.83869 7.40923 8.54512 7.45823 8.42192C7.5494 8.19269 7.78433 7.92882 8.16303 7.63029L8.54699 7.32643C8.67322 7.23048 9.32369 6.77243 9.32369 6.36689C9.32369 5.96134 9.25304 5.81218 9.04943 5.5896C8.84581 5.36703 8.38745 5.29539 7.97368 5.29539C7.56693 5.29539 7.21935 5.40341 7.04928 5.67706C6.97052 5.8038 6.90611 5.93901 6.85966 6.07696C6.84138 6.13123 6.82519 6.19951 6.81108 6.2818C6.78364 6.44182 6.6449 6.5588 6.48254 6.5588H5.69776C5.51366 6.5588 5.36442 6.40956 5.36442 6.22546L5.36495 6.2066L5.36655 6.18781C5.37503 6.11329 5.3837 6.05131 5.39258 6.00185C5.52254 5.27812 5.85168 4.74589 6.37999 4.40514C6.79376 4.13505 7.30219 4 7.90531 4Z" fill="#86909C"></path>
                                        </svg>
                                        <span>关于</span>
                                        <svg width="16" height="16" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg" className="more-icon">
                                            <path fillRule="evenodd" clipRule="evenodd" d="M10.8523 8L5.43113 2.57885C5.30095 2.44868 5.30095 2.23762 5.43113 2.10745L5.90253 1.63604C6.03271 1.50587 6.24376 1.50587 6.37394 1.63604L12.2665 7.5286C12.5268 7.78895 12.5268 8.21106 12.2665 8.47141L6.37394 14.364C6.24376 14.4941 6.03271 14.4941 5.90253 14.364L5.43113 13.8926C5.30095 13.7624 5.30095 13.5513 5.43113 13.4212L10.8523 8Z" fill="#86909C"></path>
                                        </svg>
                                    </a>
                                    <ul className="nav-menu more-dropdown-list">
                                        <div className="nav-menu-item-group">
                                            <li className="nav-menu-item">
                                                <a href="/gold-miner" rel="noopener noreferrer" target="_blank">
                                                    关于我们
                                                </a>
                                            </li>
                                        </div>
                                    </ul>
                                </li>
                            </div>
                            <div className="nav-menu-item-group">
                                <li className="nav-menu-item">
                                    <a>
                                        <svg width="16" height="16" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg" className="fengwei">
                                            <path fillRule="evenodd" clipRule="evenodd" d="M10.6667 2.55473C13.0212 3.58347 14.6667 5.93291 14.6667 8.66667C14.6667 12.3486 11.6819 15.3333 8 15.3333C4.3181 15.3333 1.33333 12.3486 1.33333 8.66667C1.33333 5.93291 2.97879 3.58347 5.33333 2.55473V4.04684C3.7392 4.969 2.66667 6.69259 2.66667 8.66667C2.66667 11.6122 5.05448 14 8 14C10.9455 14 13.3333 11.6122 13.3333 8.66667C13.3333 6.69259 12.2608 4.969 10.6667 4.04684V2.55473ZM7.33333 8.66667V1.33333C7.33333 1.14924 7.48257 1 7.66667 1H8.33333C8.51743 1 8.66667 1.14924 8.66667 1.33333V8.66667C8.66667 8.85076 8.51743 9 8.33333 9H7.66667C7.48257 9 7.33333 8.85076 7.33333 8.66667Z" fill="#86909C"></path>
                                        </svg>
                                        <span onClick={this.login_out}>退出</span>
                                    </a>
                                </li>
                            </div>
                        </ul>
                    ) : (undefined)}
                </li>
            </React.Fragment>
        )
    }
}

export default Menu;