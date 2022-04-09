import Article from './article/index';
import Taglist from './taglist/index';
import CommentList from './commentlist/index';
import Actionbar from './actionbar/index';

const index = () => {
    return (
        <div className="main-area article-area shadow">
            <Article />
            <Taglist />
            <CommentList />
            <Actionbar />
        </div>
    )
}
export default index;