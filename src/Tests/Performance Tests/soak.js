import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    stages: [
        { duration: '2m', target: 400 }, // ramp up to 400 users
        { duration: '3h56m', target: 400 }, // stay at 400 for ~4 hours
        { duration: '2m', target: 0 }, // scale down. (optional)
    ],
};
const API_BASE_URL  = 'http://20.103.147.242';
//insert id to be tested on
const userId  = '';

export default function () {
    // Here, we set the endpoint to test.
    http.get(`${API_BASE_URL}/prescription/user/${userId}`);

    sleep(1);
}