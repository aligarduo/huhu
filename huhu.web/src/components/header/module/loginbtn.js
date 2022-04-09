import React from 'react';
import ReactDOM from 'react-dom'
import { Provider } from 'react-redux';
import store from '../../../redux/index';
import Login from '../../login/index';

class Loginbtn extends React.Component {
    // 弹出登录窗口
    login_show_box() {
        const div = document.getElementsByClassName("user-permissions")[0]
        ReactDOM.render(
            <Provider store={store}>
                <Login />
            </Provider>, div
        );
        document.body.style.overflow = 'hidden';
    }
    render() {
        return (
            <React.Fragment>
                <li className="header-nav-item auth">
                    <button className="login-button" onClick={this.login_show_box}>登录</button>
                </li>
            </React.Fragment>
        )
    }
}
export default Loginbtn;