import './uploadvideo.css';
import React from 'react';
import Upload from './upload';
import Affirm from './affirm';

class uploadvideo extends React.Component {
    constructor(props) {
        super(props);
        this.state = {}
    }

    render() {
        return (
            <main role="main">
                <Upload />
                {/* <Affirm /> */}
            </main>
        )
    }
}

export default uploadvideo;