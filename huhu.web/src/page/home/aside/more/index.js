import './more.css';
import React from 'react';
import ReactDOM from 'react-dom'
import { Provider } from 'react-redux';
import store from '../../../../redux/index';
import Login from '../../../../components/login/index';

class HUHU_more extends React.Component {
    //判断登录状态
    is_login = () => {
        let token = localStorage.getItem("passport_csrf_token");
        if (token)
            return true;
        else
            return false;
    }
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
    //跳转到写文章
    write_article = () => {
        if (!this.is_login()) {
            this.login_show_box();
            return;
        }
        window.location.href = "/editor/drafts/new?v=2";
    }
    //跳转到发视频
    write_upload_video = () => {
        if (!this.is_login()) {
            this.login_show_box();
            return;
        }
        window.location.href = "/upload-video";
    }

    render() {
        return (
            <div className="sidebar-block more-feature">
                <div className="feature-stat" onClick={() => this.write_article()}>
                    <div className="stat-icon">
                        <svg version="1.1" viewBox="0 0 32 32" xmlns="http://www.w3.org/2000/svg" width="32" height="32">
                            <path fill="#4E8DF6" d="M21.8,29.8H6.4c-2.1,0-3.9-1.7-3.9-3.7V5.9c0-2,1.7-3.7,3.9-3.7h15.4c2.1,0,3.9,1.7,3.9,3.7v20.3C25.7,28.2,23.9,29.8,21.8,29.8z" /><path fill="#FFFFFF" d="M16.1,8.5H7.8C7,8.5,6.3,7.8,6.3,7V6.6c0-0.8,0.7-1.5,1.5-1.5H16c0.8,0,1.5,0.7,1.5,1.5V7C17.6,7.8,16.9,8.5,16.1,8.5z M14.3,14.2H7.8c-0.8,0-1.5-0.6-1.5-1.5v-0.4c0-0.8,0.7-1.5,1.5-1.5h6.5c0.8,0,1.5,0.7,1.5,1.5v0.4C15.8,13.6,15.1,14.2,14.3,14.2z" /><path fill="#E1E1DF" d="M28.4,5.5l-1.6-1.1c-0.7-0.4-1.6-0.2-2,0.5l-1,1.6C23.6,6.7,23.7,7,24,7.2l3.2,2c0.2,0.1,0.5,0.1,0.7-0.1l1-1.6C29.3,6.8,29.1,5.9,28.4,5.5z M16.5,19.3l3.2,2c0.2,0.1,0.5,0.1,0.7-0.1l6.4-10.4c0.1-0.2,0.1-0.6-0.2-0.7l-3.2-2c-0.2-0.1-0.5-0.1-0.7,0.1l-6.4,10.4C16.2,18.9,16.2,19.2,16.5,19.3z M15.7,25.2l3.3-2.3c0.3-0.2,0.3-0.7,0-0.9l-2.8-1.7c-0.3-0.2-0.7,0-0.8,0.4l-0.5,4.1C14.8,25.2,15.3,25.5,15.7,25.2z" /><path fill="#2166CC" d="M21.9,14.6c0.3,0.2,0.6,0.1,0.9-0.2l1.1-1.8c0.2-0.3,0.1-0.6-0.2-0.9c-0.3-0.2-0.6-0.1-0.9,0.2l-1.1,1.8C21.5,14.1,21.6,14.4,21.9,14.6z" />
                        </svg>
                    </div>
                    <div className="stat-hint">写文章</div>
                </div>
                <div className="feature-stat" onClick={()=>this.write_upload_video()}>
                    <div className="stat-icon">
                        <svg t="1643467932266" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="11312" width="32" height="32">
                            <path d="M905.6 771.2l-254.4-62.4c-4.8-1.6-9.6-1.6-12.8-1.6C603.2 704 576 673.6 576 640v-67.2c0-35.2 27.2-64 62.4-67.2 4.8 0 9.6-1.6 12.8-1.6l254.4-62.4c25.6-6.4 54.4 1.6 70.4 20.8 9.6 11.2 16 25.6 16 41.6v203.2c0 16-6.4 30.4-16 41.6-16 20.8-44.8 28.8-70.4 22.4z" fill="#E1E1DF" p-id="11313"></path><path d="M392 198.4a121.6 118.4 0 1 0 243.2 0 121.6 118.4 0 1 0-243.2 0Z" fill="#2166CC" p-id="11314"></path><path d="M222.4 235.2a86.4 84.8 0 1 0 172.8 0 86.4 84.8 0 1 0-172.8 0Z" fill="#2166CC" p-id="11315"></path><path d="M705.6 944H160c-70.4 0-128-57.6-128-128V436.8c0-70.4 57.6-128 128-128h545.6c70.4 0 128 57.6 128 128V816c0 70.4-57.6 128-128 128z" fill="#4E8DF6" p-id="11316"></path><path d="M526.4 612.8l-172.8-97.6c-11.2-6.4-24 1.6-24 14.4v195.2c0 12.8 12.8 19.2 24 14.4L526.4 640c11.2-6.4 11.2-20.8 0-27.2z" fill="#FFFFFF" p-id="11317"></path>
                        </svg>
                    </div>
                    <div className="stat-hint">发视频</div>
                </div>
            </div>
        )
    }
}

export default HUHU_more;