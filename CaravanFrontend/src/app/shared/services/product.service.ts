import { Injectable, inject } from '@angular/core';
import { Product } from '../models/product.model';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  product: Product | undefined;
  products: Product[] = [];
  newData!: Product | undefined;

  private baseUrl: string = 'https://localhost:7141/Product';
  private http = inject(HttpClient);

constructor() { }

create(formData: any): Observable<any> {
  return this.http.post<any>(`${this.baseUrl}/CreateProduct`, formData);
}

delete(productName: string): Observable<any> {
  return this.http.delete<any>(`${this.baseUrl}/DeleteProduct`, {params: {name: productName}});
}

edit(productName: string, editedProduct: Product): Observable<any>{
  return this.http.put<any>(`${this.baseUrl}/EditProduct`, editedProduct, {params: {name: productName}});
}

getAllProducts(): Observable<Product[]> {
  return this.http.get<Product[]>(`${this.baseUrl}/GetAllProducts`);
}


getProduct(id: number): Observable<Product | undefined> {
  return this.getAllProducts().pipe(
    map((products) => products.find((p) => p.id === id)),
    catchError((error) => {
      console.error(error);
      return new Observable<Product | undefined>();
    })
  );
}
getProductByName(productName: string): Observable<Product | undefined>{
  return this.getAllProducts().pipe(
    map((products) => products.find((p) => p.name === productName)),
    catchError((error) => {
      console.error(error);
      return new Observable<Product | undefined>();
    })
  );
}

deleteProduct(productName: string): void{
  console.log(productName);
  this.delete(productName).subscribe(
    response => {
      console.log('Product deleted successfully', response)
      // this.toastr.success('Product deleted successfully', response);
      this.getAllProducts().subscribe(products => {
        this.products = products;
      });
    },
    error => {
      console.log('Error when deleting product', error)
      // this.toastr.error('Error when deleting product', error)
    }

  )
}

}
