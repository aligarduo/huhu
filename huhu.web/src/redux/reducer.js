import {
    SCROLL,
    LOADING_START,
    LOADING_END,
    RECOMMEND_FEED,
    ARTICLE_DETAILED,
    RECOMMEND_TAG_LIST, //首页文章标签
    ADVERTISING_CLOSED,
    USERINFO, //用户个人基本信息
    ARTICLE_COMMENTS, //文章评论
    ARTICLE_AUTHORINFO, //文章作者信息
    USER_HOMEPAGE_ARTICLE, //用户个人主页文章
    SEARCH,
    ADVERTISING,
    ARTICLE_HOT,
    OVERALL_DATA,
} from './constant'

const defaultState = {
    scroll: true,
    loading: false,
    recommend_feed: [],
    article_detailed: '',
    recommend_tag_list: '', //首页文章标签
    advertising_closed: [],//***
    userinfo: '', //用户个人基本信息
    article_comments: [], //文章评论
    article_authorinfo: '', //文章作者信息
    user_homepage_article: [], //用户个人主页文章
    search: [],
    advertising: [],
    article_hot: [],
    overall_data: null,
};
function reducer(state = defaultState, action) {
    switch (action.type) {
        case SCROLL: return { ...state, scroll: action.scroll };
        // case RECOMMEND_FEED: return { ...state, recommend_feed: [...state.recommend_feed, action.recommend_feed] };
        case RECOMMEND_FEED: return { ...state, recommend_feed: action.recommend_feed };
        case LOADING_START: return { ...state, loading: true };
        case LOADING_END: return { ...state, loading: false };
        case ARTICLE_DETAILED: return { ...state, article_detailed: action.article_detailed };
        case RECOMMEND_TAG_LIST: return { ...state, recommend_tag_list: action.recommend_tag_list };
        case ADVERTISING_CLOSED: return { ...state, advertising_closed: [...state.advertising_closed, action.advertising_closed] };//***
        case USERINFO: return { ...state, userinfo: action.userinfo }; //用户个人基本信息
        case ARTICLE_COMMENTS: return { ...state, article_comments: action.article_comments }; //文章评论
        case ARTICLE_AUTHORINFO: return { ...state, article_authorinfo: action.article_authorinfo }; //文章评论
        case USER_HOMEPAGE_ARTICLE: return { ...state, user_homepage_article: action.user_homepage_article }; //用户个人主页文章
        case SEARCH: return { ...state, search: action.search }; //
        case ADVERTISING: return { ...state, advertising: action.advertising }; //
        case ARTICLE_HOT: return { ...state, article_hot: action.article_hot }; //
        case OVERALL_DATA: return { ...state, overall_data: action.overall_data }; //
        default:
            return state;
    }
}

export default reducer;