import md5 from 'js-md5'
import encrypt_help from '../utils/onlysign_help';
const { uuid } = encrypt_help();
let lodash = require('lodash');

/**
 * 接口参数签名
 * @param {*} config 请求配置
 */
export const sign = config => {
    // 获取到秒级的时间戳,与后端对应
    let timestamps = new Date().getTime().toString().substring(0, 13);
    let nonces = uuid().substring(2, 6);
    let header = {
        appid: '65d59543-d944-4366-a3c3-83d23dd5cb1a',
        timestamp: timestamps,
        nonce: nonces,
        sign: '',
    }

    let signStr = lodash.toString(header.appid);
    signStr += lodash.toString(header.timestamp);
    signStr += lodash.toString(header.nonce);
    if (config.params) {
        // url参数签名
        let pArray = []
        for (let p in config.params) {
            pArray.push(p);
        }
        let sArray = pArray.sort()
        for (let item of sArray) {
            signStr += item + lodash.toString(config.params[item]);
        }
    } else if (config.data) {
        // body参数签名
        signStr += JSON.stringify(config.data);
    }

    // 签名核心逻辑
    let newsignStr = lodash.sortBy(signStr, s => s.charCodeAt(0)).join('');
    let s = md5(newsignStr).toLowerCase();
    header.sign = s;
    config = Object.assign(config, { headers: header });
    return config;
}