export interface LikeRequest {
  Id: string;
  Login: string;
  UserId: string;
  Like: boolean;
}

export interface Like {
  Id?: string | undefined;
  LikesCount?: number | undefined;
  IsLiked?: boolean | undefined;
}
