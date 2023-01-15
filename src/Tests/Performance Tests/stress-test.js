import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    stages: [
        { duration: '2m', target: 100 }, // below normal load
        { duration: '5m', target: 100 },
        { duration: '2m', target: 200 }, // normal load
        { duration: '5m', target: 200 },
        { duration: '2m', target: 300 }, // around the breaking point
        { duration: '5m', target: 300 },
        { duration: '2m', target: 400 }, // beyond the breaking point
        { duration: '5m', target: 400 },
        { duration: '10m', target: 0 }, // scale down. Recovery stage.
    ],
};
const API_BASE_URL = 'http://20.103.147.242';
//insert id to be tested on
const userId = '';

export default function ()
{
    const res = http.get(`${API_BASE_URL}/prescription/user/${userId}`);

    check(res, {
        'is status 200': (x) => x.status === 200
    });

    sleep(1);
}