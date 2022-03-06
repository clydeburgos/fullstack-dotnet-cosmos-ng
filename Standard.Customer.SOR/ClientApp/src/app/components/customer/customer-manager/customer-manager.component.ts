import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { CustomerModel } from 'src/app/models/customer.model';
import { CustomerService } from 'src/app/services/customer.service';

@Component({
  selector: 'app-customer-manager',
  templateUrl: './customer-manager.component.html',
  styleUrls: ['./customer-manager.component.css']
})
export class CustomerManagerComponent implements OnInit {
  public isLoading: boolean;

  @Input('data') customerData: any;
  @Output() savedCustomerEmit = new EventEmitter();
  
  public isNew: boolean = false;
  public model: CustomerModel;
  public errors: any[];

  constructor(private customerService: CustomerService, private toastr: ToastrService) { 
    this.model = new CustomerModel();
    this.errors = [];
  }

  ngOnInit(){
    this.setData();
  }

  setData(){
    if(this.customerData) 
    {
      this.model = this.customerData;
    }
    this.isNew = !this.customerData;
  }

  create(){
    this.isLoading = true;
    this.customerService.create(this.model).subscribe(() => {
      this.handleSuccessSave('created');
      this.errors = [];
      this.isLoading = false;
    }, (error: any) => {
      this.handleError(error, 'creation');
      this.isLoading = false;
    });
  }

  update(){
    this.isLoading = true;
    this.customerService.update(this.model).subscribe(() => {
      this.handleSuccessSave('updated');
      this.errors = [];
      this.isLoading = false;
    }, (error: any) => {
      this.handleError(error, 'update');
      this.isLoading = false;
    });
  }

  private handleError(e, command){
    this.toastr.error(`Customer ${ command } experienced some issues!`, 'Error');
      let errorKeys = Object.keys(e.error.errors);
      this.errors = [];
      errorKeys.forEach(element => {
        let message = e.error.errors[element];
        this.errors.push(message);
      });
  }

  private handleSuccessSave(command){
    this.model = new CustomerModel();
    this.savedCustomerEmit.emit(command);
    this.toastr.success(`Customer ${command}!`, 'Success');
  }

}
