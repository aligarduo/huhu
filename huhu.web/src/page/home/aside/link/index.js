import React from 'react'

const list = () => {
    let _list = [{
        item: [
            {
                icon: "",
                url: "/about",
                matter: "关于我们"
            }, {
                icon: "",
                url: "/term/huhu-terms",
                matter: "用户协议"
            }, {
                icon: "",
                url: "/term/privacy",
                matter: "隐私政策"
            }, {
                icon: "",
                url: "/pitaschio",
                matter: "使用指南"
            }]
    }, {
        item: [{
            icon: "",
            url: "https://beian.miit.gov.cn/",
            matter: "粤ICP备2021135245号-1"
        }]
    }, {
        item: [{
            icon: "assets/images/png/beian-icon.png",
            url: "http://www.beian.gov.cn/portal/registerSystemInfo?recordcode=44090402441139",
            matter: "粤公网安备 44090402441139号"
        }, {
            icon: "",
            url: "",
            matter: "版权所有：乎乎-huhu"
        }, {
            icon: "",
            url: "",
            matter: "举报邮箱：2966994290@qq.com"
        }]
    }, {
        item: [{
            icon: "",
            url: "",
            matter: `©2021-${new Date().getFullYear()} 乎乎`,
        }]
    }]
    return _list;
}

const HUHU_List = () => {
    return (
        <div className="sidebar-block link-block">
            {list().map((items, i) => {
                return (
                    <ul key={i} className="link-list">
                        {items.item && items.item.map((itemc, k) => {
                            return (
                                <li key={k} className="item">
                                    {itemc.url === "" ?
                                        <span>{itemc.matter}</span> :
                                        <React.Fragment>
                                            {itemc.icon === "" ? (undefined) : (
                                                <img src={require(`../../../../${itemc.icon}`).default} alt="" className="matter-icon" />
                                            )}
                                            <a href={itemc.url} rel="noreferrer" target="_blank" >{itemc.matter}</a>
                                        </React.Fragment>
                                    }
                                </li>
                            )
                        })}
                    </ul>
                )
            })}
        </div>
    )
}

export default HUHU_List;