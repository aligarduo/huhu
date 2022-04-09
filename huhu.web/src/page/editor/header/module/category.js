import React from 'react';
import http from '../../../../server/instance';
import Toast from '../../../../components/toast'
import servicePath from '../../../../server/api'

class category extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            flag: true,
            cate: '',
            cate_active: null,
            categoryList: []
        }
    }
    componentDidMount() {
        const _this = this;
        const api = servicePath.category_api;
        http.get(api, {}, { sign: false, token: false }, false).then(res => {
            if (res.data.err_no !== 0) {
                Toast.info(res.data.err_msg, 3000)
            }
            _this.setState({ categoryList: res.data.data })
        })
    }
    change_active(index, category_id) {
        this.setState({ cate: index })
        if (this.props.onClick) {
            let cate = category_id
            this.props.onClick({ cate })
        }
    }
    componentDidUpdate(){
        if (this.state.flag && this.state.categoryList) {
            for (let i = 0; i < this.state.categoryList.length; i++) {
                if (this.state.categoryList[i].category_id === this.props.value) {
                    this.setState({ flag: false, cate: i });
                }
            }
        }
    }
    render() {
        return (
            <div className="form-item">
                <div className="label required category-label">分类：</div>
                <div className="form-item-content category-list">
                    {this.state.categoryList.map((item, index) => {
                        return (
                            <div key={index}
                                className={`item${this.state.cate === index ? ' active' : ''}`}
                                onClick={this.change_active.bind(this, index, item.category_id)}>
                                {item.category_name}
                            </div>
                        )
                    })}
                </div>
            </div>
        )
    }
}
export default category;