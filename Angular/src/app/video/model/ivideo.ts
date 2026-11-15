import { IUser } from "../../user/userModels/iuser";

export interface IVideo {
    id:number,
    title:string,
    thumbnailUrl:string,
    videoUrl:string,
    userName:string,
    created:Date,
}
