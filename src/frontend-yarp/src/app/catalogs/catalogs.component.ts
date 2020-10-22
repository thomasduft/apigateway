import { Observable } from 'rxjs';

import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { HttpWrapperService } from '../core/services';
import { CatalogItem } from './models';

@Component({
  selector: 'tw-catalogs',
  templateUrl: './catalogs.component.html',
  styleUrls: ['./catalogs.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class CatalogsComponent implements OnInit {
  public catalogs$: Observable<Array<CatalogItem>>;

  constructor(
    private http: HttpWrapperService
  ) { }

  ngOnInit(): void {
    this.catalogs$ = this.http.get<Array<CatalogItem>>('catalog-api/catalogs');
  }
}
