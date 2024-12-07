import { create } from "zustand";
import { persist } from "zustand/middleware";
import type { User } from "../types/auth";

interface AuthState {
	user: User | null;
	accessToken: string | null;
	setUser: (user: User) => void;
	setAccessToken: (token: string) => void;
	logout: () => void;
}

export const useAuthStore = create<AuthState>()(
	persist(
		(set) => ({
			user: null,
			accessToken: null,
			setUser: (user) => set({ user }),
			setAccessToken: (token) => set({ accessToken: token }),
			logout: () => set({ user: null, accessToken: null }),
		}),
		{
			name: "auth-storage",
		},
	),
);
