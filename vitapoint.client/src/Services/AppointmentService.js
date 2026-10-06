import { apiRequest } from "../Clients/apiClient";

export async function GetAppointments() {
    const response = await apiRequest('appointment', 'GET', null);

    return response;
}

export async function CancelAppointment(id) {
    const response = await apiRequest(`appointment/${id}`, 'DELETE', null);

    return response;
}

export async function GetDoctors() {
    const response = await apiRequest(`appointment/available-doctors`, 'GET', null);

    return response;
}

export async function GetUnavailableTimes(date, doctorId) {
    const response = await apiRequest(`appointment/unavailable-appointments?doctorId=${doctorId}&date=${date}`, `GET`, null);

    return response;
}

export async function PostNewAppointment(body) {
    const response = await apiRequest('appointment/', 'POST', body);

    return response;
}