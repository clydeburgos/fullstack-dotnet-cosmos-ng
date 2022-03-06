import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { CustomerListComponent } from '../customer/customer-list/customer-list.component';
import { CustomerManagerComponent } from '../customer/customer-manager/customer-manager.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {

  managerView: boolean = false;
  customerData: any;
  
  @ViewChild('customerList', {static : false}) customerList: CustomerListComponent;
  @ViewChild('customerManager', { static : false }) customerManager: CustomerManagerComponent;

  constructor(private changeDetect: ChangeDetectorRef) {
    
  }

  ngOnInit(): void {
   
  }
  
  create(){
    this.managerView = true;
    this.customerData = null;
  }

  cancel(){
    this.managerView = false;
  }

  savedCustomerEmit(){
    this.customerList.getCustomers();
    this.customerData = null;
    this.managerView = false;
    this.changeDetect.detectChanges();
  }

  showDetailCustomerEmit(data) {
    if(data) {
      this.managerView = true;
      this.customerData = data;
    } else {
      this.managerView = false;
    }
    this.changeDetect.detectChanges();
    setTimeout(() => {
      this.customerManager.setData();
    }, 500);
    
  }
}
