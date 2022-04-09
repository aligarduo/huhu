import React from 'react';
import './suspend.css';

class suspension extends React.Component {
    constructor(props) {
        super(props);
        this.state = { hasScrolled: false }
        this.onScroll = this.onScroll.bind(this);
    }

    componentDidMount() {
        window.onscroll = this.onScroll;
    }

    onScroll = () => {
        if (document.documentElement.scrollTop > 280 && !this.state.hasScrolled) {
            this.setState({ hasScrolled: true })
        } else if (document.documentElement.scrollTop < 280 && this.state.hasScrolled) {
            this.setState({ hasScrolled: false })
        }
    }

    scrollToTop = () => {
        document.documentElement.scrollTop = 0
    }

    render() {
        return (
            <React.Fragment>
                <div className="main-alert-list"></div>
                <div className="suspension-panel">
                    <button title="回到顶部" className="btn to-top-btn"
                        style={{ display: this.state.hasScrolled ? 'block' : 'none' }}
                        onClick={this.scrollToTop}>
                        <i className="ion-android-arrow-dropup"></i>
                    </button>
                    <button title="建议反馈" className="btn meiqia-btn">
                        <i className="ion-chatbubble-working"></i>
                    </button>
                </div>
            </React.Fragment>
        )
    }
}
export default suspension;