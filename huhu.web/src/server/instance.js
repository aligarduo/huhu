import axios from 'axios';
import Loading from './loading';
import $http from './request';

class http {
    // GET
    static async get(url, params, open_Loading) {
        open_Loading ? Loading(open_Loading) : Loading(undefined)
        return await $http.get(url, params);
    }

    // POST
    static async post(url, params, headers, open_Loading) {
        open_Loading ? Loading(open_Loading) : Loading(undefined)
        return await $http.post(url, params, headers);
    }

    // ALL
    static async all(arr) {
        if (Object.prototype.toString.call(arr) === "[object Array]") {
            return await axios.all(arr)
        } else {
            const error = new Error("错误参数")
            try {
                throw error
            } catch (error) {
                alert(error)
            }
        }
    }

}

export default http;