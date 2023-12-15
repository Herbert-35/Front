import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BaseService, ApiResult } from '../base.service';
import { Observable } from 'rxjs';
import { State } from './state';
@Injectable({
  providedIn: 'root',
})
export class StateService
  extends BaseService<State> {
  constructor(
    http: HttpClient) {
    super(http);
  }
  getData(
    pageIndex: number,
    pageSize: number,
    sortColumn: string,
    sortOrder: string,
    filterColumn: string | null,
    filterQuery: string | null
  ): Observable<ApiResult<State>> {
    var url = this.getUrl("api/States");
    var params = new HttpParams()
      .set("pageIndex", pageIndex.toString())
      .set("pageSize", pageSize.toString())
      .set("sortColumn", sortColumn)
      .set("sortOrder", sortOrder);
    if (filterColumn && filterQuery) {
      params = params
        .set("filterColumn", filterColumn)
        .set("filterQuery", filterQuery);
    }
    return this.http.get<ApiResult<State>>(url, { params });
  }
  get(id: number): Observable<State> {
    var url = this.getUrl("api/States/" + id);
    return this.http.get<State>(url);
  }
  put(item: State): Observable<State> {
    var url = this.getUrl("api/States/" + item.id);
    return this.http.put<State>(url, item);
  }
  post(item: State): Observable<State> {
    var url = this.getUrl("api/States");
    return this.http.post<State>(url, item);
  }
  isDupeField(stateId: number, fieldName: string, fieldValue: string):
    Observable<boolean> {
    var params = new HttpParams()
      .set("stateId", stateId)
      .set("fieldName", fieldName)
      .set("fieldValue", fieldValue);
    var url = this.getUrl("api/States/IsDupeField");
    return this.http.post<boolean>(url, null, { params });
  }
}
