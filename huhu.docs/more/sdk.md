## 应用接入ID
**Web 端：** `df55846546681a0384f39fa5`  
**Android 端：** `5ef2fcda8f064dff16aba6e0`  

## 使用8位MD5加密

在 Recat 中使用
```js
import md5 from 'js-md5' //导入MD5依赖包
let md5_8 = md5("需要加密的数据").substring(8,16) //在加密后默认的32位md5中截取8位
```

在 Vue 中使用
```js
import md5 from 'js-md5' //导入MD5依赖包
let md5_8 = md5("需要加密的数据").substring(8,16) //在加密后默认的32位md5中截取8位
```

## 使用16位MD5加密

在 Recat 中使用
```js
import md5 from 'js-md5' //导入MD5依赖包
let md5_16 = md5("需要加密的数据").substring(8,24) //在加密后默认的32位md5中截取16位
```

在 Vue 中使用
```js
import md5 from 'js-md5' //导入MD5依赖包
let md5_16 = md5("需要加密的数据").substring(8,24) //在加密后默认的32位md5中截取16位
```

## 使用32位MD5加密

在 Recat 中使用
```js
import md5 from 'js-md5' //导入MD5依赖包
let md5_32 = md5("需要加密的数据") //32位md5加密
```

在 Vue 中使用
```js
import md5 from 'js-md5' //导入MD5依赖包
let md5_32 = md5("需要加密的数据") //32位md5加密
```

## 安全签名规则

1、拼接签名数据（ `appid` + `timestamp` + `nonce` + `post或get的参数` ）  
2、将字符串中字符按 `ASCII码` 升序排序  
3、使用MD5加密字符串  

在 Recat 中使用  

```js
import md5 from 'js-md5'
import { uuid } from "uuid"
const lodash = require('lodash')

export const sign = config => {
    let times = new Date().getTime().toString().substr(0, 13)
    let nonces = uuid().substring(1, 6)
    let header = {
        appid: 'appid',
        timestamp: times,
        nonce: nonces,
        sign: ''
    }

    let signStr = lodash.toString(header.appid)
    signStr += lodash.toString(header.timestamp)
    signStr += lodash.toString(header.nonce)
    if (config.params) {
        let pArray = []
        for (let p in config.params) {
            pArray.push(p)
        }
        let sArray = pArray.sort()
        for (let item of sArray) {
            signStr += item + lodash.toString(config.params[item])
        }
    } else if (config.data) {
        signStr += JSON.stringify(config.data)
    }

    let newsignStr = lodash.sortBy(signStr, s => s.charCodeAt(0)).join('')
    header.sign = md5(newsignStr).toLowerCase()
    config = Object.assign(config, { headers: header })
    return config
}
```