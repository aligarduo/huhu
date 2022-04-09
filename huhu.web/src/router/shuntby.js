import React from 'react';
import { Route, Switch } from 'react-router-dom';
import Header from '../components/header/index';
import Suspend from '../components/suspend/index';
import OpenApp from '../components/openapp/open';
import Detail from '../page/detail/index';
import HomeRouter from '../page/home';
import PublishedSucceed from '../page/editor/header/module/published_succeed';
import PublishedError from '../page/editor/header/module/published_error';
import UserHome from '../page/userhome/index';
import Search from '../page/search';
import Settings from '../page/settings';
import Terms from '../page/terms';
import Privacy from '../page/privacy';
import Notfound from '../components/404/notfound';
import Advertising from '../page/advertising';
import Topic from '../page/topic';
import Notification from '../page/notification';
import About from '../page/about';

import UploadVideo from '../page/uploadvideo/index';
import Video from '../page/video';

class Shuntby extends React.Component {
    render() {
        return (
            <div id="_layout" key={this.props.location.key}>
                <div className="view-container container">
                    <Header />
                    <Switch>
                        <Route exact path="/" component={HomeRouter} />
                        <Route exact path="/hot" component={HomeRouter} />
                        <Route exact path="/topic" component={Topic} />
                        <Route exact path="/video" component={Video} />
                        <Route exact path="/let-me-answer" component={Topic} />

                        <Route exact path="/upload-video" component={UploadVideo} />

                        <Route exact path="/published_succeed" component={PublishedSucceed} />
                        <Route exact path="/published_error" component={PublishedError} />
                        <Route exact path="/post/:id" component={Detail} />
                        <Route exact path="/pin/:id" component={Advertising} />
                        <Route exact path="/user/settings/:setting" component={Settings} />
                        <Route exact path="/user/:user_id" component={UserHome} />
                        <Route exact path="/search" component={Search} />
                        <Route exact path="/notification" component={Notification} />
                        <Route exact path="/term/huhu-terms" component={Terms} />
                        <Route exact path="/term/privacy" component={Privacy} />
                        <Route exact path="/about" component={About} />
                        <Route component={Notfound} />
                    </Switch>
                </div>
                <div className="global-component-box">
                    <Suspend />
                    <OpenApp />
                    <div className="user-permissions"></div>
                </div>
            </div >
        )
    }
}
export default Shuntby;