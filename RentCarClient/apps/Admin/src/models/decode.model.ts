export interface DecodeModel{
    id: string;
    fullName: string;
    email: string;
    role: string;
    permissions: string[];
    branch: string;
}

export const initialDecode: DecodeModel = {
    id: '',
    fullName: '',
    email: '',
    role: '',
    permissions: [],
    branch: ''
}