export class DropdownDTO {
  public value: number;
  public id: number;
  public description: string;
}
export class DropdownViewModelWithTitle {
  public title: number;
  public list: Array<DropdownDTO>;
}

export class DonationDropdownsViewModel {
  public donationType: DropdownViewModelWithTitle;
  public cityList: DropdownViewModelWithTitle;
}
