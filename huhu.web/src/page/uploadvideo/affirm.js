import React from 'react';

class affirm extends React.Component {
    constructor(props) {
        super(props);
        this.state = {}
    }

    render() {
        return (
            <div className='main-container video-upload-affirm'>
                <div className='affirm-title'>上传列表</div>
                <div className='upload-affirm'>
                    <div className='affirm-left'>
                        <div className='upload-list'>
                            <div className='upload-filename'>hd.mp4</div>
                            <div className='upload-state'>
                                <span>上传成功</span>
                                <span>1MB</span>
                            </div>
                        </div>
                        <div className='upload-list'>
                            <div className='upload-filename'>hd.mp4</div>
                            <div className='upload-state'>
                                <span>上传成功</span>
                                <span>1MB</span>
                            </div>
                        </div>
                    </div>
                    <div className='affirm-right'>

                        <div class="affirm-item">
                            <div class="affirm-hint">封面</div>
                            <div class="affirm-cover-box">
                                <img class="cover-image" src="https://pic1.zhimg.com/v2-991a469be5c73a2ce1523469f83c9620.jpg?source=382ee89a" alt="" />
                            </div>
                        </div>


                        <div class="affirm-item">
                            <div class="affirm-hint">标题</div>
                            <label class="VideoUploadForm-input Input-wrapper Input-wrapper--large">
                                <input class="Input" placeholder="输入视频标题" value="_hd" />
                                <div class="VideoUploadForm-itemLimit">3/40</div>
                            </label>
                        </div>

                        <div class="affirm-item">
                            <div class="affirm-hint">简介</div>
                            <label class="VideoUploadForm-input VideoUploadForm-input--multiline Input-wrapper Input-wrapper--multiline Input-wrapper--large">
                                <textarea rows="4" class="Input" placeholder="填写视频简介，让更多人找到你的视频"></textarea>
                                <div class="VideoUploadForm-itemLimit VideoUploadForm-itemLimit--multiline">0/300</div>
                            </label>
                        </div>


                        <div class="VideoUploadForm-buttonGroup">
                            <button type="button" class="Button VideoUploadForm-submitButton Button--primary Button--blue">发布视频</button>
                            <button type="button" class="Button Button--secondary Button--blue">取消发布</button>
                        </div>



                    </div>
                </div>
            </div>
        )
    }
}

export default affirm;