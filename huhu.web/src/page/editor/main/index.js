import React from "react";
import Emitter from '../events';
import http from '../../../server/instance';
import { Editor } from "@bytemd/react";
import zhHans from "bytemd/lib/locales/zh_Hans.json";
import gfm from "@bytemd/plugin-gfm";
import gemoji from "@bytemd/plugin-gemoji";
import highlight from "@bytemd/plugin-highlight-ssr";
import mediumZoom from "@bytemd/plugin-medium-zoom";
import math from "@bytemd/plugin-math-ssr"
import mermaid from "@bytemd/plugin-mermaid"
import "bytemd/dist/index.min.css";
import "highlight.js/styles/vs.css";
import servicePath from '../../../server/api'

const plugins = [gfm(), gemoji(), highlight(), mediumZoom(), math(), mermaid()];
class Main extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            editor_value: '',
            url: ''
        }
    }

    componentDidMount() {
        //订阅事件
        this.eventEmitter = Emitter.addListener('editor_data', (msg) => {
            this.setState({ editor_value: msg })
        });
    }

    uploadImage = async (files) => {
        const url = await new Promise(function (resolve, reject) {
            if (!files.length) return;
            let formData = new FormData();
            formData.append('cover', files[0], files[0].name);
            const api = servicePath.get_imgurl_api__original_wmark
            http.post(api, formData, { sign: false, token: false }, false).then(function (callback_data) {
                if (callback_data.data.err_no !== 0) {
                    alert('支持 jpg、png 且大小 3M 以内的图片');
                    return;
                }
                resolve(callback_data);
            })
        });
        return [{
            alt: files.map((i) => i.name),
            url: url.data.data.main_url,
        }];
    }

    render() {
        return (
            <div className="main">
                <Editor
                    locale={zhHans}
                    value={this.state.editor_value}
                    plugins={plugins}
                    onChange={(e) => {
                        this.setState({ editor_value: e });
                        Emitter.emit('changeMessage', e); //发布事件
                    }}
                    uploadImages={async (files) =>
                        this.uploadImage(files)
                    }
                />
            </div>
        )
    }
}
export default Main;