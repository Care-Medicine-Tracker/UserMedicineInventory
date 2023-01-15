import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    stages: [
        { duration: '5m', target: 100 }, // simulate ramp-up of traffic from 1 to 100 users over 5 minutes.
        { duration: '10m', target: 100 }, // stay at 100 users for 10 minutes
        { duration: '5m', target: 0 }, // ramp-down to 0 users
    ],
    thresholds: {
        http_req_duration: ['p(99)<1500'], // 99% of requests must complete below 1.5s
    },
};
const API_BASE_URL  = 'http://20.103.147.242';
//insert id to be tested on
const userId  = '';

export default function () {
    const res = http.get(`${API_BASE_URL}/prescription/user/${userId}`);

    // An assertion
    check(res, {
        'is status 200': (x) => x.status === 200
    });

    sleep(1);
}