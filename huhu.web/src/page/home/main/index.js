import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import store from '../../../redux/index';
import Recommend from './entry/recommend';
import Hot from './entry/hot';
import url_help from '../../../utils/url_help';
const { urlparam } = url_help();

class index extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            nav_List: [{
                url: '/',
                content: '推荐',
            },
            {
                url: '/hot',
                content: '热榜',
            }],
            param: urlparam(window.location.href, 3),
            module: null,
        };
    }

    componentDidMount() {
        //重置redux
        this.reset_redux();
        //一进入就加载
        this.Into_load();
    }
    //重置redux
    reset_redux = () => {
        store.dispatch({
            type: "RECOMMEND_FEED",
            recommend_feed: [],
        })
    }
    //一进入就加载
    Into_load = () => {
        switch (this.state.param) {
            case '': this.setState({ module: <Recommend /> }); break;
            case 'hot': this.setState({ module: <Hot /> }); break;
            default: break;
        }
    }
    


    render() {
        return (
            <div className="entry-list-timeline">
                <div className="entry-list-container shadow">
                    <header className="header-list-wrap">
                        <nav className="list-nav">
                            <ul className="nav-list left">
                                {this.state.nav_List.map((item, index) => {
                                    return (
                                        <li key={index}
                                            className={`nav-item${this.state.param === item.url.replace('/', '') ? ' active' : ''}`}>
                                            <Link key={index} to={item.url} onClick={() => { this.setState({ active: index }) }}>
                                                {item.content}
                                            </Link>
                                        </li>
                                    )
                                })}
                            </ul>
                        </nav>
                    </header>
                    {this.state.module}
                </div>
            </div>
        )
    }
}

const mapStateToProps = (state) => {
    return {
        recommend_feed: state.recommend_feed,
    }
}
export default connect(mapStateToProps)(index);