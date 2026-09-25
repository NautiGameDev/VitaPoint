import { apiRequest } from "../Clients/apiClient";

export async function GetRootMessages() {
    const response = await apiRequest('message/root_messages', 'GET', null);

    return response;
}

export async function GetMessageThread(id) {
    const response = await apiRequest(`message/${id}`, 'GET', null);

    return response;
}

export async function PostNewMessage(body) {
    const response = await apiRequest('message', 'POST', body);

    return response;
}

export async function GetContacts() {
    const response = await apiRequest('message/get-contacts', 'GET', null);

    return response;
}