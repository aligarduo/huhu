import './notfound.css';
import React from 'react';
import notfound_icon from '../../assets/images/svg/notfound.svg';

class NotFound extends React.Component {
    componentDidMount() {
        document.title = '404 - 乎乎';
        document.getElementsByTagName('body')[0].classList.add('notfound');
    }

    componentWillUnmount() {
        document.title = '乎乎 - 记录点点滴滴';
        document.getElementsByTagName('body')[0].classList.remove('notfound');
    }

    back = () => {
        window.history.back()
    }

    render() {
        return (
            <div className="error-page">
                <div className="error-page-container">
                    <div className="error-page-text">
                        <h1 className="error-page-title">404</h1>
                        <p className="error-page-subtitle">你似乎来到了没有知识存在的荒原</p>
                        <a className="button button--primary button--blue error-page-primarybutton" href="/">
                            去往首页
                        </a>
                        <div className="error-page-otherbuttonContainer">
                            或者
                            <div className="button--plain button--blue error-page-goBackbutton" onClick={() => this.back()}>
                                返回上页
                                <svg fill="currentColor" viewBox="0 0 24 24" width="24px" height="24px">
                                    <path d="M9.218 16.78a.737.737 0 0 0 1.052 0l4.512-4.249a.758.758 0 0 0 0-1.063L10.27 7.22a.737.737 0 0 0-1.052 0 .759.759 0 0 0-.001 1.063L13 12l-3.782 3.716a.758.758 0 0 0 0 1.063z" fillRule="evenodd"></path>
                                </svg>
                            </div>
                        </div>
                    </div>
                    <div className="error-page-errorImageContainer not-select no-touch">
                        <img className="error-page-errorImage" src={notfound_icon} alt="page error" />
                    </div>
                </div>
            </div>
        )
    }
}
export default NotFound;