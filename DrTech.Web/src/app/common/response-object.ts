export class ResponseObject<TObject> {
    public statusCode: number;
    public statusMessage: string;
    public data: TObject;
}
