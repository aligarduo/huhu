import svg from './svg.svg';

const appDownload = () => {
    return (
        <div className="sidebar-block app-download">
            <a href="/app" target="_blank" rel="noreferrer">
                <div className="app-link">
                    <img src={svg} className="app-download-svg" />
                    <div className="content-box-s">
                        <div className="headline">下载APP</div>
                        <div className="desc">记录下点点滴滴</div>
                    </div>
                </div>
            </a>
        </div>
    )
}
export default appDownload;