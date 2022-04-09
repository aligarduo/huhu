import React from 'react';
import { Select } from 'antd';
import http from '../../../../server/instance';
import Toast from '../../../../components/toast'
import servicePath from '../../../../server/api'

class Tag extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            flag: true,
            tag_active: '至少添加一个',
            tag_list: [],
        }
    }
    componentDidMount() {
        this.GetTag();
    }
    GetTag = () => {
        const _this = this;
        const api = servicePath.tag_api__query_tag_list;
        http.post(
            api, {
            "cursor": "0",
            "limit": 200
        }, { sign: false, token: true }, false).then(res => {
            if (res.data.err_no !== 0) {
                Toast.info(res.data.err_msg, 3000)
            }
            _this.setState({ tag_list: res.data.data })
        })
    }
    change_active(val) {
        this.setState({ tag_active: val }, () => {
            if (this.props.onChange) {
                let tags = this.state.tag_list[this.state.tag_active].tag_id;
                this.props.onChange({ tags })
            }
        })
    }
    componentDidUpdate() {
        if (this.state.flag && this.props.value && this.props.value.length > 0) {
            for (let n = 0; n < this.props.value.length; n++) {
                for (let i = 0; i < this.state.tag_list.length; i++) {
                    if (this.state.tag_list[i].tag_id === this.props.value[n]) {
                        this.setState({ flag: false, tag_active: this.state.tag_list[i].tag_name });
                    }
                }
            }
        }
    }
    render() {
        return (
            <div className="form-item">
                <div className="label required">标签：</div>
                <div className="form-item-content">
                    <div className="tag-input">
                        <Select
                            onChange={(val) => this.change_active(val)}
                            onBlur={() => { }}
                            onSearch={(val) => { }}
                            showSearch={(val) => { }}
                            className="byte-select__input"
                            value={this.state.tag_active}
                            optionFilterProp="children">
                            {this.state.tag_list.map((item, index) => {
                                return (
                                    <Select.Option key={index}>{item.tag_name}</Select.Option>
                                )
                            })}
                        </Select>
                    </div>
                </div>
            </div>
        )
    }
}
export default Tag;