import React from 'react';
import { connect } from 'react-redux';
import mediumZoom from 'medium-zoom';
import marked from 'marked';
import hljs from "highlight.js";
import timestamp_help from '../../utils/timestamp_help.js';
const { yeardate } = timestamp_help();

class Content extends React.Component {
    constructor(props) {
        super(props);
        this.state = {}
    }

    componentDidMount() {
        marked.setOptions({
            renderer: new marked.Renderer(),
            gfm: true,
            pedantic: false,
            sanitize: false,
            tables: true,
            breaks: true,
            smartLists: true,
            smartypants: true,
            highlight: function (code) {
                return hljs.highlightAll();
            }
        })
    }

    componentDidUpdate = () => {
        let p = document.getElementsByTagName('article')[0].querySelectorAll('img');
        mediumZoom(p, {
            margin: 24,
            background: 'rgba(0, 0, 0, 0.7)',
            scrollOffset: 0,
            metaClick: false
        });
    }

    render() {
        const { advertising } = this.props;
        let mark_content = null;
        if (advertising.data) {
            document.title = advertising.data.title;
            mark_content = marked(advertising.data.content);
        } else {
            mark_content = '';
            document.title = '很快就好...';
        }
        return (
            <React.Fragment>
                <article className="article advertising">
                    <h1 className="article-title">
                        {advertising.data ? (advertising.data.title) : (undefined)}
                    </h1>
                    {advertising.data && advertising.data.picture !== "" ? (
                        <img src={advertising.data.picture} alt="" className="lazy article-hero" />
                    ) : undefined}
                    <div className="article-content">
                        <div className="markdown-body"
                            dangerouslySetInnerHTML={{ __html: mark_content }}>
                        </div>
                    </div>
                </article>
                <div className="advertising-info-block">
                    <div className="advertising-meta-box">
                        <span>——</span>
                        <time className="advertising-time">{advertising.data ? yeardate('YYYY年mm月dd日', advertising.data.ctime) : ''}</time>
                    </div>
                </div>
            </React.Fragment >
        )
    }
}

const mapStateToProps = (state) => {
    return {
        advertising: state.advertising,
    }
}
export default connect(mapStateToProps)(Content);