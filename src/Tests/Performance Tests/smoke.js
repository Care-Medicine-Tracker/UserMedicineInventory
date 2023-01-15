import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    vus: 1, // 1 user looping for 1 minute
    duration: '1m',

    thresholds: {
        http_req_duration: ['p(99)<1500'], // 99% of requests must complete below 1.5s
    },
};

const API_BASE_URL  = 'http://20.103.147.242';
//insert id to be tested on
const userId  = '';

export default function () {
    const res = http.get(`${API_BASE_URL}/prescription/user/${userId}`);
    check(res, { 'status was 200': (r) => r.status == 200 });
    sleep(1);
}