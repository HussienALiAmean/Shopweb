export interface JwtPayload {

  sub: string;
  jti: string;
  UserId: string;
  Roles: string;
  exp: number;
  iss: string;
  aud: string;
}
