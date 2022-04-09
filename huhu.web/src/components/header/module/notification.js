import React from 'react';
import { Link } from 'react-router-dom';

class Notification extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            news: 0,
        }
    }
    render() {
        return (
            <li className="header-nav-item notification">
                <Link to={"/notification"} className="app-link-c" >
                    <img src={require('../../../assets/images/svg/news.svg').default} alt="" />
                    {this.state.news > 0 ? <span className="count">0</span> : null}
                </Link>
            </li>
        )
    }
}
export default Notification;
