
//isRetry tests for recursive calls to prevent infinite loop. Used when token is refreshed
export async function apiRequest(endpoint, method, body, isRetry = false) {
    const options = {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        credentials: 'include'
    }

    if (body !== null && body !== undefined) {
        options.body = JSON.stringify(body);
    }

    try {
        const response = await fetch(`/api/${endpoint}`, options);


        //Handle refresh token if auth token is expired
        if (response.status === 401 && !isRetry && endpoint !== 'account/login' && endpoint !== 'account/register') {
            const refreshResponse = await attemptRefresh();

            if (refreshResponse.status === 200) {
                return await apiRequest(endpoint, method, body, true);
            }
            else {
                return {
                    status: 401,
                    message: "Session expired. Please login again.",
                    data: null
                }
            }
        }

        if (!response.ok) {
            let errorMessage = "Unknown error making fetch request";

            try {
                const errorData = await response.json();

                if (errorData && errorData.message) {
                    errorMessage = errorData.message;
                }
            }
            catch (jsonError) {
                console.log("No JSON body in error message");
                console.log(jsonError);
            }

            return {
                status: response.status,
                message: errorMessage,
                data: null
            };
        }

        const data = await response.json();

        const message = data.message ? data.message : "";

        return {
            status: response.status,
            message: message,
            data: data
        }

    }
    catch (error) {
        const errorMessage = error instanceof Error ? error.message : "An unexpected error has occurred";

        return {
            status: 500,
            message: errorMessage,
            data: null
        };
    }
}

async function attemptRefresh() {
    try {
        const response = await fetch('/api/account/refresh', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            credentials: 'include'
        });

        return response;
    } catch {
        return false;
    }
}