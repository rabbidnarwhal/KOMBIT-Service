import { Component, OnInit, Input } from '@angular/core';
import { NzMessageService, UploadFile } from 'ng-zorro-antd';
import { FormBuilder, Validators, FormGroup, FormControl } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { QuillDeltaToHtmlConverter } from 'quill-delta-to-html';

import { QuillEditorComponent } from 'ngx-quill';
import { FileUploadService } from 'src/app/services/file-upload.service';
import { NewProduct } from 'src/app/models/new-product';
import { AuthService } from 'src/app/services/auth.service';
import { ProductService } from 'src/app/services/product.service';
import { AttachmentFileResponse, FotoResponse, EditProductResponse } from 'src/app/models/edit-product-response';
import { EventsService } from 'src/app/services/events.service';

@Component({
  selector: 'app-product-post',
  templateUrl: './product-post.component.html',
  styleUrls: [ './product-post.component.scss' ]
})
export class ProductPostComponent implements OnInit {
  @Input() header: any;
  @Input() pid = 0;

  currency: string;
  currencyFetchedFromDetail = false;

  fileListVideo = [];
  fileListImage = [];
  fileListMarketingKit = [];
  fileListProductCertificate = [];
  fileListProductImplementation = [];
  fileListExistingClient = [];

  deletedImages = [];
  deletedMarketingKits = [];
  deletedProductCertificate = [];
  deletedProductImplementation = [];
  deteledExistingClient = [];

  isLoading = false;
  isSkeleton = false;
  isEdit = false;
  isContactOpened = false;

  errorMessage = '';

  previewVisible = false;
  previewImage = '';

  listSolution = [];
  listUser = [];
  modules: any;
  oldProductData: any;
  videoUrl: any;

  quill = {};
  quillContent = {};

  segmentName = 'description';

  validateForm: FormGroup;

  constructor(
    private message: NzMessageService,
    private fb: FormBuilder,
    private sanitizer: DomSanitizer,
    private fileUploadService: FileUploadService,
    private authService: AuthService,
    private productService: ProductService,
    private eventsService: EventsService
  ) {
    this.quill = {};
    this.quillContent = {};
    this.modules = {
      toolbar: {
        container: [
          [ 'bold', 'italic', 'underline' ],
          [ { indent: '-1' }, { indent: '+1' } ],
          [ { list: 'ordered' }, { list: 'bullet' } ],
          [ { size: [ 'small', false, 'large', 'huge' ] } ],
          [ { color: [] }, { background: [] } ],
          [ { align: [] } ],
          [ 'image' ]
        ]
      }
    };
  }

  async ngOnInit() {
    this.isSkeleton = true;
    this.loadParameterData();
    this.validateForm = this.fb.group({
      productName: [ , [ Validators.required ] ],
      solution: [ , [ Validators.required ] ],
      Description: [ , [ Validators.required ] ],
      isSupplierAsContact: [ true ],
      selectedContact: [ this.authService.getUserId(), [ Validators.required ] ],
      selectedContactPhone: [ this.authService.getPrincipal().phoneNumber ],
      selectedContactNameManual: [ , [ Validators.required ] ],
      selectedContactPhoneManual: [ , [ Validators.required ] ],
      Benefit: [],
      price: [],
      Feature: [],
      productCertificate: [],
      productImplementation: []

      // BusinessTarget: [],
      // Implementation: [],
      // Credentials: [],
      // Faq: []
    });

    this.isSkeleton = false;
    if (this.pid > 0) {
      this.isLoading = true;
      this.isSkeleton = true;
      this.loadEditableProductData(this.pid);
    }
  }

  async loadEditableProductData(id) {
    this.productService
      .fetchEditableProductData(id)
      .then((res: EditProductResponse) => {
        this.isEdit = true;
        this.oldProductData = {
          companyId: res.companyId,
          holdingId: res.holdingId,
          posterId: res.posterId,
          productId: id
        };

        this.currencyFetchedFromDetail = true;
        this.currency = res.currency;

        this.validateForm.get('productName').setValue(res.productName);
        this.validateForm.get('solution').setValue(res.categoryId);
        this.validateForm
          .get('price')
          .setValue(this.currency + ' ' + String(res.price).replace(/\B(?=(\d{3})+(?!\d))/g, ','));
        this.validateForm.get('Description').setValue(res.description);
        this.validateForm.get('Feature').setValue(res.feature);
        this.validateForm.get('Benefit').setValue(res.benefit);
        // this.validateForm.get('BusinessTarget').setValue(res.businessTarget);
        // this.validateForm.get('Implementation').setValue(res.implementation);
        // this.validateForm.get('Credentials').setValue(res.credentials);
        // this.validateForm.get('Faq').setValue(res.faq);

        if (res.posterAsContact || res.contactId !== res.posterId) {
          this.validateForm.get('selectedContact').setValue(res.contactId);
          this.validateForm.get('selectedContactPhone').setValue(res.contactHandphone);
        } else {
          this.validateForm.get('isSupplierAsContact').setValue(false);
          this.validateForm.get('selectedContactPhoneManual').setValue(res.contactHandphone);
          this.validateForm.get('selectedContactNameManual').setValue(res.contactName);
        }

        res.foto.map((item: FotoResponse) => {
          const fileList = {
            uid: item.id,
            name: item.fotoName,
            path: item.fotoPath,
            url: item.fotoPath,
            status: 'done'
          };
          this.fileListImage = [ ...this.fileListImage, fileList ];
        });

        res.productCertificate.map((item: FotoResponse) => {
          const fileList = {
            uid: item.id,
            name: item.fotoName,
            path: item.fotoPath,
            url: item.fotoPath,
            status: 'done'
          };
          this.fileListProductCertificate = [ ...this.fileListProductCertificate, fileList ];
        });

        res.productClient.map((item: FotoResponse) => {
          const fileList = {
            uid: item.id,
            name: item.fotoName,
            path: item.fotoPath,
            url: item.fotoPath,
            status: 'done'
          };
          this.fileListExistingClient = [ ...this.fileListExistingClient, fileList ];
        });

        res.productImplementation.map((item: FotoResponse) => {
          const fileList = {
            uid: item.id,
            name: item.fotoName,
            path: item.fotoPath,
            url: item.fotoPath,
            useCase: item.useCase,
            status: 'done'
          };
          this.validateForm.addControl('productImplementation_' + fileList.uid, new FormControl(item.title));
          this.fileListProductImplementation = [ ...this.fileListProductImplementation, fileList ];
        });

        res.attachment.map((attachment: AttachmentFileResponse) => {
          const fileList = {
            uid: attachment.id,
            name: attachment.fileName,
            path: attachment.filePath,
            url: attachment.filePath,
            type: attachment.fileType,
            status: 'done'
          };
          this.fileListMarketingKit.push(fileList);
        });

        if (res.videoPath) {
          const name = res.videoPath.split('/');
          const fileList = {
            uid: 0,
            name: name[name.length - 1],
            path: res.videoPath,
            url: res.videoPath,
            status: 'done'
          };
          this.fileListVideo = [ fileList ];
        }
        this.isSkeleton = false;
        this.isLoading = false;
      })
      .catch((error) => {
        this.message.error(error, { nzDuration: 5000, nzPauseOnHover: true, nzAnimate: true });
      });
  }

  async loadParameterData() {
    try {
      const promise = await Promise.all([ this.productService.getListUser(), this.productService.getListSolution() ]);
      this.listUser = promise[0];
      this.listSolution = promise[1];

      if (!this.currencyFetchedFromDetail) {
        const currency = await this.productService.getCurrency();
        if (!this.currencyFetchedFromDetail) {
          this.currency = currency;
        }
      }
    } catch (error) {
      this.message.error(error, { nzDuration: 5000, nzPauseOnHover: true, nzAnimate: true });
    }
  }

  /** Handle Preview */
  handlePreviewImage = (file: UploadFile) => {
    this.previewImage = file.url || file.thumbUrl;
    this.previewVisible = true;
  };
  /** END Handle Preview */

  /** Handle Delete */
  handleDeleteImage = (file: UploadFile) => {
    const fileList = this.removeItemFromFileList(file, this.deletedImages, this.fileListImage);
    this.fileListImage = fileList;
  };

  handleDeleteMarketingKit = (file: UploadFile) => {
    const fileList = this.removeItemFromFileList(file, this.deletedMarketingKits, this.fileListMarketingKit);
    this.fileListMarketingKit = fileList;
  };

  handleDeleteProductCertificate = (file: UploadFile) => {
    const fileList = this.removeItemFromFileList(file, this.deletedProductCertificate, this.fileListProductCertificate);
    this.fileListProductCertificate = fileList;
  };

  handleDeleteProductImplementation(file: UploadFile) {
    const fileList = this.removeItemFromFileList(
      file,
      this.deletedProductImplementation,
      this.fileListProductImplementation
    );
    this.fileListProductImplementation = fileList;
    this.validateForm.removeControl('productImplementation_' + file.uid);
  }

  handleDeleteExistingClient(file: UploadFile) {
    const fileList = this.removeItemFromFileList(file, this.deteledExistingClient, this.fileListExistingClient);
    this.fileListExistingClient = fileList;
  }
  /** End Handle Delete */

  removeItemFromFileList(file: UploadFile, fileList: Array<any>, deleteFileList: Array<any>) {
    if (!String(file.path).includes('blob:', 0)) {
      deleteFileList = [ ...deleteFileList, file ];
    }
    return fileList.filter((x) => x.uid !== file.uid);
  }

  /** Handle Before Upload */
  beforeUploadVideo = (file: UploadFile): boolean => {
    this.fileListVideo = [];
    const blob = window.URL.createObjectURL(file);
    const sanitize = this.sanitizer.bypassSecurityTrustUrl(blob);
    const fileList = {
      uid: 0,
      name: file.name,
      path: blob,
      url: sanitize
    };
    setTimeout(() => {
      this.fileListVideo = [ fileList ];
    }, 100);
    return false;
  };

  beforeUploadImage = (file: UploadFile): boolean => {
    const fileList = this.getFileList(file, this.fileListImage);
    this.fileListImage = [ ...this.fileListImage, fileList ];
    return false;
  };

  beforeUploadMarketingKit = (file: UploadFile): boolean => {
    const fileList = this.getFileList(file, this.fileListMarketingKit);
    if (file.type.toString().includes('presentation') || file.type.toString().includes('powerpoint')) {
      fileList.type = 'ppt';
    } else if (file.type.toString().includes('jpeg') || file.type.toString().includes('png')) {
      fileList.type = 'jpg';
    } else if (file.type.toString().includes('pdf')) {
      fileList.type = 'pdf';
    } else if (file.type.toString().includes('word')) {
      fileList.type = 'doc';
    } else {
      fileList.type = 'unknown';
    }
    this.fileListMarketingKit.push(fileList);
    return false;
  };

  beforeUploadProductCertificate = (file: UploadFile): boolean => {
    const fileList = this.getFileList(file, this.fileListProductCertificate);
    this.fileListProductCertificate.push(fileList);
    return false;
  };

  beforeUploadProductImplementation = (file: UploadFile): boolean => {
    const fileList = this.getFileList(file, this.fileListProductImplementation);
    fileList.useCase = file.type.toString().includes('image') ? 'implementationImage' : 'implementationVideo';
    this.fileListProductImplementation = [ ...this.fileListProductImplementation, fileList ];
    this.validateForm.addControl('productImplementation_' + fileList.uid, new FormControl(''));
    return false;
  };

  beforeUploadExistingClient = (file: UploadFile): boolean => {
    const fileList = this.getFileList(file, this.fileListExistingClient);
    this.fileListExistingClient.push(fileList);
    return false;
  };
  /** End Handle Before Upload */

  getFileList(file, fileList: Array<any>) {
    const blob = window.URL.createObjectURL(file);
    const sanitize = this.sanitizer.bypassSecurityTrustUrl(blob);
    return {
      uid: fileList.length ? fileList[fileList.length - 1].uid + 1 : 0,
      name: file.name,
      path: blob,
      url: sanitize,
      useCase: '',
      type: ''
    };
  }

  changeSegment(segment) {
    this.segmentName = segment;
  }

  async submitForm() {
    this.validatingForm();
    if (this.validateForm.valid) {
      if (this.fileListImage.length < 1) {
        this.message.error('Photo is required');
        return;
      }
      if (!this.isContactOpened) {
        this.message.error('Contact should be added');
        return;
      }
      if (this.listUser.length < 1 && this.validateForm.get('isSupplierAsContact').value) {
        this.message.warning('User list is empty, please wait a moment, then try again!');
        this.listUser = await this.productService.getListUser();
        return;
      }
      const actionMessage = this.message.loading('Action in progress..', { nzDuration: 0 }).messageId;
      this.isLoading = true;
      let data: NewProduct = new NewProduct(this.authService.getPrincipal());

      /** Image upload */
      try {
        data = await this.uploadPhoto(data);

        /** Marketing kit Upload */
        data = await this.uploadMarketingKit(data);

        /** Marketing kit Upload */
        data = await this.uploadProductCertificate(data);

        /** Marketing kit Upload */
        data = await this.uploadProductImplementation(data);

        /** Marketing kit Upload */
        data = await this.uploadExistingClient(data);

        /** Video upload */
        data = await this.uploadVideo(data);

        /** Quill image upload */
        await this.uploadQuillImage();

        /** Publish product */
        await this.publishProduct(data);
        this.isLoading = false;
        this.message.remove(actionMessage);
      } catch (error) {
        this.isLoading = false;
        this.message.error(error);
      }
    }
  }

  async uploadPhoto(data: NewProduct) {
    const dataPromise = await this.uploadFileData(data.Foto, this.fileListImage, this.deletedImages, 'foto');
    data.Foto = dataPromise.data;
    this.fileListImage = dataPromise.fileList;
    this.deletedImages = dataPromise.deletedList;
    return data;
  }

  async uploadMarketingKit(data: NewProduct) {
    const dataPromise = await this.uploadFileData(
      data.Attachment,
      this.fileListMarketingKit,
      this.deletedMarketingKits,
      'attachment'
    );
    data.Attachment = dataPromise.data;
    this.fileListMarketingKit = dataPromise.fileList;
    this.deletedMarketingKits = dataPromise.deletedList;
    return data;
  }

  async uploadProductCertificate(data: NewProduct) {
    const dataPromise = await this.uploadFileData(
      data.ProductCertificate,
      this.fileListProductCertificate,
      this.deletedProductCertificate,
      'certificate'
    );
    data.ProductCertificate = dataPromise.data;
    this.fileListProductCertificate = dataPromise.fileList;
    this.deletedProductCertificate = dataPromise.deletedList;
    return data;
  }

  async uploadProductImplementation(data: NewProduct) {
    const dataPromise = await this.uploadFileData(
      data.ProductImplementation,
      this.fileListProductImplementation,
      this.deletedProductImplementation,
      'implementation'
    );
    data.ProductImplementation = dataPromise.data;
    this.fileListProductImplementation = dataPromise.fileList;
    this.deletedProductImplementation = dataPromise.deletedList;
    return data;
  }

  async uploadExistingClient(data: NewProduct) {
    const dataPromise = await this.uploadFileData(
      data.ProductClient,
      this.fileListExistingClient,
      this.deteledExistingClient,
      'client'
    );
    data.ProductClient = dataPromise.data;
    this.fileListExistingClient = dataPromise.fileList;
    this.deteledExistingClient = dataPromise.deletedList;
    return data;
  }

  async uploadFileData(
    data: Array<any>,
    fileList: Array<any>,
    deletedList: Array<any>,
    useCase: string
  ): Promise<{ data: Array<any>; fileList: Array<any>; deletedList: Array<any> }> {
    if (fileList.length) {
      const promise = fileList.map(async (element, index) => {
        if ((element.hasOwnProperty('status') && element.status !== 'done') || !element.hasOwnProperty('status')) {
          const productName = this.validateForm.get('productName').value;
          try {
            let fileData = null,
              result = null;
            if (useCase === 'attachment') {
              result = await this.fileUploadService.uploadUrlData(
                productName,
                'kit',
                element.path,
                useCase,
                element.name
              );
              fileData = {
                FileName: element.name,
                FilePath: result.path,
                FileType: element.type,
                Id: 0
              };
            } else if (useCase === 'implementation') {
              result = await this.fileUploadService.uploadUrlData(
                productName,
                'foto',
                element.path,
                element.useCase,
                element.name
              );
              fileData = {
                FotoName: result.name,
                FotoPath: result.path,
                Id: 0,
                Title: this.validateForm.get('productImplementation_' + element.uid).value,
                UseCase: element.useCase
              };
            } else {
              result = await this.fileUploadService.uploadUrlData(
                productName,
                'foto',
                element.path,
                useCase,
                element.name
              );

              fileData = {
                FotoName: result.name,
                FotoPath: result.path,
                Id: 0,
                UseCase: useCase
              };
            }
            data = [ ...data, fileData ];
            fileList[index].url = result.path;
            fileList[index].path = result.path;
            fileList[index]['status'] = 'done';
          } catch (error) {
            this.message.error(error, { nzDuration: 5000, nzPauseOnHover: true, nzAnimate: true });
          }
        } else {
          let fileData = null;
          if (useCase === 'attachment') {
            fileData = {
              FileName: element.name,
              FilePath: element.path,
              FileType: element.type,
              Id: element.uid
            };
          } else if (useCase === 'implementation') {
            fileData = {
              FotoName: element.name,
              FotoPath: element.path,
              Id: element.uid,
              Title: this.validateForm.get('productImplementation_' + element.uid).value,
              UseCase: element.useCase
            };
          } else {
            fileData = {
              FotoName: element.name,
              FotoPath: element.path,
              Id: element.uid,
              UseCase: useCase
            };
          }

          data = [ ...data, fileData ];
        }
      });
      await Promise.all(promise);
    }
    if (deletedList.length) {
      deletedList.map((element) => {
        let fileData = null;
        if (useCase === 'attachment') {
          fileData = {
            FileName: null,
            FilePath: null,
            Id: element.uid,
            FileType: element.type
          };
        } else if (useCase === 'implementation') {
          fileData = {
            FotoName: null,
            FotoPath: null,
            Id: element.uid,
            Title: this.validateForm.get('productImplementation_' + element.uid).value,
            UseCase: element.useCase
          };
        } else {
          fileData = {
            FotoName: null,
            FotoPath: null,
            Id: element.uid,
            UseCase: useCase
          };
        }
        data = [ ...data, fileData ];
      });
    }
    return {
      data: data,
      fileList: fileList,
      deletedList: deletedList
    };
  }

  async uploadVideo(data: NewProduct) {
    if (
      this.fileListVideo.length &&
      ((this.fileListVideo[0].hasOwnProperty('status') && this.fileListVideo[0].status !== 'done') ||
        !this.fileListVideo[0].hasOwnProperty('status'))
    ) {
      try {
        const productName = this.validateForm.get('productName').value;
        const result = await this.fileUploadService.uploadUrlData(
          productName,
          'video',
          this.fileListVideo[0].path,
          'video',
          this.fileListVideo[0].name
        );
        data.VideoPath = result.path;
        this.fileListVideo[0].url = result.path;
        this.fileListVideo[0]['status'] = 'done';
      } catch (error) {
        this.message.error(error, { nzDuration: 5000, nzPauseOnHover: true, nzAnimate: true });
      }
    } else if (this.fileListVideo.length) {
      data.VideoPath = this.fileListVideo[0].path;
    }
    return data;
  }

  async uploadQuillImage() {
    const productName = this.validateForm.get('productName').value;
    for (const key in this.quill) {
      if (this.quill.hasOwnProperty(key)) {
        const element = this.quill[key];
        this.quillContent[key] = element.getContents();
        if (this.quillContent[key].ops.length) {
          const promiseContent = this.quillContent[key].ops.map(async (item) => {
            try {
              if (item.hasOwnProperty('insert') && item.insert.hasOwnProperty('image')) {
                if (item.insert.image.indexOf('http://') === -1 && item.insert.image.indexOf('https://') === -1) {
                  const result = await this.fileUploadService.uploadDataUri(
                    productName,
                    'foto',
                    item.insert.image,
                    key
                  );
                  item.insert = { image: result.path };
                }
              }
            } catch (error) {
              this.message.error(error, { nzDuration: 5000, nzPauseOnHover: true, nzAnimate: true });
            }
          });
          await Promise.all(promiseContent);
        }
      }
    }
  }

  async publishProduct(data: NewProduct) {
    // data.BusinessTarget = this.validateForm.get('BusinessTarget').value;
    data.Certificate = this.validateForm.get('productCertificate').value;
    data.CategoryId = this.validateForm.get('solution').value;
    data.Currency = this.currency;
    data.Price = this.validateForm.get('price').value
      ? +this.validateForm.get('price').value.split(' ')[1].replace(/,/g, '')
      : null;
    data.Implementation = this.validateForm.get('productImplementation').value;
    data.IsIncludePrice = data.Price ? true : false;
    data.ProductName = this.validateForm.get('productName').value;
    if (this.isEdit) {
      data.PosterId = this.oldProductData.posterId;
      data.CompanyId = this.oldProductData.companyId;
      data.HoldingId = this.oldProductData.holdingId;
    }

    if (this.validateForm.get('isSupplierAsContact').value) {
      data.ContactId = this.validateForm.get('selectedContact').value;
      const userIndex = this.listUser.findIndex((x) => x.id === data.ContactId);
      data.ContactName = this.listUser[userIndex].name;
      data.ContactHandphone = this.listUser[userIndex].handphone;
      if (data.ContactId === data.PosterId) {
        data.PosterAsContact = true;
      }
    } else {
      data.ContactId = data.PosterId;
      data.ContactHandphone = this.validateForm.get('selectedContactPhoneManual').value;
      data.ContactName = this.validateForm.get('selectedContactNameManual').value;
    }

    for (const segment in this.quill) {
      if (this.quillContent.hasOwnProperty(segment)) {
        const converter = new QuillDeltaToHtmlConverter(this.quillContent[segment].ops, {});
        data[segment] = converter.convert();
      } else {
        data[segment] = this.validateForm.get(segment).value;
      }
    }

    try {
      if (this.isEdit) {
        await this.productService.editProduct(data, this.oldProductData.productId);
      } else {
        await this.productService.postProduct(data);
      }
      this.isLoading = false;
      this.message.success('Product published', { nzDuration: 5000, nzPauseOnHover: true, nzAnimate: true });

      const content = {
        state: false,
        type: ''
      };
      this.eventsService.setModalState(content);
    } catch (error) {
      this.message.error(error, { nzDuration: 5000, nzPauseOnHover: true, nzAnimate: true });
    }
  }

  validatingForm() {
    let errorToogle = false;
    for (const objectKey in this.validateForm.controls) {
      if (this.validateForm.controls.hasOwnProperty(objectKey)) {
        if (objectKey === 'selectedContactNameManual' || objectKey === 'selectedContactPhoneManual') {
          if (this.validateForm.get('isSupplierAsContact').value) {
            // if (this.validateForm.controls[i].value === '0') {
            //   this.validateForm.controls[i].setValue(null);
            // }
            this.validateForm.controls[objectKey].setValue('0');
          }
        }
        this.validateForm.controls[objectKey].markAsDirty();
        this.validateForm.controls[objectKey].updateValueAndValidity();

        if (!errorToogle && this.validateForm.get(objectKey).errors) {
          errorToogle = true;
          const regularCase = objectKey.replace(/([A-Z])/g, ' $1').replace(/^./, (str) => {
            return str.toUpperCase();
          });
          this.message.error(regularCase + ' is required');
        }
      }
    }
  }

  openContact() {
    if (this.isContactOpened) {
      this.validateForm.get('isSupplierAsContact').setValue(!this.validateForm.get('isSupplierAsContact').value);
    } else {
      this.isContactOpened = true;
    }
  }

  showContactSubText() {
    if (this.isContactOpened) {
      return this.validateForm.get('isSupplierAsContact').value ? 'to manual input' : 'to contact selection';
    } else {
      return;
    }
  }

  /** Event handler when quill is created. */
  quillCreated(item: QuillEditorComponent, editorName: string) {
    this.quill[editorName] = item;
  }
}
