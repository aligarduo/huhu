import 'antd/dist/antd.css';
import './assets/font/ionicons.min.css';
import './assets/font/iconfont.css';
import './index.css';
import React from 'react';
import ReactDOM from 'react-dom';
import store from './redux/index';
import { Provider } from 'react-redux';
import Route from './router/route';

ReactDOM.render(
  <Provider store={store}>
    <Route />
  </Provider>,
  document.getElementById('_huhu')
);