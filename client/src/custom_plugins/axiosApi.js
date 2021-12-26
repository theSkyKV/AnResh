import axios from 'axios';

export function post(url, params = {}) {
    return axios.post(url, params, { headers: 
        { 
            'Content-Type': 'application/json',
        }
    });
}

export function get(url, params = {}) {
    return axios.get(url, { params: params });
}