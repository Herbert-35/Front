import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BaseService, ApiResult } from '../base.service';
import { Observable } from 'rxjs';
import { Location } from './location';
import { State } from '../states/state';
@Injectable({
  providedIn: 'root',
})
export class LocationService
  extends BaseService<Location> {
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
  ): Observable<ApiResult<Location>> {
    var url = this.getUrl("api/Locations");
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
    return this.http.get<ApiResult<Location>>(url, { params });
  }
  get(id: number): Observable<Location> {
    var url = this.getUrl("api/Locations/" + id);
    return this.http.get<Location>(url);
  }
  put(item: Location): Observable<Location> {
    var url = this.getUrl("api/Location/" + item.id);
    return this.http.put<Location>(url, item);
  }
  post(item: Location): Observable<Location> {
    var url = this.getUrl("api/Location");
    return this.http.post<Location>(url, item);
  }

  getStates(
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
  isDupeLocation(item: Location): Observable<boolean> {
    var url = this.getUrl("api/Locations/isDupeLocation");
    return this.http.post<boolean>(url, item);
  }
}
