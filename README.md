# VacationCalendar1.0
Application for maintaining employees vacation days

## Used technologies:
  .NET Core 2.1
  Angular 7.3
  Bootrap 4.3
  FontAwsome 4.7
  
## Application usage:
```
  1.login
    -for ADMIN use 
      username: vedran password: vedran
    -for USER use 
      username: ivan password: ivan
    -for ANONYMUS use 
      username: jan password: jan
    
  2. Vacation calendar usage description
    -after login you will get vacation calendar for all user in view mode.
    -above calendar there is name of displayed month and two buttons for month change
    -in top left corner of the calendar you can find button for acces edit mode
    -in edit mode bellow every user name you can find button for insert/update action
    -depending user role you can edit vacation data for users
    -click on button opens form for update/insert
    -after choosing period and vacation type click save in left bottom corner button of update/insert form
    -save action will, depending on choosed period, insert new vacation data or update if record exists for same date
    -after save proper message will appear in top of calendar
    -in edit mode bellow every inserted vacation you can find delete button
    -delete action will delete record for one vacation date.
    -in top left corner of application you can find button for logout
  
