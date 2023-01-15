import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    stages: [
        { duration: '1m', target: 10 }, // simulate ramp-up of traffic from 1 to 10 users over 1 minutes.
        { duration: '2m', target: 10 }, // stay at 10 users for 2 minutes
        { duration: '1m', target: 0 }, // ramp-down to 0 users
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