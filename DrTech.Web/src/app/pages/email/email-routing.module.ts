import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SentComponent } from './sent/sent.component';
import { NotSentComponent } from './notsent/notsent.component';
import { EmailComponent } from './email.component';

const routes: Routes = [{
  path: '',
  component: EmailComponent,
  children: [
    {
      path: 'sent',
      component: SentComponent ,
    },
    {
      path: 'notsent',
      component: NotSentComponent ,
    },
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class EmailRoutingModule { }

export const routedComponents = [
  EmailComponent,
  SentComponent,
  NotSentComponent
];
