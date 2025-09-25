import axios from 'axios';

const api = axios.create({
    //baseURL: 'http://localhost:44300'
    baseURL: 'https://api.gabrielsanztech.com.br',
    // baseURL: 'https://projeto-gsystem-api-server-fsbcbbc4gpf0b2hd.brazilsouth-01.azurewebsites.net',
});

export default api;