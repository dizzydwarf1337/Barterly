export interface User {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  profilePicturePath?: string | null;
  favPostIds: string[];
  notificationCount: number;
}
