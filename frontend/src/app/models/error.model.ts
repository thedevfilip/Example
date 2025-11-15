export interface Error {
  code: string;
  description: string;
}

export const ErrorNone: Error = {
  code: '',
  description: ''
};