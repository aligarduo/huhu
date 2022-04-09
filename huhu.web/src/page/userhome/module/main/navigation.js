import React from 'react';
import { connect } from 'react-redux'

class Navigation extends React.Component {
    render() {
        return (
            <div className="list-header">
                <div className="header-content">
                    <div className="nav-item active">
                        <div className="item-title">文章</div>
                    </div>
                </div>
            </div>
        )
    }
}

const mapStateToProps = (state) => {
    return {}
}
export default connect(mapStateToProps)(Navigation);