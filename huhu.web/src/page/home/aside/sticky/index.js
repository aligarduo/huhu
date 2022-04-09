import React from 'react';
import { connect } from 'react-redux'
import Banner from '../banner'
import AppDownload from '../../../../components/appdownload'

class Sticky extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            show_state: false
        }
    }

    componentDidMount() {
        document.addEventListener('scroll', this.hscroll.bind(this))
    }

    hscroll() {
        let scrollTop = document.documentElement.scrollTop
        let clientHeight = document.getElementsByTagName("aside")[0].clientHeight
        if (scrollTop - 60 > clientHeight)
            this.setState({ show_state: true })
        else
            this.setState({ show_state: false })
    }

    render() {
        return (
            <div className={`sidebar-block sticky-block${this.state.show_state ? ' top' : ''}`}
                style={{ top: `${this.props.scroll ? "" : "20px"}` }}>
                <Banner />
                <AppDownload />
            </div>
        )
    }
}

const mapStateToProps = (state) => {
    return {
        scroll: state.scroll,
    }
}
export default connect(mapStateToProps)(Sticky);