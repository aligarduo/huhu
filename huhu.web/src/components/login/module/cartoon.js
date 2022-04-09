import React from 'react'

class Cartoon extends React.Component {
    render() {
        return (
            <div className="panfish">
                <img src={require('../../../assets/images/svg/normal.svg').default} alt=""
                    className="normal"
                    style={{ display: (this.props.state === 'normal') ? 'block' : 'none' }}
                />
                <img src={require('../../../assets/images/svg/greeting.svg').default} alt=""
                    className="greeting"
                    style={{ display: (this.props.state === 'greeting') ? 'block' : 'none' }}
                />
                <img src={require('../../../assets/images/svg/blindfold.svg').default} alt=""
                    className="blindfold"
                    style={{ display: (this.props.state === 'blindfold') ? 'block' : 'none' }}
                />
            </div>
        )
    }
}

export default Cartoon;