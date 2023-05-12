export class CreateForAppUserDto{
    name: string;
    surname: string;
    email: string;
    password: string;
    address: string;
    isActive: boolean;
    createDate: Date;
    roleId: number;
}
