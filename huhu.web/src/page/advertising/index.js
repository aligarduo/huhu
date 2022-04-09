import './advertising.css';
import React from 'react';
import { connect } from 'react-redux'
import store from '../../redux/index';
import http from '../../server/instance';
import servicePath from '../../server/api';
import Content from './content';
import Loading from '../../components/loading';

class index extends React.Component {
    constructor(props) {
        super(props);
        this.state = {}
    }

    componentDidMount() {
        let id = this.props.match.params.id;
        this.detailed(id);
    }

    //详细
    detailed = (id) => {
        const api = servicePath.content_api__query_adverts_id;
        http.post(api, {
            "advert_id": id
        }, { sign: false, token: false }, true).then(function (callback) {
            if (callback.data.err_no === 0) {
                store.dispatch({
                    type: "ADVERTISING",
                    advertising: callback.data,
                })
                return;
            }
        })
    }


    render() {
        const { advertising } = this.props;
        return (
            <React.Fragment>
                <main className="container-detail main-container">
                    <div className="view column-view">
                        <div className={`main-area article-area shadow${advertising.data ? ' stationery' : null}`}>
                            {advertising.data ?
                                (<Content />) : (<Loading />)
                            }
                        </div>
                    </div>
                </main>
            </React.Fragment>
        )
    }
}

const mapStateToProps = (state) => {
    return {
        advertising: state.advertising,
    }
}
export default connect(mapStateToProps)(index);