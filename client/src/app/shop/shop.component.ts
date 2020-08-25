import { Component, OnInit } from '@angular/core';
import { ShopService } from './shop.service';
import { IProduct } from '../shared/models/product';
import { IProductAttribute } from '../shared/models/product-attribute';
import { pipe } from 'rxjs';
import { ShopParams } from '../shared/models/shopParams';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss'],
})
export class ShopComponent implements OnInit {
  products: IProduct[]; // products array
  brands: IProductAttribute[];
  types: IProductAttribute[];
  shopParams = new ShopParams();

  maxSize = 5;
  totalCount: number;

  constructor(private shopService: ShopService) {}

  ngOnInit() {
    this.getProducts();
    this.getTypes();
    this.getBrands();
  }

  getProducts() {
    // Returns an observable of type IPagination
    this.shopService.getProducts(this.shopParams).subscribe(
      (response) => {
        this.products = response.data;
        this.totalCount = response.count;
        this.shopParams.pageNumber = response.pageindex;
        this.shopParams.pageSize = response.pageSize;
      },
      (error) => {
        console.log(error);
      }
    );
  }

  getBrands() {
    this.shopService.getBrands().subscribe(
      (response) => {
        // added an extra array memeber and then used ... spread operator for the rest
        this.brands = [{ id: 0, name: 'All' }, ...response];
      },
      (error) => {
        console.log(error);
      }
    );
  }
  getTypes() {
    this.shopService.getTypes().subscribe(
      (response) => {
        this.types = [{ id: 0, name: 'All' }, ...response];
      },
      (error) => {
        console.log(error);
      }
    );
  }

  onBrandSelected(brandId: number) {
    this.shopParams.brandId = brandId;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onTypeSelected(typeId: number) {
    this.shopParams.typeId = typeId;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onPriceSortSelected(sort: string) {
    this.shopParams.sort = sort;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  pageChanged(event: any): void {
    // condition to prevent the event from firing twice when we also change our totalCount on param change
    if ((this.shopParams.pageNumber !== event)) {
      this.shopParams.pageNumber = event.page;
      this.getProducts();
    }
  }
}
