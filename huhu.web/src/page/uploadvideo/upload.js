import React from 'react';

class upload extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            drag_enter: false, //拖进
        }
    }

    componentDidMount() {
        //只有在ondragover中阻止默认行为才能触发ondrop而不是 ondragleave
        document.addEventListener("dragover", (e) => {
            e.preventDefault();
        }, false);
        //阻止document.ondrop的默认行为，在新窗口中打开拖进的图片
        document.addEventListener("drop", (e) => {
            e.preventDefault();
        }, false);
    }

    //拖进
    DragEnter = () => {
        this.setState({ drag_enter: true });
    }
    //停留
    DragOver = (e) => {
        e.preventDefault();
    }
    //拖离
    DragLeave = () => {
        this.setState({ drag_enter: false });
    }
    //拖进后松开
    Drop = (event) => {
        console.log(event.dataTransfer.files)
        this.setState({ drag_enter: false });
    }


    render() {
        return (
            <div className={`video-upload${this.state.drag_enter ? " video-upload--dragging" : ""}`}
                onDragEnter={() => this.DragEnter()}
                onDragOver={(e) => this.DragOver(e)}
                onDragLeave={() => this.DragLeave()}
                onDrop={(e) => this.Drop(e)}>
                <div className={`video-upload-iconbg${this.state.drag_enter ? " video-upload-iconbg--dragging" : ""}`}>
                    <svg className={`video-upload-icon${this.state.drag_enter ? " video-upload-icon--dragging" : ""}`} fill="currentColor" viewBox="0 0 24 24" width="24" height="24">
                        <path d="M16.036 19.59a1 1 0 0 1-.997.995H9.032a.996.996 0 0 1-.997-.996v-7.005H5.03c-1.1 0-1.36-.633-.578-1.416L11.33 4.29a1.003 1.003 0 0 1 1.412 0l6.878 6.88c.782.78.523 1.415-.58 1.415h-3.004v7.005z"></path>
                    </svg>
                </div>
                <div className={`video-upload-title${this.state.drag_enter ? " video-upload-title--dragging" : ""}`}>
                    {this.state.drag_enter ? "松开上传视频" : "拖放要上传的视频文件"}
                </div>
                <div className="video-upload-buttongroup">
                    <button className="video-upload-button">上传视频</button>
                    <input type="file" className="video-upload-fileinput" accept=".3gp,.asf,.avi,.dat,.f4v,.flv,.m4v,.mkv,.mov,.mp4,.mp4v,.mpe,.mpeg,.mpg,.ra,.ram,.rm,.rmvb,.vob,.webm,.wm,.wmv" multiple="" />
                </div>
                <div className="video-upload-uploadhint">一次最多上传 6 个视频</div>
                <div className="video-upload-footer">
                    <p>请不要添加无关的视频，详情查看
                        <a> 乎乎视频使用规范 </a>
                        <br />
                        上传视频，即代表你同意
                        <a>《乎乎视频用户协议》</a>
                    </p>
                </div>
            </div>
        )
    }
}

export default upload;