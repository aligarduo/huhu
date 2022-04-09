import './notification.css'
import React from 'react';

class Notification extends React.Component {
    render() {
        return (
            <main className="main-container with-view-nav">
                <div className="view notification-view">
                    <nav className="msg-view-nav">
                        <ul className="nav-list">
                            <li className="notification-nav active">
                                <span className="notification-title">系统消息</span>
                            </li>
                        </ul>
                    </nav>
                    <div className="chat-view">
                        <ul className="notification-list">
                            <li className="notification-item">
                                <div className="notification-link">
                                    <div className="left">
                                        <svg t="1639717122688" className="icon" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="6441" width="64" height="64"><path d="M512 512m-445 0a445 445 0 1 0 890 0 445 445 0 1 0-890 0Z" fill="#E6F3FF" p-id="6442"></path><path d="M348.1 698c-2.2 0-4.4-0.8-6.1-2.5-3.4-3.4-3.4-8.8 0-12.2l73.3-73.3c3.4-3.4 8.8-3.4 12.2 0 3.4 3.4 3.4 8.8 0 12.2l-73.3 73.3c-1.7 1.7-3.9 2.5-6.1 2.5zM443 689.4c-2.2 0-4.4-0.8-6.1-2.5-3.4-3.4-3.4-8.8 0-12.2l73.3-73.3c3.4-3.4 8.8-3.4 12.2 0 3.4 3.4 3.4 8.8 0 12.2l-73.3 73.3c-1.7 1.7-3.9 2.5-6.1 2.5z" fill="#B8DDFF" p-id="6443"></path><path d="M710.4 325.6l-379.6 73.7 105.5 63.2 274.1-136.9" fill="#148AF4" p-id="6444"></path><path d="M436.3 462.5l21.1 115.9 32.9-85.2 220.1-167.6z" fill="#22D0FF" p-id="6445"></path><path d="M540.1 525.5l-82.7 52.9 31.6-84.3 105.4-21z" fill="#B8DDFF" p-id="6446"></path><path d="M710.4 325.6l-84.3 252.8L489 494.1z" fill="#148AF4" p-id="6447"></path><path d="M556.3 581.7a72.2 72.2 0 1 0 144.4 0 72.2 72.2 0 1 0-144.4 0Z" fill="#22D0FF" p-id="6448"></path><path d="M322.2 620.5c-2.2 0-4.4-0.8-6.1-2.5-3.4-3.4-3.4-8.8 0-12.2l73.3-73.3c3.4-3.4 8.8-3.4 12.2 0 3.4 3.4 3.4 8.8 0 12.2L328.3 618c-1.7 1.6-3.9 2.5-6.1 2.5z" fill="#B8DDFF" p-id="6449"></path><path d="M625.9 610v-43.8c-8.2 6.3-13.6 9.4-16.5 9.4-1.3 0-2.5-0.5-3.6-1.6-1-1.1-1.6-2.3-1.6-3.7 0-1.6 0.5-2.8 1.5-3.6 1-0.8 2.8-1.8 5.4-3 3.9-1.8 7-3.8 9.3-5.8 2.3-2 4.4-4.3 6.2-6.8 1.8-2.5 3-4 3.5-4.6 0.5-0.6 1.6-0.9 3.1-0.9 1.7 0 3.1 0.7 4.1 2 1 1.3 1.5 3.1 1.5 5.4v55.1c0 6.4-2.2 9.7-6.6 9.7-2 0-3.5-0.7-4.7-2-1-1.3-1.6-3.3-1.6-5.8z" fill="#148AF4" p-id="6450"></path></svg>
                                        <div className="center">
                                            <div className="profile">
                                                <span className="notification-title">
                                                    欢迎回来【乎乎】
                                                </span>
                                            </div>
                                            <div className="info">
                                                <span className="time">~</span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </main>
        )
    }
}
export default Notification;