import '../../style/published_succeed.css';
import React from 'react';
import copy from 'copy-to-clipboard';
import Toast from '../../../../components/toast';
import sharelink_help from '../../../../utils/sharelink_help.js';
const { share_weibo, share_qq, share_weixin } = sharelink_help();

class PublishedSucceed extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            published_data: [],
        }
    }

    componentDidMount() {
        document.title = '发布成功 - 乎乎';
        document.getElementsByTagName('body')[0].classList.add('published-view');
        this.setState({ published_data: JSON.parse(sessionStorage.getItem("published_succeed")) })
    }

    componentWillUnmount() {
        document.title = '乎乎 - 时间不停，代码不止';
        document.getElementsByTagName('body')[0].classList.remove('published-view');
    }

    share_link = (type) => {
        switch (true) {
            case (type === 'weibo'): share_weibo(); break;
            case (type === 'qq'): share_qq(); break;
            case (type === 'weixin'): share_weixin(); break;
            case (type === "link"):
                copy('/post/' + this.state.published_data["article_id"]);
                Toast.info("链接已复制，快去分享给好友吧！", 2500);
                break;
            default: break;
        }
    }

    render() {
        return (
            <main className="main-container">
                <div className="published-view">
                    <div className="published-thanks">发布成功！有了你的分享，乎乎会变得更好！</div>
                    <div className="published-share-content">
                        <a href={this.state.published_data ? `/post/${this.state.published_data["article_id"]}` : null} className="published-title">
                            {this.state.published_data ? this.state.published_data["title"] : null}
                        </a>
                        <div className="share-list">
                            <div className="share-prompt">分享到：</div>
                            <div className="share-wechat" onClick={() => this.share_link('weixin')} >
                                <img src={require('../../../../assets/images/svg/wechat-icon.svg').default} className="inline" alt="inline" />
                                <span>微信</span>
                            </div>
                            <div className="share-weibo" onClick={() => this.share_link('weibo')} >
                                <img src={require('../../../../assets/images/svg/weibo-icon.svg').default} className="inline" alt="inline" />
                                <span>微博</span>
                            </div>
                            <div className="share-link" onClick={() => this.share_link('link')}>
                                <img src={require('../../../../assets/images/svg/link-icon.svg').default} className="inline" alt="inline" />
                                <span>复制链接</span>
                            </div>
                        </div>
                    </div>
                    <div className="handle">
                        <a href="/" className="back-text route-active">回到首页</a>
                    </div>
                </div>
            </main>
        )
    }
}

export default PublishedSucceed;