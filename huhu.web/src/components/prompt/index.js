import React from 'react';

class Prompt extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            confirm_dialog: false, //确认对话框
            dialog_title: '标题', //对话框标题
            dialog_cuewords: '提示语', //对话框提示语
        }
    }

    componentDidMount = () => {
        this.setState({
            dialog_title: this.props.title ? this.props.title : this.state.dialog_title,
            dialog_cuewords: this.props.cuewords ? this.props.cuewords : this.state.dialog_cuewords,
        })
    }

    cancel_delete = () => {
        let callback = this.props.cancel_delete;
        if (typeof callback === "function") {
            callback();
        } else {
            console.log("is no function");
        }
    }

    affirm_delete = () => {
        let callback = this.props.affirm_delete;
        if (typeof callback === "function") {
            callback();
        } else {
            console.log("is no function");
        }
    }

    render() {
        return (
            <React.Fragment>
                {this.props.confirm_dialog ? (
                    <div className="confirm-dialog-box">
                        <div className="confirm-dialog">
                            <div className="dialog-header">
                                <div className="dialog-title">{this.state.dialog_title}</div>
                                <button title="关闭" className="close-btn ion-close-round"></button>
                            </div>
                            <div className="dialog-body">
                                <div className="content">{this.state.dialog_cuewords}</div>
                            </div>
                            <div className="dialog-footer">
                                <button className="ctrl-btn cancel-btn" onClick={() => this.cancel_delete()}>取消</button>
                                <button className="ctrl-btn confirm-btn" onClick={() => this.affirm_delete()}>确定</button>
                            </div>
                        </div>
                    </div>
                ) : null}
            </React.Fragment>
        )
    }
}

export default Prompt;