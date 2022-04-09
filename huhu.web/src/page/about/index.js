import './about.css';
import React from 'react';

class About extends React.Component {
    componentDidMount() {
        document.title = '乎乎 - 关于我们';
    }

    componentWillUnmount() {
        document.title = '乎乎 - 记录点点滴滴';
    }

    render() {
        return (
            <main className='about'>
                <div className='cover'>
                    <img src={require(`../../assets/images/svg/about-cover.svg`).default} alt='about'></img>
                </div>
                <div className='describe'>
                    <p>💕 感谢各位用户朋友对乎乎一直以来的支持！</p>
                    <p>🔍 乎乎由三位学生创建并维护。</p>
                    <p>🌹 自上线至今，我们一直在不断优化稳定性，开发了更多实用功能。</p>
                    <p>😬 如果您有什么想法或建议，欢迎发送邮件至 2966994290@qq.com 与我们联系！</p>
                </div>
            </main>
        )
    }
}

export default About;