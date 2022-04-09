import cookie from 'react-cookies'

/**
 * 设置cookie
 * @param {*} key 键
 * @param {*} value 值
 */
const save = (key, value) => {
    cookie.save(key, value);
}

/**
 * 设置cookie并具备过期时间
 * @param {*} key 键
 * @param {*} value 值
 * @param {number} expires 过期时间（单位：小时）
 */
const save_has_expired = (key, value, expires) => {
    let cookieTime = new Date(new Date().getTime + parseInt(expires) * 3600 * 1000);
    cookie.save(key, value, { expires: cookieTime });
}

/**
 * 删除cookie
 * @param {*} key 键
 */
const remove = (key) => {
    cookie.remove(key);
}

/**
 * 读取cookie
 * @param {*} key 键
 */
const load = (key) => {
    return cookie.load(key);
}


const cookie_help = () => {
    return {
        save,
        save_has_expired,
        remove,
        load,
    }
}
export default cookie_help;
