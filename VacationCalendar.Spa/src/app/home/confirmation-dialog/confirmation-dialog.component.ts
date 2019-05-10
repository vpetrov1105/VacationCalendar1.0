import { Component, Input } from '@angular/core';
import {MatDialogRef, MAT_DIALOG_DATA, MatDialog} from '@angular/material';

@Component({
  selector: 'confirm-dialog',
  templateUrl: './confirmation-dialog.component.html',
  styleUrls: ['./confirmation-dialog.component.css']
})

export class ConfirmationDialogComponent {
  
  public confirmMessage:string;
  public showCancelButton: boolean;
  public dialogTitlle: string;

  
  constructor(public dialogRef: MatDialogRef<ConfirmationDialogComponent>) {
    this.showCancelButton = true;
    this.dialogTitlle = "Confirm";
  }

}