import React from 'react';
import Add from '../../style/add.svg';
import Load from '../../style/load.svg';
import upError from '../../style/up-error.svg';
import servicePath from '../../../../server/api';
import { Upload } from 'antd';
import toast from '../../../../components/toast';

class Cover extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            flag: true, //防止抱死
            is_contains_cover: false, //是否包含封面
            cover_url: '', //封面地址
            loaded_stat: Add, //加载状态
        }
    }

    componentDidUpdate() {
        if (this.props.value && this.props.value !== '' && this.state.flag) {
            this.setState({ flag: false, is_contains_cover: true, cover_url: this.props.value })
        }
    }

    handleBeforeUpload = (file) => {
        this.setState({ loaded_stat: Load })
        const isJPG = file.type === 'image/jpeg';
        const isPNG = file.type === 'image/png';
        if (!(isJPG || isPNG)) {
            this.setState({ loaded_stat: upError })
            toast.error("只能上传JPG、PNG格式的图片", 3000);
            return false;
        }
        const isLt2M = file.size / 1024 / 1024 < 2;
        if (!isLt2M) {
            this.setState({ loaded_stat: upError })
            toast.error("图片大小超过2M限制", 3000);
            return false;
        }
    }

    handleChange = (info) => {
        switch (info.file.status) {
            case "uploading": return;
            case "done":
                this.setState({
                    cover_url: info.file.response.data.main_url,
                    is_contains_cover: true,
                });
                if (this.props.onChange) {
                    let cover_url = info.file.response.data.main_url;
                    this.props.onChange({ cover_url })
                } break;
            default: break;
        }
    }

    deleteCover = () => {
        if (this.props.onChange) {
            this.props.onChange({ cover_url: "" });
        }
        this.setState({ is_contains_cover: false, loaded_stat: Add });
    }

    render() {
        return (
            <div className="form-item">
                <div className="label">封面：</div>
                <div className="form-item-content">
                    <div className="coverselector_container">
                        <Upload
                            name="img"
                            listType="picture-card"
                            className="avatar-uploader"
                            showUploadList={false}
                            beforeUpload={this.handleBeforeUpload}
                            action={servicePath.get_imgurl_api__cover}
                            onChange={info => this.handleChange(info)}>
                            {!this.state.is_contains_cover ? (
                                <div className="button-slot">
                                    <img src={this.state.loaded_stat} height="20" alt="cover" />
                                    <div className="upload">上传封面</div>
                                </div>
                            ) : (
                                <div className="preview-box">
                                    <img src={this.state.cover_url} alt="" className="preview-image" />
                                    <div className="delete-btn" onClick={this.deleteCover}>
                                        <svg t="1635238942199" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="857" width="20" height="20"><path d="M512 512m-435.2 0a435.2 435.2 0 1 0 870.4 0 435.2 435.2 0 1 0-870.4 0Z" fill="#FE6D68" p-id="858"></path><path d="M563.2 512l108.8-108.8c12.8-12.8 12.8-38.4 0-51.2-12.8-12.8-38.4-12.8-51.2 0L512 460.8 403.2 352c-12.8-12.8-38.4-12.8-51.2 0-12.8 12.8-12.8 38.4 0 51.2L460.8 512 352 620.8c-12.8 12.8-12.8 38.4 0 51.2 12.8 12.8 38.4 12.8 51.2 0L512 563.2l108.8 108.8c12.8 12.8 38.4 12.8 51.2 0 12.8-12.8 12.8-38.4 0-51.2L563.2 512z" fill="#FFFFFF" p-id="859"></path></svg>
                                    </div>
                                </div>
                            )}
                        </Upload>
                        <div className="addvice">建议尺寸：1024*726px</div>
                    </div>
                </div>
            </div>
        )
    }
}

export default Cover;