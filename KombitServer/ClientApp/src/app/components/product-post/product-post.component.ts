import { Component, OnInit, OnDestroy } from '@angular/core';
import { NzMessageService, UploadFile } from 'ng-zorro-antd';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { QuillDeltaToHtmlConverter } from 'quill-delta-to-html';

import Quill from 'quill';
import { QuillEditorComponent } from 'ngx-quill';
import { ApiService } from 'src/app/services/api.service';
import { FileUploadService } from 'src/app/services/file-upload.service';
import { NewProduct } from 'src/app/models/new-product';
import { AuthService } from 'src/app/services/auth.service';
import { ProductService } from 'src/app/services/product.service';
import { Router, ActivatedRoute } from '@angular/router';
const ATTRIBUTES = [ 'alt', 'height', 'width' ];

@Component({
  selector: 'app-product-post',
  templateUrl: './product-post.component.html',
  styleUrls: [ './product-post.component.scss' ]
})
export class ProductPostComponent implements OnInit {
  currency: string;
  currencyFetchedFromDetail = false;
  fileListImage = [];
  deletedImages = [];
  fileListMarketingKit = [];
  deletedMarketingKits = [];
  fileListVideo = [];
  isLoading = false;
  isSkeleton = false;
  listSolution = [];
  listUser = [];
  modules: any;
  oldProductData: any;
  videoUrl: any;
  previewImage = '';
  previewVisible = false;
  quill = {};
  quillContent = {};
  segmentName = 'description';
  validateForm: FormGroup;
  isEdit = false;

  constructor(
    private message: NzMessageService,
    private fb: FormBuilder,
    private sanitizer: DomSanitizer,
    private fileUploadService: FileUploadService,
    private authService: AuthService,
    private productService: ProductService,
    private route: Router,
    private activedRoute: ActivatedRoute
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
      isSupplierAsContact: [ true ],
      selectedContact: [ , [ Validators.required ] ],
      price: [],
      Description: [ , [ Validators.required ] ],
      BusinessTarget: [],
      Feature: [],
      Benefit: [],
      Implementation: [],
      Credentials: [],
      Faq: []
    });

    this.isSkeleton = false;
    this.checkRouteParameter();
  }

  checkRouteParameter() {
    this.activedRoute.params.subscribe((params) => {
      const id = +params['productId'];
      if (id) {
        this.isLoading = true;
        this.isSkeleton = true;
        this.loadEditableProductData(id);
      }
    });
  }

  async loadEditableProductData(id) {
    this.productService
      .fetchEditableProductData(id)
      .then((res) => {
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
        this.validateForm.get('BusinessTarget').setValue(res.businessTarget);
        this.validateForm.get('Feature').setValue(res.feature);
        this.validateForm.get('Benefit').setValue(res.benefit);
        this.validateForm.get('Implementation').setValue(res.implementation);
        this.validateForm.get('Credentials').setValue(res.credentials);
        this.validateForm.get('Faq').setValue(res.faq);

        if (res.userId !== res.posterId) {
          this.validateForm.get('isSupplierAsContact').setValue(false);
          this.validateForm.get('selectedContact').setValue(res.userId);
        }

        res.foto.map((foto) => {
          const fileList = {
            uid: foto.id,
            name: foto.fotoName,
            path: foto.fotoPath,
            url: foto.fotoPath,
            status: 'done'
          };
          this.fileListImage.push(fileList);
        });

        res.attachment.map((attachment) => {
          const fileList = {
            uid: attachment.id,
            name: attachment.fileName,
            path: attachment.filePath,
            url: attachment.filePath,
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
      const solutions = await this.productService.getListSolution();
      this.listSolution = solutions;

      const users = await this.productService.getListUser();
      this.listUser = users;

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

  handlePreviewImage = (file: UploadFile) => {
    this.previewImage = file.url || file.thumbUrl;
    this.previewVisible = true;
  };

  handleDeleteImage = (file: UploadFile) => {
    if (!String(file.path).includes('blob:', 0)) {
      this.deletedImages = [ ...this.deletedImages, file ];
    }
    this.fileListImage = this.fileListImage.filter((x) => x.uid !== file.uid);
  };

  handleDeleteMarketingKit = (file: UploadFile) => {
    if (!String(file.path).includes('blob:', 0)) {
      this.deletedMarketingKits = [ ...this.deletedMarketingKits, file ];
    }
    this.fileListMarketingKit = this.fileListMarketingKit.filter((x) => x.uid !== file.uid);
  };

  beforeUploadImage = (file: UploadFile): boolean => {
    const blob = window.URL.createObjectURL(file);
    const sanitize = this.sanitizer.bypassSecurityTrustUrl(blob);
    const fileList = {
      uid: this.fileListImage.length ? this.fileListImage[this.fileListImage.length - 1].uid + 1 : 0,
      name: file.name,
      path: blob,
      url: sanitize
    };
    this.fileListImage = [ ...this.fileListImage, fileList ];
    return false;
  };

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

  beforeUploadMarketingKit = (file: UploadFile): boolean => {
    const blob = window.URL.createObjectURL(file);
    const sanitize = this.sanitizer.bypassSecurityTrustUrl(blob);
    const fileList = {
      uid: this.fileListMarketingKit.length
        ? this.fileListMarketingKit[this.fileListMarketingKit.length - 1].uid + 1
        : 0,
      name: file.name,
      path: blob,
      url: sanitize
    };
    this.fileListMarketingKit.push(fileList);
    return false;
  };

  changeSegment(segment) {
    this.segmentName = segment;
  }

  async submitForm() {
    this.validatingForm();
    if (this.validateForm.valid) {
      if (this.fileListImage.length < 1) {
        alert('Photo is required');
        return;
      }
      const id = this.message.loading('Action in progress..', { nzDuration: 0 }).messageId;
      this.isLoading = true;
      let data: NewProduct = new NewProduct(this.authService.getPrincipal());

      /** Image upload */
      data = await this.uploadPhoto(data);

      /** Marketing kit Upload */
      data = await this.uploadMarketingKit(data);

      /** Video upload */
      data = await this.uploadVideo(data);

      /** Quill image upload */
      await this.uploadQuillImage();

      /** Publish product */
      await this.publishProduct(data);
      this.isLoading = false;
      this.message.remove(id);
    }
  }

  async uploadPhoto(data) {
    if (this.fileListImage.length) {
      const promise = this.fileListImage.map(async (element, index) => {
        if ((element.hasOwnProperty('status') && element.status !== 'done') || !element.hasOwnProperty('status')) {
          try {
            const result = await this.fileUploadService.uploadUrlData('foto', element.path, 'foto', element.name);
            data.Foto.push({
              FotoName: result.name,
              FotoPath: result.path,
              Id: 0,
              UseCase: result.useCase
            });
            this.fileListImage[index].url = result.path;
            this.fileListImage[index].path = result.path;
            this.fileListImage[index]['status'] = 'done';
          } catch (error) {
            this.message.error(error, { nzDuration: 5000, nzPauseOnHover: true, nzAnimate: true });
          }
        } else {
          data.Foto.push({
            FotoName: element.name,
            FotoPath: element.path,
            Id: element.uid,
            UseCase: 'foto'
          });
        }
      });
      await Promise.all(promise);
    }
    this.deletedImages.map((element) => {
      data.Foto.push({
        FotoName: null,
        FotoPath: null,
        Id: element.uid,
        UseCase: 'foto'
      });
    });
    return data;
  }

  async uploadMarketingKit(data: NewProduct) {
    if (this.fileListMarketingKit.length) {
      const promise = this.fileListMarketingKit.map(async (element, index) => {
        if ((element.hasOwnProperty('status') && element.status !== 'done') || !element.hasOwnProperty('status')) {
          try {
            const result = await this.fileUploadService.uploadUrlData('kit', element.path, 'attachment', element.name);
            data.Attachment.push({
              FileName: element.name,
              FilePath: result.path,
              Id: 0
            });
            this.fileListMarketingKit[index].url = result.path;
            this.fileListMarketingKit[index].path = result.path;
            this.fileListMarketingKit[index]['status'] = 'done';
          } catch (error) {
            this.message.error(error, { nzDuration: 5000, nzPauseOnHover: true, nzAnimate: true });
          }
        } else {
          data.Attachment.push({
            FileName: element.name,
            FilePath: element.path,
            Id: element.uid
          });
        }
      });
      await Promise.all(promise);
    }
    this.deletedMarketingKits.map((element) => {
      data.Attachment.push({
        FileName: null,
        FilePath: null,
        Id: element.uid
      });
    });
    return data;
  }

  async uploadVideo(data) {
    if (
      this.fileListVideo.length &&
      ((this.fileListVideo[0].hasOwnProperty('status') && this.fileListVideo[0].status !== 'done') ||
        !this.fileListVideo[0].hasOwnProperty('status'))
    ) {
      try {
        const result = await this.fileUploadService.uploadUrlData(
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
    for (const key in this.quill) {
      if (this.quill.hasOwnProperty(key)) {
        const element = this.quill[key];
        this.quillContent[key] = element.getContents();
        if (this.quillContent[key].ops.length) {
          const promiseContent = this.quillContent[key].ops.map(async (item) => {
            try {
              if (item.hasOwnProperty('insert') && item.insert.hasOwnProperty('image')) {
                if (item.insert.image.indexOf('http://') === -1 && item.insert.image.indexOf('https://') === -1) {
                  const result = await this.fileUploadService.uploadDataUri('foto', item.insert.image, key);
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
    data.BusinessTarget = this.validateForm.get('BusinessTarget').value;
    data.CategoryId = this.validateForm.get('solution').value;
    data.Currency = this.currency;
    data.Price = this.validateForm.get('price').value
      ? +this.validateForm.get('price').value.split(' ')[1].replace(/,/g, '')
      : null;
    data.IsIncludePrice = data.Price ? true : false;
    data.ProductName = this.validateForm.get('productName').value;
    if (this.isEdit) {
      data.PosterId = this.oldProductData.posterId;
      data.CompanyId = this.oldProductData.companyId;
      data.HoldingId = this.oldProductData.holdingId;
    }

    if (this.validateForm.get('isSupplierAsContact').value) {
      data.UserId = data.PosterId;
    } else {
      data.UserId = this.validateForm.get('selectedContact').value;
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
      this.route.navigate([ '' ]);
    } catch (error) {
      this.message.error(error, { nzDuration: 5000, nzPauseOnHover: true, nzAnimate: true });
    }
  }

  validatingForm() {
    for (const i in this.validateForm.controls) {
      if (i !== 'selectedContact') {
        this.validateForm.controls[i].markAsDirty();
        this.validateForm.controls[i].updateValueAndValidity();
      } else {
        if (this.validateForm.get('isSupplierAsContact').value === false) {
          if (this.validateForm.controls[i].value === '0') {
            this.validateForm.controls[i].setValue(null);
          }
          this.validateForm.controls[i].markAsDirty();
          this.validateForm.controls[i].updateValueAndValidity();
        } else {
          this.validateForm.controls[i].setValue('0');
        }
      }
    }
  }

  /** Event handler when quill is created. */
  quillCreated(item: QuillEditorComponent, editorName: string) {
    this.quill[editorName] = item;
  }
}
