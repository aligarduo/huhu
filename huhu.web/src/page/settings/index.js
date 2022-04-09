import './settings.css';
import React from 'react';
import Profile from './profile';
import Account from './account';
import { Link } from 'react-router-dom';

class Index extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            module: null, //模块
            active: 0, //当前模块索引
        }
    }

    componentDidMount() {
        let token = localStorage.getItem("passport_csrf_token");
        if (!token) {
            alert("初始化失败，请检查登录设置");
            return;
        }
        document.getElementsByTagName('body')[0].classList.add('setting');
        let setting = this.props.match.params.setting;
        switch (true) {
            case (setting === "profile"):
                document.title = '个人资料 - 乎乎';
                this.setState({ active: 0, module: <Profile /> });
                break;
            case (setting === "account"):
                document.title = '账号设置 - 乎乎';
                this.setState({ active: 1, module: <Account /> });
                break;
            default: break;
        }
    }

    componentWillUnmount() {
        document.getElementsByTagName('body')[0].classList.remove('setting');
    }

    render() {
        return (
            <main className="container main-container with-view-nav">
                <div className="setting-view">
                    <div className="setting-sidebar">
                        <Link to="/user/settings/profile">
                            <div className={`setting-nav-item${this.state.active === 0 ? ' active' : ''}`}>个人资料</div>
                        </Link>
                        <Link to="/user/settings/account">
                            <div className={`setting-nav-item${this.state.active === 1 ? ' active' : ''}`}>账号设置</div>
                        </Link>
                    </div>
                    <div className="sub-view-box shadow">
                        {this.state.module}
                    </div>
                </div>
            </main>
        )
    }
}
export default Index;