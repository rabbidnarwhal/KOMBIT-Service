import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class FileUploadService {
  constructor(private apiService: ApiService, private authService: AuthService) {}

  uploadUrlData(
    type: string,
    path: string,
    useCase: string,
    fileName = null
  ): Promise<{ path: string; name: string; useCase: string }> {
    return new Promise((resolve, reject) => {
      this.apiService
        .getBlob(path)
        .then((blob) => this.uploadFile(blob, type, useCase, fileName))
        .then((res) => {
          const response = {
            useCase: useCase,
            name: res.name,
            path: res.path
          };
          resolve(response);
        })
        .catch((err) => {
          reject(err);
        });
    });
  }

  uploadDataUri(
    type: string,
    dataUri: string,
    useCase: string
  ): Promise<{ path: string; name: string; useCase: string }> {
    return new Promise((resolve, reject) => {
      const blobs = this.convertDataURItoBlob(dataUri);
      this.uploadFile(blobs.blob, type, useCase, 'name.' + blobs.mimeType.split('/')[1])
        .then((res) => {
          const response = {
            useCase: useCase,
            name: res.name,
            path: res.path
          };
          resolve(response);
        })
        .catch((err) => {
          reject(err);
        });
    });
  }

  uploadFile(blob: Blob, type: string, useCase: string, fileName = null): Promise<{ name: string; path: string }> {
    const formData: FormData = new FormData();
    if (fileName) {
      formData.append('File', blob, fileName);
    } else {
      formData.append('File', blob);
    }
    formData.append('UserId', this.authService.getUserId() + '');
    formData.append('UseCase', useCase);
    formData.append('Type', type);
    const headers = {
      'Content-Type': 'multipart/form-data'
    };
    return this.apiService.post('/upload/product/', formData, { headers: headers });
  }

  convertDataURItoBlob(dataURI) {
    // convert base64/URLEncoded data component to raw binary data held in a string
    let byteString;
    if (dataURI.split(',')[0].indexOf('base64') >= 0) {
      byteString = atob(dataURI.split(',')[1]);
    } else {
      byteString = unescape(dataURI.split(',')[1]);
    }

    // separate out the mime component
    const mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];

    // write the bytes of the string to a typed array
    const ia = new Uint8Array(byteString.length);
    for (let i = 0; i < byteString.length; i++) {
      ia[i] = byteString.charCodeAt(i);
    }

    return { blob: new Blob([ ia ], { type: mimeString }), mimeType: mimeString };
  }
}
