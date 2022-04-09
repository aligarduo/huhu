import './download.css';
import React from 'react';

class Download extends React.Component {
    componentDidMount() {
        document.title = '乎乎客户端';
        document.body.style.overflow = 'hidden';
    }

    componentWillUnmount() {
        document.title = '乎乎 - 记录点点滴滴';
        document.body.style.overflow = 'inherit';
    }

    start_download = () => {
        alert('乎乎移动端将在不久之后上线!');
    }

    render() {
        return (
            <div className="download-guide">
                <div className='download-guide-entirety'>
                    <div className="download-guide-header">
                        <div className="download-guide-headermain">
                            <div className="download-guide-headerLogo"></div>
                        </div>
                        <div className="download-guide-headerhint">
                            <div>有问题，就会有答案</div>
                            <div>高质量的问答社区</div>
                        </div>
                    </div>
                    <div className="download-guide-body">
                        <button className="download-guide-download-button"
                            onClick={() => this.start_download()}>
                            下载 App
                        </button>
                    </div>
                </div>
            </div>
        )
    }
}
export default Download;