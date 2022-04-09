import store from '../redux/index';

function loadinger(open_Loading) {
    if (open_Loading === true) {
        store.dispatch({
            type: "LOADING_START",
        })
    } else {
        store.dispatch({
            type: "LOADING_END",
        })
    }
}
export default loadinger;