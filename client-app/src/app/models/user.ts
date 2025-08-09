export default interface User {
    id: string,
    firstName: string,
    lastName: string,
    email: string,
    token: string | undefined,
    profilePicturePath: string | undefined,
    role: string | undefined,
}