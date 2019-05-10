import { ISelectedPeriod } from 'src/interfaces/selected-period.interface';
import { ResponseMessage } from 'src/models/response-message.model';


export class UtiliyFunctions {

    public static get currentMonthYear(): ISelectedPeriod {
        let currentDateTime = new Date();
        let period = <ISelectedPeriod>
        {   month: currentDateTime.getMonth() + 1,
            year: currentDateTime.getFullYear(),
        }
        return period
    }

    public  static handleResponse(msg): ResponseMessage {
        let response = new ResponseMessage
        response = msg
        return response
    }

    public  static handleError(err): ResponseMessage {
        let response = new ResponseMessage
        response = err.error
        return response
    }
}