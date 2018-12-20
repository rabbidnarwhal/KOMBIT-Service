import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { catchError, map } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private url: string;
  constructor(private http: HttpClient) {
    this.url = environment.API_URL;
  }

  getUrl(): string {
    return this.url;
  }

  get(endpoint: string, httpOptions?: any): Promise<any> {
    const options = this.createRequestHeader(httpOptions ? httpOptions : {});
    return this.http
      .get(this.getUrl() + endpoint, options)
      .pipe(map(this.extractData), catchError(this.handleError))
      .toPromise();
  }

  delete(endpoint: string, httpOptions?: any): Promise<any> {
    const options = this.createRequestHeader(httpOptions ? httpOptions : {});
    return this.http
      .delete(this.getUrl() + endpoint, options)
      .pipe(map(this.extractData), catchError(this.handleError))
      .toPromise();
  }

  getBlob(url: string): Promise<any> {
    return this.http.get(url, { responseType: 'blob' }).pipe(catchError(this.handleError)).toPromise();
  }

  post(endpoint: string, body: any, httpOptions?: any): Promise<any> {
    const options = this.createRequestHeader(httpOptions ? httpOptions : {});
    return this.http
      .post(this.getUrl() + endpoint, body, options)
      .pipe(map(this.extractData), catchError(this.handleError))
      .toPromise();
  }

  private extractData(res: Response): any {
    const body = res;
    return body || {};
  }

  private createRequestHeader(options?: any): any {
    let headers = options.hasOwnProperty('headers') ? options.headers : {};
    headers['Access-Control-Allow-Origin'] = '*';
    if (!headers.hasOwnProperty('Content-Type')) {
      headers['Content-Type'] = 'application/json';
    } else if (headers['Content-Type'] === 'multipart/form-data') {
      headers = {};
    }
    options.headers = headers;
    return options;
  }

  private handleError(error: HttpErrorResponse): any {
    console.log('Error base', error);
    if (error.hasOwnProperty('error')) {
      if (error.error instanceof ErrorEvent) {
        console.error('An error occurred:', error.error.message);
      } else if (error.error && error.error.hasOwnProperty('Message')) {
        return throwError(error.error.Message);
      } else if (error.error && error.error.hasOwnProperty('ExceptionMessage')) {
        return throwError(error.error.ExceptionMessage);
      } else if (error.error && error.error.hasOwnProperty('errorMessage')) {
        if (error.error.errorMessage instanceof Array) {
          return throwError(error.error.errorMessage.join('\n'));
        } else {
          return throwError(error.error.errorMessage);
        }
      } else {
        console.error(`error message`, error.message);
      }
      // return throwError(`Something bad happened; please try again later.`);
      return throwError(error.error.message);
    }
  }
}
