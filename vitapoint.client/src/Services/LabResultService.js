import { apiRequest } from "../Clients/apiClient";

export async function GetLabResults() {
    const response = await apiRequest('labresult', 'GET', null);
    return response;
}

export async function GetLabResultById(id) {
    const response = await apiRequest(`labresult/${id}`, 'GET', null);

    return response;
}