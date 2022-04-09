import React from 'react'
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import http from '../../../../server/instance';
import servicePath from '../../../../server/api';
import store from '../../../../redux/index';

class Banner extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            bannerList: [],
        }
    }

    componentDidMount() {
        this.query_adverts();
    }

    //广告
    query_adverts = () => {
        const t = this;
        const api = servicePath.content_api__query_adverts;
        http.post(api, {
            rank: 98,
            layout: 1
        }, {
            headers: { sign: false }
        }, false).then(function (callback) {
            t.setState({ bannerList: callback.data.data })
        })
    }

    close_banner(i) {
        store.dispatch({
            type: "ADVERTISING_CLOSED",
            advertising_closed: i
        })
    }

    render() {
        const { advertising_closed } = this.props;
        return (
            <React.Fragment>
                {this.state.bannerList.length > 0 && this.state.bannerList.map((item, index) => {
                    return advertising_closed.indexOf(index) !== -1 ? (undefined) : (
                        <div key={index} className="sidebar-block banner-block">
                            <div className="banner">
                                <Link to={"/pin/" + item.advert_id} rel="nofollow noopener noreferrer" target="_blank" >
                                    <img src={item.picture} alt=""
                                        className="banner-image"
                                        onError={(e) => {
                                            try {
                                                e.nativeEvent.target.offsetParent.classList.add('banner-image-error');
                                                e.nativeEvent.target.classList.remove('banner-image');
                                                e.nativeEvent.target.classList.add('banner-image-error');
                                                e.nativeEvent.target.src = require('../../../../assets/images/svg/image-loading-failed.svg').default;
                                            } catch { }
                                        }}
                                    />
                                </Link>
                                <div className="ctrl-box">
                                    <i className="ion-close-round close-btn" onClick={this.close_banner.bind(this, index)}></i>
                                    <a className="label" href="/?utm_campaign=banner" target="_blank" rel="noopener noreferrer">
                                        <span className="inco">投放</span>
                                        <span>广告</span>
                                    </a>
                                </div>
                            </div>
                        </div>
                    )
                })}
            </React.Fragment >
        )
    }
}

const mapStateToProps = (state) => {
    return {
        advertising_closed: state.advertising_closed,
    }
}
export default connect(mapStateToProps)(Banner);