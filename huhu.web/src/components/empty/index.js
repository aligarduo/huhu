import './empty.css';

function Empty() {
    return (
        <div className="activity-empty" >
            <svg width="66" height="68" viewBox="0 0 66 68" className="empty-icon" >
                <g fill="none" fillRule="evenodd" transform="translate(4 3)" >
                    <g fill="#F7F7F7" >
                        <path d="M9 10h23.751v3.221H9zM9 16.494h41.083v4.026H9zM9 26.104h23.751v3.221H9zM9 42.208h23.751v3.221H9zM9 33.351h41.083v4.026H9zM9 49.455h41.083v4.026H9z" ></path>
                    </g>
                    <rect width="56" height="60" x="1.139" y="1.338" stroke="#EBEBEB" strokeWidth="2" rx="6" ></rect>
                </g>
            </svg>
            <span className="empty-text">这里什么也没有</span>
        </div>
    )
}

export default Empty;