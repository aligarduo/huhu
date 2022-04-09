import { BrowserRouter, Route, Switch } from 'react-router-dom';
import Editor from '../page/editor';
import Draft from '../page/draft';
import Shuntby from './shuntby';
import Target from '../page/target';
import Download from '../page/app/download';
import ShareWeixin from '../components/share';

const ShuntBy = () => {
  return (
    <BrowserRouter>
      <Switch>
        <Route exact path="/editor/drafts/:new" component={Editor} />
        <Route exact path="/editor/update/:new" component={Editor} />
        <Route exact path="/editor/drafts" component={Draft} />
        <Route exact path="/skiplink" component={Target} />
        <Route exact path="/app" component={Download} />
        <Route exact path="/share_weixin" component={ShareWeixin} />
        <Route path="*" component={Shuntby} />
      </Switch >
    </BrowserRouter>
  );
}
export default ShuntBy;