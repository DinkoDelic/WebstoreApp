import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { Basket, IBasket, IBasketItem, IBasketTotals } from '../shared/models/basket';
import { map } from 'rxjs/operators';
import { IProduct } from '../shared/models/product';

@Injectable({
  providedIn: 'root',
})
export class BasketService {
  baseUrl = environment.apiUrl;
  private basketSource = new BehaviorSubject<IBasket>(null);
  // $ at the end cause it's an Observable;
  basket$ = this.basketSource.asObservable();
  private basketTotalSource = new BehaviorSubject<IBasketTotals>(null);
  basketTotal$ = this.basketTotalSource.asObservable();

  constructor(private http: HttpClient) {}

  getBasket(id: string) {
    return this.http.get(this.baseUrl + 'basket?id=' + id).pipe(
      map((basket: IBasket) => {
        // .next() method in BehaviourSubject to assign value
        this.basketSource.next(basket);
        // Set basket total cost
        this.calculateTotals();
      })
    );
  }

  // updates basket
  setBasket(basket: IBasket) {
    return this.http.post(this.baseUrl + 'basket', basket).subscribe(
      (response: IBasket) => {
        this.basketSource.next(response);
        this.calculateTotals();
      },
      (error) => {
        console.log(error);
      }
    );
  }

  // returns current basket values
  getCurrentBasketValue() {
    return this.basketSource.value;
  }

  addItemToBasket(item: IProduct, quantity = 1) {

    const itemToAdd: IBasketItem = this.mapProductItemToBasketItem( item, quantity);

    const basket = this.getCurrentBasketValue() ?? this.createBasket();

    basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);

    this.setBasket(basket);
  }

  
  incrementItemQuantity(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id);
    basket.items[foundItemIndex].quantity++;
    this.setBasket(basket);
  }

  decrementItemQuantity(item: IBasketItem) {
    
    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id);
    if(basket.items[foundItemIndex].quantity > 1)
    {
      basket.items[foundItemIndex].quantity--;
      this.setBasket(basket);
    }
    else {
      this.removeItemFromBasket(item);
    }
  }


  removeItemFromBasket(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();
    if(basket.items.some(x => x.id === item.id))
    {
      // Returns an array of all the items that do not match the removed item id
      basket.items = basket.items.filter(i => i.id !== item.id);
      if(basket.items.length > 0)
      {
        this.setBasket(basket);
      }
      else
      {
        this.deleteBasket(basket);
      }
    }
  }
  deleteBasket(basket: IBasket) {
    return this.http.delete(this.baseUrl + 'basket?id=' + basket.id).subscribe(() => {
      this.basketSource.next(null);
      this.basketTotalSource.next(null);
      localStorage.removeItem('basket_id');
    }, error => {
      console.log(error);
    });
  }

  private calculateTotals() {
    const basket = this.getCurrentBasketValue();
    const shipping = 0;
    // b being item, a being total result
    // goes through every item calculating it's price and quantity and adding it to result which is initiated to 0 first)
    const subtotal = basket.items.reduce((a, b) => (b.price * b.quantity) + a, 0);
    const total = subtotal + shipping;
    this.basketTotalSource.next({shipping, subtotal, total});
  }


  private addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[]
  {
    const index = items.findIndex(i => i.id === itemToAdd.id);
    // if the item is not found in the basket, add it
    if (index === -1)
    {
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    }
    // else update the quantity of existing item
    else
    {
      items[index].quantity += quantity;
    }
    // return basket
    return items;
  }

  // Create new basket if there isn't one there
  private createBasket(): IBasket {
    const basket = new Basket();
    // basket id stored in local storage
    localStorage.setItem('basket_id', basket.id);

    return basket;
  }

  private mapProductItemToBasketItem(item: IProduct, quantity: number): IBasketItem {
    // returns an object
    return {
      id: item.id,
      productName: item.name,
      price: item.price,
      // knows that it maps to quantity parameter
      quantity,
      pictureUrl: item.pictureUrl,
      brand: item.productBrand,
      type: item.productType,
    };
  }
}
