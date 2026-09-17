import { apiRequest } from "../Clients/apiClient";

export async function postLogin(email, password) {
    const body = {email, password}
    const response = await apiRequest('account/login', 'POST', body);

    return response;
}

export async function registerAccount(email, password, activationCode, dob, zip) {
    const body = { email, password, activationCode, dob, zip };
    const response = await apiRequest('account/register', 'POST', body);

    return response;
}

export async function attemptRefresh() {
    const response = await apiRequest('account/refresh', 'POST', null);

    return response;
}

export async function logout() {
    const response = await apiRequest('account/logout', 'POST', null);

    return response;
}