import Signin from './signin/index';
import Banner from './banner/index';
import Sticky from './sticky/index';
import Author from './author/index';
import Links from './link/index';
import More from './more/index';
import AppDownload from '../../../components/appdownload';

const index = () => {
    return (
        <aside className="index-aside aside">
            <Signin></Signin>
            <Banner></Banner>
            {/* <More></More> */}
            <Sticky></Sticky>
            <Author></Author>
            <AppDownload></AppDownload>
            <Links></Links>
        </aside>
    )
}

export default index;