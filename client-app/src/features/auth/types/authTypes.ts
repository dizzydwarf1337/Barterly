export interface User {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  role:string;
  profilePicturePath?: string | null;
  favPostIds: string[];
  notificationCount: number;
}
