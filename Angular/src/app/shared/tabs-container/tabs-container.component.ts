import { Component, ContentChildren, AfterContentInit, QueryList, ChangeDetectionStrategy } from '@angular/core';
import { TabComponent } from '../tab/tab.component';

@Component({
    selector: 'app-tabs-container',
    templateUrl: './tabs-container.component.html',
    styleUrls: ['./tabs-container.component.css'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class TabsContainerComponent implements  AfterContentInit{
@ContentChildren(TabComponent) tabs:QueryList<TabComponent> = new QueryList();


  constructor() {
    
  }
  ngAfterContentInit(){
    let activeTabs = this.tabs.filter(t=>
      t.active
      )

      if(! activeTabs || activeTabs.length==0){
        this.selectTab(this.tabs!.first)
      }
  }

  selectTab(tab?:TabComponent){
    this.tabs?.forEach(tab=>{
      tab.active=false
    })

    if(tab){
      tab.active=true;
    }

    return false;
  }
}
