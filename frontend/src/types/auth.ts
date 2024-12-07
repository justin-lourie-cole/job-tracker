export interface User {
	id: string;
	email: string;
	firstName: string;
	lastName: string;
	emailVerified: boolean;
}

export interface AuthResponse {
	user: User;
	accessToken: string;
	refreshToken: string;
}

export interface LoginRequest {
	email: string;
	password: string;
}

export interface RegisterRequest extends LoginRequest {
	firstName: string;
	lastName: string;
}
