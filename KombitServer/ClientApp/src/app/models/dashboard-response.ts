export class MostPopularProduct {
  productName: Array<string>;
  totalChat: Array<number>;
  totalComment: Array<number>;
  totalInteraction: Array<number>;
  totalLike: Array<number>;
  totalView: Array<number>;
}

export class ActiveCustomer {
  name: Array<string>;
  totalChat: Array<number>;
  totalComment: Array<number>;
  totalLike: Array<number>;
  totalView: Array<number>;
  totalInteraction: Array<number>;
}

export class ActiveSupplier {
  name: Array<string>;
  totalProduct: Array<number>;
}
