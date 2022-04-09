import './target.css';
import React from 'react';

class Target extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            target: null,
        }
    }

    componentDidMount() {
        document.title = '跳转提示-乎乎';
        let url = window.location.href;
        let index = url.lastIndexOf("target=");
        url = url.substring(index + 7, url.length);
        this.setState({ target: url });
    }

    continue_visit() {
        window.location.href = this.state.target;
    }

    render() {
        return (
            <div className="target-page shadow">
                <div className="target-logo"></div>
                <div className="target-content">
                    <p className="target-title"><span>即将离开乎乎，</span><span>请注意账号财产安全！</span></p>
                    <div className="target-link-container">
                        <div className="target-link-content">
                            {this.state.target}
                        </div>
                    </div>
                    <button className="target-btn" onClick={() => this.continue_visit()}>继续访问</button>
                </div>
            </div>
        )
    }
}
export default Target;