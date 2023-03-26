import { Pipe, PipeTransform } from "@angular/core";

@Pipe({name: 'convertDate'})
export class ConvertDatePipe implements PipeTransform {
  transform(value: number | undefined): string {
    return (new Date(value || 0)).toLocaleString('ru');
  }
}
