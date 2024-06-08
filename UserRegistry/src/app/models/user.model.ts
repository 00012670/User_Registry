
export interface User {
  userId: number;
  username: string;
  email: string;
  passwordHash: string;
  lastLogin: Date;
  status: Status;
  isSelected: boolean;
}

export enum Status {
  Active = 0,
  Blocked = 1
}


