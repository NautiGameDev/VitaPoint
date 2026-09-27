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