import '../../style/published_error.css';
import React from 'react';

class PublishedError extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            title: undefined,
            brief_content: undefined,
            mark_content: undefined,
        }
    }

    componentDidMount() {
        document.title = '发布失败 - 乎乎';
        let data = JSON.parse(sessionStorage.getItem("published_error"));
        this.setState({
            title: data.title,
            brief_content: data.brief_content,
            mark_content: data.mark_content,
        })
    }

    componentWillUnmount() {
        document.title = '乎乎 - 时间不停，代码不止';
    }


    render() {
        return (
            <main className="main-container">
                <div className="published-view">
                    <div className="published-error">发布失败！</div>
                    <div className='published-error-hint'>文章已自动保存至<a href='/editor/drafts'>草稿箱</a>。</div>
                    <div className="published-error-detail">
                        <p className='error-detail-title'>原因：文章含有
                            <em>
                                {(() => {
                                    let n = 0;
                                    n += this.state.title && this.state.title ? this.state.title.length : 0;
                                    n += this.state.brief_content && this.state.brief_content ? this.state.brief_content.length : 0;
                                    n += this.state.mark_content && this.state.mark_content ? this.state.mark_content.length : 0;
                                    return n;
                                })()}
                            </em>
                            处敏感词。
                        </p>
                        <ul>
                            {this.state.title ?
                                <li>标题：
                                    {this.state.title.map((item, index) => {
                                        return <span key={index}>{item}</span>;
                                    })}
                                </li> : null
                            }
                            {this.state.brief_content ?
                                <li>摘要：
                                    {this.state.brief_content.map((item, index) => {
                                        return <span key={index}>{item}</span>;
                                    })}
                                </li> : null
                            }
                            {this.state.mark_content ?
                                <li>内容：
                                    {this.state.mark_content.map((item, index) => {
                                        return <span key={index}>{item}</span>;
                                    })}
                                </li> : null
                            }
                        </ul>
                    </div>
                    <div className="handle">
                        <a href="/" className="back-text route-active">回到首页</a>
                    </div>
                </div>
            </main>
        )
    }
}

export default PublishedError;