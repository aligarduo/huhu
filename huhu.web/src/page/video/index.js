import './video.css';
import React from 'react';
import Player from 'griffith';

const sources = {
    hd: {
        // play_url: 'https://zhstatic.zhihu.com/cfe/griffith/zhihu2018_hd.mp4',
        play_url: 'https://p1-byteimg.huhu.chat/_hd.mp4',
    },
    sd: {
        // play_url: 'https://zhstatic.zhihu.com/cfe/griffith/zhihu2018_sd.mp4',
        play_url: 'https://p1-byteimg.huhu.chat/_sd.mp4',
    },
}

class Video extends React.Component {
    constructor(props) {
        super(props);
        this.state = {}
    }
    render() {
        return (
            <div className='video-player'>
                <div className='player-header'>
                    <h2>标题</h2>
                </div>
                <div className='player-body'>
                    <Player
                        locale="zh-Hans"
                        sources={sources}
                        shouldShowPageFullScreenButton="true"
                        cover="https://p1-byteimg.huhu.chat/tos-cn-i-k4u1fbpfcp/024830709e6741b18d6b2370a1188c27~tplv-k4u1fbpfcp-no-mark.jpg"
                    />

                    {/* --- */}
                    <div class="RichContent is-collapsed">
                        <div class="RichContent-inner">
                            <span class="RichText ztext CopyrightRichText-richText css-hnrfcf" itemprop="text">
                                这个横屏版的《柴犬》增加了提示指引，比之前的竖屏版更加详细。希望没有折出来的朋友能顺利地折出属于你的可爱狗狗，已经折出来的朋友可以再对照一下细节处理。
                            </span>
                            <button type="button" class="Button ContentItem-more Button--plain">
                                阅读全文
                                <span style={{ display: "inline-flex", alignItems: "center" }}>
                                    <br />
                                    <svg class="Zi Zi--ArrowDown ContentItem-arrowIcon" fill="currentColor" viewBox="0 0 24 24" width="24" height="24">
                                        <path d="M12 13L8.285 9.218a.758.758 0 0 0-1.064 0 .738.738 0 0 0 0 1.052l4.249 4.512a.758.758 0 0 0 1.064 0l4.246-4.512a.738.738 0 0 0 0-1.052.757.757 0 0 0-1.063 0L12.002 13z" fill-rule="evenodd"></path>
                                    </svg>
                                </span>
                            </button>
                        </div>
                        <div class="ContentItem-actions">
                            <div class="ContentItem-actions ZVideoToolbar ContentItem-action ZVideoItem-toolbar">
                                <span>
                                    <button aria-label="赞同 1773 " aria-live="polite" type="button" class="Button VoteButton VoteButton--up">
                                        <span style={{ display: "inline-flex", alignItems: "center" }}>
                                            <br />
                                            <svg class="Zi Zi--TriangleUp VoteButton-TriangleUp" fill="currentColor" viewBox="0 0 24 24" width="10" height="10">
                                                <path d="M2 18.242c0-.326.088-.532.237-.896l7.98-13.203C10.572 3.57 11.086 3 12 3c.915 0 1.429.571 1.784 1.143l7.98 13.203c.15.364.236.57.236.896 0 1.386-.875 1.9-1.955 1.9H3.955c-1.08 0-1.955-.517-1.955-1.9z" fill-rule="evenodd"></path>
                                            </svg>
                                        </span>
                                        赞同 1773
                                    </button>
                                    <button aria-label="反对" aria-live="polite" type="button" class="Button VoteButton VoteButton--down">
                                        <span style={{ display: "inline-flex", alignItems: "center" }}>
                                            <br />
                                            <svg class="Zi Zi--TriangleDown" fill="currentColor" viewBox="0 0 24 24" width="10" height="10">
                                                <path d="M20.044 3H3.956C2.876 3 2 3.517 2 4.9c0 .326.087.533.236.896L10.216 19c.355.571.87 1.143 1.784 1.143s1.429-.572 1.784-1.143l7.98-13.204c.149-.363.236-.57.236-.896 0-1.386-.876-1.9-1.956-1.9z" fill-rule="evenodd"></path>
                                            </svg>
                                        </span>
                                    </button>
                                </span>
                                <button type="button" class="Button ContentItem-action Button--plain Button--withIcon Button--withLabel">
                                    <span style={{ display: "inline-flex", alignItems: "center" }}>
                                        <br />
                                        <svg class="Zi Zi--Comment Button-zi" fill="currentColor" viewBox="0 0 24 24" width="1.2em" height="1.2em">
                                            <path d="M10.241 19.313a.97.97 0 0 0-.77.2 7.908 7.908 0 0 1-3.772 1.482.409.409 0 0 1-.38-.637 5.825 5.825 0 0 0 1.11-2.237.605.605 0 0 0-.227-.59A7.935 7.935 0 0 1 3 11.25C3 6.7 7.03 3 12 3s9 3.7 9 8.25-4.373 9.108-10.759 8.063z" fill-rule="evenodd"></path>
                                        </svg>
                                    </span>
                                    131 条评论
                                </button>
                                <div class="Popover ShareMenu ContentItem-action">
                                    <div class="ShareMenu-toggler" id="Popover157-toggle" aria-haspopup="true" aria-expanded="false" aria-owns="Popover157-content">
                                        <button type="button" class="Button Button--plain Button--withIcon Button--withLabel">
                                            <span style={{ display: "inline-flex", alignItems: "center" }}>
                                                <br />
                                                <svg class="Zi Zi--Share Button-zi" fill="currentColor" viewBox="0 0 24 24" width="1.2em" height="1.2em">
                                                    <path d="M2.931 7.89c-1.067.24-1.275 1.669-.318 2.207l5.277 2.908 8.168-4.776c.25-.127.477.198.273.39L9.05 14.66l.927 5.953c.18 1.084 1.593 1.376 2.182.456l9.644-15.242c.584-.892-.212-2.029-1.234-1.796L2.93 7.89z" fill-rule="evenodd"></path>
                                                </svg>
                                            </span>
                                            分享
                                        </button>
                                    </div>
                                </div>
                                <button type="button" class="Button ContentItem-action Button--plain Button--withIcon Button--withLabel">
                                    <span style={{ display: "inline-flex", alignItems: "center" }}>
                                        <br />
                                        <svg class="Zi Zi--Star Button-zi" fill="currentColor" viewBox="0 0 24 24" width="1.2em" height="1.2em">
                                            <path d="M5.515 19.64l.918-5.355-3.89-3.792c-.926-.902-.639-1.784.64-1.97L8.56 7.74l2.404-4.871c.572-1.16 1.5-1.16 2.072 0L15.44 7.74l5.377.782c1.28.186 1.566 1.068.64 1.97l-3.89 3.793.918 5.354c.219 1.274-.532 1.82-1.676 1.218L12 18.33l-4.808 2.528c-1.145.602-1.896.056-1.677-1.218z" fill-rule="evenodd"></path>
                                        </svg>
                                    </span>
                                    收藏
                                </button>
                                <button aria-live="polite" currentuser="[object Object]" type="button" class="Button ContentItem-action Button--plain Button--withIcon Button--withLabel">
                                    <span style={{ display: "inline-flex", alignItems: "center" }}>
                                        <br />
                                        <svg class="Zi Zi--Heart Button-zi" fill="currentColor" viewBox="0 0 24 24" width="1.2em" height="1.2em">
                                            <path d="M2 8.437C2 5.505 4.294 3.094 7.207 3 9.243 3 11.092 4.19 12 6c.823-1.758 2.649-3 4.651-3C19.545 3 22 5.507 22 8.432 22 16.24 13.842 21 12 21 10.158 21 2 16.24 2 8.437z" fill-rule="evenodd"></path>
                                        </svg>
                                    </span>
                                    喜欢
                                </button>
                                <button type="button" class="Button ContentItem-action Button--plain">
                                    <span style={{ display: "inline-flex", alignItems: "center" }}>
                                        <br />
                                        <svg class="Zi Zi--Report" fill="currentColor" viewBox="0 0 24 24" width="14" height="14">
                                            <path d="M19.947 3.129c-.633.136-3.927.639-5.697.385-3.133-.45-4.776-2.54-9.949-.888-.997.413-1.277 1.038-1.277 2.019L3 20.808c0 .3.101.54.304.718a.97.97 0 0 0 .73.304c.275 0 .519-.102.73-.304.202-.179.304-.418.304-.718v-6.58c4.533-1.235 8.047.668 8.562.864 2.343.893 5.542.008 6.774-.657.397-.178.596-.474.596-.887V3.964c0-.599-.42-.972-1.053-.835z" fill-rule="evenodd"></path>
                                        </svg>
                                    </span>
                                    举报
                                </button>
                                <div class="Popover ContentItem-action">
                                    <button aria-label="更多" id="Popover158-toggle" aria-haspopup="true" aria-expanded="false" aria-owns="Popover158-content" type="button" class="Button OptionsButton Button--plain Button--withIcon Button--iconOnly">
                                        <span style={{ display: "inline-flex", alignItems: "center" }}>
                                            <br />
                                            <svg class="Zi Zi--Dots Button-zi" fill="currentColor" viewBox="0 0 24 24" width="1.2em" height="1.2em">
                                                <path d="M5 14a2 2 0 1 1 0-4 2 2 0 0 1 0 4zm7 0a2 2 0 1 1 0-4 2 2 0 0 1 0 4zm7 0a2 2 0 1 1 0-4 2 2 0 0 1 0 4z" fill-rule="evenodd"></path>
                                            </svg>
                                        </span>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        )
    }
}

export default Video;