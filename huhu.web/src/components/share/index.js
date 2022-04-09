import './share.css';
import React from 'react';
import QRCode from 'qrcode.react';

class Share extends React.Component {
    componentDidMount() {
        document.title = '分享到微信好友和朋友圈';
        document.getElementsByTagName('body')[0].classList.add('share');
    }
    render() {
        return (
            <React.Fragment>
                <div className="share-content">
                    <div className="share-qrcode">
                        <QRCode
                            value={this.props.location.search.replace('?url=', '')}
                            size={200}
                            fgColor="#000000"
                            renderAs="canvas"
                        />
                    </div>
                </div>
                <div className="share-tips-area">
                    <div className="share-tips">手机微信扫描二维码，点击右上角 ··· 按钮</div>
                    <div className="share-tips">分享到微信好友或朋友圈</div>
                </div>
            </React.Fragment>
        )
    }
}

export default Share;