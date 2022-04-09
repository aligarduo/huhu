import React from 'react';

class Search extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            search_history: false, //搜索框特效
            pull_down_history: false, //下拉框显示隐藏
            search_Key: '', //搜索关键字
            search_record: [], //搜索记录
        }
    }

    componentDidMount() {
        let url_key = this.getQueryString("query");
        this.setState({
            search_Key: url_key === null ? '' : url_key,
        })
        document.addEventListener('click', () => {
            this.setState({
                search_history: false,
            })
        });

        let search = localStorage.getItem('search_record');
        if (search !== null) {
            this.setState({ search_record: JSON.parse(search) });
        }
    }

    //根据参数名获取对应的url参数
    getQueryString = (name) => {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null)
            return decodeURI(r[2]);
        return null;
    }

    //搜索框特效
    search_input_show_hide = (e) => {
        this.stopPropagation(e);
        this.setState({ search_history: !this.state.search_history });
    }

    //下拉框显示隐藏
    pull_down = (e) => {
        if (e.target.className === 'more' || e.target.className === 'more-pull-icon') {
            this.setState({ pull_down_history: !this.state.pull_down_history });
        }
    }

    //回车
    search_Keydown = (e) => {
        if (e.keyCode === 13) {
            ///开始搜索
            this.begin_search();
        }
    }

    //开始搜索
    begin_search = () => {
        if (this.state.search_Key !== "") {
            this.setState({ search_record: [...this.state.search_record, this.state.search_Key] }, () => {
                localStorage.setItem('search_record', JSON.stringify(this.state.search_record));
            });
            window.location.href = `/search?query=${this.state.search_Key}`;
        }
    }

    //阻止冒泡
    stopPropagation(e) {
        e.nativeEvent.stopImmediatePropagation();
    }

    //清除localStorage
    clear_search_record = () => {
        this.setState({ search_record: [], search_history: !this.state.search_history }, () => {
            localStorage.setItem('search_record', JSON.stringify([]));
        });
    }

    //点击历史记录
    click_history = (e) => {
        window.location.href = `/search?query=${e.target.outerText}`;
    }


    render() {
        return (
            <li className="search-add">
                <ul className="search-add-ul">
                    <li className="header-nav-item search">
                        <div role="search" className={`search-form${this.state.search_history ? ' active' : ''}`} >
                            <input type="search" maxLength="32"
                                placeholder={`${this.state.search_history ? '...' : '搜索乎乎内容...'}`}
                                className={`search-input${this.state.search_history ? ' search-form active' : ''}`}
                                onClick={(e) => this.search_input_show_hide(e)}
                                onKeyDown={(e) => this.search_Keydown(e)}
                                onChange={(e) => this.setState({ search_Key: e.target.value })}
                                value={this.state.search_Key}
                            />
                            <button className="search-icon" onClick={() => this.begin_search()}>
                                <span className="search-icon-primary">
                                    <br />
                                    <svg fill="#8590a6" viewBox="0 0 24 24" width="18" height="18"><path d="M17.068 15.58a8.377 8.377 0 0 0 1.774-5.159 8.421 8.421 0 1 0-8.42 8.421 8.38 8.38 0 0 0 5.158-1.774l3.879 3.88c.957.573 2.131-.464 1.488-1.49l-3.879-3.878zm-6.647 1.157a6.323 6.323 0 0 1-6.316-6.316 6.323 6.323 0 0 1 6.316-6.316 6.323 6.323 0 0 1 6.316 6.316 6.323 6.323 0 0 1-6.316 6.316z" fillRule="evenodd"></path></svg>
                                </span>
                            </button>
                            {this.state.search_record && this.state.search_record.length > 0 ? (
                                <div className="typehead"
                                    style={{ display: this.state.search_history ? "block" : "none" }}
                                    onClick={this.stopPropagation}>
                                    <div className="typehead-title">
                                        <span>搜索历史</span>
                                        <span className="clear" onClick={() => this.clear_search_record()}>清空</span>
                                    </div>
                                    <div className="list">
                                        {this.state.search_record.map((item, index) => {
                                            return (
                                                <div key={index} onClick={(e) => this.click_history(e)}>{item}</div>
                                            )
                                        })}
                                    </div>
                                </div>
                            ) : null}
                        </div>
                    </li>
                </ul>
            </li>
        )
    }
}

export default Search;