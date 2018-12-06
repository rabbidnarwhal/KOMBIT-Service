import { LoginResponse } from './login-response';

export class NewProduct {
  Attachment: Array<AttachmentFileRequest>;
  Benefit: string;
  // BusinessTarget: string;
  CategoryId: number;
  Certificate: string;
  CompanyId: number;
  ContactHandphone: string;
  ContactId: number;
  ContactName: string;
  // Credentials: string;
  Currency: string;
  Description: string;
  // Faq: string;
  Feature: string;
  Foto: Array<Foto>;
  HoldingId: number;
  Implementation: string;
  IsIncludePrice: boolean;
  PosterAsContact: boolean;
  PosterId: number;
  Price: number;
  ProductCertificate: Array<Foto>;
  ProductClient: Array<Foto>;
  ProductImplementation: Array<ProductImplementation>;
  ProductName: string;
  VideoPath: string;
  constructor(data: LoginResponse) {
    this.Attachment = [];
    this.Benefit = null;
    // this.BusinessTarget = null;
    this.CategoryId = null;
    this.Certificate = null;
    this.CompanyId = data.companyId;
    this.ContactHandphone = null;
    this.ContactId = null;
    this.ContactName = null;
    // this.Credentials = null;
    this.Currency = null;
    this.Description = null;
    // this.Faq = null;
    this.Feature = null;
    this.Foto = [];
    this.HoldingId = data.holdingId;
    this.Implementation = null;
    this.IsIncludePrice = false;
    this.PosterAsContact = false;
    this.PosterId = data.id;
    this.Price = null;
    this.ProductCertificate = [];
    this.ProductClient = [];
    this.ProductImplementation = [];
    this.ProductName = null;
    this.VideoPath = null;
  }
}

export class Foto {
  FotoName: string;
  FotoPath: string;
  Id: number;
  UseCase: string;
}

export class ProductImplementation extends Foto {
  Title: string;
}

export class AttachmentFileRequest {
  FileName: string;
  FilePath: string;
  Id: number;
}
