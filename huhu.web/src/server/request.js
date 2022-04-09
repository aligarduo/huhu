import axios from 'axios'
import { sign } from './sign'
import Loading from './loading';

const http = axios.create({
	timeout: 300000,
})

//拦截器 请求前做的
http.interceptors.request.use(function (config) {
	if (config['sign']) {
		config = sign(config); // 核心签名逻辑
	}
	if (config['token']) {
		const token = localStorage.getItem("passport_csrf_token");
		config.headers.passport_csrf_token = token;
	}
	return config;
}, function (error) {
	//处理响应错误
	return Promise.reject(error);
})

//响应器 响应后做的
http.interceptors.response.use(function (response) {
	Loading(false);
	return response;
}, function (error) {
	//处理响应错误
	return Promise.reject(error);
})

export default http;