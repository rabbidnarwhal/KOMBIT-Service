export class EditProductResponse {
  attachment: Array<AttachmentFileResponse>;
  benefit: string;
  businessTarget: string;
  categoryId: number;
  certificate: string;
  companyId: number;
  contactEmail: string;
  contactHandphone: string;
  contactId: number;
  contactName: string;
  credentials: string;
  currency: string;
  description: string;
  faq: string;
  feature: string;
  foto: Array<FotoResponse>;
  holdingId: number;
  implementation: string;
  isIncludePrice: boolean;
  isPromoted: boolean;
  posterAsContact: boolean;
  posterId: number;
  price: number;
  productCertificate: Array<FotoResponse>;
  productClient: Array<FotoResponse>;
  productImplementation: Array<FotoResponse>;
  productName: string;
  videoPath: string;
}

export class FotoResponse {
  fotoName: string;
  fotoPath: string;
  id: number;
  useCase: string;
  productId: number;
  title: string;
}

export class AttachmentFileResponse {
  fileName: string;
  filePath: string;
  fileType: string;
  id: number;
  productId: number;
}
