export type Role = 'ADMIN' | 'KITCHEN' | 'HOUSE' | 'CARE_SPECIALIST';

export type FakeUser = {
    displayName: string;
    roles: Role[];
};

// keep key in memory so refresh works
const KEY = 'omb_fake_user';

export function getUser(): FakeUser | null {
    const raw = localStorage.getItem(KEY);
    return raw ? JSON.parse(raw) : null;
}

export function setUser(user: FakeUser) {
    localStorage.setItem(KEY, JSON.stringify(user));
}

export function clearUser() {
    localStorage.removeItem(KEY);
}

export function hasRole(user: FakeUser | null, role: Role){
    return !!user?.roles.includes(role);
}

