import { IProduct } from './product';

export interface IPagination {
  pageindex: number;
  pageSize: number;
  count: number;
  data: IProduct[];
}
