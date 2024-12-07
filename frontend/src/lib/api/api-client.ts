import { useAuthStore } from "../../stores/auth-store";

const BASE_URL = "/api";

async function fetchClient(endpoint: string, options: RequestInit = {}) {
	// Add default headers
	const headers = new Headers({
		"Content-Type": "application/json",
		...(options.headers as Record<string, string>),
	});

	// Add auth token if available
	const token = useAuthStore.getState().accessToken;
	if (token) {
		headers.set("Authorization", `Bearer ${token}`);
	}

	try {
		const response = await fetch(`${BASE_URL}${endpoint}`, {
			...options,
			headers,
		});

		// Handle 401 unauthorized
		if (response.status === 401) {
			useAuthStore.getState().logout();
			throw new Error("Unauthorized");
		}

		// Handle non-2xx responses
		if (!response.ok) {
			throw new Error(`HTTP error! status: ${response.status}`);
		}

		return await response.json();
	} catch (error) {
		return Promise.reject(error);
	}
}

export default fetchClient;
