import { apiRequest } from "../Clients/apiClient";

export async function GetPatientData() {
    const response = await apiRequest('patient', 'GET', null);

    return response;
}

export async function UpdatePatientData(body) {
    const response = await apiRequest('patient', 'PUT', body);

    return response;
}