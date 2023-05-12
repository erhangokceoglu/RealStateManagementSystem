export class CreateRealStateDto {
    islandNo: string; 
    parcelNo: string; 
    qualification: number;
    address: string;
    createDate: Date;
    latitude?: string;
    longitude?: string;
    provinceId: number;
    districtId: number;
    neighbourhoodId: number;
    isActive: boolean;
}

export enum Qualification {
    Land = "Arsa",
    Residential = "Mesken",
    Field = "Tarla"
}