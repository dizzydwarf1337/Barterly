export default interface ApiResponse<T> {
    isSuccess: boolean,
    value: T,
    error: string | undefined
}