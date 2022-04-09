import md5 from 'js-md5'

const md5_8 = (param) => {
    return md5(param).substring(8, 16);
}

const md5_16 = (param) => {
    return md5(param).substring(8, 24);
}

const encrypt_help = () => {
    return {
        md5_8,
        md5_16,
    }
}
export default encrypt_help;
