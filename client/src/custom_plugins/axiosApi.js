import axios from 'axios';
import qs from 'qs';

export function post(url, params = {}) {
    return axios.post(url, params, { headers: 
        { 
            'Content-Type': 'application/json',
            'Authorization': sessionStorage.getItem("accessToken")
        }
    });
}

export function get(url, params = {}, headers = {}) {
    return axios.get(url, { params: params, paramsSerializer: function (params) {
        return qs.stringify(params, {arrayFormat: 'repeat'})
      }, headers: headers });
}