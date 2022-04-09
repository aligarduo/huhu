import React from "react";
import { connect } from 'react-redux'

class index extends React.Component {
    render() {
        const { article_detailed } = this.props;
        return (
            <React.Fragment>
                <div className="tag-list-box">
                    <div className="tag-list">
                        <div className="tag-list-title">{article_detailed ? "贴个标签" : ""}</div>
                        <div className="tag-list-container">
                            {article_detailed && article_detailed.data.tags.map((item, index) => {
                                return (
                                    <span className="item" key={index}>
                                        <img src={item.icon} alt="" className="lazy tag-icon" />
                                        <div className="tag-title">{item.tag_name}</div>
                                    </span>
                                )
                            })}
                        </div>
                    </div>
                </div>
            </React.Fragment>
        )
    }
}

const mapStateToProps = (state) => {
    return {
        article_detailed: state.article_detailed,
    }
}
export default connect(mapStateToProps)(index);