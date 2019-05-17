export interface IVacationData {
   id: number
   calendarDate: Date
   day: number
   dayName: string
   isOnVacation: boolean
   vacationTypeID: number
   userID: number
   rowVersion: any
   isNonWorkingDay: boolean
   isToday: boolean
}
