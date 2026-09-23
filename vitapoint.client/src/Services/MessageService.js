import { apiRequest } from "../Clients/apiClient";

export async function GetRootMessages() {
    const response = await apiRequest('message/root_messages', 'GET', null);

    return response;
}

export async function GetMessageThread(id) {
    const response = await apiRequest(`message/${id}`, 'GET', null);

    return response;
}