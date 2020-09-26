import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IPagination } from '../shared/models/pagination';
import { IProductAttribute } from '../shared/models/product-attribute';
import { map } from 'rxjs/operators';
import { ShopParams } from '../shared/models/shopParams';
import { IProduct } from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) { }

  getProducts (shopParams: ShopParams) {

    let params = new HttpParams();

    if (shopParams.brandId !== 0)
    {
      params = params.append('brandId', shopParams.brandId.toString());
    }
    if (shopParams.typeId !== 0)
    {
      params = params.append('typeId', shopParams.typeId.toString());
    }
    if (shopParams.sort)
    {
      params = params.append('sort', shopParams.sort);
    }
    if (shopParams.pageNumber !== 0)
    {
      params = params.append('pageIndex', shopParams.pageNumber.toString());
    }
    if (shopParams.pageSize !== 0)
    {
      params = params.append('pageSize', shopParams.pageSize.toString());
    }
    if(shopParams.search)
    {
      params = params.append('search', shopParams.search);
    }

    return this.http.get<IPagination>(this.baseUrl + 'products', {observe: 'response', params})
    // pipe serves as a wrapper for any rxjs operators
      .pipe(
        map(response => {
          // returns the body of our request with the data in it
          return response.body;
        })
      )
  }

  getBrands () {
    return this.http.get<IProductAttribute[]>(this.baseUrl + 'products/' + 'brands');
  }

  getTypes () {
    return this.http.get<IProductAttribute[]>(this.baseUrl + 'products/' + 'types');
  }

  getProduct( id:number) {
    return this.http.get<IProduct>(this.baseUrl + 'products/' + id);
  }
}
