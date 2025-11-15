import { Error, ErrorNone } from './error.model';

export class Result<TValue> {
  private constructor(
    private readonly _value?: TValue,
    private readonly _error: Error = ErrorNone
  ) {}

  static success<T>(value: T) {
    return new Result<T>(value, ErrorNone);
  }

  static failure<T>(error: Error) {
    return new Result<T>(undefined, error);
  }

  match<TResult>(
    onSuccess: (value: TValue) => TResult,
    onFailure: (error: Error) => TResult
  ) {
    return this._error === ErrorNone
      ? onSuccess(this._value!)
      : onFailure(this._error);
  }
}