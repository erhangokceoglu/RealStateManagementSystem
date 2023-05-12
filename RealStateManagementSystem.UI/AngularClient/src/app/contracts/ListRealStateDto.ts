export class ListRealStateDto {
    id : number
    province: string;
    district: string;
    neighborhood: string;
    islandNo: string; 
    parcelNo: string; 
    qualification: string;
    address: string;
}

export enum Qualification {
    Land = "Arsa",
    Residential = "Mesken",
    Field = "Tarla"
}