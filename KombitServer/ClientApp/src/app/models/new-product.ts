export class NewProduct {
  Attachment: Array<AttachmentFileRequest>;
  Benefit: string;
  BusinessTarget: string;
  CategoryId: number;
  CompanyId: number;
  Credentials: string;
  Currency: string;
  Description: string;
  Faq: string;
  Feature: string;
  Foto: Array<Foto>;
  HoldingId: number;
  Implementation: string;
  IsIncludePrice: boolean;
  IsPromoted: boolean;
  PosterId: number;
  Price: number;
  ProductName: string;
  UserId: number;
  VideoPath: string;
  constructor(data) {
    this.Attachment = [];
    this.Benefit = null;
    this.BusinessTarget = null;
    this.CategoryId = null;
    this.CompanyId = data.companyId;
    this.Credentials = null;
    this.Currency = null;
    this.Description = null;
    this.Faq = null;
    this.Feature = null;
    this.Foto = [];
    this.HoldingId = data.holdingId;
    this.Implementation = null;
    this.IsIncludePrice = false;
    this.IsPromoted = false;
    this.PosterId = data.id;
    this.Price = null;
    this.ProductName = null;
    this.UserId = null;
    this.VideoPath = null;
  }
}

export class Foto {
  UseCase: string;
  FotoName: string;
  FotoPath: string;
  Id: number;
}

export class AttachmentFileRequest {
  FileName: string;
  FilePath: string;
  Id: number;
}
