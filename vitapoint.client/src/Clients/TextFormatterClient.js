export function FormatDate(dateString) {
    if (!dateString) return '';

    return new Date(dateString).toLocaleString('en-US', {
        month: 'short',
        day: 'numeric',
        year: 'numeric',
        hour: 'numeric',
        minute: '2-digit',
    });
}

export function FormatDateOnly(dateString) {
    if (!dateString) return 'n/a';

    return new Date(dateString).toLocaleString('en-US', {
        month: 'short',
        day: 'numeric',
        year: 'numeric'
    });
}

export function FormatTimeOnly(timeString) {
    if (!timeString) return 'n/a';

    return new Date(`2026-10-06T${timeString}`).toLocaleString('en-US', {
        hour: 'numeric',
        minute: '2-digit',
    });
}