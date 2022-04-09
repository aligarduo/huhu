
const urlparam = (url, position) => {
    return url.split("/")[position];
}

const url_help = () => {
    return {
        urlparam,
    }
}
export default url_help;
