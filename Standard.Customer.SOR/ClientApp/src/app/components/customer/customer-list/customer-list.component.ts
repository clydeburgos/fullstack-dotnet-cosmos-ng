import { Component, EventEmitter, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { DataStateChangeEventArgs, GridComponent, PageSettingsModel } from '@syncfusion/ej2-angular-grids';
import { Observable } from 'rxjs';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { CustomerModel } from 'src/app/models/customer.model';
import { CustomerService } from 'src/app/services/customer.service';

@Component({
  selector: 'app-customer-list',
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.css']
})
export class CustomerListComponent implements OnInit {

  public isFetching: boolean;
  public customerData: BehaviorSubject<CustomerModel[]>;
  public customers: any[];
  public filterOptions: any;
  public searchKeyword: string = '';
  public state : DataStateChangeEventArgs;

  public orderBy: string = 'uniqueid';
  public sortDir: string = 'asc';

  @ViewChild('customerGrid', { static : false }) public grid: GridComponent;
  @Output() showDetailCustomerEmit = new EventEmitter();

  public selectedRowData: any;
  public pageSettings: PageSettingsModel;
  constructor(private customerService: CustomerService) { 
    this.customerData = new BehaviorSubject(null);
  }

  ngOnInit() {
    this.pageSettings = { pageSize: 20, pageSizes: true };
    this.state = { skip: 0, take: 10, search: [], sorted: [] };
    this.filterOptions = {
      type: 'Menu'
    }
    this.getCustomers();
  }

  search(){
    this.getCustomers(true);
  }

  getCustomers(searched: boolean = false){
    this.isFetching = true;
    this.customerService.getMany(this.state.skip, this.state.take, searched ? this.searchKeyword : '', 
    this.orderBy, this.sortDir).subscribe((res : any[]) => {
      this.customers = res;
      this.isFetching = false;
    }, (error: any) => {
      //toast
    }, () => {
    });
  }


  rowSelected(args){
    this.selectedRowData = args.data;
   }

  viewDetails(data){
    this.showDetailCustomerEmit.emit(data);
  }

  delete(data){
    this.isFetching = true;
    this.selectedRowData = data; // set the current row
    this.grid.selectRow(Number(data.index)); //data contains the index, so automatically pick the index for the selectRow

    if(!this.selectedRowData){
      return;
    }

    this.grid.deleteRecord();
    this.grid.refresh();

    this.customerService.delete(data.id).subscribe((res) => {
      this.getCustomers();
      this.showDetailCustomerEmit.emit(null);
    });
  }
}
