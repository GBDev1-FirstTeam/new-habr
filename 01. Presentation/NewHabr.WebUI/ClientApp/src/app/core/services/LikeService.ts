import { Injectable } from "@angular/core";
import { lastValueFrom } from "rxjs";
import { LikeData } from "../models/Like";
import { LikeMode } from "../static/LikeMode";

@Injectable({
    providedIn: 'root',
})
export class LikeService {
  like(likeData: LikeData, id: string | undefined, func: any) {
    if(likeData.isLiked) {
      lastValueFrom(func(id || LikeMode.None, LikeMode.Like))
    }
    else {
      lastValueFrom(func(id || LikeMode.None, LikeMode.Unlike))
    }
  }
}
