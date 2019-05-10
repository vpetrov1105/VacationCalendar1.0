import { Role } from 'src/models/role.model';

export class ILoginUser {
    id: number;
    username: string;
    password: string;
    firstName: string;
    lastName: string;
    role: Role;
    token?: string;
}