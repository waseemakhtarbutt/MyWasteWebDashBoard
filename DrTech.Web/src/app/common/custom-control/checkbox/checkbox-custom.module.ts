/**
 * @license
 * Copyright Akveo. All Rights Reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 */

import { NgModule } from '@angular/core';

import { NbSharedModule } from '@nebular/theme/components/shared/shared.module';
import { NbCheckboxComponentCustom } from './checkbox-custom-component';

@NgModule({
  imports: [
    NbSharedModule,
  ],
  declarations: [NbCheckboxComponentCustom],
  exports: [NbCheckboxComponentCustom],
})
export class NbCheckboxCustomModule { }
