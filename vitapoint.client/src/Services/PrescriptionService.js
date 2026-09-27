import { apiRequest } from "../Clients/apiClient";

export async function GetPrescriptions() {
    const response = await apiRequest('prescription', 'GET', null);
    return response;
}

export async function RequestRefill(id) {
    alert(`Requesting refill for ${id}`);
    const response = await apiRequest(`prescription/${id}`, 'PUT', null);
    
    return response;
}