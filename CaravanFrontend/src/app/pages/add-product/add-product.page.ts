import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { IonContent, IonHeader, IonTitle, IonToolbar } from '@ionic/angular/standalone';
import { ProductService } from 'src/app/shared/services/product.service';
import { Product } from 'src/app/shared/models/product.model';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.page.html',
  styleUrls: ['./add-product.page.scss'],
  standalone: true,
  imports: [IonContent, IonHeader, IonTitle, IonToolbar, CommonModule, ReactiveFormsModule]
})
export class AddProductPage implements OnInit {
  AddProductForm!: FormGroup;
  photo!: File;
  photos!: File[];

  private formBuilder = inject(FormBuilder);
  private productService = inject(ProductService);

  constructor(){}

  ngOnInit(): void {
    this.AddProductForm = this.formBuilder.group({
      name: '',
      category: '',
      price: 0,
      description: ''
    });
  }

  upload(){

    console.log(this.photo);
    var product: Product = this.AddProductForm.value as Product;
    console.log(product);
    var formData = new FormData;
    formData.append('photo', this.photo);
    formData.append('product', JSON.stringify(product));
    this.productService.create(formData).subscribe(
      {
        next: (res:any) =>{
          console.log(res);
        },
        error: (err: any) =>{
          console.log(err);
        }
      }
    )
  }

  selectPhoto(evt: any){
    this.photo = evt.target.files[0];
  }

}
