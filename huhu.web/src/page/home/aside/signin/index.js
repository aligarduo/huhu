import './signin.css';
import React from 'react';
import helloSignin from '../../../../assets/images/svg/hello-signin.svg';

const TimeState = () => {
    let timeNow = new Date();
    let hours = timeNow.getHours();
    let text = "~";
    if (hours >= 0 && hours <= 10) {
        text = "早上好 ~";
    } else if (hours > 10 && hours <= 14) {
        text = "中午好 ~";
    } else if (hours > 14 && hours <= 18) {
        text = "下午好 ~";
    } else if (hours > 18 && hours <= 24) {
        text = "晚上好 ~";
    }
    return text;
}

const Signin = () => {
    return (
        <div className="signin-tip signin">
            <div className="first-line-h">
                <div className="icon-text">
                    <img src={helloSignin} className="signin-svg" />
                    <span className="signin-title">{TimeState()}</span>
                </div>
                <button className="signin-btn">
                    <span className="btn-text">去签到</span>
                </button>
            </div>
            <div className="second-line">连续签到赢乎乎惊喜好礼</div>
        </div>
    )
}

export default Signin;