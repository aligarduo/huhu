import React from 'react';

class Summary extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            flag: true,
            wordCount: 0,
            maxLength: 100,
            textarea_val: '',
        }
    }

    Statistical = (e) => {
        let value = e.target.value
        if (this.props.onChange) {
            this.props.onChange({ value })
        }
        this.setState({
            textarea_val: value,
            wordCount: value.replace(/\s+/g, '').length
        })
    }

    componentDidUpdate(){
        if (this.state.flag && this.props.value !== "") {
            this.setState({
                flag: false,
                textarea_val: this.props.value,
                wordCount: this.props.value.replace(/\s+/g, '').length,
            });
        }
    }

    render() {
        return (
            <div className="summary-box form-item">
                <div className="label">摘要：</div>
                <div className="summary form-item-content">
                    <span style={{ color: "rgb(238, 77, 56)" }}>{this.state.wordCount}/{this.state.maxLength}</span>
                    <div className="summary-textarea">
                        <textarea
                            maxLength={this.state.maxLength}
                            value={this.state.textarea_val}
                            rows="5"
                            spellCheck="false"
                            className="byte-input__textarea"
                            onChange={(e) => this.Statistical(e)}>
                        </textarea>
                    </div>
                </div>
            </div>
        )
    }
}
export default Summary;