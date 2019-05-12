import { Component, Input, OnInit } from '@angular/core';
import { ResponseMessage } from '../../../models/response-message.model';

@Component({
    selector: 'app-validation-summary',
    templateUrl: './validation-summary.component.html',
    styleUrls: ['./validation-summary.component.css']
})
export class ValidationSummaryComponent implements OnInit {
    @Input() response: ResponseMessage

    constructor() { }

    ngOnInit() { }

}
