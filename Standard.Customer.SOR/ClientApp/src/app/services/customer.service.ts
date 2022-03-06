import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { CUSTOMER_CREATE_URL, CUSTOMER_DELETE_URL, CUSTOMER_GET_URL, CUSTOMER_MANY_URL, CUSTOMER_UPDATE_URL } from "../constants/customer-endpoints";
import { CustomerModel } from "../models/customer.model";

@Injectable({
    providedIn: 'root'
  })
  export class CustomerService {
  
    constructor(private http: HttpClient){
      
    }
  
    getMany(skip : number = 0, take : number = 0, searchKeyword: string = '', orderBy: string = '', sortOrder: string = ''): Observable<CustomerModel[]> {
      return this.http.get<any>(`${ environment.apiUrl }${ CUSTOMER_MANY_URL }?page=${skip}&pageSize=${take}&searchKeyword=${searchKeyword}&orderByColumn=${orderBy}&orderBy=${sortOrder}`);
    }
  
    get(id: string): Observable<CustomerModel> {
      return this.http.get<any>(`${ environment.apiUrl }${ CUSTOMER_GET_URL }/${ id }`);
    }
  
    create(payload: CustomerModel) {
      return this.http.post<any>(`${ environment.apiUrl }${ CUSTOMER_CREATE_URL }`, payload);
    }
  
    update(payload: CustomerModel){
      return this.http.put<any>(`${ environment.apiUrl }${ CUSTOMER_UPDATE_URL }`, payload);
    }
  
    delete(id: string){
      return this.http.delete<any>(`${ environment.apiUrl }${ CUSTOMER_DELETE_URL }/${ id }`, {});
    }
  
  }
  