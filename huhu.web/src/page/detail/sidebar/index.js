import '../catalog.css'
import React from "react";
import { connect } from 'react-redux'
import marked from 'marked'
import Catalog from 'progress-catalog';
import AppDownload from '../../../components/appdownload';
import numbers_help from '../../../utils/numbers_help.js';
const { number_divide } = numbers_help();

class index extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            show_state: false,
            checked: 0,
            catalog_title_pure_flag: true,
            catalog_title_pure: false,
        }
    }

    componentDidMount() {
        const { article_detailed } = this.props;
        if (article_detailed) {
            marked.setOptions({
                renderer: new marked.Renderer(),
                gfm: true,
                pedantic: false,
                sanitize: false,
                tables: true,
                breaks: true,
                smartLists: true,
                smartypants: true,
            })
        }
        window.addEventListener('scroll', this.hscroll.bind(this), false);
    }

    componentDidUpdate() {
        const { article_detailed } = this.props;
        if (this.state.catalog_title_pure_flag && article_detailed.data) {
            this.setState({ catalog_title_pure_flag: false });
        }
        this.catalog();
    }

    catalog = () => {
        Catalog({
            contentEl: 'markdown-body',
            catalogEl: 'catalog-body',
            selector: ['h1', 'h2', 'h3', 'h4', 'h5', 'h6'],
            cool: true
        });

        let obj = document.getElementById("catalog-body").firstChild;
        let a = obj.getElementsByTagName("ul");
        if (!this.state.catalog_title_pure && a.length > 0) {
            this.setState({ catalog_title_pure: true });
        }
    }

    hscroll() {
        let scrollTop = document.documentElement.scrollTop;
        let clientHeight = document.getElementsByClassName("sidebar-author")[0].clientHeight;
        if (scrollTop - 80 > clientHeight)
            this.setState({ show_state: true });
        else
            this.setState({ show_state: false });
    }

    render() {
        const { article_detailed } = this.props;
        return (
            <div className={`detail-sidebar sidebar-author${this.state.show_state ? ' top sticky' : ''}`}>
                <div className="detail-sidebar-block author-block">
                    <div className="block-title">关于作者</div>
                    <div className="block-body">

                        <div className="stat-item user-item item">
                            <a href={article_detailed ? '/user/' + article_detailed.data.author_user_info.user_id : ''} target="_blank" rel="noreferrer">
                                <div className="avatar bounce-effect"
                                    style={{ backgroundImage: `url(${article_detailed ? article_detailed.data.author_user_info.avatar_large : ''})` }}>
                                </div>
                            </a>
                            <div className="info-box-s">
                                <a href={article_detailed ? '/user/' + article_detailed.data.author_user_info.user_id : ''} target="_blank" rel="noreferrer">
                                    <span className="name">
                                        {article_detailed ? article_detailed.data.author_user_info.user_name : ''}
                                    </span>
                                    <span className="rank">
                                        {article_detailed ? (
                                            <img
                                                src={require("../../../assets/images/lv/lv" + article_detailed.data.author_user_info.level + ".svg").default}
                                                alt={`lv-${article_detailed.data.author_user_info.level}`}
                                            />
                                        ) : undefined}
                                    </span>
                                </a>
                                {article_detailed ? (
                                    <a href={article_detailed ? '/user/' + article_detailed.data.author_user_info.user_id : ''} target="_blank" rel="noreferrer">
                                        <div className="position">
                                            {article_detailed.data.author_user_info.job_title}
                                        </div>
                                    </a>
                                ) : undefined}
                            </div>
                        </div>

                        <div className="user-obtain-item">
                            <div className="user-obtain-item-stat">
                                {article_detailed ? (
                                    <React.Fragment>
                                        <div className="user-obtain-item-stat-title">获得点赞</div>
                                        <div className="user-obtain-item-stat-count">{article_detailed.data.article_info.digg_count}</div>
                                    </React.Fragment>
                                ) : (undefined)}
                            </div>
                            <div className="user-obtain-item-stat">
                                {article_detailed ? (
                                    <React.Fragment>
                                        <div className="user-obtain-item-stat-title">文章被阅读</div>
                                        <div className="user-obtain-item-stat-count">{article_detailed ? number_divide(article_detailed.data.article_info.view_count) : 0}</div>
                                    </React.Fragment>
                                ) : (undefined)}
                            </div>
                        </div>

                    </div>
                </div>

                <AppDownload />

                <div className="sticky-block-box">
                    <div className="catalog-block pure">
                        <nav className="article-catalog">
                            <div className="catalog-title-pure">{this.state.catalog_title_pure ? "目录" : null}</div>
                            <div className="catalog-body" id="catalog-body"></div>
                        </nav>
                    </div>
                </div>

            </div >
        )
    }
}

const mapStateToProps = (state) => {
    return {
        article_detailed: state.article_detailed,
    }
}
export default connect(mapStateToProps)(index);