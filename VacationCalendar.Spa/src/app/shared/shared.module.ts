import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ValidationSummaryComponent } from './validation-summary/validation-summary.component';
import { ConfirmationDialogComponent } from './confirmation-dialog/confirmation-dialog.component';

@NgModule({
  declarations: [
    ValidationSummaryComponent,
    ConfirmationDialogComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    ValidationSummaryComponent,
    ConfirmationDialogComponent
]
})
export class SharedModule { }
